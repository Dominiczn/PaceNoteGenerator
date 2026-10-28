using Pace_Note_Generator.Backend.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pace_Note_Generator.Backend
{
    public class Pacenote
    {
        public Direction? Direction;
        public CornerSeverity? CornerSeverity;
        public PacenoteType Type;
        public double? StraightLength;
        public bool IsLong;
        public bool Tightens;
        public bool Widens;
        public List<string> Hazards = new();
    }
}
