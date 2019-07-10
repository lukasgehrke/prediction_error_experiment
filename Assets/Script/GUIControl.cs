using UnityEngine;
using System.Collections;
using Assets.LSL4Unity.Scripts; // reference the LSL4Unity namespace to get access to all classes
using Uniduino; // adding arduino
//using SinglePulse;

public class GUIControl : MonoBehaviour {

    // create LSL markerstream
    public static LSLMarkerStream marker;

    // add arduino
    public static Arduino arduino;

    public static GameObject table, plane, leapmotion, instruct, endexp, startPos, rehastim, resting;
    GameObject centerEyeAnchor, index_finger, hand;
    GameObject cubeMiddle, cubeLeft, cubeRight;

    GameObject[] cubeGameObjArr = new GameObject[3];  // Game Object Array

    Vector3 cube1Vector, cube2Vector;
    //HandStyleControl hSC;

    // Cube Seq if only Bigger cube has been used
    public static int[] CubeSeq = new int[] { 0, 1, 2 }; 

    public static SphereCollider myCollider;
    
    public static int eventMsg;

    int maxTrial;
    int trialCounter = 0;
    float sphereRadius = 0;
    public static float actualTime = 0; // start timer at 0
    int selectedHandStyle = 1; 

    public static int[] NormalConflictArr;
    
    int trialSeqCounter = 0;
    public static int cubeSeqCounter = 0;
    
    public static bool flagTouchEvent = false;
    public static bool flagStart = false;
    
    // trial logic handler
    public static bool updateTrialLogic = false;
    public static bool updateEndOfTrialLogic = false;
    public static bool experimentEnd = false;

    // sets intensity level of vibrotactile feedback
    public static int vibroIntensity;
    public static float vibro_feedback_duration;
    public static float elapsed;
    public static bool vibroFeedback = false;
    public static bool vibroFeedbackCondition = false;

    // sets intensity level of EMS/FES feedback
    public static int emsIntensity;
    public static bool emsFeedbackCondition = false;
    public static int pulseCount;
    public static int width;
    public static int current;

    // event markers feedback channel
    public static string feedback_type = "";

    // parameter variables
    int volatility;
    int volatility_level;
    public static float isi_time = -5;
    public static float reaction_time = 0;
    public static float reaction_start_time = 0;
    
    RandomizeArray RanArr = new RandomizeArray();
    
    // Use this for initialization
    void Start()
    {

        InitilizeVaribles(); // Read all variable from XML file, currently only the number of trials

        // init LSL markerstream
        marker = FindObjectOfType<LSLMarkerStream>();

        // init arduino
        arduino = Arduino.global;
		arduino.Setup(ConfigurePins);
        
        // Finding the game object 
        table = GameObject.Find("Table");
        plane = GameObject.Find("Plane");
        leapmotion = GameObject.Find("LeapHandController");
        instruct = GameObject.Find("IntroGUI");
        endexp = GameObject.Find("End");
        startPos = GameObject.Find("startPosDisc");
        resting = GameObject.Find("resting");

        // init hand model
        index_finger = GameObject.Find("f_index.03_end");
        hand = GameObject.Find("vr_hand_R");

        // rehastim interface
        rehastim = GameObject.Find("RehaStim");

        // For Big Cube
        cubeLeft = GameObject.Find("CubeLeft");
        cubeMiddle = GameObject.Find("CubeMiddle");
        cubeRight = GameObject.Find("CubeRight");

        // Assign game objects to arrays
        cubeGameObjArr[0] = cubeLeft;
        cubeGameObjArr[1] = cubeMiddle;
        cubeGameObjArr[2] = cubeRight;

        // Randomize Cube Appearance Sequence
        RandomizeArray.ShuffleArray(CubeSeq);

        // Trial definition error with 1s and 0s for either conflict or normal trial depending on volatility of simulation
        if (volatility == 1)
        {
            //NormalConflictArr = RandomizeArray.GenerateArray(maxTrial, 50); // Randomize Normal and Conflict Trial    
            NormalConflictArr = RandomizeArray.GenerateArraySequences(maxTrial, 0.1, 5);
        }
        else if (volatility == 0)
        {
            //NormalConflictArr = RandomizeArray.GenerateArray(maxTrial, volatility_level); // Randomize Normal and Conflict Trial    
            NormalConflictArr = RandomizeArray.GenerateArraySequences(maxTrial, 0.1, 5);
        }
        
        // instruct and leapmotion game object remain visible and only table, plane and the end instruction are made invisible
        table.gameObject.GetComponent<Renderer>().enabled = false;
        plane.gameObject.GetComponent<Renderer>().enabled = false;
        instruct.gameObject.gameObject.GetComponent<Canvas>().enabled = true;
        endexp.gameObject.gameObject.GetComponent<Canvas>().enabled = false;
        resting.gameObject.GetComponent<Renderer>().enabled = false;

        // hide startPosDisc
        startPos.SetActive(false);
        //startPos.gameObject.GetComponent<Renderer>().enabled = false;
        //startPos.gameObject.GetComponent<SphereCollider>().enabled = false;

        //index_finger.gameObject.GetComponent<SphereCollider>().enabled = true;

        DisableAllCube(); // set color to white, disable renderer and sphereCollider

        //hSC = GameObject.Find("LeapHandController").GetComponent<HandStyleControl>();

    }
    
