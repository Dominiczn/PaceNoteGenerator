using System;
using System.ComponentModel.DataAnnotations;
using Pace_Note_Generator.Backend;
using Pace_Note_Generator.Backend.Analysis;

namespace ConsoleTestApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Node> nodes = new List<Node>
            {
                new Node(52.087681, -0.017658),
                new Node(52.087518, -0.018002),
                new Node(52.08638, -0.020195),
                new Node(52.086159, -0.020863),
                new Node(52.086102, -0.021362),
                new Node(52.08602, -0.022466),
                new Node(52.085957, -0.022929),
                new Node(52.084717, -0.027879),
                new Node(52.084575, -0.028393),
                new Node(52.08447, -0.028696),
                new Node(52.084362, -0.028892),
                new Node(52.084288, -0.02898),
                new Node(52.084204, -0.029058),
                new Node(52.084022, -0.029191),
                new Node(52.083569, -0.029496),
                new Node(52.083372, -0.029629),
                new Node(52.08304, -0.029884),
                new Node(52.082852, -0.030128),
                new Node(52.082735, -0.030379),
                new Node(52.0827, -0.030611),
                new Node(52.082708, -0.030815),
                new Node(52.082724, -0.030947),
                new Node(52.082746, -0.031077),
                new Node(52.082793, -0.031342),
                new Node(52.082881, -0.031864),
                new Node(52.082898, -0.032006),
                new Node(52.082907, -0.032296),
                new Node(52.082906, -0.032472),
                new Node(52.082894, -0.032647),
                new Node(52.082865, -0.032805),
                new Node(52.082826, -0.032939),
                new Node(52.082784, -0.03307),
                new Node(52.082725, -0.033178),
                new Node(52.082491, -0.033533),
                new Node(52.082133, -0.034078),
                new Node(52.082084, -0.034185),
                new Node(52.082049, -0.034287),
                new Node(52.082021, -0.034398),
                new Node(52.082, -0.034517),
                new Node(52.081989, -0.034641),
                new Node(52.081983, -0.03523),
                new Node(52.081929, -0.036667),
                new Node(52.081918, -0.037444),
                new Node(52.081928, -0.037873),
                new Node(52.08193, -0.038001),
                new Node(52.081947, -0.03837),
                new Node(52.081945, -0.038386),
            };

            


            PathAnalyser testPath = new PathAnalyser(nodes);
            {
                
                List<Pacenote> pacenotes = testPath.AnalysePath();
                for (int i = 0; i < pacenotes.Count; i++)
                {
                    if (pacenotes[i].IsStraight == true && pacenotes[i].StraightLength < 100) { continue; }
                    Console.WriteLine($"{pacenotes[i].CornerSeverity} {pacenotes[i].Direction}");
                }
                

                //testPath.GroupCorners();

                //Console.WriteLine(Geomath.CircumcircleRadius(nodes[2], nodes[3], nodes[4]));
                //Console.WriteLine(Geomath.CalculateCrossProuct(nodes[0], nodes[1], nodes[2]));
            }
        }
    }

}
