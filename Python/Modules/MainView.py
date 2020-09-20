import tkinter as tk
from Modules.NameService import Icons
from Modules.NameService import HandStates
from tkinter.ttk import *
import cv2
import numpy as np


class Payload(object):
    def __init__ (self):
        self.right = False
        self.left = False
        self.down = False
        self.up = False
        self.regler = False
        self.power = False
        self.data = bytearray([0x00, 0x00, 0x00, 0x00, 0x00])
        self.velocityLeft = 0
        self.velocityRight = 0
        self.reglerlock = False
        self.powerlock = False
        self.IsHalted = False
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
    
class MainModel(object):
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
        
        
        
class MainView(object):
    def __init__(self, root, resolution,delayTime):
        super().__init__()
        self.root = root
        self.icons = Icons()
        self.BuildUp(root)
        self.resolution = resolution
        self.delayTime = delayTime

    def BindData(self, threadController,mainModel):
        self.threadController = threadController
        self.model = mainModel

    def BuildUp(self, root):
        # Init
        root.title("Crane Control")
        root.style = Style()
        root.style.theme_use("default")
        # Grid
        root.grid_columnconfigure(0, weight=4)
        root.grid_columnconfigure(1, weight=1)

        self.CameraFrame = Frame(root, relief=tk.RAISED, borderwidth=2)
        self.CameraFrame.grid(row=0, column=0, sticky="nsew")
        self.InfoFrame = Frame(root, relief=tk.RAISED, borderwidth=2)
        self.InfoFrame.grid(row=0, column=1, sticky="nsew")

        self.InfoFrame.grid_columnconfigure(0)
        self.InfoFrame.grid_columnconfigure(1)
        self.InfoFrame.grid_columnconfigure(2)
        # Camera Feed
        self.picturebox = tk.Label(self.CameraFrame)
        self.picturebox.pack(fill=tk.BOTH, expand=True)
        # Port
        PortLabel = tk.Label(self.InfoFrame, text="Port :")
        PortLabel.grid(row=0, column=0, sticky="nsew")
        self.PortTxt = tk.Text(self.InfoFrame, height=1, width=30)
        self.PortTxt.grid(row=1, column=0, columnspan=3, sticky="nw")
        self.PortTxt.insert(tk.END, chars="54000")
        # Client
        ClientLabel = tk.Label(self.InfoFrame, text="Client :")
        ClientLabel.grid(row=2, column=0, sticky="nsew")
        self.ClientTxt = tk.Text(self.InfoFrame, height=1, width=30)
        self.ClientTxt.grid(row=3, column=0, columnspan=3, sticky="nw")
        self.ClientTxt.insert(tk.END, chars="None")
        # Power
        self.PowerButton = tk.Button(
            self.InfoFrame, image= self.icons.poweroff, bg='white')
        self.PowerButton.grid(row=4, column=0, sticky="nsew")
        # Up
        self.UpIndicator = tk.Button(
            self.InfoFrame, image=self.icons.up, bg='white')
        self.UpIndicator.grid(row=4, column=1, sticky="nsew")
        # Regler
        self.ReglerIndicator = tk.Button(
            self.InfoFrame, image=self.icons.regler, bg='white')
        self.ReglerIndicator.grid(row=4, column=2, sticky="nsew")
        # Left
        self.LeftIndicator = tk.Button(
            self.InfoFrame, image=self.icons.left, bg='white')
        self.LeftIndicator.grid(row=5, column=0, sticky="nsew")
        # Right
        self.RightIndicator = tk.Button(
            self.InfoFrame, image=self.icons.right, bg='white')
        self.RightIndicator.grid(row=5, column=2, sticky="nsew")
        # Down
        self.DownIndicator = tk.Button(
            self.InfoFrame, image=self.icons.down, bg='white')
        self.DownIndicator.grid(row=6, column=1, sticky="nsew")
        # Velocity
        VelocityLabel = tk.Label(self.InfoFrame, text="Velocity :")
        VelocityLabel.grid(row=7, column=0, sticky="nsew")
        self.VelocityTxt = tk.Text(self.InfoFrame, height=1, width=30)
        self.VelocityTxt.grid(row=8, column=0, columnspan=3, sticky="nw")
        self.VelocityTxt.insert(tk.END, chars="0")
        # Halt Button
        self.HaltBtn = tk.Button(
            self.InfoFrame, text="Halt Server", command=self.haltServer)
        self.HaltBtn.grid(row=9, column=0)
        # Stop Button
        self.RestartBtn = tk.Button(
            self.InfoFrame, text="Restart Server", command=self.restartServer)
        self.RestartBtn.grid(row=9, column=2)
        
        
    def drawImage(self):
        self.Refresh()
        self.root.after(self.delayTime, self.drawImage)

    def Refresh(self):
        if(self.model is not None):
            
            if(not self.threadController.tcpServer.addr == None):
                self.ClientTxt.delete("1.0", tk.END)
                self.ClientTxt.insert(
                    tk.END, chars=self.threadController.tcpServer.addr)
                
                
            if(self.threadController.tcpServer.payload.power):
                self.PowerButton.config(image=self.icons.poweron)
            else:
                self.PowerButton.config(image=self.icons.poweroff)

            if(self.threadController.tcpServer.payload.regler):
                self.ReglerIndicator.config(bg='green')
            else:
                self.ReglerIndicator.config(bg='white')

            if(self.threadController.tcpServer.payload.up):
                self.UpIndicator.config(bg='green')
            else:
                self.UpIndicator.config(bg='white')

            if(self.threadController.tcpServer.payload.down):
                self.DownIndicator.config(bg='green')
            else:
                self.DownIndicator.config(bg='white')

            if(self.threadController.tcpServer.payload.left):
                self.LeftIndicator.config(bg='green')
                self.VelocityTxt.delete("1.0", tk.END)
                self.VelocityTxt.insert(tk.END, chars=self.threadController.tcpServer.payload.velocityLeft)
            else:
                self.LeftIndicator.config(bg='white')

            if(self.threadController.tcpServer.payload.right):
                self.RightIndicator.config(bg='green')
                self.VelocityTxt.delete("1.0", tk.END)
                self.VelocityTxt.insert(
                    tk.END, chars=self.threadController.tcpServer.payload.velocityRight)
            else:
                self.RightIndicator.config(bg='white')

            if(not self.threadController.tcpServer.payload.right and not self.threadController.tcpServer.payload.left):
                self.threadController.tcpServer.payload.velocityRight = 0
                self.threadController.tcpServer.payload.velocityLeft = 0
                self.VelocityTxt.delete("1.0", tk.END)
                self.VelocityTxt.insert(tk.END, chars="0")
                
                
#region Extensions
    def ChangeBackground(self,button,condition,colortrue,colorfalse):
        if(condition):
            button.config(bg=colortrue)
        else:
             button.config(bg=colorfalse)
             
    def ChangeImage(self,button,condition,imagetrue,imagefalse):
        if(condition):
            button.config(image=imagetrue)
        else:
            button.config(image=imagefalse)
#endregion  