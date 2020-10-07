using System;
using System.Collections.Specialized;
using System.Linq;
using System.Numerics;
using Microsoft.Kinect;

namespace Kinect.Buffers
{
    public class HandGesturesBuffer : Buffer<HandState>
    {
        private bool directionLock;
        private int bufferWaitSize;
        private double filter = 5.0 / 3.0;

        public HandGesturesBuffer(int threshhould, int bufferWaitSize, double filter) : base(threshhould)
        {
            this.bufferWaitSize = bufferWaitSize;
            this.filter = filter;
        }

        private int count => Values.Count;

        public HandGestureDirections CalculateHandGestureDirections(Vector3 vector, HandState currentState)
        {
            if (count < 2) return HandGestureDirections.None;
            if (directionLock)
            {
                if (currentState == HandState.Lasso || Values.ElementAt(count - 1) == HandState.Lasso ||
                    Values.ElementAt(count - 2) == HandState.Lasso)
                    return returnHandGestureDirections(vector.X, vector.Y, filter);
                directionLock = false;
            }
            else
            {
                if (currentState == HandState.Lasso && Values.ElementAt(count - 1) == HandState.Lasso)
                {
                    directionLock = true;
                    return returnHandGestureDirections(vector.X, vector.Y, filter);
                }
            }

            return HandGestureDirections.None;
        }


        public bool CalculateOpenHandGestureDirections(HandState curreHandState)
        {
            if (count > bufferWaitSize)
            {


                if (Values.GetRange(count - bufferWaitSize, bufferWaitSize).Any(x => x != HandState.Closed))
                {
                    return false;
                }
                else
                {
                    if (Values.GetRange(0, count - bufferWaitSize).Any(x => x == HandState.Closed))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }

                }
            }
            return false;
        }

        private HandGestureDirections returnHandGestureDirections(float x, float y,double filter)
        {
            double a = x / y;
            double b = y / x;
            if (a > filter || b > filter)
            {
                if (Math.Abs(x) > Math.Abs(y))
                {
                    if (x > 0) //right
                        return HandGestureDirections.Right;
                    return HandGestureDirections.Left;
                }

                if (y > 0) //up
                    return HandGestureDirections.Up;
                return HandGestureDirections.Down;
            }

            return HandGestureDirections.None;

        }
    }
}