    void ConfigurePins() {
		arduino.pinMode(3, PinMode.PWM);
	}

    // Disable All Cubes rendering and collider properties
    public void DisableAllCube()
    {

        // Change color to white to all Big Cubes
        cubeLeft.gameObject.SetActive(false);
        cubeMiddle.gameObject.SetActive(false);
        cubeRight.gameObject.SetActive(false);

        cubeLeft.gameObject.GetComponent<Renderer>().material.color = Color.white;
        cubeMiddle.gameObject.GetComponent<Renderer>().material.color = Color.white;
        cubeRight.gameObject.GetComponent<Renderer>().material.color = Color.white;

        // // Disable redering of all Big Cubes
        // cubeLeft.gameObject.GetComponent<Renderer>().enabled = false;
        // cubeMiddle.gameObject.GetComponent<Renderer>().enabled = false;
        // cubeRight.gameObject.GetComponent<Renderer>().enabled = false;

        // // Disable Sphere Collider for Big Cubes
        // cubeLeft.gameObject.GetComponent<SphereCollider>().enabled = false;
        // cubeMiddle.gameObject.GetComponent<SphereCollider>().enabled = false;
        // cubeRight.gameObject.GetComponent<SphereCollider>().enabled = false;

    }

    // Intialize all variable from XML file
    public void InitilizeVaribles() 
    {
        LoadXMLData xml = new LoadXMLData();
        //hSC = GameObject.Find("LeapHandController").GetComponent<HandStyleControl>();
        xml.GetData();
        maxTrial = int.Parse(xml.obj["trial"]); // Maximum number of trials for experiment ... 12;
        
        volatility = int.Parse(xml.obj["volatility"]); // Maximum number of trials for experiment ... 12;

        feedback_type = "visual";
        //sphereRadius = //float.Parse(xml.obj["height"]);
        //vibroIntensity = int.Parse(xml.obj["vibroIntensity"]); // set intensity of vibro feedback
        //emsIntensity = int.Parse(xml.obj["emsIntensity"]); // set intensity of ems feedback
        vibroFeedbackCondition = bool.Parse(xml.obj["vibroFeedbackCondition"]); // set intensity of ems feedback
        if (vibroFeedbackCondition){
            feedback_type = "vibro";
        }
        vibro_feedback_duration = float.Parse(xml.obj["vibro_feedback_duration"]); // 

        emsFeedbackCondition = bool.Parse(xml.obj["emsFeedbackCondition"]); // set intensity of ems feedback
        if (emsFeedbackCondition){
            feedback_type = "ems";
        }
        width = int.Parse(xml.obj["width"]);
        current = int.Parse(xml.obj["current"]);
        pulseCount = int.Parse(xml.obj["pulseCount"]);
        
        NormalConflictArr = new int[maxTrial];
    }

    // Start of experiment
    public void ControlState()
    {    
        if (Input.GetMouseButtonDown(0))
        // after hitting any key once, the Input.anyKeyDown is otherwise false for every frame update
        // therefore the if clause code is only run once at the beginning when hitting any key to start
        {
            // Making instruction invisible in scene and start rendering table and plane
            flagStart = true;
            table.gameObject.GetComponent<Renderer>().enabled = true;
            plane.gameObject.GetComponent<Renderer>().enabled = true;
            instruct.gameObject.GetComponent<Canvas>().enabled = false;
            //hSC.selectHand(selectedHandStyle);
            startPos.SetActive(true);
            //startPos.gameObject.GetComponent<Renderer>().enabled = true;
            // enable collider which triggers trial start when touched
            //startPos.gameObject.GetComponent<SphereCollider>().enabled = true;

            resting.gameObject.GetComponent<Renderer>().enabled = true;
            if (vibroFeedbackCondition){
                marker.Write("block:start;condition:vibro");
                Debug.Log("block:start;condition:vibro");
            }else{
                marker.Write("block:start;condition:visual");
                Debug.Log("block:start;condition:visual");
            }
        }
    }

