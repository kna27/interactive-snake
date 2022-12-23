import UdpComms as U
import cv2 as cv
import numpy as np
import matplotlib.pyplot as plt
import time
import os
import argparse
import random

'''
This programs uses UdpComms.py from Siliconifier's github repository: https://github.com/Siliconifier/Python-Unity-Socket-Communication
'''

#Arguments
parser = argparse.ArgumentParser()
parser.add_argument("--save_video", default = False, type = bool, help = "True or False to save video of all camera feed")
parser.add_argument("--skip_callibration", default = False, type = bool, help = "True or False to skip callibration process")
parser.add_argument("--fps", default = 30, type = int, help = "Set FPS of all cameras")
parser.add_argument("--width", default = 1280, type = int, help = "Set width for cameras")
parser.add_argument("--height", default = 720, type = int, help = "Set height for cameras")
args = parser.parse_args()

#Initializes the game
os.startfile("interactive-design-snake-main\Snake\Build\interactive-design-snake.exe")

# Create UDP socket to use for sending (and receiving)
sock = U.UdpComms(udpIP="127.0.0.1", portTX=8000, portRX=8001, enableRX=True, suppressWarnings=True)
i = 0
dataArray = [25, 25]
final = "000000"

LeftValue = random(0, 100)
RightValue = random(0, 100)

while True:

    #Send Stuff
    dataArray[0] = LeftValue
    dataArray[1] = RightValue
    print("Array:")
    print(dataArray)

    data_str = "" 
    for i in dataArray:
        if i < 100:
            data_str += "0"
        if i < 10:
            data_str += "0"
        data_str += str(i)
    #print(data_str)
    sock.SendData(data_str) # Send this string to other application
    i += 1
    data = sock.ReadReceivedData() # read data
    if data != None: # if NEW data has been received since last ReadReceivedData function call
        print(data) # print new received data

    #How to Exit Loop
    if ( cv.waitKey(5) & 0xFF==ord("q") ):
        break
    print("")

print("End")

cv.destroyAllWindows
