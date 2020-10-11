private HandGestureDirections returnHandGestureDirections(float x, float y,double filter)
        {
            double a = x / y;
            double b = y / x;
            if (a > filter || b > filter)
            {
                if (Math.Abs(x) > Math.Abs(y))
                {
                    if (x > 0)
                        return HandGestureDirections.Right;
                    return HandGestureDirections.Left;
                }
                if (y > 0)
                    return HandGestureDirections.Up;
                return HandGestureDirections.Down;
            }
            return HandGestureDirections.None;
        }