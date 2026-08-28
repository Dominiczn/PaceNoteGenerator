using Pace_Note_Generator.Backend.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pace_Note_Generator.Backend
{
    public static class Geomath
    {
        //converts node1's coordinates to local equirectangular approximation in meters relative to node2
        public static (double, double) ConvertToEquirectangular(Node node1, Node node2)
        {
            const int R = 6371009; //R is the mean radius of Earth in metres (WGS 84)

            double deltaLat = ToRadians(node1.Latitude - node2.Latitude);
            double deltaLon = ToRadians(node1.Longitude - node2.Longitude);
            double avgLat = ToRadians((node1.Latitude + node2.Latitude)/2);

            double x = R * deltaLon * Math.Cos(avgLat);
            double y = R * deltaLat;

            return (x, y);
        }


        //both self explanatory
        public static double Pythagoras(double b, double c)
        {
            return Math.Sqrt((b * b) + (c * c));
        }
        public static double ToRadians(double num)
        {
            return (Math.PI / 180) * num;
        }


        //helper method so getting distance is only 1 method
        public static double GetDistance(Node node1, Node node2)
        {
            (double x, double y) = Geomath.ConvertToEquirectangular(node1, node2);
            return Geomath.Pythagoras(x, y);
        }

        //calculates the radius of the circumcircle using the distances between points a, b, and c 
        public static double CircumcircleRadius(Node node1, Node node2, Node node3)
        {
            double sideA = GetDistance(node1, node2);
            double sideB = GetDistance(node2, node3);
            double sideC = GetDistance(node1, node3);
            return (sideA * sideB * sideC) / (Math.Sqrt((sideA + sideB + sideC) * (sideB + sideC - sideA) * (sideC + sideA - sideB) * (sideA + sideB - sideC)));
        }

        //helper method so calculating the direction of a corner (3 nodes at a time) direction is only 1 method
        public static Direction? CalculateNodesDirection(Node node1, Node node2, Node node3)
        {
            double radius = Geomath.CalculateTurnAngle(node1, node2, node3);

            if (radius >= CornerThresholds.Straight) { return Direction.Left; }
            else if (radius <= -(CornerThresholds.Straight)) { return Direction.Right; }
            else { return null; }
        }

        public static CornerSeverity? CalculateCornerSeverity(double cornerAngle)
        {
            CornerSeverity? severity = null;
            switch(cornerAngle)
            {
                case var n when Math.Abs(n) <= CornerThresholds.Straight:
                    severity = CornerSeverity.Straight;
                    break;

                case var n when Math.Abs(n) > CornerThresholds.Straight && Math.Abs(n) <= CornerThresholds.Six:
                    severity = CornerSeverity.Six;
                    break;

                case var n when Math.Abs(n) > CornerThresholds.Six && Math.Abs(n) <= CornerThresholds.Five:
                    severity = CornerSeverity.Five;
                    break;

                case var n when Math.Abs(n) > CornerThresholds.Five && Math.Abs(n) <= CornerThresholds.Four:
                    severity = CornerSeverity.Four;
                    break;

                case var n when Math.Abs(n) > CornerThresholds.Four && Math.Abs(n) <= CornerThresholds.Three:
                    severity = CornerSeverity.Three;
                    break;

                case var n when Math.Abs(n) > CornerThresholds.Three && Math.Abs(n) <= CornerThresholds.Square:
                    severity = CornerSeverity.Square;
                    break;

                case var n when Math.Abs(n) > CornerThresholds.Square && Math.Abs(n) <= CornerThresholds.Two:
                    severity = CornerSeverity.Two;
                    break;

                case var n when Math.Abs(n) > CornerThresholds.Two && Math.Abs(n) <= CornerThresholds.One:
                    severity = CornerSeverity.One;
                    break;

                case var n when Math.Abs(n) > CornerThresholds.One:
                    severity = CornerSeverity.Hairpin;
                    break;
            }

            return severity;
        }

        //calculates the 2D cross product of the 3 nodes to figure out where a corner is and which way it turns
        public static double CalculateTurnAngle(Node node1, Node node2, Node node3)
        {
            (double BAx, double ABy) = Geomath.ConvertToEquirectangular(node1, node2);
            (double CBx, double BCy) = Geomath.ConvertToEquirectangular(node2, node3);

            double angleAB = Math.Atan2(ABy, BAx);
            double angleBC = Math.Atan2(BCy, CBx);

            double angleDiff = angleBC - angleAB;

            if (angleDiff > Math.PI) angleDiff -= 2 * Math.PI;
            if (angleDiff < -Math.PI) angleDiff += 2 * Math.PI;

            return angleDiff * (180.0 / Math.PI);

        }
    }
}
