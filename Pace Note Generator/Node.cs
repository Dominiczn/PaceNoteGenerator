using System;
using System.Collections.Generic;
using System.Text;

namespace Pace_Note_Generator
{
    public class Node
    {
        public double latitude { protected set; get; }
        public double longitude { protected set; get; }

        public Node(double lat, double lon)
        {
            latitude = lat;
            longitude = lon;
        }

        public double DistanceTo(double lat, double lon)
        {
            return Geomath.GetDistance(latitude, longitude, lat, lon);
        }
    }
}
