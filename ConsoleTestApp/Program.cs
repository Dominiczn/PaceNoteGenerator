using System;
using System.ComponentModel.DataAnnotations;
using Pace_Note_Generator.Backend;
using Pace_Note_Generator.Backend.Enums_and_Structs;

namespace ConsoleTestApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Node> nodes = new List<Node>
            {
                new Node(43.924265, 7.010258),
                new Node(43.924327, 7.010186),
                new Node(43.924326, 7.01009),
                new Node(43.924251, 7.010031),
                new Node(43.924132, 7.010042),
                new Node(43.923863, 7.010116),
                new Node(43.92377, 7.010104),
                new Node(43.923734, 7.01003),
                new Node(43.92376, 7.009937),
                new Node(43.923823, 7.009909),
                new Node(43.924208, 7.009894),
                new Node(43.924491, 7.009996),
                new Node(43.924645, 7.010035),
                new Node(43.924739, 7.010027),
                new Node(43.924822, 7.009962),
                new Node(43.92496, 7.009688),
                new Node(43.925188, 7.009259),
                new Node(43.925327, 7.008563),
                new Node(43.925591, 7.007635),
                new Node(43.925851, 7.006777),
                new Node(43.925934, 7.006516),
                new Node(43.926133, 7.00615),
                new Node(43.926212, 7.005823),
                new Node(43.926271, 7.005275),
            };

            /*
            double angle = Geomath.CalculateTurnAngle(nodes[4], nodes[5], nodes[6]);
            Console.WriteLine(angle);
            */

            
            PathAnalyser testPath = new PathAnalyser(nodes);
            {
                List<List<Node>> allGroups = new List<List<Node>>();
                List<Node> usedNodes = new List<Node>();

                for (int i = 0; i < nodes.Count; i++)
                {
                    nodes.RemoveAll(n => usedNodes.Contains(n));
                    List<Node> group = testPath.CornerGrouping();
                    allGroups.Add(group);
                    foreach (var node in group)
                    {
                        usedNodes.Add(node);
                    }
                }

                Console.WriteLine(allGroups);
                int sdf = 0;
            }
            
            
        }
    }

}
