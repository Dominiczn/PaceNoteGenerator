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
                new Node(52.36989, -0.456244),
                new Node(52.369446, -0.455545),
                new Node(52.368894, -0.454589),
                new Node(52.368238, -0.453255),
                new Node(52.367845, -0.452408),
                new Node(52.367226, -0.450815),
                new Node(52.366699, -0.44908),
                new Node(52.366274, -0.447346),
                new Node(52.365964, -0.445955),
                new Node(52.365779, -0.444743),
                new Node(52.365006, -0.438839),
                new Node(52.364631, -0.43576),
                new Node(52.364154, -0.431907),
                new Node(52.363961, -0.430364),
                new Node(52.363903, -0.429928),
                new Node(52.363111, -0.424017),
                new Node(52.363029, -0.423312),
                new Node(52.362844, -0.42186),
                new Node(52.362712, -0.42086),
                new Node(52.36253, -0.419345),
                new Node(52.36232, -0.417614),
                new Node(52.361732, -0.413103),
                new Node(52.361623, -0.412232),
                new Node(52.361401, -0.411149),
                new Node(52.36124, -0.410425),
                new Node(52.360989, -0.409441),
                new Node(52.360846, -0.40896),
                new Node(52.360718, -0.4086),
            };

            /*
            double angle = Geomath.CalculateTurnAngle(nodes[4], nodes[5], nodes[6]);
            Console.WriteLine(angle);
            */


            PathAnalyser testPath = new PathAnalyser(nodes);
            {
                /*
                List<List<Node>> allGroups = new List<List<Node>>();
                List<Node> usedNodes = new List<Node>();

                for (int i = 0; i < nodes.Count; i++)
                {
                    if (nodes.Count > 3)
                    {
                        List<Node> group = testPath.GroupCorners();
                        allGroups.Add(group);
                        foreach (var node in group)
                        {
                            usedNodes.Add(node);
                        }
                        nodes.RemoveAll(n => usedNodes.Contains(n));
                    }

                    else { break; }
                }
                */

                Console.WriteLine(testPath.CalculateStraightLength(nodes));
            }
            
            
        }
    }

}
