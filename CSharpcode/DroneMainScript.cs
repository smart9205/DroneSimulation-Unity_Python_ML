using UnityEngine;

public class DroneMainScript : MonoBehaviour
{
    public static DroneMainScript current;

    public bool cursorStartLocked = false;

    [Space]
    [Header("Status")]
    public bool isActive = false;
    public bool isGrounded = true;
    public bool isBlocked = false;

    ////
    [Space]
    [Header("General Flight Settings")]
    public ControlType mode = ControlType.Arcade;
    public enum ControlType { Manual = 0, FlyByWire = 1, Arcade = 2 };

    [Space]
    [Tooltip("BiDirectional Throttle - Allows generating downward lift force (AKA 3D mode)")]
    public bool BiDirThrottle = false;
    [Tooltip("Automatically sets enough Throttle to hover and/or automatically trims it to 50% on the stick.")]
    public bool hoverThrottle = true;
    [Range(0, 1)] public float deadZoneHT = 0.1f;

    [Space]
    [Range(0, 1)] public float startBattery = 0.95f;
    [Tooltip("The Time (in Minutes) to consume 100% Battery when Engine running at Iddle RPM - Set to 0 for no fuel consumption")]
    public float batteryMaxTime = 8f;
    [Tooltip("The Time (in Minutes) taken to consume 100% Battery with Engine running at max RPM - Set to 0 for no fuel consumption")]
    public float batteryMinTime = 1.6666666f;

    [Space]
    public bool allowDamage = true;
    public float groundHeight = 0.5f;

    [Space]
    [Range(0, 2)] public float pitchFactor = 1f;
    [Range(0, 2)] public float rollFactor = 1f, yawFactor = 1f;

    [Space]
    [Range(0, 10)] public float throttleFactor = 2f;
    [Range(0, 10)] public float thrustFactor = 2f;

    [Space]
    [Tooltip("Gives an extra horizontal thrust on Manual Mode to make easier to move - For Realistic Simulations set it to 0")]
    public float thrustAssist = 1;


    //// FlyByWire
    [Space]
    [Header("Fly By Wire Settings")]
    public float fbwPitchAngle = 15;
    public float fbwRollAngle = 30;

    [Space]
    [Tooltip("Amount of yaw rotation added during a Roll to facilitate turns - Arcade Mode only!")]
    [Range(0f, 1f)]
    public float coordinatedTurn = 0.25f;

    [Space]
    [Range(0.01f, 0.1f)] public float fbwResponseFactor = 0.05f;
    [Range(0f, 0.1f)] public float dampResponseFactor = 0.05f;

    float fbwPitchInput, fbwRollInput;
    Vector3 currentAngle, lastAngle;
    ////


    [Space]
    [Header("Turbulence and Wind")]

    public bool useTurbulence = true;
    [Range(0, 1f)] public float turbIntensity = 0.05f;
    [Range(0, 1f)] public float turbVelFactor = 0.1f;

    [Space]
    public bool useWind = false;
    public float windVel = 0.125f;
    public Vector3 windDir = new Vector3(1f, 0f, 0f);
    ////


    ////
    bool clampInput = true;

    [Space]
    [Header("Input Settings")]
    public bool useKeyboard = true;
    public KeyCode toogleCursorKey = KeyCode.Tab, recoverKey = KeyCode.Space, modeKey = KeyCode.Backspace, cameraKey = KeyCode.C;
    public KeyCode toogleActive = KeyCode.G;
    /// <summary>
    /// Main Key : WS AD QE RF
    /// </summary>
    public float pitchKeyFactor = 1f;
    public KeyCode pitchDown = KeyCode.W, pitchUp = KeyCode.S;

    public float rollKeyFactor = 1f;
    public KeyCode rollLeft = KeyCode.A, rollRight = KeyCode.D;

    public float yawKeyFactor = 1f;
    public KeyCode yawLeft = KeyCode.Q, yawRight = KeyCode.E;

    public float throttleKeyFactor = 1f;
    public KeyCode throttleUp =  KeyCode.R, throttleDown =  KeyCode.F;

    public float thrustForwardKeyFactor = 1f;
    public KeyCode thrustForward = KeyCode.None, thrustBackward = KeyCode.None; //KeyCode.Keypad8, KeyCode.Keypad2

    public float thrustLateralKeyFactor = 1f;
    public KeyCode thrustLeft = KeyCode.None, thrustRight = KeyCode.None; // KeyCode.Keypad4, KeyCode.Keypad6



