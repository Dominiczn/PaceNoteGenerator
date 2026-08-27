using System;
using System.Collections.Generic;
using System.Text;

namespace Pace_Note_Generator.Backend
{
    public struct Pacenote
    {
        public Direction? direction;
        public CornerSeverity? cornerSeverity;
        public bool isStraight;
        public int? straightLength;
        public bool isLong;
        public bool tightens;
        public bool widens;
        public string? hazard;

        public Pacenote()
        {
            isStraight = false;
            isLong = false;
            tightens = false;
            widens = false;
        }
    }
}
