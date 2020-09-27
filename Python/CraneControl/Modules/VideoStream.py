import cv2
import numpy as np
import os
import sys
from threading import Thread
import time
import platform 
from Modules.NameService import Icons


def returnVideoStream(device):
    if(any(platform.win32_ver())):
        return cv2.VideoCapture(device  + cv2.CAP_DSHOW)
    else:
        return cv2.VideoCapture(device)

class VideoStream(object):
    """Background Thread to get Videostream"""

    def __init__(self,model):
        self.IsActive = True
        self.model = model
        self.frametiming = 1/ self.model.framerate
        self.stream = returnVideoStream(self.model.videoDevice)
        ret = self.stream.set(3, self.model.resolution[0])
        ret = self.stream.set(4, self.model.resolution[1])
        (self.status, self.frame) = self.stream.read()
        self.thread = Thread(target=self.update, args=(),daemon = True)
        self.thread.start()
        
    def update(self):
        while self.IsActive:
            if(self.model.videoActive):
                if(self.stream is not None and self.stream.isOpened()):
                        (self.status, self.model.frame) = self.stream.read()
                        self.model.hasFrame = True
                        time.sleep(self.frametiming)
                else:
                    #Wait and try reopening the stream 
                    self.model.hasFrame = False
                    time.sleep(0.5)
                    self.stream = returnVideoStream(self.model.videoDevice)
                    ret = self.stream.set(3, self.model.resolution[0])
                    ret = self.stream.set(4, self.model.resolution[1])
            else:
                self.model.hasFrame = False
                time.sleep(0.2)
    
    def Stop(self):
        self.model.hasFrame = False
        self.IsActive = False
        time.sleep(0.5)
                 

  