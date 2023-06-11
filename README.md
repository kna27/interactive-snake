# Interactive Snake Game

## Created by Krish Arora and Matthew Lerman

Camera/UDP Socket code adapted from Vartan Yildiz and Vedic Patel

This is an interactive version of the classic Snake game that can be played with an audience. The left and right side's of the audience control which way the snake travels by using their phone's flashlights.

This game has a holiday theme as it was created for the Bergen County Academies' 2022 Holiday Assembly.

## Demonstration Video
https://github.com/kna27/interactive-snake/assets/43799189/ccf3fd9d-9b57-4766-9898-9da0c2900989

## Setting Up
This project uses Unity 2021.3.12f1 and cv2 to get camera input from a Python script, and then a UDP socket to send that information to the game.

To run, first build the project for Windows to `Snake/Build` then run the appropriate Python camera script for your needs:
- 1_camera.py: Use this if you are using one camera (such as a webcam, recommended for testing purposes)
- 2_camera.py: Use this if you are using two cameras (recommended if being used for an actual audience)
- random.py: Use this to feed random data for left and right for testing
