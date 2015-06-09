// These are the document design queries from CouchDB
// In some cases we did multi-view queries to get to the final result
// The subviews are all listed here for each query A-F.

Query A:
function(document) {
  if(document.tag == "freeway_loopdata" && document.speed > 100) {
    emit("topspeeds", 1);
  }
}
Reduce:
function(keys, values, rereduce) {
  return sum(values);
}


Query B:
function(document) {
  if(document.tag == "freeway_loopdata" && document.starttime.split(" ")[0] == "2011-09-22" && (document.detectorid == 1361 || document.detectorid == 1362 || document.detectorid == 1363)) {
    emit("fosterdata", parseInt(document.volume));
  }
}
Reduce:
function(keys, values, rereduce) {
  return sum(values);
}


Query C
Part 2:
function(doc) {
  if (doc.tag == "freeway_stations" && doc.locationtext == "Foster NB"){
	emit(null, doc.length);	
  }
}
Part 3:
function(doc) {
  if (doc.tag == "freeway_stations" && doc.locationtext == "Foster NB"){
	emit(null, doc.stationid);	
  }
}
Part 4: 
Map:
function(doc) {
  if (doc.tag == "freeway_loopdata" && (doc.detectorid == 1361 || doc.detectorid == 1362 || doc.detectorid == 1363)){
	var temp = doc.starttime.split(' ');
	if (temp[0] == "2011-09-22") {
		var time = new Date(Date.parse("2011\/09\/22 " + temp[1]));
		var minutes = (time.getHours() * 60) + time.getMinutes();
		if (doc.speed != "") {
			emit(Math.floor(minutes / 5), doc.speed);
		}
	}
		
  }
}
Reduce: //borrowed from http://tobyho.com/2009/10/07/taking-an-average-in-couchdb/
function(keys, values, rereduce) {
	if (!rereduce)
    	{
		var length = values.length;
		return [sum(values) / length, length];
    	}
    	else 
    	{
        var length = sum(values.map(function(v)
		{
			return v[1];
		}));
        var avg = sum(values.map(function(v)
		{
           		return v[0] * (v[1] / length)
        	}));
        return [avg, length]
    }
}
getDetectorID
function(doc)
{
  if(doc.tag == "freeway_detectors")
  {
    emit(doc.locationtext, doc.detectorid);
  }
}


Query D:
getDetectorID
function(doc)
{
  if(doc.tag == "freeway_detectors")
  {
    emit(doc.locationtext, doc.detectorid);
  }
}
getDetectorSpeeds
function(doc)
{
  if(doc.tag == "freeway_loopdata")
  {
    var temp = doc.starttime.split(' ');
    var replaced_temp = temp[0].replace(/-/g, "\/");
    var d = new Date(Date.parse(replaced_temp + " " + temp[1]));
    emit([doc.detectorid, d.valueOf(), doc.starttime], doc.speed);
  }
}
getStationLength
function(doc)
{
  if(doc.tag == "freeway_stations")
  {
    emit(doc.locationtext, doc.length);
  }
}


Query E:
getHighwayID
function(doc)
{
  if(doc.tag == "highways")
  {
    emit([doc.direction, doc.shortdirection, doc.highwayname], doc.highwayid);
  }
}
getStationIDNameAndLength
function(doc)
{
  if(doc.tag == "freeway_stations")
  {
    emit(doc.highwayid, [doc.stationid, doc.locationtext, doc.length]);
  }
}


// Note, Query F was not feasible directly in CouchDB.
// The query to find a route was done client-side in our application.
// See CouchTrafficClient/QueryF.cs for the client-side portion.

Query F
function(doc) {
  if (doc.tag == "freeway_stations"){
    emit(doc.stationid, {'downstream':doc.downstream,'locationtext':doc.locationtext});
  }
}

