import socket
import time
from time import sleep

port = 54000
powerstate = False   
s = socket.socket()          
s.connect(('192.168.188.42', port)) 
lastpowermsg = False
print("Client Created") 

def BuildMessage(bytearray):
    commands = "Package Recieved from Server: "
    global powerstate
    global lastpowermsg
    
    if(bytearray[0] == 1 and bytearray[0] != lastpowermsg):
      powerstate = not powerstate
    lastpowermsg = bytearray[0]

    if(powerstate):
        commands += "State: Active, "
    else:
        commands += "State: Deactivated, "

    if(bytearray[4] == 1):
        commands += "Regler: ON,"
    else:
        commands += "Regler: OFF,"

    if(bytearray[3]==1):
        commands += "Direction: UP"
    if(bytearray[3]==2):
        commands += "Direction: DOWN"
    if(bytearray[1]>0):
        commands += "Direction: LEFT, Velocity: "+ str(int(bytearray[1]))
    if(bytearray[2]>0):
        commands += "Direction: RIGHT, Velocity: "+ str(int(bytearray[2]))
    return commands

 
lastPackage = None
while True:
    try:
        if lastPackage is not None:
            s.send(lastPackage)
        else:
            s.send("TryConnect".encode())
        lastPackage = s.recv(1024)
        print(BuildMessage(lastPackage)+"\t Package:"+str(lastPackage))
        sleep(0.25)
    except ConnectionResetError as identifier:
        print("ConnectionResetError")
    except ConnectionAbortedError as exc:
        print("ConnectionAbortedError")
 