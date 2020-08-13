import cv2
import numpy as np
import os
import sys
from threading import Thread
import time
import platform

defaultVideoDevice = 0

def returnVideoStream(self):
    if(any(platform.win32_ver())):
        return cv2.VideoCapture(defaultVideoDevice  + cv2.CAP_DSHOW)
    else:
        return cv2.VideoCapture(defaultVideoDevice)

class VideoStream(object):
    """Background Thread to draw detections and show Videostream"""

    def __init__(self, labels, resolution, framerate):
        self.frametiming = 1/framerate
        self.IsActive = True
        self.stream = returnVideoStream(self)
        ret = self.stream.set(3, resolution[0])
        ret = self.stream.set(4, resolution[1])
        (self.status, self.frame) = self.stream.read()
        self.thread = Thread(target=self.update, args=())
        self.thread.daemon = True
        self.thread.start()
        self.labels = labels
        self.boxes = None
        self.classes = None
        self.scores = None
        self.imageHeight = resolution[1]
        self.imageWight = resolution[0]
        self.fps = (float)(1)
        

    def update(self):
        while self.IsActive:
            if self.stream.isOpened():
                (self.status, self.frame) = self.stream.read()
            time.sleep(self.frametiming)

    def read(self):
        return self.frame

    def stop(self):
        self.IsActive = False
        self.stream.release()


    def drawDetection(self):
        if(self.frame is None):
            return np.zeros((self.imageHeight,self.imageWight,3), np.uint8)
        detectionframe = self.frame.copy()
        try:
            if self.boxes.any() and self.classes.any() and self.scores.any():
                for i in range(len(self.scores)):
                    if ((self.scores[i] > 0.6) and (self.scores[i] <= 1.0)):

                        # Get bounding box coordinates and draw box
                        # Interpreter can return coordinates that are outside of image dimensions, need to force them to be within image using max() and min()
                        ymin = int(max(1, (self.boxes[i][0] * self.imageHeight)))
                        xmin = int(max(1, (self.boxes[i][1] * self.imageWight)))
                        ymax = int(
                            min(self.imageHeight, (self.boxes[i][2] * self.imageHeight)))
                        xmax = int(
                            min(self.imageWight, (self.boxes[i][3] * self.imageWight)))

                        cv2.rectangle(detectionframe, (xmin, ymin),
                                      (xmax, ymax), (10, 255, 0), 2)

                        # Draw label
                        # Look up object name from "labels" array using class index
                        object_name = self.labels[int(self.classes[i])]
                        label = '%s: %d%%' % (object_name, int(
                            self.scores[i]*100))  # Example: 'person: 72%'
                        labelSize, baseLine = cv2.getTextSize(
                            label, cv2.FONT_HERSHEY_SIMPLEX, 0.7, 2)
                        # Make sure not to draw label too close to top of window
                        label_ymin = max(ymin, labelSize[1] + 10)
                        # Draw white box to put label text in
                        cv2.rectangle(detectionframe, (xmin, label_ymin-labelSize[1]-10), (
                            xmin+labelSize[0], label_ymin+baseLine-10), (255, 255, 255), cv2.FILLED)
                        # Draw label text
                        cv2.putText(detectionframe, label, (xmin, label_ymin-7),
                                    cv2.FONT_HERSHEY_SIMPLEX, 0.7, (0, 0, 0), 2)
                # Draw framerate in corner of frame
            cv2.putText(detectionframe, 'FPS: {0:.2f}'.format(self.fps), (30, 50),
                        cv2.FONT_HERSHEY_SIMPLEX, 1, (255, 255, 0), 2, cv2.LINE_AA)
        except AttributeError:
            pass

        return detectionframe

    def updateDetection(self, boxes, classes, scores, fps):
        self.boxes = boxes
        self.classes = classes
        self.scores = scores
        self.fps = fps