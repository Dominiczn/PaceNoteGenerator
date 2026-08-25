using System;
using System.Collections.Generic;
using System.Text;

namespace Pace_Note_Generator
{
    public static class Geomath
    {
        //converts the coordinates of point2 from EPSG:4326 as used by OSRM EPSG:4087 (Equirectangular Projection) to be used for maths. the coordinates of point1 are set a 0,0, and point2 is relative to point1
        public static (double, double) ConvertToEquirectangular(double lat1, double lon1, double lat2, double lon2)
        {
            const int R = 6371009; //R is the mean radius of Earth in metres (WGS 84)

            //finds difference in latitude, longitude, and finds average latitude
            double deltaLat = ToRadians(lat1 - lat2);
            double deltaLon = ToRadians(lon1 - lon2);
            double avgLat = ToRadians((lat1 + lat2)/2);

            //converts the EPSG:4326 coordinates to EPSG4087 with cosine approximation
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
        public static double GetDistance(double lat1, double lon1, double lat2, double lon2)
        {
            (double x, double y) = Geomath.ConvertToEquirectangular(lat1, lon1, lat2, lon2);
            return Geomath.Pythagoras(x, y);
        }

        //calculates the radius of the circumcircle using the distances between points a, b, and c 
        public static double CircumcircleRadius(double a, double b, double c)
        {
            return (a * b * c) / (Math.Sqrt((a + b + c) * (b + c - a) * (c + a - b) * (a + b - c)));
        }

        //calculates the 2D cross product of the 3 nodes to figure out where a corner is and which way it turns
        public static double CalculateTurnAngle(double lat1, double lon1, double lat2, double lon2, double lat3, double lon3)
        {
            (double ABx, double ABy) = Geomath.ConvertToEquirectangular(lat1, lon1, lat2, lon2);
            (double BCx, double BCy) = Geomath.ConvertToEquirectangular(lat2, lon2, lat3, lon3);

            //finds the heading angle of each vector using arctan2
            double angleAB = Math.Atan2(ABy, ABx);
            double angleBC = Math.Atan2(BCy, BCx);

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
