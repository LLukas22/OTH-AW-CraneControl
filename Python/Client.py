import socket
import time

port = 54000
powerstate = False   
s = socket.socket()          
s.connect(('localhost', port)) 
lastpowermsg = False
print("Client Created") 

def BuildMessage(bytearray):
    commands = "Package Recieved from Server: "
    global powerstate
    global lastpowermsg
    
    #if(bytearray[0] == 1 and bytearray[0] != lastpowermsg):
    #    powerstate = not powerstate
    #lastpowermsg = bytearray[0]

    #if(powerstate):
    if(bytearray[0]):
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

 
while True:
    try:
        s.send("Recieved".encode())
        print(BuildMessage(s.recv(1024)))
        time.sleep(1)
    except ConnectionResetError as identifier:
        print("ConnectionResetError")
    except ConnectionAbortedError as exc:
        print("ConnectionAbortedError")

s.close()   