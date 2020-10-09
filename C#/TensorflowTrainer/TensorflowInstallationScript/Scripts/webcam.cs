using System;
using System.Collections.Generic;
using System.Text;

namespace TensorflowInstallationScript.Scripts
{
	public class Webcam : BaseScript
	{
		public Webcam()
		{
			ScriptName = "webcam.py";
			Script = @"

import os
import tensorflow as tf
from object_detection.utils import label_map_util
from object_detection.utils import visualization_utils as viz_utils
import cv2
import matplotlib.pyplot as plt
import numpy as np

RESOLUTION = (1280, 720)
PATH_TO_SAVED_MODEL = 'export/normal/saved_model'
PATH_TO_LABELS = 'training/labelmap.pbtxt'
MIN_SCORE_THRESH = 0.6


model = os.path.join(os.getcwd(), PATH_TO_SAVED_MODEL)
labels = os.path.join(os.getcwd(), PATH_TO_LABELS)
# Enable GPU dynamic memory allocation
gpus = tf.config.experimental.list_physical_devices('GPU')
for gpu in gpus:
    tf.config.experimental.set_memory_growth(gpu, True)
    


detect_fn = tf.saved_model.load(model)
category_index = label_map_util.create_category_index_from_labelmap(labels,use_display_name=True)
cap = cv2.VideoCapture(0  + cv2.CAP_DSHOW)
ret = cap.set(3, RESOLUTION[0])
ret = cap.set(4, RESOLUTION[1])
while(True):
    (status, frame) = cap.read()
    input_tensor = tf.convert_to_tensor(frame)
    input_tensor = input_tensor[tf.newaxis, ...]
    detections = detect_fn(input_tensor)
    num_detections = int(detections.pop('num_detections'))
    detections = {key: value[0, :num_detections].numpy()
                   for key, value in detections.items()}
    detections['num_detections'] = num_detections

    # detection_classes should be ints.
    detections['detection_classes'] = detections['detection_classes'].astype(np.int64)

    image_np_with_detections = frame.copy()

    viz_utils.visualize_boxes_and_labels_on_image_array(
          image_np_with_detections,
          detections['detection_boxes'],
          detections['detection_classes'],
          detections['detection_scores'],
          category_index,
          use_normalized_coordinates=True,
          max_boxes_to_draw=10,
          min_score_thresh=MIN_SCORE_THRESH,
          agnostic_mode=False)

    cv2.imshow('image',image_np_with_detections)
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break
cap.release()
cv2.destroyAllWindows()
";
		}
	}
}
