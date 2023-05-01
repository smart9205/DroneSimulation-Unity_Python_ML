# Project

Implement a simulator of a quadcopter.
Specifically:
- Quadcopter with two cameras in front, in a city setting
- Use Unity 3D, with physics
- Support MAVSDK API
- Integration with OpenAI Gym, PyTorch


# How to run the project

1. Run mavsdk_server.exe

This is mavsdk server for drone simulation just like embedded mavsdk server in real PX4 drone.


2. Run drone_module.py

To do this, need to install "pymavlink" for python.

"pip install pymavlink"

This file is for connecting mavsdk-server with drone in unity 3d.


3. open unity project and run it.

4. Run takeoffland.py to test.

https://drive.google.com/file/d/19VI0wijK_d7eprz5XuBIrK5zjnBK_v4M/view?usp=sharing