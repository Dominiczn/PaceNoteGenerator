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
            NodeList = new List<Node>(nodeList);
        }


        public List<Pacenote> AnalysePath()
        {
            List<Pacenote> pacenotes = new List<Pacenote>();
            List<List<Node>> allGroups = new List<List<Node>>();
            GroupCorners(allGroups);

            foreach(List<Node> nodes in allGroups)
            {
                Pacenote pacenote = ClassifyCorner(nodes, allGroups);
                if (pacenote.IsStraight)
                {
                    pacenote.StraightLength = CalculateStraightLength(nodes);
                }

                pacenotes.Add(pacenote);
            }

            return pacenotes;
        }

        //Groups nodes on a route into left corners, right corners, and straights
        //TODO: last few nodes get cut off if number of nodes in list isnt multiple of 3. FIX IT
        public void GroupCorners(List<List<Node>> allGroups)
        {
            if (NodeList.Count <= 3) { return; }

            List<Node> group = new List<Node>();
            Direction? nodesGroupDirection = Geomath.CalculateNodesDirection(NodeList[0], NodeList[1], NodeList[2]);
            NodeList[0].Direction = nodesGroupDirection;
            group.Add(NodeList[0]);

            for (int i = 1; i < NodeList.Count; i++)
            {
                if (i < NodeList.Count - 1)
                {
                    Direction? nodesDirection = Geomath.CalculateNodesDirection(NodeList[i - 1], NodeList[i], NodeList[i + 1]);
                    if (nodesDirection != nodesGroupDirection) { break; }

                    NodeList[i].Direction = nodesDirection;
                    group.Add(NodeList[i]);
                }
            }

            allGroups.Add(group);
            NodeList.RemoveRange(0, group.Count);

            GroupCorners(allGroups);
        }

        //Helper method that joins direction and severity classification into 1
        private Pacenote ClassifyCorner(List<Node> corner, List<List<Node>> allCorners)
        {
            if (corner.Count < 3 && corner[0].Direction != null) { corner = AddPreviousNodeToCorner(corner, allCorners); }
            Pacenote pacenote = ClassifyCornerSeverity(corner);
            pacenote.Direction = ClassifyCornerDirection(corner);

            return pacenote;
        }

        //decides what pacenote to give each group of nodes grouped by corner
        private Pacenote ClassifyCornerSeverity(List<Node> corner)
        {
            Pacenote pacenote = new Pacenote();
            double highestAngle = 0;
            for (int i = 1; i < corner.Count - 1; i++)
            {
                double currentTurnAngle = Geomath.CalculateTurnAngle(corner[i - 1], corner[i], corner[i + 1]);

                if (Math.Abs(currentTurnAngle) > Math.Abs(highestAngle)) { highestAngle = currentTurnAngle; }
            }

            pacenote.CornerSeverity = Geomath.CalculateCornerSeverity(highestAngle);
            if (Math.Abs(highestAngle) <= CornerThresholds.Straight) { pacenote.IsStraight = true; }

            return pacenote;
        }

        private List<Node> AddPreviousNodeToCorner(List<Node> corner, List<List<Node>> allCorners)
        {
            int cornerIndexInAllCorners = 0;
            for (int i = 0; i < allCorners.Count - 1; i++)
            {
                if (allCorners[i] == corner) { cornerIndexInAllCorners = i; break; }
            }

            if (cornerIndexInAllCorners > 0 && cornerIndexInAllCorners < allCorners.Count)
            {
                corner.Insert(0, allCorners[cornerIndexInAllCorners - 1].Last());
            }

            else { Console.WriteLine("Corner is at the edge"); }

            return corner;
        }

        //classifies which way a corner turns
        private Direction? ClassifyCornerDirection(List<Node> corner)
        {
            if (corner[1].Direction == Direction.Left) { return Direction.Left; }
            else if (corner[1].Direction == Direction.Right) {return Direction.Right; }
            return null;
        }

        
        //calculates the length of a straight by adding up the individual distances between all nodes in the straight
        private double CalculateStraightLength(List<Node> straightNodes)
        {
            double distance = 0;
            for (int i = 1; i < straightNodes.Count; i++)
            {
                distance += Geomath.GetDistance(straightNodes[i - 1], straightNodes[i]);
            }

            return distance;
        }
        
    }
}
