using System;
using Pace_Note_Generator;

namespace ConsoleTestApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //A
            double lat1 = 43.9248223;
            double lon1 = 7.0099617;
            Node A = new Node(43.9248223, 7.0099617);

            //B
            double lat2 = 43.9249598;
            double lon2 = 7.0096884;

            //C
            double lat3 = 43.9251878;
            double lon3 = 7.0092593;

            List<Node> nodes = new List<Node>
            {
                new Node(43.9248223, 7.0099617),
                new Node(43.9249598, 7.0096884),
                new Node(43.9251878, 7.0092593),
            };



            double distanceAB = Geomath.GetDistance(lat1, lon1, lat2, lon2);
            double distanceBC = Geomath.GetDistance(lat2, lon2, lat3, lon3);
            double distanceAC = Geomath.GetDistance(lat1, lon1, lat3, lon3);

            Console.WriteLine($"a: {distanceAB}\nb: {distanceBC}\nc: {distanceAC}\n");

            Console.WriteLine(Geomath.CircumcircleRadius(distanceAB, distanceBC, distanceAC));

            double crossProduct = Geomath.CalculateTurnAngle(lat1, lon1, lat2, lon2, lat3, lon3);
            Console.WriteLine(Geomath.ConvertToEquirectangular(lat1, lon1, lat2, lon2));
            Console.WriteLine(Geomath.ConvertToEquirectangular(lat2, lon2, lat3, lon3));
            Console.WriteLine(crossProduct);
        }
    }
}
