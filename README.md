# CS510CouchDB
Final group project loading traffic data into couchdb

Data used for this project provided by Professor Tufte at Portland State University:  http://web.cecs.pdx.edu/~tufte/

## Server Setup

The process of installing the primary CouchDB instance consists of:

0. Starting with an Ubuntu Trusty AWS Instance.
1. sudo apt-get update
2. sudo apt-get dist-upgrade
3. sudo add-apt-repository ppa:couchdb/stable -y
4. 1. sudo apt-get update
5. sudo apt-get install couchdb
6. sudo sed -i 's/;bind_address = 127.0.0.1/bind_address = 0.0.0.0/g' /etc/couchdb/local.ini
7. sudo restart couchdb
8. From there, set the admin password in futon and start loading or replicating data!

## Loading Data

1. Run Stacy's script to convert the data to JSON.
2. Loop over each result with Stacy's other script to bulk import the data into CouchDB.

## Replicating Data

1. Go to Replicator in the Futon interface.
2. Choose to Replicate from a remote database, using the other instance's internal IP: http://x.x.x.x:5984/traffic/
3. Name the local database it will replicate to "traffic" as well.
4. Check the "Continuous" box and click "Replicate".
5. You can monitor the progress of the replication under the "Status" page when logged in as an admin.
