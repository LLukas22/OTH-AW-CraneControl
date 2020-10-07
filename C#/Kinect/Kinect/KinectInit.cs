using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using Kinect.Buffers;
using Kinect.Fingerdetection;
using Microsoft.Kinect;
using Prism.Events;
using SharedRessources;

namespace Kinect
{
    public class KinectInIt
    {
        public delegate void KinectImageEventHandler(object sender, FrameEventArgs a);

        private const int bufferSize = 30;
        private const int bufferWaitSize = 10;
        private readonly IList<Body> bodies;
        private readonly BodyFrameReader bodyReader;
        private readonly DepthFrameReader depthReader;
        private readonly HandsController handsController;
        private readonly ColorFrameReader colorReader;

        private readonly InfraredFrameReader infraredReader;
        //private readonly ColorFrameReader colorReader;

        private readonly KinectSensor sensor;
        private Body body;

        public Bitmap kinectBitmap;

        static private readonly double  DirectionFilter = 5.0 / 3.0;
        private readonly Instructions kinectInstructions = new Instructions();
        private readonly HandGesturesBuffer leftHandBuffer = new HandGesturesBuffer(bufferSize, bufferWaitSize,DirectionFilter);

        private readonly HandGesturesBuffer rightHandBuffer = new HandGesturesBuffer(bufferSize, bufferWaitSize, DirectionFilter);

        public Frames Frame { get; set; }


        public KinectInIt(IEventAggregator eventAggregator, Frames f)
        {
            Frame = f;
            EventAggregator = eventAggregator;
            sensor = KinectSensor.GetDefault();

            if (sensor != null)
            {
                depthReader = sensor.DepthFrameSource.OpenReader();
                depthReader.FrameArrived += DepthReader_FrameArrived;

                infraredReader = sensor.InfraredFrameSource.OpenReader();
                infraredReader.FrameArrived += InfraredReader_FrameArrived;

                colorReader = sensor.ColorFrameSource.OpenReader();
                colorReader.FrameArrived += ColorReader_FrameArrived;

                bodyReader = sensor.BodyFrameSource.OpenReader();
                bodyReader.FrameArrived += BodyReader_FrameArrived;
                bodies = new Body[sensor.BodyFrameSource.BodyCount];

                // Initialize the HandsController and subscribe to the HandsDetected event.
                handsController = new HandsController();
                handsController.HandsDetected += HandsController_HandsDetected;

                sensor.Open();
            }
        }

        private IEventAggregator EventAggregator { get; }

        private void HandsController_HandsDetected(object sender, HandCollection e)
        {
            if (rightHandBuffer.CalculateOpenHandGestureDirections(body.HandRightState))
                kinectInstructions.controllerState = !kinectInstructions.controllerState;

            if (leftHandBuffer.CalculateOpenHandGestureDirections(body.HandLeftState))
                kinectInstructions.axisState = !kinectInstructions.axisState;


            if (body.HandRightState == HandState.Lasso || body.HandLeftState == HandState.Lasso)
            {
                var handTipLeftJoint = body.Joints[JointType.HandTipLeft];
                var handTipLeftJointVector = new Vector3(handTipLeftJoint.Position.X, handTipLeftJoint.Position.Y,
                    handTipLeftJoint.Position.Z);
                var handLeftJoint = body.Joints[JointType.HandLeft];
                var handLeftJointVector = new Vector3(handLeftJoint.Position.X, handLeftJoint.Position.Y,
                    handLeftJoint.Position.Z);

                var handLeft = Vector3.Subtract(handTipLeftJointVector, handLeftJointVector);


                var handTipRightJoint = body.Joints[JointType.HandTipRight];
                var handTipRightJointVector = new Vector3(handTipRightJoint.Position.X, handTipRightJoint.Position.Y,
                    handTipRightJoint.Position.Z);
                var handRightJoint = body.Joints[JointType.HandRight];
                var handRightJointVector = new Vector3(handRightJoint.Position.X, handRightJoint.Position.Y,
                    handRightJoint.Position.Z);

                var handRight = Vector3.Subtract(handTipRightJointVector, handRightJointVector);

                switch (rightHandBuffer.CalculateHandGestureDirections(handRight, body.HandRightState))
                {
                    case HandGestureDirections.Right:
                        kinectInstructions.rightState = true;
                        break;
                    case HandGestureDirections.Left:
                        kinectInstructions.leftState = true;
                        break;
                    case HandGestureDirections.Up:
                        kinectInstructions.upState = true;
                        break;
                    case HandGestureDirections.Down:
                        kinectInstructions.downState = true;
                        break;
                    default:
                        switch (leftHandBuffer.CalculateHandGestureDirections(handLeft, body.HandLeftState))
                        {
                            case HandGestureDirections.Right:
                                kinectInstructions.rightState = true;
                                break;
                            case HandGestureDirections.Left:
                                kinectInstructions.leftState = true;
                                break;
                            case HandGestureDirections.Up:
                                kinectInstructions.upState = true;
                                break;
                            case HandGestureDirections.Down:
                                kinectInstructions.downState = true;
                                break;
                        }

                        break;
                }
            }

            // Display the results!
            if (e.HandLeft != null)
            {
                // Draw contour.
                if (Frame == Frames.ir || Frame == Frames.depth)
                {
                    foreach (var point in e.HandLeft.ContourDepth) DrawEllipse(point, Brushes.Green, 2.0);
                }

                if (Frame == Frames.color)
                {
                    foreach (var point in e.HandLeft.ContourColor) DrawEllipse(point, Brushes.Green, 4.0);
                }


                // Draw fingers.
                foreach (var finger in e.HandLeft.Fingers) DrawEllipse(finger.DepthPoint, Brushes.White, 4.0);
            }


            if (e.HandRight != null)
            {
                // Draw contour.
                if (Frame == Frames.ir || Frame == Frames.depth)
                {
                    foreach (var point in e.HandRight.ContourDepth) DrawEllipse(point, Brushes.Green, 2.0);
                }

                if (Frame == Frames.color)
                {
                    foreach (var point in e.HandRight.ContourColor) DrawEllipse(point, Brushes.Green, 4.0);
                }

                // Draw fingers.
                foreach (var finger in e.HandRight.Fingers) DrawEllipse(finger.DepthPoint, Brushes.White, 4.0);
            }

            EventAggregator.GetEvent<FrameEvent>().Publish(new FrameEventArgs(kinectBitmap, kinectInstructions));
            leftHandBuffer.Add(body.HandLeftState);
            rightHandBuffer.Add(body.HandRightState);
        }

