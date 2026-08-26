using System;
using System.Collections.Generic;
using System.Text;

namespace Pace_Note_Generator.Backend
{
    public class Node
    {
        public double Latitude { protected set; get; }
        public double Longitude { protected set; get; }

        public Node(double lat, double lon)
        {
            Latitude = lat;
            Longitude = lon;
        }

        public double DistanceTo(Node node)
        {
            return Geomath.GetDistance(this, node);
        }
    }
}