    //  Control all trials 
    public void ControlTrial()
    {
        if (maxTrial >= trialSeqCounter && !experimentEnd) 
        {
            // add the time taken to render last frame, experiment logic is based on this parameter
            // actual time is constantly growing
            actualTime += Time.deltaTime;
            
            // the following loop is then run each frame for a new started trial
            if(actualTime >= isi_time && actualTime <= 0)
            {
                // do once to update sequence of trials and to write marker
                if (updateTrialLogic)
                {
                    // Visualize Cubes and assigned condition, do that only once
                    CubeVisible(cubeGameObjArr[CubeSeq[cubeSeqCounter]]); // determine which cube to render visible
                    AssignedCondition(NormalConflictArr[trialSeqCounter], cubeGameObjArr[CubeSeq[cubeSeqCounter]]); // Assigning Cube Condition randomly
                    // Enable flag to detect events of GameObject Cube
                    flagTouchEvent = true;
                    
                    // Generate Event Message dynamically
                    if (vibroFeedbackCondition)
                    {
                        marker.Write("box:spawned;condition:vibro;trial_nr:"+(trialSeqCounter+1)+";normal_or_conflict:"+NormalConflictArr[trialSeqCounter]+";cube:"+cubeGameObjArr[CubeSeq[cubeSeqCounter]]+";isi_time:"+(isi_time+6));
                        Debug.Log("box:spawned;condition:vibro;trial_nr:"+(trialSeqCounter+1)+";normal_or_conflict:"+NormalConflictArr[trialSeqCounter]+";cube:"+cubeGameObjArr[CubeSeq[cubeSeqCounter]]+";isi_time:"+(isi_time+6));
                    }else{
                        marker.Write("box:spawned;condition:visual;trial_nr:"+(trialSeqCounter+1)+";normal_or_conflict:"+NormalConflictArr[trialSeqCounter]+";cube:"+cubeGameObjArr[CubeSeq[cubeSeqCounter]]+";isi_time:"+(isi_time+6));
                        Debug.Log("box:spawned;condition:visual;trial_nr:"+(trialSeqCounter+1)+";normal_or_conflict:"+NormalConflictArr[trialSeqCounter]+";cube:"+cubeGameObjArr[CubeSeq[cubeSeqCounter]]+";isi_time:"+(isi_time+6));
                    }
                    
                    reaction_start_time = actualTime;
                    trialSeqCounter = trialSeqCounter + 1;
                    updateTrialLogic = false;
                    cubeSeqCounter = cubeSeqCounter + 1;

                    // reshuffle cube appearance after one "cube order" (currently 3 objects) has been presented
                    if (cubeSeqCounter == CubeSeq.Length-1)
                    {
                        cubeSeqCounter = 0;
                        RandomizeArray.ShuffleArray(CubeSeq); // Re-Randomize Cube Appearance Sequence
                        // in TU Berlin experiment (04/2018) cube location is not considered a factor of interest
                        // and is not going to be examined. Therefore randomize appearance of cube locations!   
                    }
                }
            }

            // // turn off vibro feedback
            // if((actualTime - vibro_feedback_duration) > 0 && vibroFeedback) {
			//     arduino.analogWrite(3, 0); // turn off vibro feedback
            //     vibroFeedback = false;
            //     marker.Write("vibroFeedback:off");//trial:start;trial_nr:"+(trialSeqCounter+1)+";normal_or_conflict:"+NormalConflictArr[trialSeqCounter]+";cube:"+cubeGameObjArr[CubeSeq[cubeSeqCounter]]+";isi_time:"+(isi_time+6));
            //     Debug.Log("vibroFeedback:off");//trial:start;trial_nr:"+(trialSeqCounter+1)+";normal_or_conflict:"+NormalConflictArr[trialSeqCounter]+";cube:"+cubeGameObjArr[CubeSeq[cubeSeqCounter]]+";isi_time:"+(isi_time+6));
            // }

            if (vibroFeedback)
            {
                if (elapsed > vibro_feedback_duration)
                {
                    arduino.analogWrite(3, 0); // turn off vibro feedback
                    vibroFeedback = false;
                    marker.Write("vibroFeedback:off;vibro_feedback_duration:"+elapsed);//trial:start;trial_nr:"+(trialSeqCounter+1)+";normal_or_conflict:"+NormalConflictArr[trialSeqCounter]+";cube:"+cubeGameObjArr[CubeSeq[cubeSeqCounter]]+";isi_time:"+(isi_time+6));
                    Debug.Log("vibroFeedback:off;vibro_feedback_duration:"+elapsed);//trial:start;trial_nr:"+(trialSeqCounter+1)+";normal_or_conflict:"+NormalConflictArr[trialSeqCounter]+";cube:"+cubeGameObjArr[CubeSeq[cubeSeqCounter]]+";isi_time:"+(isi_time+6));
                }
                else
                {
                    elapsed += Time.deltaTime;
                }
            }

            //if trial duration runs out, pause and wait for start of next trial
            if (actualTime > 0 && !experimentEnd)
            {
                if (updateEndOfTrialLogic && !experimentEnd)
                {
                    DisableAllCube();
                    startPos.SetActive(true);
                    //startPos.gameObject.GetComponent<Renderer>().enabled = true;
                    //startPos.gameObject.GetComponent<SphereCollider>().enabled = true;
                    updateEndOfTrialLogic = false;
                    marker.Write("visualFeedback:off");
                    Debug.Log("visualFeedback:off");
                }                
            }

            // experiment end determined by maxTrial
            if(maxTrial == trialSeqCounter && actualTime > 0) // after all trials are finished
            {
                if (!experimentEnd)
                {
                    DisableAllCube();
                    startPos.SetActive(false);
                    //startPos.gameObject.GetComponent<Renderer>().enabled = false;
                    //startPos.gameObject.GetComponent<SphereCollider>().enabled = false;
                    table.gameObject.GetComponent<Renderer>().enabled = false;
                    plane.gameObject.GetComponent<Renderer>().enabled = false;
                    endexp.gameObject.gameObject.GetComponent<Canvas>().enabled = true;
                    if (vibroFeedbackCondition){
                        marker.Write("block:end;condition:vibro");
                        Debug.Log("block:end;condition:vibro");
                    }else{
                        marker.Write("block:end;condition:visual");
                        Debug.Log("block:end;condition:visual");
                    }
                    experimentEnd = true;
                }
            }
        }
    }    

