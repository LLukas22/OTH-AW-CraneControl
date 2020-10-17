#####################
# This Programm will detect Hand Gestures via the Webcam.
# Main Programm executes Detection and updates Thread Controller
# Thread Controller gives data to TCPServer and VideoStream
# TCPServer send data via TCP-IP
# VideoStream shows detections live
####################

#####################
# Imports
#####################
from Modules.NameService import TfVersion
import os
import tkinter as tk
from tkinter.ttk import *
from Modules.MainView import MainView
from Modules.Model import Model
import Modules.TfBinding as TF

#####################
# Program
#####################




# Settings
#####################
tfVersion = TfVersion.Tf2
min_conf_threshold = 0.5
buffer_Lenght = 60
evaluation_Slice_Size = 5
min_Positive_Occurrances = 4
webcam_resolution = (1280, 720)
framerate = 30
port = 54000
acceleration = 2
videoDevice = 0
litemodel = 'TFModels/mobilnet_300x300v2.tflite'
heavymodel = 'TFModels/mobilnet_300x300.pb'
tfVersion = TF.Initialize(tfVersion)
if tfVersion is TfVersion.Tf2:
    heavymodel = 'TFModels/ResNet 50'
labels = TF.returnLabels(os.path.join(os.getcwd(), 'TFModels/labelmap.txt'),tfVersion)
#####################

view = tk.Tk()
view.bind('<Escape>', lambda e: view.quit())
model = Model(port,webcam_resolution,framerate,min_conf_threshold,labels,litemodel,heavymodel,buffer_Lenght,evaluation_Slice_Size,min_Positive_Occurrances,acceleration,videoDevice,tfVersion)
app = MainView(view, model)
app.drawImage()
view.mainloop()
#Cleanup#
app.Close()
del app
del view
del model
exit(0)
