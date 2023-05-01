using UnityEngine;
using UnityEngine.UI;

public class DroneHUD : MonoBehaviour
{
    public static DroneHUD current;


    //Config Variables
    [Header("Config References")]
    public bool isActive = false;

    [Tooltip("Link your Drone Transform here!")] public Transform aircraft;
    [Tooltip("If your Drone have a RigidBody, link it here!")] public Rigidbody aircraftRB;


    [Space]
    public string activeMsg = "Drone Activated";
    public DisplayMsg consoleMsg;
    public RectTransform mainPanel;
    //

    [Space(5)]
    [Header("Roll")]
    public bool useRoll = true;
    public float rollAmplitude = -1, rollOffSet = 0;
    [Range(0, 1)] public float rollFilterFactor = 0.25f;
    public RectTransform horizonRoll;
    public Text horizonRollTxt;

    [Space(5)]
    [Header("Pitch")]
    public bool usePitch = true;
    public float pitchAmplitude = -8.4f, pitchOffSet = 0, pitchXOffSet = 0, pitchYOffSet = 0;
    [Range(0, 1)] public float pitchFilterFactor = 0.125f;
    public RectTransform horizonPitch;
    public Text horizonPitchTxt;

    [Space(5)]
    [Header("Heading & TurnRate")]
    public bool useHeading = true;
    public float headingAmplitude = 1, headingOffSet = 0;
    [Range(0, 1)] public float headingFilterFactor = 0.1f;
    public RectTransform compassHSI;
    public Text headingTxt;
    public CompassBar compassBar;
    public RollDigitIndicator headingRollDigit;


    [Space]
    public bool useTurnRate = true;
    public float turnRateAmplitude = 1, turnRateOffSet = 0;
    [Range(0, 1)] public float turnRateFilterFactor = 0.1f;
    public Text turnRateTxt;
    public ArrowIndicator turnRateIndicator;
    public PointerIndicator turnRatePointer;


    [Space(5)]
    [Header("Altitude")]
    public bool useAltitude = true;
    public float altitudeAmplitude = 1, altitudeOffSet = 0;
    [Range(0, 1)] public float altitudeFilterFactor = 0.5f;
    public RollDigitIndicator altitudeRollDigit;
    public PointerIndicator altitudePointer;
    public Text altitudeTxt;

    [Space(5)]
    [Header("AirSpeed")]
    public bool useSpeed = true;
    public float speedAmplitude = 1, speedOffSet = 0;
    [Range(0, 1)] public float speedFilterFactor = 0.25f;
    public NeedleIndicator speedNeedle;
    public ArrowIndicator speedArrow;
    public RollDigitIndicator speedRollDigit;
    public PointerIndicator speedPointer;
    public Text speedTxt, absSpeedTxt;


    [Space(5)]
    [Header("Vertical Velocity")]
    public bool useVV = true;
    public float vvAmplitude = 1, vvOffSet = 0;
    [Range(0, 1)] public float vvFilterFactor = 0.1f;
    public NeedleIndicator vvNeedle;
    public ArrowIndicator vvArrow;
    public RollDigitIndicator vvRollDigit;
    public bool roundVV = true, showDecimalVV = true;
    public float roundFactorVV = 0.1f;
    public Text verticalSpeedTxt;

    [Space(5)]
    [Header("Horizontal Velocity")]
    public bool useHV = true;
    public float hvAmplitude = 1, hvOffSet = 0;
    [Range(0, 1)] public float hvFilterFactor = 0.1f;
    public NeedleIndicator hvNeedle;
    public ArrowIndicator hvArrow;
    public bool roundHV = true, showDecimalHV = true;
    public float roundFactorHV = 0.1f;
    public Text horizontalSpeedTxt;


    [Space(5)]
    [Header("G-Force")]
    public bool useGForce = true;
    public float gForceAmplitude = 1, gForceOffSet = 0;
    [Range(0, 1)] public float gForceFilterFactor = 0.25f;
    public Text gForceTxt, maxGForceTxt, minGForceTxt;


    [Space(5)]
    [Header("AOA, AOS and GlidePath")]
    public bool useAlphaBeta = true;
    public float alphaAmplitude = 1, alphaOffSet = 0;
    [Range(0, 1)] public float alphaFilterFactor = 0.25f;
    public NeedleIndicator alphaNeedle;
    public ArrowIndicator alphaArrow;
    public Text alphaTxt;

    [Space]
    public float betaAmplitude = 1;
    public float betaOffSet = 0;
    [Range(0, 1)] public float betaFilterFactor = 0.25f;
    public NeedleIndicator betaNeedle;
    public ArrowIndicator betaArrow;
    public Text betaTxt;

