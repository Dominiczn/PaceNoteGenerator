using System;
using System.Collections.Generic;
using System.Text;

namespace Pace_Note_Generator.Backend
{
    public class Pacenote
    {
        public int PacenoteId;
        public Direction Direction;
        public CornerSeverity CornerSeverity;
        public int StraightLength;
        public bool IsLong;
        public bool Tightens;
        public bool Widens;
        public List<string> Hazards = new();
    }
}
