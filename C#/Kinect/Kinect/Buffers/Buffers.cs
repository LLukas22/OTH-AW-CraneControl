using System.Collections.Generic;
using System.Linq;

namespace Kinect.Buffers
{
    public class Buffer<TValue>
    {
        private readonly int threshhould;

        public Buffer(int threshhould)
        {
            this.threshhould = threshhould;
            Values = new List<TValue>();
        }

        public List<TValue> Values { get; set; }

        public void Add(TValue value)
        {
            Values.Add(value);
            if (Values.Count > threshhould) Values.Remove(Values.First());
        }
    }
}