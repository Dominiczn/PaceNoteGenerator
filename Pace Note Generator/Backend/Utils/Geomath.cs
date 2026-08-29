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

        public static double ToDegrees(double num)
        {
            return (180 / Math.PI) * num;
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
        public static Direction CalculateNodesDirection(Node node1, Node node2, Node node3)
        {
            double crossProduct = CalculateCrossProuct(node1, node2, node3);

            if (crossProduct >= 0) { return Direction.Left; }
            else { return Direction.Right; }
        }

        public static CornerSeverity? ClassifyCornerSeverity(double cornerAngle)
        {
            CornerSeverity? severity = null;
            switch(cornerAngle)
            {
                case var a when Math.Abs(a) <= CornerThresholds.Straight:
                    severity = CornerSeverity.Straight;
                    break;

                case var a when Math.Abs(a) > CornerThresholds.Straight && Math.Abs(a) <= CornerThresholds.Six:
                    severity = CornerSeverity.Six;
                    break;

                case var a when Math.Abs(a) > CornerThresholds.Six && Math.Abs(a) <= CornerThresholds.Five:
                    severity = CornerSeverity.Five;
                    break;

                case var a when Math.Abs(a) > CornerThresholds.Five && Math.Abs(a) <= CornerThresholds.Four:
                    severity = CornerSeverity.Four;
                    break;

                case var a when Math.Abs(a) > CornerThresholds.Four && Math.Abs(a) <= CornerThresholds.Three:
                    severity = CornerSeverity.Three;
                    break;

                case var a when Math.Abs(a) > CornerThresholds.Three && Math.Abs(a) <= CornerThresholds.Square:
                    severity = CornerSeverity.Square;
                    break;

                case var a when Math.Abs(a) > CornerThresholds.Square && Math.Abs(a) <= CornerThresholds.Two:
                    severity = CornerSeverity.Two;
                    break;

                case var a when Math.Abs(a) > CornerThresholds.Two && Math.Abs(a) <= CornerThresholds.One:
                    severity = CornerSeverity.One;
                    break;

                case var a when Math.Abs(a) > CornerThresholds.One:
                    severity = CornerSeverity.Hairpin;
                    break;
            }

            return severity;
        }

        //calculates the 2D cross product of the 3 nodes to figure out where a corner is and which way it turns
        public static double CalculateCrossProuct(Node node1, Node node2, Node node3)
        {
            (double ABx, double ABy) = Geomath.ConvertToEquirectangular(node1, node2);
            (double BCx, double BCy) = Geomath.ConvertToEquirectangular(node2, node3);

            double crossProduct = (ABx * BCy) - (BCx * ABy);

            return crossProduct;
        }
    }
}
