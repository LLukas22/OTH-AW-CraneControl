import tkinter as tk
from Modules.NameService import Icons
from Modules.NameService import HandStates
from tkinter.ttk import *
import cv2
import numpy as np
from Modules.Model import Model

class Payload(object):
    def __init__ (self,model):
        self.model = Model(model)
        self.data = bytearray([0x00, 0x00, 0x00, 0x00, 0x00])
        self.reglerlock = False
        self.powerlock = False
        self.lastPowerState = not self.power

    def update(self,handstate):
        self.reset()
        if(handstate != HandStates.Power): self.powerlock = False
        if(handstate != HandStates.Toggle): self.reglerlock = False
        if(handstate == HandStates.Power and not self.powerlock): 
            self.power = not self.power
            self.powerlock = True
        if(handstate == HandStates.Toggle and not self.reglerlock): 
            self.regler = not self.regler
            self.reglerlock = True
        if(handstate == HandStates.Up): 
            self.up = True
        if(handstate == HandStates.Down): 
            self.down = True
        if(handstate == HandStates.Right): 
            self.velocityLeft = 0
            self.velocityRight += 8
            if(self.velocityRight>100): self.velocityRight = 100
            self.right = True
        if(handstate == HandStates.Left):
            self.velocityRight = 0
            self.velocityLeft += 8
            if(self.velocityLeft>100): self.velocityLeft = 100
            self.left = True
    
    def reset(self):
        self.right = False
        self.left = False
        self.down = False
        self.up = False

    def buildPackage(self):
        self.data = bytearray([0x00, 0x00, 0x00, 0x00, 0x00])
        if(self.IsHalted):return self.data
        if(not self.power ==  self.lastPowerState):
            self.lastPowerState = self.power
            self.data[0] = 1
            return self.data
        if(self.regler):
            self.data[4]=1
        else:
            self.data[4]=2
        
        if(self.up): self.data[3]=1
        if(self.down): self.data[3]=2
        if(self.left): self.data[1]= self.velocityLeft
        if(self.right): self.data[2]= self.velocityRight
        return self.data