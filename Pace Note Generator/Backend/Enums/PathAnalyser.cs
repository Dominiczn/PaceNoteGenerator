using Pace_Note_Generator.Backend.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Pace_Note_Generator.Backend.Enums_and_Structs
{
    public class PathAnalyser
    {
        public List<Node> NodeList { private set; get; }

        public PathAnalyser(List<Node> nodeList)
        {
            NodeList = nodeList;
        }

        public List<Node> CornerGrouping()
        {
            List<Node> group = new List<Node>();
            Direction? nodesGroupDirection = Geomath.CalculateNodesDirection(NodeList[0], NodeList[1], NodeList[2]);
            NodeList[0].Direction = nodesGroupDirection;
            group.Add(NodeList[0]);

            for (int i = 1; i < NodeList.Count; i++)
            {
                if (i < NodeList.Count - 1)
                {
                    Direction? nodesDirection = Geomath.CalculateNodesDirection(NodeList[i - 1], NodeList[i], NodeList[i + 1]);
                    if (nodesDirection == nodesGroupDirection) {group.Add(NodeList[i]);}
                    else { break; }
                    NodeList[i].Direction = nodesDirection; 
                }
            }

            return group;
        }
    }
}
