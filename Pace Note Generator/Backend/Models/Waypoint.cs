using GMap.NET.WindowsPresentation;
using Pace_Note_Generator.Backend.Enums_and_Structs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pace_Note_Generator.Backend
{
    public class Waypoint : Node
    {
        public WaypointType Type; //this refers to start, checkpoint, or end
        public GMapMarker? Marker;
        public Waypoint(double lat, double lon, WaypointType type) : base(lat, lon)
        {
            Type = type;
        }
    }
}
