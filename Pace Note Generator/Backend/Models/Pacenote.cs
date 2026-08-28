using System;
using System.Collections.Generic;
using System.Text;

namespace Pace_Note_Generator.Backend
{
    public struct Pacenote
    {
        public Direction? Direction;
        public CornerSeverity? CornerSeverity;
        public bool IsStraight;
        public double? StraightLength;
        public bool IsLong;
        public bool Tightens;
        public bool Widens;
        public string? Hazard;

        public Pacenote()
        {
            IsStraight = false;
            IsLong = false;
            Tightens = false;
            Widens = false;
        }
    }
}
