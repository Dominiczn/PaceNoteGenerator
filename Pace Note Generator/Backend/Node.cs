using System;
using System.Collections.Generic;
using System.Text;

namespace Pace_Note_Generator.Backend
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

        public double DistanceTo(Node node)
        {
            return Geomath.GetDistance(this, node);
        }
    }
}
