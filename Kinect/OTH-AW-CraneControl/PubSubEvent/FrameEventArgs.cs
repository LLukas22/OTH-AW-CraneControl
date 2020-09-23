using System.Drawing;

namespace SharedRessources
{
    public class FrameEventArgs
    {
        public FrameEventArgs(Bitmap bitmap, Instructions instructions)
        {
            Bitmap = bitmap;
            Instructions = instructions;
        }

        public Bitmap Bitmap { get; }
        public Instructions Instructions { get; }
    }
}