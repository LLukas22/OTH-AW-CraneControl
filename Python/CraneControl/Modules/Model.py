import tkinter as tk
from Modules.NameService import Icons
from Modules.NameService import HandStates
from tkinter.ttk import *
import cv2
import numpy as np
import psutil
import os


class Model(object):
    """Model to hold all data"""

    def __init__(self, port, resolution, framerate, threshold, labels, lightmodel, heavymodel, buffer_Lenght, evaluation_Slice_Size, min_Positive_Occurrances, acceleration, videoDevice,useTfLight):
        self.resolution = resolution
        self.lightmodel = lightmodel
        self.heavymodel = heavymodel
        self.buffer_Lenght = buffer_Lenght
        self.evaluation_Slice_Size = evaluation_Slice_Size
        self.min_Positive_Occurrances = min_Positive_Occurrances
        self.acceleration = acceleration
        self.client = "None"
        self.port = port
        self.echo = None
        self.useTfLight = useTfLight
        self.labels = labels
        self.threshold = threshold
        self.down = False
        self.up = False
        self.left = False
        self.right = False
        self.regler = False
        self.power = False
        self.lastPowerState = False
        self.rightVelocity = 0
        self.leftVelocity = 0
        self.boxes = None
        self.classes = None
        self._scores = None
        self.frame = None
        self.detectionTime = 0
        self.serverActive = True
        self.tfActive = True
        self.videoActive = True
        self.hasFrame = False
        self.framerate = framerate
        self.handState = HandStates.No
        self.HandBuffer = []
        self.videoDevice = videoDevice

    @property
    def scores(self):
        return self._scores

    @scores.setter
    def scores(self, newscores):
        # Find Best detection and Append to HandBuffer
        if(not self.tfActive or not self.hasFrame):
            return
        try:
            validscores = []
            detection = HandStates.No
            for score in newscores:
                if(score > 0.00 and score < 1.00):
                    validscores.append(score)
            if validscores:
                current = max(validscores)
                if(current is not None):
                    if (current > self.threshold):
                        object_name = self.labels[int(
                            self.classes[validscores.index(current)])]
                        switcher = {
                            'Up': HandStates.Up,
                            'Down': HandStates.Down,
                            'Left': HandStates.Left,
                            'Right': HandStates.Right,
                            'Power': HandStates.Power,
                            'Toggle': HandStates.Toggle
                        }
                        detection = switcher.get(object_name, HandStates.No)
                    else:
                        detection = HandStates.No
            self.HandBuffer.append(detection)
            if(len(self.HandBuffer) > self.buffer_Lenght):
                self.HandBuffer.remove(self.HandBuffer[0])
        except IndexError as exe:
            self.PrintError("scores.setter", exe)

        # Get active Slice
        activeslice = self.HandBuffer[-self.evaluation_Slice_Size:]
        if(self.handState == HandStates.No):
            if(activeslice.count(activeslice[-1]) >= self.min_Positive_Occurrances):
                self.handState = activeslice[-1]
        else:
            if(activeslice.count(self.handState) == 0):
                self.handState = HandStates.No

        # Set bool Properties
        self.resetDirection()
        if(self.handState != HandStates.Power):
            self.powerlock = False
        if(self.handState != HandStates.Toggle):
            self.reglerlock = False
        if(self.handState == HandStates.Power and not self.powerlock):
            self.power = not self.power
            self.powerlock = True
        elif(self.handState == HandStates.Toggle and not self.reglerlock):
            self.regler = not self.regler
            self.reglerlock = True
        elif(self.handState == HandStates.Up):
            self.up = True
        elif(self.handState == HandStates.Down):
            self.down = True
        elif(self.handState == HandStates.Right):
            self.leftVelocity = 0
            self.rightVelocity += self.acceleration
            if(self.rightVelocity > 100):
                self.rightVelocity = 100
            self.right = True
        elif(self.handState == HandStates.Left):
            self.rightVelocity = 0
            self.leftVelocity += self.acceleration
            if(self.leftVelocity > 100):
                self.leftVelocity = 100
            self.left = True
        else:
            self.rightVelocity = 0
            self.leftVelocity = 0

        self._scores = newscores

    def resetDirection(self):
        self.right = False
        self.left = False
        self.down = False
        self.up = False

    @property
    def package(self):
        data = bytearray([0x00, 0x00, 0x00, 0x00, 0x00])
        if(not self.serverActive):
            return data
        if(not self.power == self.lastPowerState):
            self.lastPowerState = self.power
            data[0] = 1
            return data
        if(self.regler):
            data[4] = 1
        else:
            data[4] = 2
        if(self.up):
            data[3] = 1
        if(self.down):
            data[3] = 2
        if(self.left):
            data[1] = self.leftVelocity
        if(self.right):
            data[2] = self.rightVelocity
        return data

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
        if(self.hasFrame):
            try:
                detectionframe = self.frame.copy()
                if(type(self.boxes) is np.ndarray and type(self.classes) is np.ndarray and type(self._scores) is np.ndarray):
                    if self.boxes.any() and self.classes.any() and self._scores.any():
                        for i, score in enumerate(self._scores):
                            if (score > self.threshold and score <= 1.0):

                                # Get bounding box coordinates and draw box
                                # Interpreter can return coordinates that are outside of image dimensions, need to force them to be within image using max() and min()
                                ymin = int(
                                    max(1, (self.boxes[i][0] * self.imageHeight)))
                                xmin = int(
                                    max(1, (self.boxes[i][1] * self.imageWidth)))
                                ymax = int(
                                    min(self.imageHeight, (self.boxes[i][2] * self.imageHeight)))
                                xmax = int(
                                    min(self.imageWidth, (self.boxes[i][3] * self.imageWidth)))

                                cv2.rectangle(
                                    detectionframe, (xmin, ymin), (xmax, ymax), (10, 255, 0), 2)

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

            except AttributeError as attributeError:
                print("Model.frameWithDetections encountered Error: {0}".format(
                    attributeError))
        else:
            detectionframe = self.emptyImage

        color = (255, 255, 0)
        cpuUsage = psutil.cpu_percent()
        if(cpuUsage > 90 or self.detectionTime > 200):
            color = (0, 0, 255)
        cv2.putText(detectionframe, 'Cpu:{0}%,{1:0.1f}ms'.format(
            cpuUsage, self.detectionTime), (30, 50), cv2.FONT_HERSHEY_SIMPLEX, 1, color, 2, cv2.LINE_AA)
        return detectionframe

    def PrintError(self, funktion, error):
        print("Model.{0} encountered Error: {1}".format(funktion, error))

    @property
    def frameRGBA(self):
        return cv2.cvtColor(self.frameWithDetections, cv2.COLOR_BGR2RGBA)

    @property
    def emptyImage(self):
        image = np.zeros(
            [self.imageHeight, self.imageWidth, 3], dtype=np.uint8)
        image.fill(255)
        font = cv2.FONT_HERSHEY_SIMPLEX
        text = "No Camera Found!"
        fontsacle = 2
        thickness = 5
        color = (0, 0, 255)
        textsize = cv2.getTextSize(text, font, fontsacle, thickness)[0]
        textX = int((image.shape[1] - textsize[0]) / 2)
        textY = int((image.shape[0] + textsize[1]) / 2)
        cv2.putText(image, text, (textX, textY),
                    font, fontsacle, color, thickness)
        return image
