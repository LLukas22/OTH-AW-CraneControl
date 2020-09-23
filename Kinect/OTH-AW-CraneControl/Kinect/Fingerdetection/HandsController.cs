using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Kinect;

namespace Kinect.Fingerdetection
{
    /// <summary>
    ///     Detects human hands in the 3D and 2D space.
    /// </summary>
    public class HandsController
    {
        private readonly GrahamScan _grahamScan = new GrahamScan();
        private readonly PointFilter _lineThinner = new PointFilter();
        private readonly int DEFAULT_DEPTH_HEIGHT = 424;
        private readonly int DEFAULT_DEPTH_WIDTH = 512;
        private readonly float DEPTH_THRESHOLD = 80; // 8cm
        private readonly ushort MAX_DEPTH = ushort.MaxValue;
        private readonly ushort MIN_DEPTH = 500;

        private byte[] _handPixelsLeft;
        private byte[] _handPixelsRight;

        /// <summary>
        ///     Creates a new instance of <see cref="HandsController" />.
        /// </summary>
        public HandsController()
        {
            CoordinateMapper = KinectSensor.GetDefault().CoordinateMapper;
            DetectLeftHand = true;
            DetectRightHand = true;
        }

        /// <summary>
        ///     Creates a new instance of <see cref="HandsController" /> with the specified coordinate mapper.
        /// </summary>
        /// <param name="coordinateMapper">The coordinate mapper that will be used during the finger detection process.</param>
        public HandsController(CoordinateMapper coordinateMapper)
        {
            CoordinateMapper = coordinateMapper;
        }

        /// <summary>
        ///     The width of the depth frame.
        /// </summary>
        public int DepthWidth { get; set; }

        /// <summary>
        ///     the height of the depth frame.
        /// </summary>
        public int DepthHeight { get; set; }

        /// <summary>
        ///     The coordinate mapper that will be used during the finger detection process.
        /// </summary>
        public CoordinateMapper CoordinateMapper { get; set; }

        /// <summary>
        ///     Determines whether the algorithm will detect the left hand.
        /// </summary>
        public bool DetectLeftHand { get; set; }

        /// <summary>
        ///     Determines whether the algorithm will detect the right hand.
        /// </summary>
        public bool DetectRightHand { get; set; }

        /// <summary>
        ///     Raised when a new pair of hands is detected.
        /// </summary>
        public event EventHandler<HandCollection> HandsDetected;

        /// <summary>
        ///     Updates the finger-detection engine with the new data.
        /// </summary>
        /// <param name="data">An array of depth values.</param>
        /// <param name="body">The body to search for hands and fingers.</param>
        public unsafe void Update(ushort[] data, Body body)
        {
            fixed (ushort* frameData = data)
            {
                Update(frameData, body);
            }
        }

        /// <summary>
        ///     Updates the finger-detection engine with the new data.
        /// </summary>
        /// <param name="data">An IntPtr that describes depth values.</param>
        /// <param name="body">The body to search for hands and fingers.</param>
        public unsafe void Update(IntPtr data, Body body)
        {
            var frameData = (ushort*)data;

            Update(frameData, body);
        }

