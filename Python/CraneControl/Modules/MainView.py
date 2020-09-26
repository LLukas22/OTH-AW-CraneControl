import tkinter as tk
from Modules.NameService import Icons
from Modules.NameService import HandStates
from Modules.ThreadController import ThreadController
from tkinter.ttk import *
import cv2
from PIL import ImageTk, Image
import numpy as np

  
class MainView(object):
    def __init__(self, root, model):
        super().__init__()
        self.root = root
        self.icons = Icons()
        self.model = model
        self.BuildUp(root)
        self.threadController = ThreadController(model)
        self.threadController.Start()

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
        self.PortTxt.insert(tk.END, chars=self.model.port)
        # Client
        ClientLabel = tk.Label(self.InfoFrame, text="Client :")
        ClientLabel.grid(row=2, column=0, sticky="nsew")
        self.ClientTxt = tk.Text(self.InfoFrame, height=1, width=30)
        self.ClientTxt.grid(row=3, column=0, columnspan=3, sticky="nw")
        self.ClientTxt.insert(tk.END, chars="None")
        # Power
        self.PowerButton = tk.Button(self.InfoFrame, image= self.icons.poweroff, bg='white')
        self.PowerButton.grid(row=4, column=0, sticky="nsew")
        # Up
        self.UpButton = tk.Button(self.InfoFrame, image=self.icons.up, bg='white')
        self.UpButton.grid(row=4, column=1, sticky="nsew")
        # Regler
        self.ReglerButton = tk.Button(self.InfoFrame, image=self.icons.regler, bg='white')
        self.ReglerButton.grid(row=4, column=2, sticky="nsew")
        # Left
        self.LeftButton = tk.Button(self.InfoFrame, image=self.icons.left, bg='white')
        self.LeftButton.grid(row=5, column=0, sticky="nsew")
        # Right
        self.RightButton = tk.Button(self.InfoFrame, image=self.icons.right, bg='white')
        self.RightButton.grid(row=5, column=2, sticky="nsew")
        # Down
        self.DownButton = tk.Button(self.InfoFrame, image=self.icons.down, bg='white')
        self.DownButton.grid(row=6, column=1, sticky="nsew")
        # Velocity
        VelocityLabel = tk.Label(self.InfoFrame, text="Velocity :")
        VelocityLabel.grid(row=7, column=0, sticky="nsew")
        self.VelocityTxt = tk.Text(self.InfoFrame, height=1, width=30)
        self.VelocityTxt.grid(row=8, column=0, columnspan=3, sticky="nw")
        self.VelocityTxt.insert(tk.END, chars="0")
       
        #Server Control
        self.StopServerBtn = tk.Button(self.InfoFrame, text="Stop Server", command = self.StopServer)
        self.StopServerBtn.grid(row=9, column=0)
        self.RestartServerBtn = tk.Button(self.InfoFrame, text="Restart Server",command = self.RestartServer)
        self.RestartServerBtn.grid(row=9, column=2)
        
        #TF Control
        self.StopTensorflowBtn = tk.Button(self.InfoFrame, text="Stop Tensorflow",command = self.StopTensorflow)
        self.StopTensorflowBtn.grid(row=10, column=0)
        self.RestartTensorflowBtn = tk.Button(self.InfoFrame, text="Restart Tensorflow",command = self.RestartTensorflow)
        self.RestartTensorflowBtn.grid(row=10, column=2)
        
         #VideoStream Control
        self.StopVideoStreamBtn = tk.Button(self.InfoFrame, text="Stop VideoStream",command = self.StopVideoStream)
        self.StopVideoStreamBtn.grid(row=11, column=0)
        self.RestartVideoStreamBtn = tk.Button(self.InfoFrame, text="Restart VideoStream",command = self.RestartVideoStream)
        self.RestartVideoStreamBtn.grid(row=11, column=2)
     
    #region ButtonClicks
    
    def StopServer(self):
        self.model.serverActive = not self.model.serverActive
        self.SetButtonText(self.StopServerBtn,self.model.serverActive,"Stop Server","Start Server")
       

    def RestartServer(self):
        self.threadController.RestartTCPServer()
    
    def StopTensorflow(self):
        self.model.tfActive = not self.model.tfActive
        self.SetButtonText(self.StopTensorflowBtn,self.model.tfActive,"Stop Tensorflow","Start Tensorflow")
        
    
    def RestartTensorflow(self):
        self.threadController.RestartTensorflow()
      
    
    def StopVideoStream(self):
        self.model.videoActive = not self.model.videoActive
        self.SetButtonText(self.StopVideoStreamBtn,self.model.videoActive,"Stop VideoStream","Start VideoStream")
    
    def RestartVideoStream(self):
        self.threadController.RestartVideoStream()

    
    #endregion   
    
    def Close(self):
        self.threadController.Stop()
           
        
    def drawImage(self):
        self.Refresh()
        self.root.after(self.model.delayTime, self.drawImage)

    def Refresh(self):
        if(self.model is not None):

           
            img = Image.fromarray(self.model.frameRGBA)
            imgtk = ImageTk.PhotoImage(image=img)
            self.picturebox.imgtk = imgtk
            self.picturebox.configure(image=imgtk)
           
            self.ClientTxt.delete("1.0", tk.END)
            self.ClientTxt.insert(tk.END, chars=self.model.client)
            
            self.ChangeImage(self.PowerButton,self.model.power,self.icons.poweron,self.icons.poweroff)
            
            self.ChangeBackground(self.ReglerButton,self.model.regler,'green','white')
            self.ChangeBackground(self.UpButton,self.model.up,'green','white')
            self.ChangeBackground(self.DownButton,self.model.down,'green','white')
            self.ChangeBackground(self.LeftButton,self.model.left,'green','white')
            self.ChangeBackground(self.RightButton,self.model.right,'green','white')
           
            if(self.model.left):
                self.SetText(self.VelocityTxt,self.model.leftVelocity)
            elif(self.model.right):
                 self.SetText(self.VelocityTxt,self.model.rightVelocity)
            else:
                self.SetText(self.VelocityTxt,"0")
                
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
            
    def SetText(self,textbox,message):
        textbox.delete("1.0", tk.END)
        textbox.insert(tk.END, chars=message)
        
    def SetButtonText(self,button,condition,textTrue,textFalse):
        if(condition):
            button.config(text = textTrue)
        else:
            button.config(text = textFalse)
        
#endregion  

