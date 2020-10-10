import importlib.util
from threading import Thread
import time
import numpy as np
import cv2
import argparse
import os
import sys
import datetime
from Modules.NameService import TfVersion
# Check if TF or TFLite

if importlib.util.find_spec('tensorflow') is not None:
    import tensorflow as tf
if importlib.util.find_spec('tflite_runtime') is not None:
    import tflite_runtime.interpreter as tflite


def Initialize(tfversion):
    pkg = importlib.util.find_spec('tensorflow')
    if pkg is None or tfversion is TfVersion.TfLite:
        return TfVersion.TfLite
    else:
        return tfversion


def Inception(model):
    if(model.tfVersion is TfVersion.TfLite):
        return TFLiteInception(model)
    else:
        return TFInception(model)


def returnLabels(path, tfVersion):
    with open(path, 'r') as f:
        labels = [line.strip() for line in f.readlines()]
        if(tfVersion is not  TfVersion.TfLite):
            labels.insert(0, "None")
        return labels


class TFInception(object):
    def __init__(self, model):
        self.model = model
        self.IsActive = True
        #Allow GPU to allocate more memory 
        gpus = tf.config.experimental.list_physical_devices('GPU')
        if gpus:
            for gpu in gpus:
                tf.config.experimental.set_memory_growth(gpu, True)
                
        if(self.model.tfVersion is TfVersion.Tf1):
            self.LoadTf1()
            self.thread = Thread(target=self.DetectTf1, args=(), daemon=True)
        else:
            self.LoadTf2()
            self.thread = Thread(target=self.DetectTf2, args=(), daemon=True)
            
        self.thread.start()

    def LoadTf1(self):
        self.detection_graph = tf.Graph()
        with self.detection_graph.as_default():
            od_graph_def = tf.compat.v1.GraphDef()
            with tf.io.gfile.GFile(os.path.join(os.getcwd(), self.model.heavymodel), 'rb') as fid:
                serialized_graph = fid.read()
                od_graph_def.ParseFromString(serialized_graph)
            tf.import_graph_def(od_graph_def, name='')
        self.session = tf.compat.v1.Session(graph=self.detection_graph)
    
    def LoadTf2(self):
        self.saved_model = tf.saved_model.load(self.model.heavymodel)
        pass
    
    def DetectTf1(self):
        while(self.IsActive):
            if(self.model.tfActive and self.model.hasFrame):
                start = datetime.datetime.now()
                image_np_expanded = np.expand_dims(
                    self.model.frame.copy(), axis=0)
                image_tensor = self.detection_graph.get_tensor_by_name(
                    'image_tensor:0')
                boxes = self.detection_graph.get_tensor_by_name(
                    'detection_boxes:0')
                scores = self.detection_graph.get_tensor_by_name(
                    'detection_scores:0')
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
                
                
    def DetectTf2(self):
        while(self.IsActive):
            if(self.model.tfActive and self.model.hasFrame):
                start = datetime.datetime.now()
                input_tensor = tf.convert_to_tensor(self.model.frame.copy())
                input_tensor = input_tensor[tf.newaxis, ...]
                detections = self.saved_model(input_tensor)
                num_detections = int(detections.pop('num_detections'))
                detections = {key: value[0, :num_detections].numpy()
                   for key, value in detections.items()}
                detections['num_detections'] = num_detections
                detections['detection_classes'] = detections['detection_classes'].astype(np.int64)
                end = datetime.datetime.now()
                self.model.detectionTime = (end-start).microseconds/1000
                self.model.classes =  detections['detection_classes']
                self.model.boxes =  detections['detection_boxes']
                self.model.scores = detections['detection_scores']
                pass
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

    def __init__(self, model):
        self.IsActive = True
        self.model = model
        self.interpreter = tflite.Interpreter(
            model_path=os.path.join(os.getcwd(), self.model.lightmodel))
        self.interpreter.allocate_tensors()

        self.input_details = self.interpreter.get_input_details()
        self.output_details = self.interpreter.get_output_details()

        self.modelheight = self.input_details[0]['shape'][1]
        self.modelwidth = self.input_details[0]['shape'][2]

        self.floating_model = (self.input_details[0]['dtype'] == np.float32)

        self.input_mean = 127.5
        self.input_std = 127.5

        self.thread = Thread(target=self.Detect, args=(), daemon=True)
        self.thread.start()

    def Detect(self):
        while(self.IsActive):
            if(self.model.tfActive and self.model.hasFrame):
                start = datetime.datetime.now()
                frame_rgb = cv2.cvtColor(
                    self.model.frame.copy(), cv2.COLOR_BGR2RGB)
                frame_resized = cv2.resize(
                    frame_rgb, (self.modelheight, self.modelwidth))
                input_data = np.expand_dims(frame_resized, axis=0)

                # Normalize pixel values if using a floating model (i.e. if model is non-quantized)
                if self.floating_model:
                    input_data = (np.float32(input_data) -
                                  self.input_mean) / self.input_std

                self.interpreter.set_tensor(
                    self.input_details[0]['index'], input_data)
                self.interpreter.invoke()
                end = datetime.datetime.now()
                self.model.detectionTime = (end-start).microseconds/1000
                self.model.boxes = self.interpreter.get_tensor(
                    self.output_details[0]['index'])[0]
                self.model.classes = self.interpreter.get_tensor(
                    self.output_details[1]['index'])[0]
                self.model.scores = self.interpreter.get_tensor(
                    self.output_details[2]['index'])[0]
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
