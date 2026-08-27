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
                new Node(43.907215, 6.985005),
                new Node(43.907216, 6.985155),
                new Node(43.907175, 6.985291),
                new Node(43.907052, 6.985313),
                new Node(43.906907, 6.985336),
                new Node(43.906821, 6.985456),
                new Node(43.906804, 6.985591),
                new Node(43.906758, 6.985717),
                new Node(43.906625, 6.985745),
                new Node(43.906272, 6.985352),
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
                    if (nodes.Count > 3)
                    {
                        List<Node> group = testPath.CornerGrouping();
                        allGroups.Add(group);
                        foreach (var node in group)
                        {
                            usedNodes.Add(node);
                        }
                        nodes.RemoveAll(n => usedNodes.Contains(n));
                    }

                    else { break; }
                }

                Console.WriteLine(allGroups);
                int sdf = 0;
            }
            
            
        }
    }

}
