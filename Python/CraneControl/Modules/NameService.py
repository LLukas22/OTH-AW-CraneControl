from enum import Enum
from PIL import ImageTk, Image
import os

class HandStates(Enum):
    No = 0
    Up = 1
    Down = 2
    Left = 3
    Right = 4
    Power = 5
    Toggle = 6
    
class TfVersion(Enum):
   Tf1 = 0
   Tf2 = 1
   TfLite = 2
    
class Icons(object):
     def __init__ (self):
        self.right = ImageTk.PhotoImage(Image.open(os.path.join(
            os.getcwd(), 'Images/right.png')).resize((50, 50), Image.ANTIALIAS))
        self.left = ImageTk.PhotoImage(Image.open(os.path.join(
            os.getcwd(), 'Images/left.png')).resize((50, 50), Image.ANTIALIAS))
        self.up = ImageTk.PhotoImage(Image.open(os.path.join(
            os.getcwd(), 'Images/up.png')).resize((50, 50), Image.ANTIALIAS))
        self.down = ImageTk.PhotoImage(Image.open(os.path.join(
            os.getcwd(), 'Images/down.png')).resize((50, 50), Image.ANTIALIAS))
        self.regler = ImageTk.PhotoImage(Image.open(os.path.join(
            os.getcwd(), 'Images/regler.png')).resize((50, 50), Image.ANTIALIAS))
        self.poweron = ImageTk.PhotoImage(Image.open(os.path.join(
            os.getcwd(), 'Images/poweron.png')).resize((50, 50), Image.ANTIALIAS))
        self.poweroff = ImageTk.PhotoImage(Image.open(os.path.join(
            os.getcwd(), 'Images/poweroff.png')).resize((50, 50), Image.ANTIALIAS))

      
   