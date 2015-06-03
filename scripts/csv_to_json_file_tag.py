#!/usr/bin/env python
import sys
import csv
import json
import re

if len(sys.argv) != 3:
  print 'Incorrect number of arguments.'
  print 'Usage: csv_to_json.py path_to_csv path_to_json'
  exit()

print 'Argument List:', str(sys.argv)
csvFileName = sys.argv[1]
jsonFileName = sys.argv[2]

csvFile = open (csvFileName, 'rU')
myReader = csv.reader(csvFile)
header = myReader.next()
print "Header fields:", header

myReader = csv.DictReader( csvFile, fieldnames = header) 

jsonFile = open( jsonFileName, 'w')  

# far too fancy regex for my tastes.
# grabs words, makes the tag the last word prior to ".csv"
fileTag = re.findall(r"[\w']+", csvFileName)[-2:-1][0]

def writeNRecords(n):
  count = 0
  jsonFile.write("{"docs": [")
  for row in myReader:
    if count != 0:
      jsonFile.write(", ")
    row['tag'] = fileTag
    parsedJson = json.dumps( row )
    jsonFile.write(parsedJson)
    count += 1
    if 0 == (count % n):
      break
  jsonFile.write("] }")

writeNRecords(10000)

print "up to 10,000 JSON records saved to: ", jsonFileName  

