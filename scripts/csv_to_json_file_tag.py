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
jsonFileArray = sys.argv[2].split(".")

csvFile = open (csvFileName, 'rU')
myReader = csv.reader(csvFile)
header = myReader.next()
print "Header fields:", header

myReader = csv.DictReader( csvFile, fieldnames = header) 

# far too fancy regex for my tastes.
# grabs words, makes the tag the last word prior to ".csv"
fileTag = re.findall(r"[\w']+", csvFileName)[-2:-1][0]

jsonFileCount = 0

def writeNRecords(n):
  count = 0
  for row in myReader:
    if count == 0:
      jsonFileName = jsonFileArray[0] + "_" + str(jsonFileCount) + "." + jsonFileArray[1]
      jsonFile = open( jsonFileName, 'w')  
      jsonFile.write("{\"docs\": [")
    if count != 0:
      jsonFile.write(", ")
    row['type'] = fileTag
    for key in row:
      try:
        row[key] = int(row[key])
      except:
        pass
    parsedJson = json.dumps( row )
    jsonFile.write(parsedJson)
    count += 1
    if 0 == (count % n):
      break
  if count != 0:
    jsonFile.write("] }")
    print str(count) + " JSON records saved to: ", jsonFileName  
  return (jsonFileCount + 1)

for x in range(0,2000):
  jsonFileCount = writeNRecords(10000)