    [Space]
    public bool useMouse = true;

    public float pitchMouseFactor = 1f;
    public string pitchMouse = "Mouse Y";

    public float rollMouseFactor = 1f;
    public string rollMouse = "Mouse X";

    public float yawMouseFactor = 1f;
    public string yawMouse = "";    //"Mouse X"

    public float throttleMouseFactor = 1f;
    public string throttleMouse = ""; //"Mouse ScrollWheel"

    public float thrustForwardMouseFactor = 1f;
    public string thrustForwardMouse = ""; //"Mouse Y"

    public float thrustLateralMouseFactor = 1f;
    public string thrustLateralMouse = ""; //Mouse X"


    [Space]
    public bool useMobile = true;
    [Tooltip("Show Mobile controls even in other platforms")]public bool forceMobile = false;
    public float pitchMobileFactor = 1f, rollMobileFactor = 1f, yawMobileFactor = 1f, throttleMobileFactor = 1f, thrustForwardMobileFactor = 1f, thrustLateralMobileFactor = 1f;


    [Space]
    public bool useJoystick = true;

    public KeyCode recoverJoystick = KeyCode.Joystick1Button1, modeJoystick = KeyCode.Joystick1Button2, cameraJoystick = KeyCode.Joystick1Button3;

    public float pitchAxisFactor = 1f;
    public string pitchAxis = "Vertical";

    public float rollAxisFactor = 1f;
    public string rollAxis = "Horizontal";

    public float yawAxisFactor = 1f;
    public string yawAxis = ""; //"Yaw"

    public float throttleAxisFactor = 1f;
    public string throttleAxis = ""; //"Throttle"

    public float thrustForwardAxisFactor = 1f;
    public string thrustForwardAxis = ""; //"Vertical"

    public float thrustLateralAxisFactor = 1f;
    public string thrustLateralAxis = ""; //"Horizontal"

    [Space]
    public bool useMAVSDKApi = true;
    public float pitchApiFactor = 1f;
    public int pitchApi;

    public float rollApiFactor = 1f;
    public int rollApi;

    public float yawApiFactor = 1f;
    public int yawApi;    //"Mouse X"

    public float throttleApiFactor = 1f;
    public int throttleApi; //"Mouse ScrollWheel"

    public float thrustForwardApiFactor = 1f;
    public int thrustForwardApi; //"Mouse Y"

    public float thrustLateralApiFactor = 1f;
    public int thrustLateralApi; //Mouse X"
    ////


    //
    [Header("References")]

    [Space]
    public Rigidbody rigidBody;

    [Space]
    public DroneHUD droneHUD;
    public RectTransform pitchArea, mobileInput;
    public GameObject[] cameras;
    int cameraIndex = 0, camNull = 0;

    [Space]
    public AudioClip touchSND;
    public AudioClip hitSND, damageSND;
    public string touchMSG = "Touch", hitMSG = "Hit!", damageMSG = "Propeller Damage!";

    [Space]
    public FlashImg recoverFlashImgBut;
    public Spinning[] propellers;


    [Space]
    [Header("Read Only!")]
    public Vector3 inputTorque;
    public Vector3 inputForce;
    //

    //drag is a property of the rigidbody component that determines how much an object slows down over time due to air resistance or other forces.
    float linearDrag = 0, angDrag = 0;

    PythonTest pythonTest;

    public float takeoffVelocity = 5f; // m/s
    public float takeoffAltitude = 10f; // meters
    public float takeoffThrust = 1f; // normalized thrust factor

    private bool isTakingOff = false;
    private bool isLanding = false;

    private bool isFlygingToTarget= false;
    public float latitude = -86.4f;
    public float longitude = -54.0f;
    public float target_alt = 20f;
    public float speed = 5f;

    private Vector3 targetPosition;


