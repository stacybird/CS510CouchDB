using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CouchTrafficClient
{
    class QueryD : QueryBase
    {
        public override string Run()
        {
            List<object> detectorIDs = new List<object> { }, speeds = new List<object> { };
            double stationLength = 0;
            // i. Find station length of "Foster NB" station
            List<object> keys = new List<object> { "Foster NB" }, startkeys = new List<object> { "" }, endkeys = new List<object> { "" };
            var result = Query("queryD", "getStationLength", keys, false);
            List<object> outobject;
            if (result.TryGetValue("Foster NB", out outobject))
            {
                stationLength = Convert.ToDouble(outobject.First().ToString());
            }
            else
            {
                // Return error
            }

            // ii. Find all the detector IDs of "Foster NB"
            keys.Clear();
            result.Clear();
            outobject.Clear();
            keys = new List<object> { "Foster NB" };
            result = Query("queryD", "getDetectorIDs", keys, false);
            if (result.TryGetValue("Foster NB", out outobject))
            {
                detectorIDs = outobject;
            }
            else
            {
                // Return error
            }

            // iii
            int detectorCount = 0, speedCount = 0, timePeriodCount = 0;
            double totalSpeed = 0, totalDetectorSpeed = 0, avgTotalDetectorSpeed = 0, totalAverageTravelTime = 0;
            // 9/22/2011 7:00:00 -> 9/22/2011 8:59:40
            for (long timeInstance = 1316700000000; timeInstance <= 1316707180000; timeInstance += 20000) // for each time instance
            {
	            foreach (long detectorID in detectorIDs) // for each detector of a station
	            {
                    startkeys.Clear();
                    endkeys.Clear();
                    result.Clear();
                    speeds.Clear();
                    startkeys = new List<object> { detectorID, timeInstance };
                    endkeys = new List<object> { detectorID, timeInstance, new List<object>{} };
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
	                totalAverageTravelTime += (stationLength / avgTotalDetectorSpeed)*3600;
                    avgTotalDetectorSpeed = 0;
                    timePeriodCount++;
                }
            }
            // 9/22/2011 16:00:00 -> 9/22/2011 17:59:40
            for (long timeInstance = 1316732400000; timeInstance <= 1316739580000; timeInstance += 20000) // for each time instance
            {
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
                    totalAverageTravelTime += (stationLength / avgTotalDetectorSpeed) * 3600;
                    avgTotalDetectorSpeed = 0;
                    timePeriodCount++;
                }
            }

            var returnvalue = totalAverageTravelTime / timePeriodCount;
            return string.Format("The average travel time for 7-9AM and 4-6PM on September 22, 2011 for station Foster NB in seconds = {0}.", returnvalue.ToString());
        }
    }
}
