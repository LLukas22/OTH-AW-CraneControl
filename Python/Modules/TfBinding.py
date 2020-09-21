
import importlib.util
from threading import Thread
import time
import numpy as np
import cv2
import argparse
import os
import sys
import datetime

lightmodel = 'detectV2.tflite'
heavymodel = 'frozen_inference_graph.pb'

# Check if TF or TFLite
pkg = importlib.util.find_spec('tensorflow')
useLite = False
if pkg is None:
    from tflite_runtime.interpreter import Interpreter
    useLite = True
else:
    import tensorflow as tf


def Inception(model):
    if(useLite):
        return TFLiteInception(model)
    else:
        return TFInception(model)


def returnLabels(path):
    with open(path, 'r') as f:
        labels = [line.strip() for line in f.readlines()]
        if(not useLite):
            labels.insert(0, "None")
        return labels


class TFInception(object):
    def __init__(self,model):
        self.model = model
        self.IsActive = True
        gpus = tf.config.experimental.list_physical_devices('GPU')
        if gpus:
            for gpu in gpus:
                    print(gpu)
                    tf.config.experimental.set_memory_growth(gpu,True)
        self.detection_graph = tf.Graph()
        with self.detection_graph.as_default():
            od_graph_def = tf.compat.v1.GraphDef()
            with tf.io.gfile.GFile(os.path.join(os.getcwd(), self.model.heavymodel), 'rb') as fid:
                serialized_graph = fid.read()
                od_graph_def.ParseFromString(serialized_graph)
                tf.import_graph_def(od_graph_def, name='')
        self.session = tf.compat.v1.Session(graph=self.detection_graph)
        self.thread = Thread(target=self.Detect, args=())
        self.thread.daemon = True
        self.thread.start()

    def Detect(self):
        while(self.IsActive):
            if(self.model.tfActive and self.model.hasFrame):
                start = datetime.datetime.now()
                image_np_expanded = np.expand_dims(self.model.frame.copy(), axis=0)
                image_tensor = self.detection_graph.get_tensor_by_name(
                    'image_tensor:0')
                boxes = self.detection_graph.get_tensor_by_name('detection_boxes:0')
                scores = self.detection_graph.get_tensor_by_name('detection_scores:0')
                classes = self.detection_graph.get_tensor_by_name(
                    'detection_classes:0')
                num_detections = self.detection_graph.get_tensor_by_name(
                    'num_detections:0')
                (boxesresult, scoresresult, classesresult, num_detectionsresult) = self.session.run(
                    [boxes, scores, classes, num_detections],
                    feed_dict={image_tensor: image_np_expanded})
                end = datetime.datetime.now()
                self.model.detectionTime = (end-start).microseconds/1000
                self.model.classes = classesresult[0]
                self.model.boxes = boxesresult[0]
                self.model.scores = scoresresult[0]
            else:
                self.model.detectionTime = 0
                self.model.classes = []
                self.model.boxes = []
                self.model.scores = []
                time.sleep(0.1)
                
    def Stop(self):
        self.IsActive = False 
        time.sleep(0.1)
        self.model.detectionTime = 0
        self.model.classes = []
        self.model.boxes = []
        self.model.scores = []
      
class TFLiteInception(object):
    """TFLite Detection"""

    def __init__(self,model):
        self.IsActive = True
        self.model = model
        self.interpreter = Interpreter(
            model_path=os.path.join(os.getcwd(),  self.model.lightmodel))
        self.interpreter.allocate_tensors()

        self.input_details = self.interpreter.get_input_details()
        self.output_details = self.interpreter.get_output_details()

        self.modelheight = self.input_details[0]['shape'][1]
        self.modelwidth = self.input_details[0]['shape'][2]

        self.floating_model = (self.input_details[0]['dtype'] == np.float32)

        self.input_mean = 127.5
        self.input_std = 127.5
        
        
        self.thread = Thread(target=self.Detect, args=())
        self.thread.daemon = True
        self.thread.start()

    def Detect(self):
        while(self.IsActive):
            if(self.model.tfActive and self.model.hasFrame):
                start = datetime.datetime.now()
                self.frame_rgb = cv2.cvtColor(self.model.frame.copy(), cv2.COLOR_BGR2RGB)
                self.frame_resized = cv2.resize(
                    self.frame_rgb, (self.modelheight, self.modelwidth))
                self.input_data = np.expand_dims(self.frame_resized, axis=0)

                # Normalize pixel values if using a floating model (i.e. if model is non-quantized)
                if self.floating_model:
                    self.input_data = (np.float32(self.input_data) -
                                    self.input_mean) / self.input_std

                self.interpreter.set_tensor(
                    self.input_details[0]['index'], self.input_data)
                self.interpreter.invoke()
                end = datetime.datetime.now()
                self.model.detectionTime = (end-start).microseconds/1000
                self.model.boxes = self.interpreter.get_tensor(self.output_details[0]['index'])[0]
                self.model.classes = self.interpreter.get_tensor(self.output_details[1]['index'])[0]
                self.model.scores = self.interpreter.get_tensor(self.output_details[2]['index'])[0]
            else:
                self.model.detectionTime = 0
                self.model.classes = []
                self.model.boxes = []
                self.model.scores = []
                time.sleep(0.1)
                
    def Stop(self):
        self.IsActive = False 
        time.sleep(0.1) 
        self.model.detectionTime = 0
        self.model.classes = []
        self.model.boxes = []
        self.model.scores = []
        
        
