using System;
using System.Collections.Generic;
using System.Text;

namespace Pace_Note_Generator.Backend.Utils
{
    public static class CornerThresholds
    {
        public const double ComfortableCorneringGForce = 0.3;

        //these are the minimum speeds in m/s for the corner (except One as that is the maximum and anything below 6m/s is a hairpin
        public const int Straight = 27;
        public const int Six = 22;
        public const int Five = 18;
        public const int Four = 14;
        public const int Three = 11;
        public const int Two = 8;
        public const int One = 6;
    }
}
