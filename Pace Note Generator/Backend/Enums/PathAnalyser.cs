using Pace_Note_Generator.Backend.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pace_Note_Generator.Backend.Enums_and_Structs
{
    public class PathAnalyser
    {
        public List<Node> NodeList { private set; get; }

        public PathAnalyser(List<Node> nodeList)
        {
            NodeList = nodeList;
        }

        //SELFNOTE: this uses the first node of the 3 for the node for that direction, if the results are off by a node or 2, move to middle
        public List<Node> CornerGrouping()
        {
            List<Node> group = new List<Node>();
            Direction? nodesGroupDirection = Geomath.CalculateNodesDirection(NodeList[0], NodeList[1], NodeList[2]);

            for (int i = 0; i < NodeList.Count; i++)
            {
                if (NodeList.Count < NodeList.Count + 2)
                {
                    Direction? nodesDirection = Geomath.CalculateNodesDirection(NodeList[i], NodeList[i + 1], NodeList[i + 2]);
                    if (nodesDirection == nodesGroupDirection) {group.Append(NodeList[i]);}
                }
            }

            return group;
        }
    }
}