    public void trialStart()
    {
        // disable collide on start disc
        startPos.SetActive(false);
        //startPos.gameObject.GetComponent<Renderer>().enabled = false;
        //startPos.gameObject.GetComponent<SphereCollider>().enabled = false;

        // set trial timer to 7 seconds (2 sec ISI, 5 secs experiment)
        actualTime = -6.0f;
        updateTrialLogic = true;
        
        // // enable vibro feedback condition
        // if (vibroFeedbackCondition){
        //     vibroFeedback = true;
        // }

        isi_time = Random.Range(-5.0f, -4.0f);
    }

    public void nextTrial()
    {
        // set wait time to 2 second, displays visual feedback for 2 second before hiding it and starting next trial
        if (actualTime < -2.5f)
        {
            actualTime = -2.5f;
        }
        updateEndOfTrialLogic = true;
    }

    // Enable selected cube for rendering and collider properties
    public void CubeVisible(GameObject GO)
    {
        DisableAllCube();
        
        GO.SetActive(true);
        //GO.gameObject.GetComponent<Renderer>().enabled = true;
        //GO.gameObject.GetComponent<SphereCollider>().enabled = true;
    }

    // Feedback of Cube on touch
    public void VisualFeeback(GameObject GO, int eventCode)
    {
        
        // disable both possible colliders
        GO.gameObject.GetComponent<SphereCollider>().enabled = false; // no collision with the cube anymore
        //GO.gameObject.GetComponent<BoxCollider>().enabled = false; // no collision with the cube anymore
        
        // three feedback conditions in total
        // 1: visual only
        GO.gameObject.GetComponent<Renderer>().material.color = Color.red;

        // calculate reaction time between stimulus onset and touching the stimulus cubes
        reaction_time = actualTime - reaction_start_time;

        // 2: vibrotactile
        if (vibroFeedbackCondition){
            vibroFeedback = true;
            arduino.analogWrite(3, 150); // led ON
            elapsed = 0;
            GUIControl.marker.Write("box:touched;condition:vibro;vibroFeedback:on;reaction_time:"+reaction_time+";trial_nr:"+(trialSeqCounter+1)+";normal_or_conflict:"+NormalConflictArr[trialSeqCounter]+";cube:"+cubeGameObjArr[CubeSeq[cubeSeqCounter]]+";isi_time:"+(isi_time+6)+";vibro_duration:"+vibro_feedback_duration);
            Debug.Log("box:touched;condition:vibro;vibroFeedback:on;reaction_time:"+reaction_time+";trial_nr:"+(trialSeqCounter+1)+";normal_or_conflict:"+NormalConflictArr[trialSeqCounter]+";cube:"+cubeGameObjArr[CubeSeq[cubeSeqCounter]]+";isi_time:"+(isi_time+6)+";vibro_duration:"+vibro_feedback_duration);
        }
        
        // 3: EMS/FES
        else if (emsFeedbackCondition){
            for (int i = 0; i < pulseCount; i++)
            {
                SinglePulse.sendSinglePulse(1, width, current);
            }
            GUIControl.marker.Write("box:touched;emsFeedback:on;reaction_time:"+reaction_time+";trial_nr:"+(trialSeqCounter+1)+";normal_or_conflict:"+NormalConflictArr[trialSeqCounter]+";cube:"+cubeGameObjArr[CubeSeq[cubeSeqCounter]]+";isi_time:"+(isi_time+6)+";current:"+current+";width:"+width+";pulseCount:"+pulseCount);
            Debug.Log("box:touched;emsFeedback:on;reaction_time:"+reaction_time+";trial_nr:"+(trialSeqCounter+1)+";normal_or_conflict:"+NormalConflictArr[trialSeqCounter]+";cube:"+cubeGameObjArr[CubeSeq[cubeSeqCounter]]+";isi_time:"+(isi_time+6)+";current:"+current+";width:"+width+";pulseCount:"+pulseCount);
        }

        else{
             // LSL marking feedback type and intensity, todo correct make this better
            GUIControl.marker.Write("box:touched;condition:visual;reaction_time:"+reaction_time+";trial_nr:"+(trialSeqCounter+1)+";normal_or_conflict:"+NormalConflictArr[trialSeqCounter]+";cube:"+cubeGameObjArr[CubeSeq[cubeSeqCounter]]+";isi_time:"+(isi_time+6));
            Debug.Log("box:touched;condition:visual;reaction_time:"+reaction_time+";trial_nr:"+(trialSeqCounter+1)+";normal_or_conflict:"+NormalConflictArr[trialSeqCounter]+";cube:"+cubeGameObjArr[CubeSeq[cubeSeqCounter]]+";isi_time:"+(isi_time+6));
        }

        // continue with next trial
        nextTrial();

    }

