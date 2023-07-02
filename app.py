from flask import *
from flask_cors import CORS
import json
app = Flask(__name__)
CORS(app)

from transformers import AutoTokenizer, AutoModelForTokenClassification
from transformers import pipeline
tokenizer = AutoTokenizer.from_pretrained("bert-base-cased")
model = AutoModelForTokenClassification.from_pretrained("dslim/bert-base-NER")
nlp = pipeline("token-classification", model=model, tokenizer=tokenizer, aggregation_strategy="average")
from flair.data import Sentence
from flair.models import SequenceTagger
tagger = SequenceTagger.load("kaliani/flair-ner-skill")


@app.route('/')
def go_home():
    return "Hello"

@app.route("/query", methods=["POST","GET"])
def query():
    query = request.json['_query']
    dict = {"ORG": "", "LOC": "", "PER": "", "MISC": "","SKILL":""}
    for obj in nlp(query):
        dict[obj['entity_group']]+=(obj['word']+" ")
    sentence = Sentence(query)
    tagger.predict(sentence)
    for entity in sentence.get_spans('ner'):
        dict["SKILL"]+=(entity.text+"")
    return json.dumps(dict)

if __name__ == '__main__':
    app.run(host="localhost",port=5000)