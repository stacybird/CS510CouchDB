using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CouchTrafficClient
{
    class QueryE : QueryBase
    {
        public override string Run()
        {
            List<object> detectorIDs = new List<object> { }, speeds = new List<object> { };
            List<double> stationLength = new List<double> { };
            List<string> stationName = new List<string> { };
            int highwayID = 0;
            // i. Find highway ID of "I-205 NB"
            List<object> keys = new List<object> { new List<object>{"NORTH", "N", "I-205"} }, startkeys = new List<object> { "" }, endkeys = new List<object> { "" };
            var result = Query("queryE", "getHighwayID", keys, false);
            List<object> outobject;
            highwayID = Convert.ToInt32(result.Values.First().First().ToString());

            // ii. Find station names and lengths of previously found highway ID
            keys.Clear();
            result.Clear();
            //outobject.Clear();
            keys = new List<object> { highwayID };
            result = Query("queryE", "getStationIDNameAndLength", keys, false);
            if (result.TryGetValue(highwayID.ToString(), out outobject))
            {
                foreach (List<object> o in outobject)
                {
                    stationName.Add(o.ElementAt(1).ToString());
                    stationLength.Add(Convert.ToDouble(o.ElementAt(2).ToString()));
                }
            }
            else
            {
                // Return error
            }

            // iii. Find all the detector IDs of each previously found station name
            List<object> detectorIDList = new List<object> { };
            foreach (string sn in stationName)
            {
                keys.Clear();
                result.Clear();
                outobject = new List<object> { };
                keys = new List<object> { sn };
                result = Query("queryD", "getDetectorIDs", keys, false);
                if (result.TryGetValue(sn, out outobject))
                {
                    detectorIDList.Add(outobject);
                }
                else
                {
                    detectorIDList.Add(new List<object> { });
                } 
            }
            outobject = new List<object> { };

            // iv For each time period between 7-9AM and 4-6PM on September 22, 2011: Find the average of the speeds for all detector IDs as previously found. Then calculate the travel time for that station ID, for all station IDs:
            int detectorCount = 0, speedCount = 0, timePeriodCount = 0, stationCount = 0;
            double totalSpeed = 0, totalDetectorSpeed = 0, avgTotalDetectorSpeed = 0, totalAverageTravelTime = 0;
            // 9/22/2011 7:00:00 -> 9/22/2011 8:59:40
            for (long timeInstance = 1316700000000; timeInstance <= 1316707180000; timeInstance += 20000) // for each time instance
            {
                stationCount = 0;
                foreach (string sn in stationName) // for each station name
                {
                    detectorIDs = (List<object>)detectorIDList.ElementAt(stationCount);
                    foreach (long detectorID in detectorIDs) // for each detector of a station
                    {
                        startkeys.Clear();
                        endkeys.Clear();
                        result.Clear();
                        speeds.Clear();
                        startkeys = new List<object> { detectorID, timeInstance };
                        endkeys = new List<object> { detectorID, timeInstance, new List<object> { } };
                        result = QueryWithStartAndEnd("queryD", "getDetectorSpeeds", startkeys, endkeys, false);
                        if (result.Count > 0)
                        {
                            if (result.Values.First().First() != "")
                            {
                                speeds.Add(result.Values.First().First());
                            }
                        }
                        foreach (long speed in speeds) // for each speed of a detector
                        {
                            totalSpeed += speed;
                            speedCount++;
                        }
                        if (speedCount > 0)
                        {
                            totalDetectorSpeed += totalSpeed / speedCount;
                            totalSpeed = 0;
                            speedCount = 0;
                            detectorCount++;
                        }
                    }
                    if (detectorCount > 0)
                    {
                        avgTotalDetectorSpeed = totalDetectorSpeed / detectorCount;
                        totalDetectorSpeed = 0;
                        detectorCount = 0;
                        totalAverageTravelTime += (stationLength.ElementAt(stationCount) / avgTotalDetectorSpeed);
                        avgTotalDetectorSpeed = 0;
                    }
                    stationCount++;
                }
                timePeriodCount++;
            }
            var returnValueAM = (totalAverageTravelTime / timePeriodCount) * 60;
            totalAverageTravelTime = 0;
            timePeriodCount = 0;
            // 9/22/2011 16:00:00 -> 9/22/2011 17:59:40
            for (long timeInstance = 1316732400000; timeInstance <= 1316739580000; timeInstance += 20000) // for each time instance
            {
                stationCount = 0;
                foreach (string sn in stationName) // for each station name
                {
                    detectorIDs = (List<object>)detectorIDList.ElementAt(stationCount);
                    foreach (long detectorID in detectorIDs) // for each detector of a station
                    {
                        startkeys.Clear();
                        endkeys.Clear();
                        result.Clear();
                        speeds.Clear();
                        startkeys = new List<object> { detectorID, timeInstance };
                        endkeys = new List<object> { detectorID, timeInstance, new List<object> { } };
                        result = QueryWithStartAndEnd("queryD", "getDetectorSpeeds", startkeys, endkeys, false);
                        if (result.Count > 0)
                        {
                            if (result.Values.First().First() != "")
                            {
                                speeds.Add(result.Values.First().First());
                            }
                        }
                        foreach (long speed in speeds) // for each speed of a detector
                        {
                            totalSpeed += speed;
                            speedCount++;
                        }
                        if (speedCount > 0)
                        {
                            totalDetectorSpeed += totalSpeed / speedCount;
                            totalSpeed = 0;
                            speedCount = 0;
                            detectorCount++;
                        }
                    }
                    if (detectorCount > 0)
                    {
                        avgTotalDetectorSpeed = totalDetectorSpeed / detectorCount;
                        totalDetectorSpeed = 0;
                        detectorCount = 0;
                        totalAverageTravelTime += (stationLength.ElementAt(stationCount) / avgTotalDetectorSpeed);
                        avgTotalDetectorSpeed = 0;
                    }
                    stationCount++;
                }
                timePeriodCount++;
            }

            var returnValuePM = (totalAverageTravelTime / timePeriodCount) * 60;
            return string.Format("The average travel time for the I-205 NB freeway in minutes on September 22, 2011 for\n7-9AM = {0}\n4-6PM = {1}.", returnValueAM.ToString(), returnValuePM.ToString());
        }
    }
}
