import sys
from Modules.NameService import HandStates
import os
import socket
from threading import Thread
import time


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


class Server(object):
    """Server to send/recieve Data"""

    def __init__(self, port, labels,threshhold, bufferSize=1024):
        self.threshhold = threshhold
        self.labels = labels
        self.BUFFER_SIZE = bufferSize
        self.HandBuffer = []
        self.ActiveHandstate = HandStates.No
        self.payload = Payload()
        self.conn = None
        self.addr = None
        self.Tcp_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.Tcp_socket.setsockopt(socket.SOL_SOCKET,socket.SO_REUSEADDR, 1)
        self.Tcp_socket.bind(('', port))
        self.Tcp_socket.listen(5)
        self.IsActive = True
        self.thread = Thread(target=self.sendData, args=())
        self.thread.daemon = True
        self.thread.start()

    def sendData(self):
        ##ConnectionResetError
            while self.IsActive:
                try:
                    self.conn, self.addr = self.Tcp_socket.accept()
                    while self.IsActive:
                        rec = self.conn.recv(self.BUFFER_SIZE)
                        self.conn.sendall(self.payload.buildPackage())
                except (ConnectionResetError,OSError) as exc:
                    self.conn = None
                    self.addr = None
            if(not self.conn == None):
                self.conn.close()

    def Update(self, classes, scores):
        # Find Best detection and Append to HandBuffer
        try:
            self.validscores = []
            for score in scores:
                if(score > 0.00 and score < 1.00):
                    self.validscores.append(score)
            if self.validscores:
                self.current = max(self.validscores)
                if(self.current is not None):
                    if (self.current > self.threshhold):
                        object_name = self.labels[int(
                            classes[self.validscores.index(self.current)])]
                        switcher = {
                            'Up': HandStates.Up,
                            'Down': HandStates.Down,
                            'Left': HandStates.Left,
                            'Right': HandStates.Right,
                            'Power': HandStates.Power,
                            'Toggle': HandStates.Toggle
                        }
                        detection = switcher.get(object_name, HandStates.No)
                    else:
                        detection = HandStates.No
            else:
                detection = HandStates.No

            self.HandBuffer.append(detection)
            if(len(self.HandBuffer) > 30):
                self.HandBuffer.remove(self.HandBuffer[0])
            self.RefreshData()
        except IndexError as exe:
            self.validscores = []
            self.current = None
            print("Exeption Ocurred in TCPServer Update")

    def RefreshData(self):
        activeslice = self.HandBuffer[-3:]
        if(self.ActiveHandstate == HandStates.No):
            if(activeslice.count(activeslice[-1])>=2):
                self.ActiveHandstate = activeslice[-1]
        else:
            if(activeslice.count(self.ActiveHandstate)==0):
                self.ActiveHandstate = HandStates.No
        self.payload.update(self.ActiveHandstate)
        
    def stop(self):
        self.IsActive = False
        time.sleep(0.5)
        self.Tcp_socket.close()
        self.Tcp_socket = None
        


        
    