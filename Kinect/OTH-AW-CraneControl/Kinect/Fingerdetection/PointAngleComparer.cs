using System.Collections.Generic;

namespace Kinect.Fingerdetection
{
    internal class PointAngleComparer : IComparer<DepthPointEx>
    {
        private readonly DepthPointEx p0;

        public PointAngleComparer(DepthPointEx zeroPoint)
        {
            p0 = zeroPoint;
        }

        public int Compare(DepthPointEx p1, DepthPointEx p2)
        {
            if (p1.Equals(p2)) return 0;

            var value = Compare(p0, p1, p2);

            if (value == 0) return 0;
            if (value < 0) return 1;
            return -1;
        }

        public static float Compare(DepthPointEx p0, DepthPointEx p1, DepthPointEx p2)
        {
            return (p1.X - p0.X) * (p2.Y - p0.Y) - (p2.X - p0.X) * (p1.Y - p0.Y);
        }
    }
}