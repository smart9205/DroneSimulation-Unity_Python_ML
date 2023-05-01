from pymavlink import mavutil
import time
import threading
import UdpComms as U
import asyncio
import multiprocessing

sock = U.UdpComms(udpIP="127.0.0.1", portTX=8000, portRX=8001, enableRX=True, suppressWarnings=True)

the_connection = mavutil.mavlink_connection('udpout:127.0.0.1:14540', baud=57600)

status_code = mavutil.mavlink.MAV_SEVERITY_NOTICE
status_txt = "Drone is healthy"
 
drone_state = "off"
telemetry = {"Altitude":0, "Speed":0, "Roll":0, "Pitch":0, "Heading":0, "isGrounded":-1, "isActive":-1}

takeoff_alt = 10
exit_event = multiprocessing.Event()

state = mavutil.mavlink.MAV_STATE_ACTIVE
#   MAV_STATE_STANDBY
#   MAV_STATE_EMERGENCY
#   MAV_STATE_POWEROFF
#   MAV_STATE_FLIGHT_TERMINATION

mode_flag = mavutil.mavlink.MAV_MODE_FLAG_AUTO_ENABLED
# 	MAV_MODE_FLAG_SAFETY_ARMED
#   MAV_MODE_FLAG_MANUAL_INPUT_ENABLED
#   MAV_MODE_FLAG_STABILIZE_ENABLED
#   MAV_MODE_FLAG_GUIDED_ENABLED
#   MAV_MODE_FLAG_AUTO_ENABLED
#   MAV_MODE_FLAG_TEST_ENABLED
#   MAV_MODE_FLAG_CUSTOM_MODE_ENABLED

def sendCommand(action):
    sock.SendData(action)

def takeoff(exit_event):
    while(telemetry["Altitude"] < 10):
        print("------takeoff") 
        print(telemetry["Altitude"])     
        sendCommand("takeoff")
        time.sleep(0.3)
        if (exit_event.is_set()):
            break
    print("--------exit taking off")
    print(telemetry["Altitude"])  

def land():
    while (telemetry["isGrounded"] != 1):
        print("------land")
        sendCommand("land")
        time.sleep(1)
    exit_event.clear()

async def connect():
    while True:
        data = sock.ReadReceivedData() # read data
        if data != None: # if NEW data has been received since last ReadReceivedData function call
            break    


def send_heartbeat():
    while True:
        the_connection.mav.heartbeat_send(mavutil.mavlink.MAV_TYPE_QUADROTOR, mavutil.mavlink.MAV_AUTOPILOT_GENERIC, 0, 0, 0)
        
        msg = the_connection.recv_match(type='COMMAND_LONG')        
        if msg:
            the_connection.mav.extended_sys_state_send(mavutil.mavlink.MAV_VTOL_STATE_UNDEFINED, mavutil.mavlink.MAV_LANDED_STATE_ON_GROUND)
            the_connection.mav.command_ack_send(msg.command, mavutil.mavlink.MAV_RESULT_ACCEPTED)
            if msg.command == mavutil.mavlink.MAV_CMD_COMPONENT_ARM_DISARM:
                if msg.param1 == 1.0:
                    print("------arming")
                    sendCommand("arm")
                if msg.param1 == 0.0:
                    print("------disarming")
                    sendCommand("disarm")
            if msg.command == mavutil.mavlink.MAV_CMD_NAV_TAKEOFF:             
                print("------takeoff") 
                print(telemetry["Altitude"])     
                sendCommand("takeoff")
            if msg.command == mavutil.mavlink.MAV_CMD_NAV_LAND:  
                print("------land")
                sendCommand("land")         

        data = sock.ReadReceivedData() # data from Drone simulation
        if data != None:
            print(data) 
            data = data.split(",")
            for word in data:
                telemetry[word.split(':')[0]] = int(word.split(':')[1])

        time.sleep(0.5) 

async def run():         
    heartbeat_thread = threading.Thread(target=send_heartbeat)
    heartbeat_thread.start()



if __name__ == "__main__":
    asyncio.run(run())






############################### COMMAND PROTOCOL ###################################

# command - 511:
# 	MAV_CMD_SET_MESSAGE_INTERVAL
# 	means: Request the target system(s) emit a single instance of a specified message

# command - 400, param1 - 1.0:
# 	MAV_CMD_COMPONENT_ARM_DISARM command
# 	means: arm or disarm
# 	1.0  =>  arm
# 	0.0  =>  disarm

# command - 22:
# 	MAV_CMD_NAV_TAKEOFF
# 	means: Takeoff from ground / hand

# 	1: Pitch	Minimum pitch (if airspeed sensor present), desired pitch without sensor	deg	
# 	2	Empty	
# 	3	Empty	
# 	4: Yaw	Yaw angle (if magnetometer present), ignored without magnetometer. NaN to use the current system yaw heading mode (e.g. yaw towards next waypoint, yaw to home, etc.).	deg
# 	5: Latitude	Latitude	
# 	6: Longitude	Longitude	
# 	7: Altitude	Altitude

# command - 21:
# 	MAV_CMD_NAV_LAND
# 	means: Land at location.

# 	1: Abort Alt	Minimum target altitude if landing is aborted (0 = undefined/use system default).		m
# 	2: Land Mode	Precision land mode.	PRECISION_LAND_MODE	
# 	3	Empty.		
# 	4: Yaw Angle	Desired yaw angle. NaN to use the current system yaw heading mode (e.g. yaw towards next waypoint, yaw to home, etc.).		deg
# 	5: Latitude	Latitude.		
# 	6: Longitude	Longitude.		
# 	7: Altitude	Landing altitude (ground level in current frame).