    [Space]
    public bool useGlidePath = true;
    [Range(0, 1)] public float glidePathFilterFactor = 0.1f;
    public float glideXDeltaClamp = 600f, glideYDeltaClamp = 700f;
    public RectTransform glidePath;


    [Space(5)]
    [Header("Engine and Fuel")]
    public bool useEngine = true;
    public float engineAmplitude = 100;
    [Range(-1, 1)] public float engineOffSet = 0;
    [Range(0, 1)] public float engineFilterFactor = 0.05f;
    public PointerIndicator enginePointer;
    public RollDigitIndicator engineRollDigit;
    public Slider engineSliderUI;
    public Image engineFillUI;
    public Text engineTxt;

    [Space]
    public bool useFuel = true;
    public float fuelAmplitude = 100;//, fuelOffSet = 0;
    [Range(0, 1)] public float fuelFilterFactor = 0.0125f;
    public PointerIndicator fuelPointer;
    public RollDigitIndicator fuelRollDigit;
    public Slider fuelSliderUI;
    public Image fuelFillUI;
    public Text fuelTxt;

    [Space]
    public float fuelFlowAmplitude = 1;
    public Image fuelFlowFillUI;
    public Text fuelFlowTxt;


    [Space(5)]
    [Header("Temperature")]
    public bool useTemperature = false;
    public float temperatureAmplitude = 120, temperatureOffSet = 0;
    [Range(0, 1)] public float temperatureFilterFactor = 0.25f;
    public RollDigitIndicator temperatureRollDigit;
    public PointerIndicator temperaturePointer;
    public Slider temperatureSliderUI;
    public Image temperatureFillUI;
    public Text temperatureTxt;

    [Space(5)]
    [Header("Flaps & Gear")]
    public bool useFlaps = false;
    [Range(0, 1)] public float flapsFilterFactor = 0.05f;
    public Slider flapsSliderUI;
    public Image flapsFillUI;
    public Text flapsTxt;

    [Space]
    public bool useGear = false;
    public GameObject[] gearLights;
    public FlashImg[] gearFlashLights;
    public Text gearTxt;




    [Space]
    [Header("--- Manual Controlers ---")]

    [Space]
    public bool gearDown = false;

    [Space]
    public int flapsIndex = 0;
    public string[] flaps = new string[4] { "0", "10", "15", "25" };

    [Space]
    [Tooltip("If True, Engine RPM will be calculated automaticaly.")] public bool autoRPM = true;
    public float maxEngine = 100, maxSpeed = 100;
    [Range(0, 1)] public float engineTarget = 0.75f;
    public float idleEngine = 25f, criticalEngine = 90f;
    public AudioSource EngineAS;
    public float minPitch = 0.25f, maxPitch = 2.0f;

    [Space]
    [Tooltip("Set it to False if you wish to manually control Temperature value")] public bool autoTemperature = false;
    public float maxTemperature = 120;
    [Range(0, 1)] public float temperatureTarget = 0.5f;
    public float idleTemperature = 35f;
    [Tooltip("Multiplier to how fast the Temperature increases and decreases (Default 1)")] public float tempFlow = 1f;

    [Space]
    [Space]
    [Tooltip("If False, you can set manually the value for Fuel, otherwise it will be automatically controlled by the script.")]
    public bool autoFuel = true;
    public float maxFuel = 100;
    [Range(0, 3)] public float fuelTarget = 0.8f;

    [Tooltip("The Time (in Minutes) to consume 100% Fuel when Engine running at Iddle RPM - Set to 0 for no fuel consumption")]
    public float fuelMaxTime = 8f;
    [Tooltip("The Time (in Minutes) taken to consume 100% Fuel with Engine running at max RPM - Set to 0 for no fuel consumption")]
    public float fuelMinTime = 1.6666666f;


    [Space]
    [Header("Keys")]
    public bool useKeys = true;
    public KeyCode gearKey = KeyCode.G, flapsUpKey = KeyCode.PageUp, flapsDownKey = KeyCode.PageDown, resetKey = KeyCode.T;



    //All Flight Variables
    [Space(10)]
    [Header("Flight Variables - ReadOnly!")]
    public int flap;
    public float currentFlap, gear, speed, absSpeed;
    public float altitude, pitch, roll, heading, turnRate, gForce, maxGForce, minGForce, alpha, beta, vv, hv, engine, fuel, fuelFlow, temperature;
    //


    //Internal Calculation Variables
    Vector3 currentPosition, lastPosition, relativeSpeed, absoluteSpeed, lastSpeed, relativeAccel;

    Vector3 angularSpeed;
    Quaternion currentRotation, lastRotation, deltaTemp;
    float angleTemp = 0.0f;
    Vector3 axisTemp = Vector3.zero;

    float engineReNormalized, fuelReNormalized;

    MoveObject mapCamScript;
    Camera mapCam;