        private void BodyReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            kinectInstructions.resetDirections();
            using (var bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    bodyFrame.GetAndRefreshBodyData(bodies);

                    body = bodies.Where(b => b.IsTracked).FirstOrDefault();
                }
            }
        }

        private void ColorReader_FrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
            if (Frame == Frames.color)
            {
                using (var frame = e.FrameReference.AcquireFrame())
                {
                    kinectBitmap = frame.ReturnBitmap();
                    if (frame != null && body == null)
                        EventAggregator.GetEvent<FrameEvent>()
                            .Publish(new FrameEventArgs(frame.ReturnBitmap(), kinectInstructions));
                }
            }
        }

        private void InfraredReader_FrameArrived(object sender, InfraredFrameArrivedEventArgs e)
        {
            if (Frame == Frames.ir)
            {
                using (var frame = e.FrameReference.AcquireFrame())
                {
                    kinectBitmap = frame.ReturnBitmap();
                    if (frame != null && body == null)
                        EventAggregator.GetEvent<FrameEvent>()
                            .Publish(new FrameEventArgs(frame.ReturnBitmap(), kinectInstructions));
                }
            }
        }

        private void DepthReader_FrameArrived(object sender, DepthFrameArrivedEventArgs e)
        {
            using (var frame = e.FrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    // 2) Update the HandsController using the array (or pointer) of the depth depth data, and the tracked body.
                    using (var buffer = frame.LockImageBuffer())
                    {
                        handsController.Update(buffer.UnderlyingBuffer, body);
                    }

                    if (Frame == Frames.depth)
                    {
                        kinectBitmap = frame.ReturnBitmap();
                        if (frame != null && body == null)
                            EventAggregator.GetEvent<FrameEvent>()
                                .Publish(new FrameEventArgs(frame.ReturnBitmap(), kinectInstructions));
                    }
                }
                if (body == null) kinectInstructions.resetDirections();
            }
        }

        private void DrawEllipse(DepthSpacePoint point, Brush brush, double radius)
        {
            if (kinectBitmap == null) return;
            using (var g = Graphics.FromImage(kinectBitmap))
            {
                using (var pen = new Pen(brush, 1))
                {
                    g.DrawEllipse(pen, new Rectangle((int)point.X, (int)point.Y, (int)radius, (int)radius));
                }
            }
        }

        private void DrawEllipse(ColorSpacePoint point, Brush brush, double radius)
        {
            if (kinectBitmap == null) return;
            using (var g = Graphics.FromImage(kinectBitmap))
            {
                using (var pen = new Pen(brush, 1))
                {
                    g.DrawEllipse(pen, new Rectangle((int)point.X, (int)point.Y, (int)radius, (int)radius));
                }
            }
        }

        public void Stop()
        {
            sensor.Close();
        }
    }
}