using System.ComponentModel;

namespace Kinect
{
    public enum Frames
    {
        [Description("Infrared")] ir = 0,
        [Description("Color")] color = 1,
        [Description("depth")] depth = 2
    }
}