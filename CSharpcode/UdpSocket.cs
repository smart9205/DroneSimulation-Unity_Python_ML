

using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UdpSocket : MonoBehaviour
{
    [HideInInspector] public bool isTxStarted = false;

    [SerializeField] string IP = "127.0.0.1"; // local host
    [SerializeField] int rxPort = 8000; // port to receive data from Python on
    [SerializeField] int txPort = 8001; // port to send data to Python on

    // Create necessary UdpClient objects
    UdpClient client;
    IPEndPoint remoteEndPoint;
    Thread receiveThread; // Receiving Thread

    PythonTest pythonTest;
    DroneHUD droneHUD;
    DroneMainScript droneScript;

    
    IEnumerator SendDataCoroutine() // DELETE THIS: Added to show sending data from Unity to Python via UDP
    {
        int i = 0;
        while (true)
        {
            string alt = "Altitude:" + droneHUD.altitude.ToString("0");
            string speed = "Speed:" + droneHUD.absSpeed.ToString("00");
            string roll = "Roll:" + droneHUD.roll.ToString("000");
            string pitch = "Pitch:" + droneHUD.pitch.ToString("000");
            string heading = "Heading:" + (droneHUD.heading < 0 ? (droneHUD.heading + 360f).ToString("000") : droneHUD.heading.ToString("000"));
            string isGrounded = "isGrounded:" + (droneScript.isGrounded ? 1 : 0).ToString();
            string isActive = "isActive:" + (droneScript.isActive ? 1 : 0).ToString();

            string data = alt + "," + speed + "," + roll + "," + pitch + "," + heading + "," + isGrounded + "," + isActive;
            //SendData("Sent from Unity: " + i.ToString());
            SendData(data);
            i++;
            yield return new WaitForSeconds(1f);
        }
    }

    public void SendData(string message) // Use to send data to Python
    {
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            client.Send(data, data.Length, remoteEndPoint);
        }
        catch (Exception err)
        {
            print(err.ToString());
        }
    }

    void Awake()
    {
        // Create remote endpoint (to Matlab) 
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), txPort);

        // Create local client
        client = new UdpClient(rxPort);

        // local endpoint define (where messages are received)
        // Create a new thread for reception of incoming messages
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();

        // Initialize (seen in comments window)
        print("UDP Comms Initialised");
    }

    private void Start() 
    {
        pythonTest = FindObjectOfType<PythonTest>(); // Instead of using a public variable
        droneHUD = FindObjectOfType<DroneHUD>();
        droneScript = FindObjectOfType<DroneMainScript>();
        StartCoroutine(SendDataCoroutine()); // DELETE THIS: Added to show sending data from Unity to Python via UDP
    }

    // Receive data, update packets received
    private void ReceiveData()
    {
        while (true)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref anyIP);
                string text = Encoding.UTF8.GetString(data);
                ProcessInput(text);
                
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }

    private void ProcessInput(string input)
    {
        // PROCESS INPUT RECEIVED STRING HERE
        pythonTest.UpdatePythonRcvdText(input); // Update text by string received from python
        
        if (!isTxStarted) // First data arrived so tx started
        {
            isTxStarted = true;
        }
    }

    //Prevent crashes - close clients and threads properly!
    void OnDisable()
    {
        if (receiveThread != null)
            receiveThread.Abort();

        client.Close();
    }

}