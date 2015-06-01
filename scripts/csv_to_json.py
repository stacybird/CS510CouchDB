#!/usr/bin/env python
import sys
import csv
import json

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

parsedJson = json.dumps( [ row for row in myReader ] )  
print "JSON parsed."  

jsonFile = open( jsonFileName, 'w')  
jsonFile.write(parsedJson)  
print "JSON saved to: ", jsonFileName  