    int waitInit = 6;
    //


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////// Inicialization
    void Awake()
    {
        if (current == null) current = this;
        if (mainPanel == null) mainPanel = GetComponent<RectTransform>();
        if (aircraft == null && aircraftRB == null) aircraft = Camera.main.transform;   //If there is no reference set, then it gets the MainCamera
        if (aircraft == null && aircraftRB != null) aircraft = aircraftRB.transform;
    }
    //
    //
    void OnEnable()
    {
        if (aircraft == null && aircraftRB == null) aircraft = Camera.main.transform;

        if (current == null) current = this;
        else if (current != null && current != this)
        {
            if (current.gameObject.activeInHierarchy) synchronizeValues(true, true);
            else current = this;
        }

        ResetHud();
    }
    //
    public void ResetHud()
    {
        if (aircraft == null && aircraftRB != null) aircraft = aircraftRB.transform;

        waitInit = 6;

        //Values to Reset        
        if (useGForce)
        {
            maxGForce = 0f; minGForce = 0f;
            if (maxGForceTxt != null) maxGForceTxt.text = "0.0";
            if (minGForceTxt != null) minGForceTxt.text = "0.0";
        }
        //
        if (useFlaps)
        {
            flap = -1;
            if (flapsTxt != null) flapsTxt.text = flaps[flapsIndex];
        }
        if (useGear) gear = 0.5f;
        //

        isActive = true;
        if (consoleMsg != null) consoleMsg.displayQuickMsg(activeMsg); else DisplayMsg.showAll(activeMsg, 5);
    }
    //
    public void toogleMFD()
    {
        SndPlayer.playClick();
        mainPanel.gameObject.SetActive(!mainPanel.gameObject.activeSelf);


        if (!mainPanel.gameObject.activeSelf)
        {
            isActive = false; current = null;
            if (consoleMsg != null) consoleMsg.displayQuickMsg("MFD Disabled"); else DisplayMsg.showAll("MFD Disabled", 5);
            //DisplayMsg.show("MFD Disabled", 5);
        }
        else { if (!isActive) ResetHud(); }
    }
    //
    void synchronizeValues(bool updateCurrent, bool disablePrevious)
    {
        if (current == null) return;

        flapsIndex = current.flapsIndex;
        currentFlap = current.currentFlap; flap = -1;//flap = current.flap;
        gearDown = current.gearDown; gear = 0.5f;// gear = current.gear;

        speed = current.speed;
        altitude = current.altitude;
        pitch = current.pitch;
        roll = current.roll;
        heading = current.heading;
        turnRate = current.turnRate;
        gForce = current.gForce;
        maxGForce = current.maxGForce;
        minGForce = current.minGForce;
        alpha = current.alpha;
        beta = current.beta;
        vv = current.vv;
        hv = current.hv;

        engineTarget = current.engineTarget; engine = current.engine;
        fuelTarget = current.fuelTarget; fuel = current.fuel;
        fuelFlow = current.fuelFlow;
        temperatureTarget = current.temperatureTarget; temperature = current.temperature;

        if (disablePrevious) current.transform.parent.gameObject.SetActive(false);
        if(updateCurrent) current = this;
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////// Inicialization



    ////////////////////////// Listening for Key Commands
    void Update()
    {
        if (!isActive || !useKeys) return;

        //Keys
        if (Input.GetKeyDown(resetKey)) ResetHud();
        if (Input.GetKeyDown(flapsUpKey)) flapsUp();
        if (Input.GetKeyDown(flapsDownKey)) flapsDown();
        if (Input.GetKeyDown(gearKey)) toogleGear();
        //

    }
    //////////////////////////


    //////////////////////////// Calls for Flaps + Gear Control + Configs + Etc...
    public void flapsDown() { if (flapsIndex + 1 > flaps.Length - 1) flapsIndex = flaps.Length - 1; else flapsIndex += 1; }
    public void flapsUp() { if (flapsIndex - 1 < 0) flapsIndex = 0; else flapsIndex -= 1; }
    public void toogleGear() { gearDown = !gearDown; }
    //
    public void setTextOutline(bool active = true) { foreach (Outline outLine in mainPanel.GetComponentsInChildren<Outline>(true)) outLine.enabled = active; }
    public void setGaugesColor(int index = -1)
    {
        ColorImg[] colorImg = mainPanel.GetComponentsInChildren<ColorImg>(true);
        for (int i = 0; i < colorImg.Length; i++) if (index == -1) colorImg[i].toogleColor(); else colorImg[i].setColor(index); //colorImg[i].setColor((colorImg[i].indexColor == 0) ? 1 : 0);
    }
    public void setTextColor(int index = -1)
    {
        ColorTxt[] colorTxt = mainPanel.GetComponentsInChildren<ColorTxt>(true);
        for (int i = 0; i < colorTxt.Length; i++) if (index == -1) colorTxt[i].toogleColor(); else colorTxt[i].setColor(index);
    }
    public void setBackgroundBlack(bool black = true)
    {
        ToogleImg[] masks = mainPanel.GetComponentsInChildren<ToogleImg>(true);
        if (black) for (int i = 0; i < masks.Length; i++) masks[i].disableMaskGraph(); else for (int i = 0; i < masks.Length; i++) masks[i].enableMaskGraph();
    }
    //////////////////////////// Flaps + Gear Control + Configs + Etc...



    /////////////////////////////////////////////////////// Updates and Calculations
    void FixedUpdate() //Update()
    {
        // Return if not active
        if (!isActive) return;

        //////////////////////////////////////////// Frame Calculations
        lastPosition = currentPosition;
        lastSpeed = relativeSpeed;
        lastRotation = currentRotation;

        if (aircraft != null && aircraftRB == null) //Mode Transform
        {
            currentPosition = aircraft.transform.position;
            absoluteSpeed = (currentPosition - lastPosition) / Time.fixedDeltaTime;
            relativeSpeed = aircraft.transform.InverseTransformDirection((currentPosition - lastPosition) / Time.fixedDeltaTime);
            relativeAccel = (relativeSpeed - lastSpeed) / Time.fixedDeltaTime;
            currentRotation = aircraft.transform.rotation;

            //angular speed
            deltaTemp = currentRotation * Quaternion.Inverse(lastRotation);
            angleTemp = 0.0f;
            axisTemp = Vector3.zero;
            deltaTemp.ToAngleAxis(out angleTemp, out axisTemp);
            //
            angularSpeed = aircraft.InverseTransformDirection(angleTemp * axisTemp) * Mathf.Deg2Rad / Time.fixedDeltaTime;
            //
        }
        else if (aircraft != null && aircraftRB != null)  //Mode RB
        {
            currentPosition = aircraftRB.transform.position;
            absoluteSpeed = (currentPosition - lastPosition) / Time.fixedDeltaTime;
            relativeSpeed = aircraftRB.transform.InverseTransformDirection(aircraftRB.velocity);
            relativeAccel = (relativeSpeed - lastSpeed) / Time.fixedDeltaTime;
            currentRotation = aircraft.transform.rotation;

            angularSpeed = aircraftRB.angularVelocity;
        }
        else //Zero all values
        {
            currentPosition = Vector3.zero;
            relativeSpeed = Vector3.zero;
            relativeAccel = Vector3.zero;
            angularSpeed = Vector3.zero;

            lastPosition = currentPosition;
            lastSpeed = relativeSpeed;
            lastRotation = currentRotation;
        }
        //
        if (waitInit > 0) { waitInit--; return; } //Wait some frames for stablization before starting calculating
        //
        //////////////////////////////////////////// Frame Calculations


        //////////////////////////////////////////// Compass, Heading and/or HSI + Turn Rate
        if (useHeading)
        {
            heading = Mathf.LerpAngle(heading, headingAmplitude * currentRotation.eulerAngles.y + headingOffSet, headingFilterFactor) % 360f;


            //Send values to Gui and Instruments
            if (compassHSI != null) compassHSI.localRotation = Quaternion.Euler(0, 0, heading);
            if (compassBar != null) compassBar.setValue(heading);
            if (headingRollDigit != null) headingRollDigit.setValue((heading < 0) ? (heading + 360f) : heading);
            if (headingTxt != null) { if (heading < 0) headingTxt.text = (heading + 360f).ToString("000"); else headingTxt.text = heading.ToString("000"); }

        }
        //
        if (useTurnRate)
        {
            //Mode: World Coorditates
            //turnRate = Mathf.LerpAngle(turnRate, turnRateOffSet + turnRateAmplitude * Mathf.DeltaAngle(lastRotation.eulerAngles.y, currentRotation.eulerAngles.y) / Time.fixedDeltaTime, turnRateFilterFactor) % 360f;
            //turnRate = Mathf.Round(100f * turnRate + 0.5f) / 100f;

            //Mode: Relative to Aircraft
            turnRate = Mathf.LerpAngle(turnRate, turnRateOffSet + turnRateAmplitude * (angularSpeed.y - 0.05f * angularSpeed.z) * Mathf.Rad2Deg, turnRateFilterFactor) % 360f;
            turnRate = Mathf.Round(100f * turnRate + 0.5f) / 100f;
            //////

            if (float.IsNaN(turnRate)) turnRate = 0;

            //Send values to Gui and Instruments
            if (turnRateIndicator != null) turnRateIndicator.setValue(turnRate);
            if (turnRatePointer != null) turnRatePointer.setValue(turnRate);
            if (turnRateTxt != null) { turnRateTxt.text = turnRate.ToString("0"); }
        }
        //////////////////////////////////////////// Compass, Heading and/or HSI + Turn Rate


        //////////////////////////////////////////// Roll
        if (useRoll)
        {
            //roll = Mathf.LerpAngle(roll, aircraft.rotation.eulerAngles.z + rollOffSet, rollFilterFactor) % 360;
            roll = Mathf.LerpAngle(roll, currentRotation.eulerAngles.z + rollOffSet, rollFilterFactor) % 360;


            //Send values to Gui and Instruments
            if (horizonRoll != null) horizonRoll.localRotation = Quaternion.Euler(0, 0, rollAmplitude * roll);
            if (horizonRollTxt != null)
            {
                //horizonRollTxt.text = roll.ToString("##");
                if (roll > 180) horizonRollTxt.text = (roll - 360).ToString("00");
                else if (roll < -180) horizonRollTxt.text = (roll + 360).ToString("00");
                else horizonRollTxt.text = roll.ToString("00");
            }
            //
        }
        //////////////////////////////////////////// Roll


        //////////////////////////////////////////// Pitch
        if (usePitch)
        {
            //pitch = Mathf.LerpAngle(pitch, -aircraft.eulerAngles.x + pitchOffSet, pitchFilterFactor);
            pitch = Mathf.LerpAngle(pitch, -currentRotation.eulerAngles.x + pitchOffSet, pitchFilterFactor);


            //Send values to Gui and Instruments
            if (horizonPitch != null) horizonPitch.localPosition = new Vector3(-pitchAmplitude * pitch * Mathf.Sin(horizonPitch.transform.localEulerAngles.z * Mathf.Deg2Rad) + pitchXOffSet, pitchAmplitude * pitch * Mathf.Cos(horizonPitch.transform.localEulerAngles.z * Mathf.Deg2Rad) + pitchYOffSet, 0);
            if (horizonPitchTxt != null) horizonPitchTxt.text = pitch.ToString("0");
        }
        //////////////////////////////////////////// Pitch


        //////////////////////////////////////////// Altitude
        if (useAltitude)
        {
            altitude = Mathf.Lerp(altitude, altitudeOffSet + altitudeAmplitude * currentPosition.y, speedFilterFactor);


            //Send values to Gui and Instruments
            if (altitudeRollDigit != null) altitudeRollDigit.setValue(altitude);
            if (altitudePointer != null) altitudePointer.setValue(altitude);
            if (altitudeTxt != null) altitudeTxt.text = altitude.ToString("0").PadLeft(5);
        }
        //////////////////////////////////////////// Altitude


        //////////////////////////////////////////// Speed
        if (useSpeed)
        {
            speed = Mathf.Lerp(speed, speedOffSet + speedAmplitude * relativeSpeed.z, speedFilterFactor);
            absSpeed = Mathf.Lerp(absSpeed, speedOffSet + speedAmplitude * absoluteSpeed.magnitude, speedFilterFactor);


            //Send values to Gui and Instruments
            if (speedNeedle != null) speedNeedle.setValue(speed);
            if (speedArrow != null) speedArrow.setValue(speed);
            if (speedRollDigit != null) speedRollDigit.setValue(speed);
            if (speedPointer != null) speedPointer.setValue(speed);
            if (speedTxt != null) speedTxt.text = speed.ToString("0").PadLeft(5);//.ToString("##0");

            if (absSpeedTxt != null) absSpeedTxt.text = (Mathf.Sign(speed) * absSpeed).ToString("0").PadLeft(5);//.ToString("##0");
        }
        //////////////////////////////////////////// Speed


        //////////////////////////////////////////// Vertical Velocity - VV
        if (useVV)
        {
            vv = Mathf.Lerp(vv, vvOffSet + vvAmplitude * absoluteSpeed.y, vvFilterFactor);


            //Send values to Gui and Instruments
            if (vvNeedle != null) vvNeedle.setValue(vv);
            if (vvArrow != null) vvArrow.setValue(vv);
            if (vvRollDigit != null) vvRollDigit.setValue(vv);
            if (verticalSpeedTxt != null)
            {
                if (roundVV)
                {
                    if (showDecimalVV) verticalSpeedTxt.text = (System.Math.Round(vv / roundFactorVV, System.MidpointRounding.AwayFromZero) * roundFactorVV).ToString("0.0").PadLeft(4);
                    else verticalSpeedTxt.text = (System.Math.Round(vv / roundFactorVV, System.MidpointRounding.AwayFromZero) * roundFactorVV).ToString("0").PadLeft(3);
                }
                else
                {
                    if (showDecimalVV) verticalSpeedTxt.text = (vv).ToString("0.0").PadLeft(4);
                    else verticalSpeedTxt.text = (vv).ToString("0").PadLeft(3);
                }

            }
        }
        //////////////////////////////////////////// Vertical Velocity - VV


        //////////////////////////////////////////// Horizontal Velocity - HV
        if (useHV)
        {
            hv = Mathf.Lerp(hv, hvOffSet + hvAmplitude * relativeSpeed.x, hvFilterFactor);

            //Send values to Gui and Instruments
            if (hvNeedle != null) hvNeedle.setValue(hv);
            if (hvArrow != null) hvArrow.setValue(hv);
            if (horizontalSpeedTxt != null)
            {
                if (roundHV)
                {
                    if (showDecimalHV) horizontalSpeedTxt.text = (System.Math.Round(hv / roundFactorHV, System.MidpointRounding.AwayFromZero) * roundFactorHV).ToString("0.0").PadLeft(4);
                    else horizontalSpeedTxt.text = (System.Math.Round(hv / roundFactorHV, System.MidpointRounding.AwayFromZero) * roundFactorHV).ToString("0").PadLeft(3);
                }
                else
                {
                    if (showDecimalHV) horizontalSpeedTxt.text = (hv).ToString("0.0").PadLeft(4);
                    else horizontalSpeedTxt.text = (hv).ToString("0").PadLeft(3);
                }
            }
        }
        //////////////////////////////////////////// Horizontal Velocity - HV


        //////////////////////////////////////////// Vertical G-Force 
        if (useGForce)
        {
            //G-FORCE -> Gravity + Vertical Acceleration + Centripetal Acceleration (v * w) radians
            float gTotal = 0;
            if (aircraft != null)
            {
                gTotal =
                    ((-aircraft.transform.InverseTransformDirection(Physics.gravity).y +
                    gForceAmplitude * (relativeAccel.y - angularSpeed.x * Mathf.Abs(relativeSpeed.z)
                    )) / Physics.gravity.magnitude);
            }
            else
            {
                gTotal = ((gForceAmplitude * (relativeAccel.y - angularSpeed.x * Mathf.Abs(relativeSpeed.z))) / Physics.gravity.magnitude);
            }
            //
            gForce = Mathf.Lerp(gForce, gForceOffSet + gTotal, gForceFilterFactor);
            if (float.IsNaN(gForce)) gForce = 0;
            //

            //Send values to Gui and Instruments
            if (gForceTxt != null) gForceTxt.text = gForce.ToString("0.0").PadLeft(3);
            if (gForce > maxGForce)
            {
                maxGForce = gForce;
                if (maxGForceTxt != null) maxGForceTxt.text = maxGForce.ToString("0.0").PadLeft(3);
            }
            if (gForce < minGForce)
            {
                minGForce = gForce;
                if (minGForceTxt != null) minGForceTxt.text = minGForce.ToString("0.0").PadLeft(3);
            }
            //
        }
        ////////////////////////////////////////////  Vertical G-Force 


        //////////////////////////////////////////////// AOA (Alpha) + AOS (Beta) + GlidePath (Velocity Vector)
        if (useAlphaBeta || useGlidePath)
        {
            //Calculate both Angles
            alpha = Mathf.Lerp(alpha, alphaOffSet + alphaAmplitude * Vector2.SignedAngle(new Vector2(relativeSpeed.z, relativeSpeed.y), Vector2.right), alphaFilterFactor);
            beta = Mathf.Lerp(beta, betaOffSet + betaAmplitude * Vector2.SignedAngle(new Vector2(relativeSpeed.x, relativeSpeed.z), Vector2.up), betaFilterFactor);

            ////Used in older Unity versions where Vector2.SignedAngle didnt exist
            ////int alphaSign = (int)Mathf.Sign(Vector3.Dot(Vector3.forward, Vector3.Cross(new Vector2(relativeSpeed.z, relativeSpeed.y), Vector2.right)));
            ////int betaSign = (int)Mathf.Sign(Vector3.Dot(Vector3.forward, Vector3.Cross(new Vector2(relativeSpeed.x, relativeSpeed.z), Vector2.up)));
            ////alpha = Mathf.Lerp(alpha, alphaOffSet + alphaAmplitude * alphaSign * Vector2.Angle(new Vector2(relativeSpeed.z, relativeSpeed.y), Vector2.right), alphaFilterFactor);
            ////beta = Mathf.Lerp(beta, betaOffSet + betaAmplitude * betaSign * Vector2.Angle(new Vector2(relativeSpeed.x, relativeSpeed.z), Vector2.up), betaFilterFactor);
            //

            //Apply angle values to the glidePath UI element
            if (useGlidePath && glidePath != null) glidePath.localPosition = Vector3.Lerp(glidePath.localPosition, new Vector3(Mathf.Clamp(-beta * pitchAmplitude, -glideXDeltaClamp, glideXDeltaClamp), Mathf.Clamp(alpha * pitchAmplitude, -glideYDeltaClamp, glideYDeltaClamp), 0), glidePathFilterFactor);


            //Send values to Instruments
            if (useAlphaBeta)
            {
                if (alphaNeedle != null) alphaNeedle.setValue(alpha);
                if (alphaArrow != null) alphaArrow.setValue(alpha);
                if (betaNeedle != null) betaNeedle.setValue(beta);
                if (betaArrow != null) betaArrow.setValue(beta);


                //Send values to Gui Text
                if (alphaTxt != null) alphaTxt.text = alpha.ToString("0").PadLeft(3);
                if (betaTxt != null) betaTxt.text = beta.ToString("0").PadLeft(3);
            }
            //
        }
        //////////////////////////////////////////////// AOA (Alpha) + AOS (Beta)


        //////////////////////////////////////////// Engine & Fuel
        if (useEngine)
        {
            //Auto RPM control and Fuel Condition
            //if (autoRPM) engineTarget = Mathf.Abs(idleEngine / engineAmplitude + absSpeed / maxSpeed * (1 - idleEngine / engineAmplitude)); //Set by DroneMainScript
            if (useFuel && fuelReNormalized < 0.01f) engineTarget = 0;
            //

            //Updates current Engine RPM
            engineTarget = Mathf.Clamp01(Mathf.Abs(engineTarget));
            engine = Mathf.Lerp(engine, engineAmplitude * Mathf.Clamp01(engineTarget + engineOffSet), engineFilterFactor);

            if (engineTarget == 0 && engine < 0.01f) engine = 0;
            engineReNormalized = Mathf.Clamp01((engine - engineOffSet) / engineAmplitude);

            if (useFuel && fuel == 0) { engineTarget = 0; engine = 0; /*engineReNormalized = 0;*/ }
            //

            //Engine Sound and Pitch
            if (EngineAS != null && EngineAS.isActiveAndEnabled)
            {
                if (!EngineAS.isPlaying && engineTarget > 0) EngineAS.Play();

                if (engineReNormalized > 0.01f) EngineAS.pitch = Mathf.Lerp(minPitch, maxPitch, engineReNormalized);
                else { EngineAS.Stop(); EngineAS.pitch = 1; }
            }
            //

            //Send values to Gui and Instruments
            if (engineRollDigit != null) engineRollDigit.setValue(engine);
            if (enginePointer != null) enginePointer.setValue(engine);
            if (engineSliderUI != null) engineSliderUI.value = (engine / maxEngine);
            if (engineFillUI != null) engineFillUI.fillAmount = (engine / maxEngine);
            if (engineTxt != null) engineTxt.text = engine.ToString("##0");
        }
        //
        if (useFuel)
        {
            //Calculates Fuel Consumption
            //if (autoFuel && (maxfuelFlow != 0 || idlefuelFlow != 0))
            if (autoFuel && (fuelMaxTime > 0 || fuelMinTime > 0))
            {
                if (engine != 0)
                {
                    //Consumption in Minutes for 100% Fuel
                    fuelFlow = Time.fixedDeltaTime / (60f * (fuelMaxTime - engineReNormalized * engineReNormalized * (fuelMaxTime - fuelMinTime)));
                    fuelTarget -= fuelFlow;
                }
                else fuelFlow = 0;
            }
            else fuelFlow = 0;
            //

            //Updates current Fuel value
            if (fuelTarget < 0) fuelTarget = 0;//fuelTarget = Mathf.Clamp01(fuelTarget);
            fuel = Mathf.Lerp(fuel, /*fuelOffSet +*/ fuelAmplitude * fuelTarget, fuelFilterFactor);

            if (fuel < 0) fuel = 0;
            if (fuelTarget == 0 && fuel < 0.01f) fuel = 0;
            fuelReNormalized = fuel / fuelAmplitude; //Mathf.Clamp01(fuel /*- fuelOffSet*/) / fuelAmplitude;
            //

            //Send values to Gui and Instruments
            if (fuelRollDigit != null) fuelRollDigit.setValue(fuel);
            if (fuelPointer != null) fuelPointer.setValue(fuel);
            if (fuelSliderUI != null) fuelSliderUI.value = (fuel / maxFuel);
            if (fuelFillUI != null) fuelFillUI.fillAmount = (fuel / maxFuel);
            if (fuelTxt != null) fuelTxt.text = fuel.ToString("##0");

            //Consumption in Minutes for 100% Fuel
            if (fuelFlowFillUI != null) fuelFlowFillUI.fillAmount = fuelFlow / (Time.fixedDeltaTime / (60f * fuelMinTime));
            if (fuelFlowTxt != null) fuelFlowTxt.text = (fuelAmplitude * fuelFlow * fuelFlowAmplitude / Time.fixedDeltaTime).ToString("##0.0");//.ToString("0.0").PadLeft(4);  //.ToString("##0");     
            //
        }
        //////////////////////////////////////////// Engine & Fuel


        //////////////////////////////////////////// Temperature
        if (useTemperature)
        {
            //Automatic Temperature control
            if (autoTemperature)
            {
                if (useFuel && fuel == 0) temperatureTarget += 3 * (0 - temperatureTarget) * tempFlow * Time.fixedDeltaTime / 60f; //temperatureTarget = 0;
                else
                {
                    //Backup of simpler versions
                    //////temperatureTarget += factor * (engineTarget - temperatureTarget) * tempFlow * Time.fixedDeltaTime / 60f;
                    //int factor = (engineTarget < temperatureTarget) ? 2 : 1; //Cools 2 times faster than it heats
                    //temperatureTarget += factor * (engineTarget - temperatureTarget + idleTemperature / temperatureAmplitude) * tempFlow * Time.fixedDeltaTime / 60f;

                    if (engineReNormalized >= criticalEngine / engineAmplitude)
                        temperatureTarget += 1.5f * engineReNormalized * tempFlow * Time.fixedDeltaTime / 60f;
                    //temperatureTarget += 4 * (engineReNormalized - temperatureTarget) * tempFlow * Time.fixedDeltaTime / 60f;
                    else if (engineTarget == 0)
                        temperatureTarget += 3 * (0 - temperatureTarget) * tempFlow * Time.fixedDeltaTime / 60f;
                    else if (engineReNormalized < idleTemperature / temperatureAmplitude || engineReNormalized < temperatureTarget)
                        temperatureTarget += 3 * (idleTemperature / temperatureAmplitude - temperatureTarget) * tempFlow * Time.fixedDeltaTime / 60f;
                    else
                        temperatureTarget += 1 * (engineReNormalized - temperatureTarget) * tempFlow * Time.fixedDeltaTime / 60f;
                }
            }
            //


            //Updates current Engine Temperature
            temperatureTarget = Mathf.Clamp01(temperatureTarget);
            temperature = Mathf.Lerp(temperature, temperatureAmplitude * temperatureTarget + temperatureOffSet, temperatureFilterFactor);


            //Send values to Gui and Instruments
            if (temperatureRollDigit != null) temperatureRollDigit.setValue(temperature);
            if (temperaturePointer != null) temperaturePointer.setValue(temperature / maxTemperature);
            if (temperatureSliderUI != null) temperatureSliderUI.value = (temperature / maxTemperature);
            if (temperatureFillUI != null) temperatureFillUI.fillAmount = (temperature / maxTemperature);
            if (temperatureTxt != null) temperatureTxt.text = temperature.ToString("##0");
        }
        //////////////////////////////////////////// Temeprature



        ////////////////////////////////////////////// Flaps
        if (useFlaps)
        {
            //Update Target Flap Position
            if (flap != flapsIndex)
            {
                if (flap != -1) SndPlayer.play(4);

                flapsIndex = Mathf.Clamp(flapsIndex, 0, flaps.Length - 1);
                flap = flapsIndex;
            }

            //Normalized Value of Flaps
            currentFlap = Mathf.Lerp(currentFlap, (float)(flapsIndex) / (float)Mathf.Clamp((flaps.Length - 1), 1, flaps.Length), flapsFilterFactor);


            //Send values to Gui and Instruments
            if (flapsSliderUI != null) flapsSliderUI.value = currentFlap;
            if (flapsFillUI != null) flapsFillUI.fillAmount = currentFlap;
            if (flapsTxt != null) flapsTxt.text = flaps[flapsIndex];
        }
        ////////////////////////////////////////////// Flaps


        ////////////////////////////////////////////// Gear
        if (useGear)
        {
            //Gear Changes
            if (gearDown && gear < 1)
            {
                if (gear != 0.5f) foreach (FlashImg light in gearFlashLights) light.flash();
                foreach (GameObject light in gearLights) light.gameObject.SetActive(true);

                if (gear != 0.5f) SndPlayer.play(3);
                gear = 1;


                if (gearTxt != null) gearTxt.text = "DOWN";
            }
            else if (!gearDown && gear > 0)
            {

                if (gear != 0.5f) foreach (FlashImg light in gearFlashLights) light.flash();
                foreach (GameObject light in gearLights) light.gameObject.SetActive(false);

                if (gear != 0.5f) SndPlayer.play(3);
                gear = 0;

                if (gearTxt != null) gearTxt.text = "UP";
            }
        }
        ////////////////////////////////////////////// Gear



    }
    /////////////////////////////////////////////////////// Updates and Calculations
}
