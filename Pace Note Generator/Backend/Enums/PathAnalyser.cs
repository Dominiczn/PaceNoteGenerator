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
            //group.Add(NodeList[0]);

            for (int i = 0; i < NodeList.Count; i++)
            {
                if (i < NodeList.Count - 2)
                {
                    Direction? nodesDirection = Geomath.CalculateNodesDirection(NodeList[i], NodeList[i + 1], NodeList[i + 2]);
                    if (nodesDirection == nodesGroupDirection) {group.Add(NodeList[i]);}
                    else { break; }
                    NodeList[i].Direction = nodesDirection; 
                }
            }

            return group;
        }
    }
}
