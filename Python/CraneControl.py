#####################
# This Programm will detect HAnd Gestures via the Webcam.
# Main Programm executes Detection and updates Thread Controller
# Thread Controller gives data to TCPServer and VideoStream
# TCPServer send data via TCP-IP
# VideoStream shows detections live
####################

#####################
# Imports
#####################


import importlib.util
from threading import Thread
import tkinter as tk
import time
import numpy as np
import cv2
import argparse
import os
import sys
from PIL import ImageTk, Image
from tkinter.ttk import *
import platform

sys.path.append(os.path.join(sys.path[0],'Modules'))

from TCPServer import TCPServer,Payload
import TF
from VideoStream import VideoStream


class ThreadController(object):
    """Threadcontroller to Execute Threads async"""

    def __init__(self, labels, resolution,port):
        self.freq = cv2.getTickFrequency()
        self.IsActive = True
        self.videostream = VideoStream(
            labels, resolution, framerate)
        self.tcpServer = TCPServer(int(port), labels,threshhold = min_conf_threshold)
        self.inception = TF.returnInception(self)
        self.detectionthread = Thread(target=self.detect, args=())
        self.detectionthread.daemon = True
        self.detectionthread.start()


    def restartThread(self):
        self.detectionthread = Thread(target=self.detect, args=())
        self.detectionthread.daemon = True
        self.detectionthread.start()

    def detect(self):
        while self.hasCamera():
            self.timeStart = cv2.getTickCount()
            self.inception.Detect(self.videostream.read().copy())
            self.timeEnd = cv2.getTickCount()
            self.frametime = ( self.timeEnd- self.timeStart)/ self.freq
            self.update(boxes=self.inception.boxes, classes=self.inception.classes,
                        scores=self.inception.scores, fps=1/self.frametime)
       
    def update(self, boxes, classes, scores, fps):
        if(not self.tcpServer == None):
            self.tcpServer.Update(classes=classes, scores=scores)
        if(self.videostream is not None):
            self.videostream.updateDetection(
                boxes=boxes, classes=classes, scores=scores, fps=fps)

    def hasCamera(self):
        return (self.videostream is not None and self.videostream.stream.isOpened() and self.videostream.frame is not None and self.IsActive)

    def stop(self):
        self.IsActive = False
        self.tcpServer.stop()
        self.videostream.stop()
        self.tcpServer = None
        self.videostream = None
       