        /// <summary>
        ///     Updates the finger-detection engine with the new data.
        /// </summary>
        /// <param name="data">A pointer to an array of depth data.</param>
        /// <param name="body">The body to search for hands and fingers.</param>
        public unsafe void Update(ushort* data, Body body)
        {
            if (data == null || body == null) return;

            if (DepthWidth == 0) DepthWidth = DEFAULT_DEPTH_WIDTH;

            if (DepthHeight == 0) DepthHeight = DEFAULT_DEPTH_HEIGHT;

            if (_handPixelsLeft == null) _handPixelsLeft = new byte[DepthWidth * DepthHeight];

            if (_handPixelsRight == null) _handPixelsRight = new byte[DepthWidth * DepthHeight];

            Hand handLeft = null;
            Hand handRight = null;

            var jointHandLeft = body.Joints[JointType.HandLeft];
            var jointHandRight = body.Joints[JointType.HandRight];
            var jointWristLeft = body.Joints[JointType.WristLeft];
            var jointWristRight = body.Joints[JointType.WristRight];
            var jointTipLeft = body.Joints[JointType.HandTipLeft];
            var jointTipRight = body.Joints[JointType.HandTipRight];
            var jointThumbLeft = body.Joints[JointType.ThumbLeft];
            var jointThumbRight = body.Joints[JointType.ThumbRight];

            var depthPointHandLeft = CoordinateMapper.MapCameraPointToDepthSpace(jointHandLeft.Position);
            var depthPointWristLeft = CoordinateMapper.MapCameraPointToDepthSpace(jointWristLeft.Position);
            var depthPointTipLeft = CoordinateMapper.MapCameraPointToDepthSpace(jointTipLeft.Position);
            var depthPointThumbLeft = CoordinateMapper.MapCameraPointToDepthSpace(jointThumbLeft.Position);

            var depthPointHandRight = CoordinateMapper.MapCameraPointToDepthSpace(jointHandRight.Position);
            var depthPointWristRight = CoordinateMapper.MapCameraPointToDepthSpace(jointWristRight.Position);
            var depthPointTipRight = CoordinateMapper.MapCameraPointToDepthSpace(jointTipRight.Position);
            var depthPointThumbRight = CoordinateMapper.MapCameraPointToDepthSpace(jointThumbRight.Position);

            var handLeftX = depthPointHandLeft.X;
            var handLeftY = depthPointHandLeft.Y;
            var wristLeftX = depthPointWristLeft.X;
            var wristLeftY = depthPointWristLeft.Y;
            var tipLeftX = depthPointTipLeft.X;
            var tipLeftY = depthPointTipLeft.Y;
            var thumbLeftX = depthPointThumbLeft.X;
            var thumbLeftY = depthPointThumbLeft.Y;

            var handRightX = depthPointHandRight.X;
            var handRightY = depthPointHandRight.Y;
            var wristRightX = depthPointWristRight.X;
            var wristRightY = depthPointWristRight.Y;
            var tipRightX = depthPointTipRight.X;
            var tipRightY = depthPointTipRight.Y;
            var thumbRightX = depthPointThumbRight.X;
            var thumbRightY = depthPointThumbRight.Y;

            var searchForLeftHand = DetectLeftHand && !float.IsInfinity(handLeftX) && !float.IsInfinity(handLeftY) &&
                                    !float.IsInfinity(wristLeftX) && !float.IsInfinity(wristLeftY) &&
                                    !float.IsInfinity(tipLeftX) && !float.IsInfinity(tipLeftY) &&
                                    !float.IsInfinity(thumbLeftX) && !float.IsInfinity(thumbLeftY);
            var searchForRightHand = DetectRightHand && !float.IsInfinity(handRightX) &&
                                     !float.IsInfinity(handRightY) && !float.IsInfinity(wristRightX) &&
                                     !float.IsInfinity(wristRightY) && !float.IsInfinity(tipRightX) &&
                                     !float.IsInfinity(tipRightY) && !float.IsInfinity(thumbRightX) &&
                                     !float.IsInfinity(thumbRightY);

            if (searchForLeftHand || searchForRightHand)
            {
                var distanceLeft = searchForLeftHand
                    ? CalculateDistance(handLeftX, handLeftY, tipLeftX, tipLeftY, thumbLeftX, thumbLeftY)
                    : 0.0;
                var distanceRight = searchForRightHand
                    ? CalculateDistance(handRightX, handRightY, tipRightX, tipRightY, thumbRightX, thumbRightY)
                    : 0.0;

                var angleLeft = searchForLeftHand
                    ? DepthPointEx.Angle(wristLeftX, wristLeftY, wristLeftX, 0, handLeftX, handLeftY)
                    : 0.0;
                var angleRight = searchForRightHand
                    ? DepthPointEx.Angle(wristRightX, wristRightY, wristRightX, 0, handRightX, handRightY)
                    : 0.0;

                var minLeftX = searchForLeftHand ? (int)(handLeftX - distanceLeft) : 0;
                var minLeftY = searchForLeftHand ? (int)(handLeftY - distanceLeft) : 0;
                var maxLeftX = searchForLeftHand ? (int)(handLeftX + distanceLeft) : 0;
                var maxLeftY = searchForLeftHand ? (int)(handLeftY + distanceLeft) : 0;

                var minRightX = searchForRightHand ? (int)(handRightX - distanceRight) : 0;
                var minRightY = searchForRightHand ? (int)(handRightY - distanceRight) : 0;
                var maxRightX = searchForRightHand ? (int)(handRightX + distanceRight) : 0;
                var maxRightY = searchForRightHand ? (int)(handRightY + distanceRight) : 0;

                var depthLeft = jointHandLeft.Position.Z * 1000; // m to mm
                var depthRight = jointHandRight.Position.Z * 1000;

                for (var i = 0; i < DepthWidth * DepthHeight; ++i)
                {
                    var depth = data[i];

                    var depthX = i % DepthWidth;
                    var depthY = i / DepthWidth;

                    var isInBounds = depth >= MIN_DEPTH && depth <= MAX_DEPTH;

                    var conditionLeft = depth >= depthLeft - DEPTH_THRESHOLD &&
                                        depth <= depthLeft + DEPTH_THRESHOLD &&
                                        depthX >= minLeftX && depthX <= maxLeftX &&
                                        depthY >= minLeftY && depthY <= maxLeftY;

                    var conditionRight = depth >= depthRight - DEPTH_THRESHOLD &&
                                         depth <= depthRight + DEPTH_THRESHOLD &&
                                         depthX >= minRightX && depthX <= maxRightX &&
                                         depthY >= minRightY && depthY <= maxRightY;

                    _handPixelsLeft[i] = (byte)(isInBounds && searchForLeftHand && conditionLeft ? 255 : 0);
                    _handPixelsRight[i] = (byte)(isInBounds && searchForRightHand && conditionRight ? 255 : 0);
                }

                var contourLeft = new List<DepthPointEx>();
                var contourRight = new List<DepthPointEx>();

                for (var i = 0; i < DepthWidth * DepthHeight; ++i)
                {
                    var depth = data[i];

                    var depthX = i % DepthWidth;
                    var depthY = i / DepthWidth;

                    if (searchForLeftHand)
                        if (_handPixelsLeft[i] != 0)
                        {
                            var top = i - DepthWidth >= 0 ? _handPixelsLeft[i - DepthWidth] : (byte)0;
                            var bottom = i + DepthWidth < _handPixelsLeft.Length
                                ? _handPixelsLeft[i + DepthWidth]
                                : (byte)0;
                            var left = i - 1 >= 0 ? _handPixelsLeft[i - 1] : (byte)0;
                            var right = i + 1 < _handPixelsLeft.Length ? _handPixelsLeft[i + 1] : (byte)0;

                            var isInContour = top == 0 || bottom == 0 || left == 0 || right == 0;

                            if (isInContour) contourLeft.Add(new DepthPointEx { X = depthX, Y = depthY, Z = depth });
                        }

                    if (searchForRightHand)
                        if (_handPixelsRight[i] != 0)
                        {
                            var top = i - DepthWidth >= 0 ? _handPixelsRight[i - DepthWidth] : (byte)0;
                            var bottom = i + DepthWidth < _handPixelsRight.Length
                                ? _handPixelsRight[i + DepthWidth]
                                : (byte)0;
                            var left = i - 1 >= 0 ? _handPixelsRight[i - 1] : (byte)0;
                            var right = i + 1 < _handPixelsRight.Length ? _handPixelsRight[i + 1] : (byte)0;

                            var isInContour = top == 0 || bottom == 0 || left == 0 || right == 0;

                            if (isInContour) contourRight.Add(new DepthPointEx { X = depthX, Y = depthY, Z = depth });
                        }
                }

                if (searchForLeftHand)
                    handLeft = GetHand(body.TrackingId, body.HandLeftState, contourLeft, angleLeft, wristLeftX,
                        wristLeftY);

                if (searchForRightHand)
                    handRight = GetHand(body.TrackingId, body.HandRightState, contourRight, angleRight, wristRightX,
                        wristRightY);
            }

            if (handLeft != null || handRight != null)
            {
                var hands = new HandCollection
                {
                    TrackingId = body.TrackingId,
                    HandLeft = handLeft,
                    HandRight = handRight
                };

                if (HandsDetected != null) HandsDetected(this, hands);
            }
        }

