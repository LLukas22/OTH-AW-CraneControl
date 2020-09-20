import tkinter as tk
from Modules.NameService import Icons
from Modules.NameService import HandStates
from tkinter.ttk import *
import cv2
import numpy as np




class Model(object):
    """Model to hold all data"""
    def __init__(self, port, resolution,framerate,delayTime,threshold,labels):
        self.resolution = resolution
        self.client = 0
        self.port = port
        self.labels = labels
        self.threshold = threshold
        self.down = False
        self.up = False
        self.left = False
        self.right = False
        self.regler =False
        self.power = False
        self.rightVelocity = 0
        self.LeftVelocity = 0
        self.boxes = None
        self.classes = None
        self.scores = None
        self.frame = None
        self.fps = 0
        self.serverActive = True
        self.tfActive = True
        self.framerate = framerate
        self.handState = HandStates.No
        
    @property
    def delayTime(self):
        return (int)(1/self.framerate * 1000)
    
    @property
    def imageHeight(self):
        return self.resolution[1]
    
    @property
    def imageWidth(self):
        return self.resolution[0]
    
    @property
    def frameWithDetections(self):
        if(self.frame is None):
            return np.zeros((self.imageHeight,self.imageWidth,3), np.uint8)
        detectionframe = self.frame.copy()
        try:
            if self.boxes.any() and self.classes.any() and self.scores.any():
                for i in range(len(self.scores)):
                    if ((self.scores[i] >  self.threshold) and (self.scores[i] <= 1.0)):

                        # Get bounding box coordinates and draw box
                        # Interpreter can return coordinates that are outside of image dimensions, need to force them to be within image using max() and min()
                        ymin = int(max(1, (self.boxes[i][0] * self.imageHeight)))
                        xmin = int(max(1, (self.boxes[i][1] * self.imageWight)))
                        ymax = int(
                            min(self.imageHeight, (self.boxes[i][2] * self.imageHeight)))
                        xmax = int(
                            min(self.imageWidth, (self.boxes[i][3] * self.imageWidth)))

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
    
    
    @property
    def frameRGBA(self):
        return cv2.cvtColor(self.frameWithDetections,cv2.COLOR_BGR2RGBA)