class Window(object):
    def __init__(self,root,labels,resolution):
        super().__init__()
        self.rightimage = ImageTk.PhotoImage(Image.open(os.path.join(os.getcwd(), 'Images/right.png')).resize((50,50), Image.ANTIALIAS)) 
        self.leftimage = ImageTk.PhotoImage(Image.open(os.path.join(os.getcwd(), 'Images/left.png')).resize((50,50), Image.ANTIALIAS)) 
        self.upimage = ImageTk.PhotoImage(Image.open(os.path.join(os.getcwd(), 'Images/up.png')).resize((50,50), Image.ANTIALIAS)) 
        self.downimage = ImageTk.PhotoImage(Image.open(os.path.join(os.getcwd(), 'Images/down.png')).resize((50,50), Image.ANTIALIAS)) 
        self.reglerimage = ImageTk.PhotoImage(Image.open(os.path.join(os.getcwd(), 'Images/regler.png')).resize((50,50), Image.ANTIALIAS))
        self.poweronimage = ImageTk.PhotoImage(Image.open(os.path.join(os.getcwd(), 'Images/poweron.png')).resize((50,50), Image.ANTIALIAS))
        self.poweroffimage = ImageTk.PhotoImage(Image.open(os.path.join(os.getcwd(), 'Images/poweroff.png')).resize((50,50), Image.ANTIALIAS))
        self.initUI(root)
        self.resolution = resolution
        self.restartWebcamThread = None
        self.threadController = ThreadController(labels,resolution,(int)(self.PortTxt.get("1.0",tk.END)))

    def initUI(self,root):
        #Init
        root.title("Crane Control")
        root.style = Style()
        root.style.theme_use("default")
        #Grid
        root.grid_columnconfigure(0, weight=4)
        root.grid_columnconfigure(1, weight=1)

        self.CameraFrame = Frame(root, relief=tk.RAISED, borderwidth=2)
        self.CameraFrame.grid(row=0,column=0,sticky="nsew")
        self.InfoFrame = Frame(root,relief=tk.RAISED, borderwidth=2)
        self.InfoFrame.grid(row=0,column=1,sticky="nsew")

        self.InfoFrame.grid_columnconfigure(0)
        self.InfoFrame.grid_columnconfigure(1)
        self.InfoFrame.grid_columnconfigure(2)
        #Camera Feed
        self.picturebox = tk.Label(self.CameraFrame)
        self.picturebox.pack(fill=tk.BOTH,expand=True)

        #Port
        PortLabel = tk.Label(self.InfoFrame,text="Port :")
        PortLabel.grid(row=0,column=0,sticky="nsew")
        self.PortTxt = tk.Text(self.InfoFrame,height=1,width=30)
        self.PortTxt.grid(row=1,column=0,columnspan=3,sticky="nw")
        self.PortTxt.insert(tk.END,chars="54000")
        #Client
        ClientLabel = tk.Label(self.InfoFrame,text="Client :")
        ClientLabel.grid(row=2,column=0,sticky="nsew")
        self.ClientTxt = tk.Text(self.InfoFrame,height=1,width=30)
        self.ClientTxt.grid(row=3,column=0,columnspan=3,sticky="nw")
        self.ClientTxt.insert(tk.END,chars="None")
        #Power
        self.PowerIndicator = tk.Button(self.InfoFrame,image = self.poweroffimage,bg='white')
        self.PowerIndicator.grid(row=4,column=0,sticky="nsew")
        #Up
        self.UpIndicator = tk.Button(self.InfoFrame,image = self.upimage,bg='white')
        self.UpIndicator.grid(row=4,column=1,sticky="nsew")
        #Regler
        self.ReglerIndicator = tk.Button(self.InfoFrame,image = self.reglerimage,bg='white')
        self.ReglerIndicator.grid(row=4,column=2,sticky="nsew")
        #Left
        self.LeftIndicator = tk.Button(self.InfoFrame,image = self.leftimage,bg='white')
        self.LeftIndicator.grid(row=5,column=0,sticky="nsew")
        #Right
        self.RightIndicator = tk.Button(self.InfoFrame,image = self.rightimage,bg='white')
        self.RightIndicator.grid(row=5,column=2,sticky="nsew")
        #Down
        self.DownIndicator = tk.Button(self.InfoFrame,image = self.downimage,bg='white')
        self.DownIndicator.grid(row=6,column=1,sticky="nsew")
        #Velocity
        VelocityLabel = tk.Label(self.InfoFrame,text="Velocity :")
        VelocityLabel.grid(row=7,column=0,sticky="nsew")
        self.VelocityTxt = tk.Text(self.InfoFrame,height=1,width=30)
        self.VelocityTxt.grid(row=8,column=0,columnspan=3,sticky="nw")
        self.VelocityTxt.insert(tk.END,chars="0")
        #Halt Button
        self.HaltBtn = tk.Button(self.InfoFrame,text = "Halt Server",command=self.haltServer)
        self.HaltBtn.grid(row=9,column=0)
        #Stop Button
        self.RestartBtn = tk.Button(self.InfoFrame,text = "Restart Server",command=self.restartServer)
        self.RestartBtn.grid(row=9,column=2)

    #TODO Only Restart Server
    def restartServer(self):
        if(self.threadController is not None):
            self.threadController.stop()
        time.sleep(1)
        self.threadController = None
        self.threadController = ThreadController(labels,self.resolution,(int)(self.PortTxt.get("1.0",tk.END)))

    def restartWebcam(self):
        if(self.restartWebcamThread is None or not self.restartWebcamThread.isAlive):
            self.threadController.videostream.stop()
            self.threadController.videostream = None
            self.restartWebcamThread = Thread(target=self.detectWebcam)
            self.restartWebcamThread.start()

    def detectWebcam(self):
        cap = VideoStream.returnVideoStream(self)
        while cap is None or not cap.isOpened():
             cap = VideoStream.returnVideoStream(self)
             time.sleep(1)
        cap.release()
        self.threadController.videostream = videoStream(
            labels, self.resolution, framerate)
        self.threadController.restartThread()
    

    def haltServer(self):
        if(self.threadController.tcpServer.payload.IsHalted):
            self.HaltBtn.config(text = "Halt Server")
        else:
            self.HaltBtn.config(text = "Continue Server")
        self.threadController.tcpServer.payload.IsHalted = not self.threadController.tcpServer.payload.IsHalted

    def drawImage(self):
        if(self.threadController is not None):
            if(self.threadController.hasCamera()):
                    cv2image = cv2.cvtColor(self.threadController.videostream.drawDetection(), cv2.COLOR_BGR2RGBA)
                    img = Image.fromarray(cv2image)
                    imgtk = ImageTk.PhotoImage(image=img)
                    app.picturebox.imgtk = imgtk
                    app.picturebox.configure(image=imgtk)
            else:
                img = Image.fromarray(np.zeros((self.resolution[1],self.resolution[0],3), np.uint8))
                imgtk = ImageTk.PhotoImage(image=img)
                app.picturebox.imgtk = imgtk
                app.picturebox.configure(image=imgtk)
                self.restartWebcam()

        self.drawIndicatores()
        form.after(ms,self.drawImage)
        
    def drawIndicatores(self):
        if(self.threadController is not None and self.threadController.tcpServer is not None):
            if(not self.threadController.tcpServer.addr == None):
                self.ClientTxt.delete("1.0", tk.END)
                self.ClientTxt.insert(tk.END, chars=self.threadController.tcpServer.addr)
            if(self.threadController.tcpServer.payload.power):
                self.PowerIndicator.config(image=self.poweronimage)
            else:
                self.PowerIndicator.config(image=self.poweroffimage)

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
                self.VelocityTxt.insert(
                tk.END, chars=self.threadController.tcpServer.payload.velocityLeft)
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
                self.threadController.tcpServer.payload.velocityRight=0
                self.threadController.tcpServer.payload.velocityLeft=0
                self.VelocityTxt.delete("1.0", tk.END)
                self.VelocityTxt.insert(tk.END, chars="0")


#####################
# Program
#####################

min_conf_threshold = 0.6
webcam_resolution = (1280,720)
framerate = 30
ms = (int) (1/framerate * 1000)

labels = TF.returnLabels()


form = tk.Tk()
form.bind('<Escape>', lambda e: form.quit())
app = Window(form,labels,webcam_resolution)
app.drawImage()
form.mainloop()
app.threadController.stop()
exit(1)



