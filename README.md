# Project

Implement a simulator of a quadcopter.
Specifically:
- Quadcopter with two cameras in front, in a city setting
- Use Unity 3D, with physics
- Support MAVSDK API
- Integration with OpenAI Gym, PyTorch

<table>
  <tr>
    <td><img src="https://user-images.githubusercontent.com/114035408/235415007-f5ea07c0-315e-411c-8bfa-f9141efa088e.jpg" alt="Image 1" ></td>
    <td><img src="https://user-images.githubusercontent.com/114035408/235415011-2c9dcf07-e7a2-4784-b1ff-98c5c634f5b5.jpg" alt="Image 2" width="200" height="200"></td>
  </tr>
</table>

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
