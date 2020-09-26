import Modules.TfBinding as TF
from Modules.TCPServer import Server
from Modules.VideoStream import VideoStream

class ThreadController(object):
    """Threadcontroller to Execute Threads async"""

    def __init__(self, model):
        self.model = model
        
    def Start(self):
        self.VideoStream = VideoStream(self.model)
        self.TfBinding = TF.Inception(self.model)
        self.TCPServer = Server(self.model)
        
    def RestartVideoStream(self):
        self.VideoStream.Stop()
        del self.VideoStream
        self.VideoStream = VideoStream(self.model)
        
    def RestartTensorflow(self):
        self.TfBinding.Stop()
        del self.TfBinding
        self.TfBinding = TF.Inception(self.model)
    
    def RestartTCPServer(self):
        self.TCPServer.Stop()
        del self.TCPServer
        self.TCPServer = Server(self.model)
    
    def Stop(self):
        self.VideoStream.Stop()
        self.TfBinding.Stop()
        self.TCPServer.Stop()
        