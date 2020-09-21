import sys
from Modules.NameService import HandStates
import os
import socket
from threading import Thread
import time


class Server(object):
    """Server to send/recieve Data"""

    def __init__(self,model ,bufferSize=1024):
        self.model = model
        self.BUFFER_SIZE = bufferSize
        self.conn = None
        self.addr = None
        self.echo = None
        self.Tcp_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.Tcp_socket.setsockopt(socket.SOL_SOCKET,socket.SO_REUSEADDR, 1)
        self.Tcp_socket.bind(('', self.model.port))
        self.Tcp_socket.listen(5)
        self.thread = Thread(target=self.sendData, args=())
        self.IsActive = True
        self.thread.daemon = True
        self.thread.start()

    def sendData(self):
        ##ConnectionResetError
            while self.IsActive:
                if(self.model.serverActive):
                    try:
                        self.conn, self.addr = self.Tcp_socket.accept()
                        while self.IsActive:
                                self.echo = self.conn.recv(self.BUFFER_SIZE)
                                if(self.model.serverActive):
                                    self.conn.sendall(self.model.package)
                                else:
                                    self.conn.sendall(bytearray([0x00, 0x00, 0x00, 0x00, 0x00]))
                    except (ConnectionResetError,OSError) as exc:
                        self.conn = None
                        self.addr = None
                        self.echo = None
                                              
            if(not self.conn == None):
                self.conn.close()
    
    def Stop(self):
        self.IsActive = False
        time.sleep(0.5)
        self.Tcp_socket.close()
        self.Tcp_socket = None
        


        
    