using System;
using System.Collections.Generic;
using System.Text;

namespace Pace_Note_Generator.Backend
{
    public struct Pacenote
    {
        public Direction? direction;
        public CornerSeverity? cornerSeverity;
        public int? straight;
        public bool? isLong;
        public bool? tightens;
        public bool? widens;
        public string? hazard;
    }
}
