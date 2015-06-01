#!/usr/bin/env python
import os
import sys

if len(sys.argv) != 4:
  print 'Incorrect number of arguments.'
  print 'Usage: import_to_couchdb host dbName jsonFile'
  exit()

print 'Argument List:', str(sys.argv)
hostName = sys.argv[1]
dbName = sys.argv[2]
jsonFile = sys.argv[3]

DB="http://" + hostName +":5984/" +dbName
command = "curl -d " + jsonFile + " -X POST " + DB + "/_bulk_docs"
os.system( command )