        private double CalculateDistance(float handLeftX, float handLeftY, float tipLeftX, float tipLeftY,
            float thumbLeftX, float thumbLeftY)
        {
            var distanceLeftHandTip =
                Math.Sqrt(Math.Pow(tipLeftX - handLeftX, 2) + Math.Pow(tipLeftY - handLeftY, 2)) * 2;
            var distanceLeftHandThumb =
                Math.Sqrt(Math.Pow(thumbLeftX - handLeftX, 2) + Math.Pow(thumbLeftY - handLeftY, 2)) * 2;

            return Math.Max(distanceLeftHandTip, distanceLeftHandThumb);
        }

        private Hand GetHand(ulong trackingID, HandState state, List<DepthPointEx> contour, double angle, float wristX,
            float wristY)
        {
            var convexHull = _grahamScan.ConvexHull(contour);
            var filtered = _lineThinner.Filter(convexHull);
            IList<DepthPointEx> fingers = new List<DepthPointEx>();

            if (angle > -90.0 && angle < 30.0)
                // Hand "up".
                fingers = filtered.Where(p => p.Y < wristY).Take(5).ToList();
            else if (angle >= 30.0 && angle < 90.0)
                // Thumb below wrist (sometimes).
                fingers = filtered.Where(p => p.X > wristX).Take(5).ToList();
            else if (angle >= 90.0 && angle < 180.0)
                fingers = filtered.Where(p => p.Y > wristY).Take(5).ToList();
            else
                fingers = filtered.Where(p => p.X < wristX).Take(5).ToList();

            if (contour.Count > 0 && fingers.Count > 0)
                return new Hand(trackingID, state, contour, fingers, CoordinateMapper);

            return null;
        }
    }
}