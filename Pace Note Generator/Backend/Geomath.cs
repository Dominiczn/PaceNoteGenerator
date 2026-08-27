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

            //finds difference in latitude, longitude, and finds average latitude
            double deltaLat = ToRadians(node1.Latitude - node2.Latitude);
            double deltaLon = ToRadians(node1.Longitude - node2.Longitude);
            double avgLat = ToRadians((node1.Latitude + node2.Latitude)/2);

            //converts the above deltas into a local equirectangular approximation
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
        public static double CircumcircleRadius(double a, double b, double c)
        {
            return (a * b * c) / (Math.Sqrt((a + b + c) * (b + c - a) * (c + a - b) * (a + b - c)));
        }

        //calculates the 2D cross product of the 3 nodes to figure out where a corner is and which way it turns
        public static double CalculateTurnAngle(Node node1, Node node2, Node node3)
        {
            (double BAx, double ABy) = Geomath.ConvertToEquirectangular(node1, node2);
            (double CBx, double BCy) = Geomath.ConvertToEquirectangular(node2, node3);

            //finds the heading angle of each vector using arctan2
            double angleAB = Math.Atan2(ABy, BAx);
            double angleBC = Math.Atan2(BCy, CBx);

            //calculates the difference between the two angles
            double angleDiff = angleBC - angleAB;

            //normalises the angle to keep it between -PI and PI (-180° to 180°)
            if (angleDiff > Math.PI) angleDiff -= 2 * Math.PI;
            if (angleDiff < -Math.PI) angleDiff += 2 * Math.PI;

            //converts radians to degrees for easy reading
            return angleDiff * (180.0 / Math.PI);

        }
    }
}
