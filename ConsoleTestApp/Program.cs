using System;
using Pace_Note_Generator;

namespace ConsoleTestApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Node> nodes = new List<Node>
            {
                new Node(43.9248223, 7.0099617), //Left turn
                new Node(43.9249598, 7.0096884), //Right turn
                new Node(43.9251878, 7.0092593), //Straight
            };



            double distanceAB = Geomath.GetDistance(nodes[0], nodes[1]);
            double distanceBC = Geomath.GetDistance(nodes[1], nodes[2]);
            double distanceAC = Geomath.GetDistance(nodes[0], nodes[2]);

            Console.WriteLine($"a: {distanceAB}\nb: {distanceBC}\nc: {distanceAC}\n");

            Console.WriteLine(Geomath.CircumcircleRadius(distanceAB, distanceBC, distanceAC));

            double crossProduct = Geomath.CalculateTurnAngle(nodes[0], nodes[1], nodes[2]);
            Console.WriteLine(Geomath.ConvertToEquirectangular(nodes[0], nodes[1]));
            Console.WriteLine(Geomath.ConvertToEquirectangular(nodes[1], nodes[2]));
            Console.WriteLine(crossProduct);
        }
    }
}
