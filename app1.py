from flask import *
from flask_cors import CORS
import torch
import pymongo
import spacy
import json
from flair.data import Sentence
from flair.models import SequenceTagger
from transformers import AutoTokenizer, AutoModelForTokenClassification,AutoModelForSequenceClassification,pipeline
app = Flask(__name__)
CORS(app)

client = pymongo.MongoClient("mongodb://localhost:27017/")
database = client["Applicants"]
collection = database["Resume"]
nlp1 = spacy.load("en_core_web_lg", disable=["tagger", "lemmatizer"])
tokenizer1 = AutoTokenizer.from_pretrained("bert-base-cased")
model1 = AutoModelForTokenClassification.from_pretrained("dslim/bert-base-NER")
nlp2 = pipeline("token-classification", model=model1, tokenizer=tokenizer1, aggregation_strategy="average")
tagger = SequenceTagger.load("kaliani/flair-ner-skill")
model2 = AutoModelForSequenceClassification.from_pretrained("shahrukhx01/roberta-base-boolq")
tokenizer2 = AutoTokenizer.from_pretrained("shahrukhx01/roberta-base-boolq")


def predict(question, passage):
    sequence = tokenizer2.encode_plus(question, passage, return_tensors="pt")['input_ids']
    logits = model2(sequence)[0]
    probabilities = torch.softmax(logits, dim=1).detach().cpu().tolist()[0]
    proba_yes = round(probabilities[1], 2)
    proba_no = round(probabilities[0], 2)
    return (proba_no,proba_yes)
@app.route('/')
def go_home():
    return "I am fast as Fuck, Boi!"

@app.route("/query", methods=["POST","GET"])
def query():
    query = request.json['_query']
    start = len(query);
    for obj in nlp2(query):
        if start>obj['start']:
            start=obj['start']
    sentence = Sentence(query)
    tagger.predict(sentence)
    for entity in sentence.get_spans('ner'):
        if start>entity.start_position:
            start=entity.start_position
    doc = nlp1(query)
    if len(doc.ents)>1:
        length = query.find(doc.ents[0].text)
        if start>length:
            start=length
    else:
        pass
    j=0
    list_tokens = query.split()
    while True:
        if start==0:
            break
        elif start<0:
            j-=1
            break
        else:
            start-=(len(list_tokens[j])+1)
            j+=1
    Query="Is the person "
    j = j-2 if j>1 else j-1 if j>0 else j
    for f in range(j,len(list_tokens)):
        Query=Query+list_tokens[f]+" "
    Query+="?"
    print(Query)
    Ids = []
    resumes = collection.find()
    for resume in resumes:
        v = predict(Query,resume['resume'])
        if(v[1]>v[0]):
            Ids.append(resume['_id'])
    return json.dumps({"ids":Ids})

if __name__ == '__main__':
    app.run(host="localhost",port=5001)