    void Awake()
    {
        if (rigidBody == null) rigidBody = GetComponent<Rigidbody>();
        //if (rigidBody != null) rigidBody.maxAngularVelocity = 7f; //raise default value here if necessary

        if (droneHUD != null && droneHUD.autoFuel)
        {
            droneHUD.fuelTarget = startBattery; droneHUD.fuel = startBattery * droneHUD.fuelAmplitude;
            droneHUD.fuelMaxTime = batteryMaxTime;
            droneHUD.fuelMinTime = batteryMinTime;
        }


        linearDrag = rigidBody.drag;
        angDrag = rigidBody.angularDrag;
    }
    void Start()
    {
        if (cursorStartLocked)
            Cursor.lockState = CursorLockMode.Locked; else Cursor.lockState = CursorLockMode.None;
        pythonTest = FindObjectOfType<PythonTest>();
    }
    void OnEnable()
    {
        if (current != null && current != this) current.transform.parent.gameObject.SetActive(false);
        current = this;

    }
    //
    public void detectControls()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld || forceMobile) useMobile = true; else useMobile = false;
        if (mobileInput != null) mobileInput.gameObject.SetActive(useMobile); else useMobile = false;

        if (Input.GetJoystickNames().Length > 0 && Input.GetJoystickNames().GetValue(0).ToString() != "" ) useJoystick = true;
        else useJoystick = false;

        //print(Input.GetJoystickNames().Length);
        //foreach (string i in Input.GetJoystickNames()) print(i);
    }
    //////////////////



    void FixedUpdate()
    {
        //Return if not activated
        if (!isActive) return;


        //////////////// Current and Last frame Angles
        float currentX = 0, currentY = 0, currentZ = 0;

        currentX = rigidBody.rotation.eulerAngles.x % 360;
        if (currentX > 180) currentX -= 360; else if (currentX < -180) currentX += 360;

        currentY = rigidBody.rotation.eulerAngles.y % 360;
        if (currentX > 180) currentX -= 360; else if (currentX < -180) currentX += 360;

        currentZ = rigidBody.rotation.eulerAngles.z % 360;
        if (currentZ > 180) currentZ -= 360; else if (currentZ < -180) currentZ += 360;

        currentAngle = new Vector3(currentX, currentY, currentZ);
        ////////////////




        // changed manual

       



        if ( (mode == ControlType.FlyByWire || mode == ControlType.Arcade) && !isBlocked)
        {
            if (hoverThrottle)
            {
                if (!BiDirThrottle && throttleFactor != 0)
                {
                    if (inputForce.y < 0.5f + deadZoneHT) inputForce.y = Mathf.Lerp(0, (1f / throttleFactor), inputForce.y / 0.5f);
                    else if (inputForce.y >= 0.5f - deadZoneHT) inputForce.y = Mathf.Lerp((1f / throttleFactor), throttleFactor, (inputForce.y - 0.5f) / 0.5f);
                    else inputForce.y = (1f / throttleFactor);
                }
            }
            //


            //
            rigidBody.AddForce
                (
                (isGrounded ? 0 : 1) *  inputTorque.x * thrustFactor * rigidBody.mass * Physics.gravity.magnitude * Vector3.ProjectOnPlane(rigidBody.transform.forward, Vector3.up).normalized +
                (isGrounded ? 0 : 1) * -inputTorque.z * thrustFactor * rigidBody.mass * Physics.gravity.magnitude * Vector3.ProjectOnPlane(  rigidBody.transform.right, Vector3.up).normalized +
                (inputForce.y * throttleFactor * rigidBody.mass * Physics.gravity.magnitude * Vector3.up) //(mode == ControlType.Arcade ? 1 : 0) *
                , ForceMode.Force);
            //

            rigidBody.AddRelativeForce(
                inputForce.x * thrustFactor * rigidBody.mass * Physics.gravity.magnitude,
                0,//(mode == ControlType.FlyByWire ? 1 : 0) * inputForce.y * throttleFactor * rigidBody.mass * Physics.gravity.magnitude,
                inputForce.z * thrustFactor * rigidBody.mass * Physics.gravity.magnitude,
                ForceMode.Force);
            //
            rigidBody.AddRelativeTorque
                (
                (inputTorque.x * fbwPitchAngle - currentAngle.x) * rigidBody.mass * pitchFactor * fbwResponseFactor * Vector3.right +
                 ((currentAngle.x - lastAngle.x) / Time.fixedDeltaTime) * (-dampResponseFactor / 10f) * rigidBody.mass * pitchFactor * Vector3.right +

                 inputTorque.y * rigidBody.mass * yawFactor * Vector3.up +

                (inputTorque.z * fbwRollAngle - currentAngle.z) * rigidBody.mass * rollFactor * fbwResponseFactor * Vector3.forward +
                ((currentAngle.z - lastAngle.z) / Time.fixedDeltaTime) * (-dampResponseFactor / 10f) * rigidBody.mass * rollFactor * Vector3.forward + 
                // changed acade
                0 * ( -coordinatedTurn / 10f * inputTorque.z * fbwRollAngle) * rigidBody.mass * yawFactor * Vector3.up
                , ForceMode.Force);
            //

        }


        if (useTurbulence && !isGrounded && (!isBlocked || (isBlocked && rigidBody.velocity.magnitude > 0.1f)) )
        {
            rigidBody.AddRelativeTorque(
                rigidBody.mass * (isBlocked ? 5f : 1f) *
                new Vector3 
                ( 
                    Random.Range(-turbIntensity, turbIntensity) * (1 + rigidBody.velocity.magnitude * turbVelFactor)
                    , 
                    Random.Range(-turbIntensity / 2f, turbIntensity / 2f) * (1 + rigidBody.velocity.magnitude * turbVelFactor / 2f)
                    , 
                    Random.Range(-turbIntensity, turbIntensity) * (1 + rigidBody.velocity.magnitude * turbVelFactor)
                )
                , ForceMode.Force);
        }



        if (useWind && !isGrounded)
        {
            rigidBody.AddForce( rigidBody.mass * Physics.gravity.magnitude * windVel * windDir.normalized, ForceMode.Acceleration);
        }



        lastAngle = currentAngle;

        if (isFlygingToTarget)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, targetPosition);

            if (distance > 0.1f)
            {
                rigidBody.MovePosition(transform.position + direction * speed * Time.fixedDeltaTime);
            }
        }
    }




    public void StopMove() {
        if (!this.isActive)
        {
            this.isActive = true;
            foreach (Spinning obj in propellers)
            {
                obj.isActive = true;
            }

        }
        else if (isGrounded)
        {
            this.isActive = false;
            foreach (Spinning obj in propellers)
            {
                obj.isActive = false;
            }
        }

    }

    public void Arm()
    {
        if (!this.isActive)
        {
            this.isActive = true;
            foreach (Spinning obj in propellers)
            {
                obj.isActive = true;
            }

        }
    }

    public void Disarm()
    {
        print("-----------------dis arm -------");
        print(isGrounded);
        if (isGrounded)
        {
            this.isActive = false;
            foreach (Spinning obj in propellers)
            {
                obj.isActive = false;
            }
        }
    }

    void Update()
    {
        //
        if (useKeyboard && Input.GetKeyDown(cameraKey)) changeCamera();
        if (useJoystick && Input.GetKeyDown(cameraJoystick)) changeCamera();
        //


        if (Input.GetKeyDown(toogleActive))
        {

            StopMove();

        }

        if (pythonTest.isUpdated == true)
        {
            pythonTest.isUpdated = false;
            string cmd = pythonTest.tempStr;
            print(cmd);

            pitchApi = 0;
            rollApi = 0;
            yawApi = 0;
            throttleApi = 0;
            useMAVSDKApi = true;
            if (cmd == "arm")
                Arm();
            if (cmd == "disarm")
                Disarm();
            if (cmd == "takeoff")
                isTakingOff = true;
                //throttleApi = 1;
            if (cmd == "land")
                isLanding = true;
                //throttleApi = -1;
            if (cmd == "forward")
                pitchApi = 1;
            if (cmd == "backward")
                pitchApi = -1;
            if (cmd == "left")
                rollApi = 1;
            if (cmd == "right")
                rollApi = -1;
            if (cmd == "yawLeft")
                yawApi = -1;
            if (cmd == "yawRight")
                yawApi = -1;
            //if (cmd = "")
            if (cmd == "end")
                useMAVSDKApi = false;
        }
        else
            useMAVSDKApi = false;



        if (!isActive) return;

        float alt = transform.position.y;
        print("takingoff altitude");
        print(alt);


        //Cursor lock-unlock with Tab key
        if (Input.GetKeyDown(toogleCursorKey))
        {
            if (Cursor.lockState != CursorLockMode.Locked) { Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; }
            else { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }

        }
        //

        if (useKeyboard && Input.GetKeyDown(recoverKey)) recoverAttitude();
        if (useJoystick && Input.GetKeyDown(recoverJoystick)) recoverAttitude();
        //

        if (useKeyboard && Input.GetKeyDown(modeKey)) toogleArcade();
        if (useJoystick && Input.GetKeyDown(modeJoystick)) toogleArcade();




        RaycastHit hit;
        Physics.Raycast(transform.position, -transform.up, out hit, groundHeight);
        if (hit.collider != null && hit.collider.gameObject != this.gameObject && hit.collider.transform.parent != this.transform)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

       
        if (droneHUD != null)
        {

            print("droneHUD--------------------");
    
            if (droneHUD.autoFuel)
            {
                droneHUD.fuelMaxTime = batteryMaxTime;
                droneHUD.fuelMinTime = batteryMinTime;
            }


            if (droneHUD.fuelTarget <= 0.001f || droneHUD.fuel == 0)
            {
                // changed
                isBlocked = false;
            }

            if (droneHUD.autoRPM)
            {

                print("--------------autoRPM---------------");
                if (!isBlocked) droneHUD.engineTarget = (droneHUD.idleEngine / droneHUD.engineAmplitude + ((Mathf.Abs(inputForce.magnitude) + Mathf.Abs(inputTorque.magnitude) + (droneHUD.absSpeed /(13f * thrustFactor * droneHUD.speedAmplitude) /*droneHUD.maxSpeed*/)) / 3f) * (1 - droneHUD.idleEngine / droneHUD.engineAmplitude));
                else droneHUD.engineTarget = 0;
            }


            for (int i = 0; i <= propellers.Length - 1; i++) { if(propellers[i] != null) propellers[i].factor = droneHUD.engineTarget * Mathf.Sign(inputForce.y); }

            
        }
        //



        if (isBlocked) { inputTorque = Vector3.zero; inputForce = Vector3.zero; return; }
        //


        inputTorque = new Vector3
        (
            (useKeyboard ? pitchKeyFactor * ((Input.GetKey(pitchDown) ? 1 : 0) - (Input.GetKey(pitchUp) ? 1 : 0)) : 0 ) +
            (useJoystick && pitchAxis != "" ? pitchAxisFactor * Input.GetAxis(pitchAxis)  : 0) +
            ((useMouse && Cursor.lockState == CursorLockMode.Locked && pitchMouse != "") ? pitchMouseFactor * Input.GetAxis(pitchMouse) : 0) + 
            (useMAVSDKApi ? pitchApiFactor*pitchApi : 0)
            ,
            (useKeyboard ? yawKeyFactor * ((Input.GetKey(yawRight) ? 1 : 0) - (Input.GetKey(yawLeft) ? 1 : 0)) : 0) +
            (useJoystick && yawAxis != "" ?  yawAxisFactor * Input.GetAxis(yawAxis) : 0) +
            ((useMouse && Cursor.lockState == CursorLockMode.Locked && yawMouse != "") ? yawMouseFactor * Input.GetAxis(yawMouse) : 0) + 
            (useMAVSDKApi ? yawApiFactor * yawApi : 0)
            ,
            (useKeyboard ? rollKeyFactor * ((Input.GetKey(rollLeft) ? 1 : 0) - (Input.GetKey(rollRight) ? 1 : 0)) : 0) +
            (useJoystick && rollAxis != "" ? rollAxisFactor  * -Input.GetAxis(rollAxis) : 0) +
            ((useMouse && Cursor.lockState == CursorLockMode.Locked && rollMouse != "") ? rollMouseFactor * -Input.GetAxis(rollMouse) : 0) +
            (useMAVSDKApi ? rollApiFactor * rollApi : 0)

        );

        //
        if (BiDirThrottle)
        {
            inputForce = new Vector3
            (
                (useKeyboard ? thrustLateralKeyFactor * ((Input.GetKey(thrustRight) ? 1 : 0) - (Input.GetKey(thrustLeft) ? 1 : 0)) : 0) +
                (useJoystick && thrustLateralAxis != "" ? thrustLateralAxisFactor * Input.GetAxis(thrustLateralAxis) : 0) +
                ((useMouse && Cursor.lockState == CursorLockMode.Locked && thrustLateralMouse != "") ? thrustLateralMouseFactor * Input.GetAxis(thrustLateralMouse) : 0) + 0
                
                ,
                (useKeyboard ? throttleKeyFactor * ((Input.GetKey(throttleUp) ? 1 : 0) - (Input.GetKey(throttleDown) ? 1 : 0)) : 0) +
                (useJoystick && throttleAxis != "" ? throttleAxisFactor * -Input.GetAxis(throttleAxis) : 0) +
                ((useMouse && Cursor.lockState == CursorLockMode.Locked && throttleMouse != "") ? throttleMouseFactor * -Input.GetAxis(throttleMouse) : 0) + 
                (useMAVSDKApi ? throttleApiFactor * throttleApi : 0)
                ,
                (useKeyboard ? thrustForwardKeyFactor * ((Input.GetKey(thrustForward) ? 1 : 0) - (Input.GetKey(thrustBackward) ? 1 : 0)) : 0) +
                (useJoystick && thrustForwardAxis != "" ? thrustForwardAxisFactor * Input.GetAxis(thrustForwardAxis) : 0) +
                ((useMouse && Cursor.lockState == CursorLockMode.Locked && thrustForwardMouse != "") ? thrustForwardMouseFactor * Input.GetAxis(thrustForwardMouse) : 0) + 0
                
            );
        }
        else
        {
            inputForce = new Vector3
            (
                (useKeyboard ? thrustLateralKeyFactor * ((Input.GetKey(thrustRight) ? 1 : 0) - (Input.GetKey(thrustLeft) ? 1 : 0)) : 0) +
                (useJoystick && thrustLateralAxis != "" ? thrustLateralAxisFactor * Input.GetAxis(thrustLateralAxis) : 0) +
                ((useMouse && Cursor.lockState == CursorLockMode.Locked && thrustLateralMouse != "") ? thrustLateralMouseFactor * Input.GetAxis(thrustLateralMouse) : 0) + 0
                
                ,
                (useKeyboard ? throttleKeyFactor * ((Input.GetKey(throttleUp) ? 1 : 0)) : 0) +
                (useJoystick && throttleAxis != "" ? throttleAxisFactor * (-Input.GetAxis(throttleAxis) + 1f) / 2f : 0) +
                ((useMouse && Cursor.lockState == CursorLockMode.Locked && throttleMouse != "") ? throttleMouseFactor * (-Input.GetAxis(throttleMouse) + 1f) / 2f : 0) +
                (useMAVSDKApi ? throttleApiFactor * throttleApi : 0)

                ,
                (useKeyboard ? thrustForwardKeyFactor * ((Input.GetKey(thrustForward) ? 1 : 0) - (Input.GetKey(thrustBackward) ? 1 : 0)) : 0) +
                (useJoystick && thrustForwardAxis != "" ? thrustForwardAxisFactor * Input.GetAxis(thrustForwardAxis) : 0) +
                ((useMouse && Cursor.lockState == CursorLockMode.Locked && thrustForwardMouse != "") ? thrustForwardMouseFactor * Input.GetAxis(thrustForwardMouse) : 0) + 0
                
            );
            //
            if (useKeyboard && Input.GetKey(throttleDown)) inputForce = new Vector3(inputForce.x, 0f, inputForce.z);
        }
        //
        ////////////////////////
        ///

        if (isTakingOff)
        {
            // Calculate the current velocity and altitude of the drone
            float velocity = rigidBody.velocity.magnitude;
            float altitude = transform.position.y;

            

            if (altitude < takeoffAltitude)
            {
                inputForce = new Vector3(0, 0.8f, 0);
            }
            else
            {
                rigidBody.velocity = Vector3.zero;
                inputForce = new Vector3(0, 0f, 0);
                isTakingOff = false;
            }
        }

        if (isLanding)
        {
            isTakingOff = false;

            bool isLandEndPoint = false;
            float fLandDistanceLimit = 0.5f;
            /*
            RaycastHit hit_2;
            Physics.Raycast(transform.position, -transform.up, out hit_2, fLandDistanceLimit);
            if (hit_2.collider != null && hit_2.collider.gameObject != this.gameObject && hit_2.collider.transform.parent != this.transform)
            {
                isLandEndPoint = true;
            }
            else
            {
                isLandEndPoint = false;
            }*/
            if (!isGrounded)
            {
                inputForce = new Vector3(0, -0.2f, 0);
            }
            else
            {
                isLanding = false;
                inputForce.y = 0;
                rigidBody.velocity = Vector3.zero;
                //isActive = false;
            }
        }



        //// Add enough input Force to balance weight and Hover
        if (hoverThrottle)
        {
            if (BiDirThrottle && throttleFactor != 0)
            {
                if (inputForce.y < 0.5f && inputForce.y > -0.5f)
                {
                    if (inputForce.y < deadZoneHT && inputForce.y > -deadZoneHT)
                        inputForce = new Vector3(inputForce.x, (1f / throttleFactor) * Mathf.Sign(Vector3.Project(rigidBody.transform.up, Vector3.up).y), inputForce.z);
                    else inputForce += Vector3.up * (1f / throttleFactor) * Mathf.Sign(Vector3.Project(rigidBody.transform.up, Vector3.up).y);
                }
            }
        }
        ////


        if (clampInput)
        {
            inputTorque = new Vector3(Mathf.Clamp(inputTorque.x, -1f, 1f), Mathf.Clamp(inputTorque.y, -1f, 1f), Mathf.Clamp(inputTorque.z, -1f, 1f));
            inputForce = new Vector3(Mathf.Clamp(inputForce.x, -1f, 1f), Mathf.Clamp(inputForce.y, -1f, 1f), Mathf.Clamp(inputForce.z, -1f, 1f));
        }
    }
    ////////////////////////////////////// Aircraft Input Control



    ////////////////////////////////////// Other Methods
    //
    public void setBattery(float value) { setBattery(value, true); }
    public void setBattery(float value, bool useFilter = true)
    {
        if (droneHUD != null)
        {
            value = Mathf.Clamp01(value);
            droneHUD.fuelTarget = value;
            if(!useFilter) droneHUD.fuel = value * droneHUD.fuelAmplitude;
        }
    }
    //
    public void toogleArcade()
    {
        mode += 1;
        if ((int)mode > System.Enum.GetValues(typeof(ControlType)).Length - 1) mode = 0;

        DisplayMsg.showAll(mode.ToString() + " Mode", 5f);
    }
    //

    public void recoverAttitude()
    {
        rigidBody.velocity = Vector3.zero;
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        

        if (linearDrag != 0) rigidBody.drag = linearDrag;
        if (angDrag != 0) rigidBody.angularDrag = angDrag;

        if (droneHUD != null && droneHUD.fuelTarget <= 0.01f) droneHUD.fuelTarget = 0.05f;
        if (droneHUD != null) droneHUD.ResetHud();


        isBlocked = false;
        if (recoverFlashImgBut != null) recoverFlashImgBut.stopFlash();

        DisplayMsg.showAll("Drone Recovered", 5f);
    }
    //

    //
    public void changeCamera()
    {
        if (cameras.Length == 0) return;

        cameraIndex++;
        if (cameraIndex > cameras.Length - 1) cameraIndex = 0;
        if (cameras[cameraIndex].gameObject == null)
        {
            if(camNull >= cameras.Length) { camNull = 0; return; } else { camNull++; changeCamera(); return; }
        }

        for (int i = 0; i <= cameras.Length - 1; i++)
        {
            if (cameras[i] != null)
            {
                if(i == cameraIndex) cameras[i].SetActive(true); else cameras[i].SetActive(false);
            }
        }

        if (pitchArea != null)
        {
            if (cameraIndex == 0) pitchArea.gameObject.SetActive(true); else pitchArea.gameObject.SetActive(false);
        }

    }
    //

    void OnTriggerEnter(Collider other)
    {
        if (!allowDamage) return;

        if (!isBlocked && other != null && other.gameObject != this.gameObject && other.transform.parent != this.transform)
        {
            isBlocked = true;
            if (recoverFlashImgBut != null) recoverFlashImgBut.flash();

            DisplayMsg.showAll(damageMSG);//, 5f);
            

            linearDrag = rigidBody.drag; rigidBody.drag = 0.25f;                    // affects the object's movement
            angDrag = rigidBody.angularDrag; rigidBody.angularDrag = 0.25f;         // affects the object's rotation

            rigidBody.AddForceAtPosition( rigidBody.mass * Physics.gravity.magnitude * -rigidBody.velocity/5f, transform.position, ForceMode.Impulse);
            rigidBody.AddRelativeTorque(rigidBody.mass * Physics.gravity.magnitude * -rigidBody.velocity/2f, ForceMode.Impulse);
        }
    }
    //

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude < 0.1f) return;
        else if ( collision.relativeVelocity.magnitude < 2f)
        {
            if (touchMSG != "" && !isBlocked) DisplayMsg.showAll(touchMSG, 0.75f);
            
        }
        else if (collision.relativeVelocity.magnitude >= 2f)
        {
            if (hitMSG != "" && !isBlocked) DisplayMsg.showAll(hitMSG, 0.75f);
            
        }
    }
    //

    ////////////////////////////////////// Other Methods


}