    // Assigned randomly Normal and Conflict condition
    public void AssignedCondition(int type, GameObject GO)
    {
        if (type == 0)
        {
            //GO.GetComponent<SphereCollider>().radius = 1; // will be scaled by 0.03	
            //GO.GetComponent<BoxCollider>().enabled = false;
            GO.GetComponent<SphereCollider>().enabled = true;
        }
        else
        {
            GO.GetComponent<SphereCollider>().enabled = true;
            GO.GetComponent<SphereCollider>().radius = 3f; // will be scaled by 0.03
        }
    }

    // Assigned randomly Smalle and Bigger Cube Size
    // not used in TU Berlin setup 04/2018
    public void AssignedCubeSize(int type, GameObject GO)
    {
        if (type == 0)  // Set Smaller 
        {
            GO.transform.localScale= new Vector3(0.03f, 0.03f, 0.03f);
            GO.transform.position = new Vector3(GO.transform.position.x, 0.75f, GO.transform.position.z); //Adjust Y-position
            	
        }
        else // Set Bigger
        {
            GO.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        }
    }

    // Update is called once per frame
    void Update () {

        //Debug.Log(hand.transform.position);
        //Debug.Log("finger");
        //Debug.Log(index_finger.transform.position);

        ControlState(); // run only once after hitting any key to start, in fact runs whenever key is hit
        if (flagStart) // flagStart is true after hitting any key to start at the beginning
        {
            ControlTrial();
        }
        // var distance1 = Vector3.Distance(startPos.transform.position, cubeLeft.transform.position);
        // Debug.Log(distance1);
        // var distance2 = Vector3.Distance(startPos.transform.position, cubeRight.transform.position);
        // Debug.Log(distance2);
        // var distance3 = Vector3.Distance(startPos.transform.position, cubeMiddle.transform.position);
        // Debug.Log(distance3);
    }
}
