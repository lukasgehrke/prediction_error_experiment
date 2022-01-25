using UnityEngine;
using System.Collections;
using Assets.LSL4Unity.Scripts; // reference the LSL4Unity namespace to get access to all classes
using Uniduino; // adding arduino

public class GUIControl : MonoBehaviour {

    // create LSL markerstream
    public static LSLMarkerStream marker;

    // add arduino
    public static Arduino arduino;

    public static GameObject table, plane, leapmotion, instruct, textBox, endexp, startPos, rehastim, resting, questionnaire, q, ra, la, send;
    GameObject cubeMiddle, cubeLeft, cubeRight;

    GameObject[] cubeGameObjArr = new GameObject[3];  // Game Object Array

    Vector3 cube1Vector, cube2Vector;

    // main GUI/Experiment parameters
    public int nrOfTrials = 100;
    public bool training = false;
    
    public int repeatNrOfTrials = 3;
    //[HideInInspector] // Hides var below
    private int repetition = 1;
    //[HideInInspector] // Hides var below
    private int currentBlock = 0;
    //public int volatility = 1;
    public float isiTime = -5f;

    // sets intensity level of EMS/FES feedback
    public bool emsFeedbackCondition;
    public int emsWidth = 200;
    public int emsCurrent = 5;
    public int pulseCount = 12;

    // sets intensity level of vibrotactile feedback
    public bool vibroFeedbackCondition;
    public float vibroFeedbackDuration = 0.1f;
    public int vibroStrength = 150;

    // Cube Seq if only Bigger cube has been used
    public static int[] CubeSeq = new int[] { 0, 1, 2 }; 
    float sphereRadius = 0;
    public static float actualTime = 0; // start timer at 0

    private int[] NormalConflictArr;
    
    public static int trialSeqCounter;
    public static int cubeSeqCounter = 0;
    
    [HideInInspector] // Hides var below
    public bool flagTouchEvent = false;
    public static bool flagStart = false;
    
    // trial logic handler
    public static bool updateTrialLogic = false;
    public static bool updateEndOfTrialLogic = false;
    public static bool experimentEnd = false;
    private bool endUpdate = true;

    // logic vibro feedback
    public static float elapsed;
    public static bool vibroFeedback = false;

    // event markers feedback channel
    public static string feedback_type = "";
    public static string normConflict = "";

    // parameter variables
    public static float reaction_time = 0;
    public static float reaction_start_time = 0;

    // questionnaire variables
    public static bool startedQuestionnaire = false;
    public static bool nextQuestion = false;
    public static string answer = "0";
    public static string lastAnswer = "0";
    public static int questionNr = 0;
    private string elem = "0";
    private bool answered = false;
    public static int waitInterval = 0;
    string[] questions = new string[]{"In der computererzeugten Welt hatte ich den Eindruck,\r\ndort gewesen zu sein...", 
        "Ich hatte das Gefühl, in dem virtuellen Raum zu handeln\r\nstatt etwas von außen zu bedienen.",
        "Wie bewußt war Ihnen die reale Welt, während Sie sich durch die virtuelle\r\nWelt bewegten (z.B. Geräusche, Raumtemperatur, andere Personen etc.)?",
        "Wie sehr glich ihr Erleben der virtuellen Umgebung dem Erleben einer realen Umgebung"}; 
    string[] left_anchors = new string[]{"überhaupt nicht", "trifft gar nicht zu", "extrem bewußt", "überhaupt nicht"};
    string[] right_anchors = new string[]{"sehr stark", "trifft völlig zu", "unbewußt", "vollständig"};
    
    // Use this for initialization
    void Start()
    {

        NormalConflictArr = new int[nrOfTrials];

        if (vibroFeedbackCondition){
            feedback_type = "vibro";
        }
        else if (emsFeedbackCondition){
            feedback_type = "ems";
        }else feedback_type = "visual";

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
        textBox = GameObject.Find("TextBox");
        endexp = GameObject.Find("End");
        startPos = GameObject.Find("startPosDisc");
        resting = GameObject.Find("resting");
        questionnaire = GameObject.Find("questionnaire");
        q = GameObject.Find("question");
        ra = GameObject.Find("right_anchor");
        la = GameObject.Find("left_anchor");
        send = GameObject.Find("send");

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
        if (!training)
        {
            NormalConflictArr = RandomizeArray.GenerateArraySequences(nrOfTrials, 0.1, 5);
        }

        // todo fix GenerateArraySequence function so volatility param works
        // if (volatility == 1)
        // {
        //     NormalConflictArr = RandomizeArray.GenerateArraySequences(nrOfTrials, 0.1, 5);
        // }
        // else if (volatility == 0)
        // {
        //     NormalConflictArr = RandomizeArray.GenerateArraySequences(nrOfTrials, 0.1, 5);
        // }
        
        // instruct and leapmotion game object remain visible and only table, plane and the end instruction are made invisible
        table.gameObject.GetComponent<Renderer>().enabled = false;
        plane.gameObject.GetComponent<Renderer>().enabled = false;
        instruct.gameObject.gameObject.GetComponent<Canvas>().enabled = true;
        endexp.gameObject.gameObject.GetComponent<Canvas>().enabled = false;
        resting.gameObject.GetComponent<Renderer>().enabled = false;
        questionnaire.SetActive(false);

        // hide startPosDisc
        startPos.SetActive(false);
        DisableAllCube(); // set color to white, disable renderer and sphereCollider

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

    }

    // Start of experiment
    public void ControlState()
    {    
        if (Input.GetMouseButtonDown(0))
        // after hitting any key once, the Input.anyKeyDown is otherwise false for every frame update
        // therefore the if clause code is only run once at the beginning when hitting any key to start
        {

            //trialSeqCounter = 99;
            // run a block of the experiment
            flagStart = true;
            experimentEnd = false;
            currentBlock += 1;
            trialSeqCounter = 0;

            // Making instruction invisible in scene and start rendering table and plane
            table.gameObject.GetComponent<Renderer>().enabled = true;
            plane.gameObject.GetComponent<Renderer>().enabled = true;
            instruct.gameObject.GetComponent<Canvas>().enabled = false;
            endexp.gameObject.gameObject.GetComponent<Canvas>().enabled = false;
            questionnaire.SetActive(false);

            // enable collision possibility
            startPos.SetActive(true);
            resting.gameObject.GetComponent<Renderer>().enabled = true;
            GUIControl.marker.Write("block:start;currentBlockNr:"+currentBlock+";condition:"+feedback_type+";training:"+BoolToString(training));
            Debug.Log("block:start;currentBlockNr:"+currentBlock+";condition:"+feedback_type+";training:"+BoolToString(training));
        }
    }

    //  Control all trials 
    public void ControlTrial()
    {
        // add the time taken to render last frame, experiment logic is based on this parameter
        // actual time is constantly growing
        actualTime += Time.deltaTime;

        if (trialSeqCounter < nrOfTrials && !experimentEnd) // run all trials
        {   
            // the following loop is then run each frame for a new started trial
            if(actualTime >= isiTime && actualTime <= 0)
            {
                // do once to update sequence of trials and to write marker
                if (updateTrialLogic)
                {
                    // Visualize Cubes and assigned condition, do that only once
                    CubeVisible(cubeGameObjArr[CubeSeq[cubeSeqCounter]]); // determine which cube to render visible

                    if (!training)
                    {
                        AssignedCondition(NormalConflictArr[trialSeqCounter], cubeGameObjArr[CubeSeq[cubeSeqCounter]]); // Assigning Cube Condition randomly
                    }else
                    {
                        AssignedCondition(0, cubeGameObjArr[CubeSeq[cubeSeqCounter]]);
                    }
                    
                    // Enable flag to detect events of GameObject Cube
                    flagTouchEvent = true;
                    // Generate Event Message dynamically
                    if (NormalConflictArr[trialSeqCounter] == 1 && !training){
                        normConflict = "conflict";
                    }else{
                        normConflict = "normal";
                    }
                    reaction_start_time = actualTime;
                    updateTrialLogic = false;

                    GUIControl.marker.Write("box:spawned;condition:"+feedback_type+";trial_nr:"+(((currentBlock-1)*100)+trialSeqCounter+1)+";normal_or_conflict:"+normConflict+";cube:"+cubeGameObjArr[CubeSeq[cubeSeqCounter]]+";isiTime:"+(isiTime+6));
                    Debug.Log("box:spawned;condition:"+feedback_type+";trial_nr:"+(((currentBlock-1)*100)+trialSeqCounter+1)+";normal_or_conflict:"+normConflict+";cube:"+cubeGameObjArr[CubeSeq[cubeSeqCounter]]+";isiTime:"+(isiTime+6));   
                }
            }

            //if trial duration runs out, pause and wait for start of next trial
            if (actualTime > 0 && !experimentEnd)
            {
                if (updateEndOfTrialLogic && !experimentEnd)
                {
                    DisableAllCube();
                    startPos.SetActive(true);
                    updateEndOfTrialLogic = false;
                    GUIControl.marker.Write("visualFeedback:off");
                    Debug.Log("visualFeedback:off");
                }                
            }
        }

        if (vibroFeedback)
        {
            if (elapsed > vibroFeedbackDuration)
            {
                arduino.analogWrite(3, 0); // turn off vibro feedback
                vibroFeedback = false;
                if (!startedQuestionnaire)
                {
                    GUIControl.marker.Write("vibroFeedback:off;vibroFeedbackDuration:"+elapsed);
                    Debug.Log("vibroFeedback:off;vibroFeedbackDuration:"+elapsed);
                }
            }
            else
            {
                elapsed += Time.deltaTime;
            }
        }

        // experiment end determined by nrOfTrials
        if(trialSeqCounter == nrOfTrials && actualTime > 0 && !experimentEnd) // after all trials are finished
        {
            // block end, pause after 100 trials
            if (repetition < repeatNrOfTrials)
            {
                repetition += 1;
                endexp.gameObject.gameObject.GetComponent<Canvas>().enabled = true;
                DisableAllCube();
                startPos.SetActive(false);
                flagStart = false;
            }
            else
            {
                if (!startedQuestionnaire)
                {
                    DisableAllCube();
                    startPos.SetActive(false);
                    questionnaire.SetActive(true);
                    startedQuestionnaire = true;
                    send.gameObject.GetComponent<Renderer>().material.color = Color.green;
                }

                if (nextQuestion)
                {
                    q.GetComponent<UnityEngine.UI.Text>().text = questions[questionNr];
                    la.GetComponent<UnityEngine.UI.Text>().text = left_anchors[questionNr];
                    ra.GetComponent<UnityEngine.UI.Text>().text = right_anchors[questionNr];
                    send.gameObject.GetComponent<Renderer>().material.color = Color.green;
                }
            }
        }

        if (experimentEnd && actualTime > 0 && endUpdate) //!experimentEnd
        {
            questionnaire.SetActive(false);
            table.gameObject.GetComponent<Renderer>().enabled = false;
            plane.gameObject.GetComponent<Renderer>().enabled = false;
            resting.gameObject.GetComponent<Renderer>().enabled = false;
            endexp.gameObject.gameObject.GetComponent<Canvas>().enabled = true;
            GUIControl.marker.Write("block:end;currentBlockNr:"+currentBlock+";condition:"+feedback_type+";training:"+BoolToString(training));
            Debug.Log("block:end;currentBlockNr:"+currentBlock+";condition:"+feedback_type+";training:"+BoolToString(training));
            endUpdate = false;
        }
    }    

    public void trialStart()
    {
        // disable collide on start disc
        startPos.SetActive(false);

        // set trial timer to 6 seconds (1-2 sec ISI, 4 secs response time)
        actualTime = -6.0f;
        updateTrialLogic = true;

        isiTime = Random.Range(-5.0f, -4.0f);
    }

    public void nextTrial()
    {
        trialSeqCounter = trialSeqCounter + 1;
        cubeSeqCounter = cubeSeqCounter + 1;
        // reshuffle cube appearance after one "cube order" (emsCurrently 3 objects) has been presented
        if (cubeSeqCounter == CubeSeq.Length-1)
        {
            cubeSeqCounter = 0;
            RandomizeArray.ShuffleArray(CubeSeq); // Re-Randomize Cube Appearance Sequence
            // in TU Berlin experiment (04/2018) cube location is not considered a factor of interest
            // and is not going to be examined. Therefore randomize appearance of cube locations!   
        }
        // set wait time to 2 second, displays visual feedback for 2 second before hiding it and starting next trial
        if (actualTime < -2.5f)
        {
            actualTime = -2.5f;
        }
        updateEndOfTrialLogic = true;
    }

    // write marker for questionnaire results
    public void QuestMarker(GameObject GO)
    {

        if (GO.name == "send" && answered)
        {
            GUIControl.marker.Write("ipq_question_nr_"+(questionNr+1)+"_answer:"+answer);
            Debug.Log("ipq_question_nr_"+(questionNr+1)+"_answer:"+answer);

            // set answered flag
            questionNr += 1;
            nextQuestion = true;
            actualTime = -1.0f;

            // reset colors
            GO.GetComponent<Renderer>().material.color = Color.red;
            GameObject.Find(lastAnswer).GetComponent<Renderer>().material.color = Color.white;
            answered = false;
            lastAnswer = "0";
        }

        if (!(GO.name == "send") && !(GO.gameObject.GetComponent<Renderer>().material.color == Color.red))
        {
            GO.gameObject.GetComponent<Renderer>().material.color = Color.red;
            answer = GO.name;
            answered = true;

            if (lastAnswer == "0")
            {
                lastAnswer = answer;
            }
            else
            // find gameobject of last answer and make white
            {
                GameObject.Find(lastAnswer).GetComponent<Renderer>().material.color = Color.white;
                lastAnswer = answer;
            }
        }
        
        if (questionNr > 3)
        {
            experimentEnd = true;
            //wait for 2 seconds in the main loop before showing end of currentBlock
            actualTime = -2.0f;
        }
    }

    // Enable selected cube for rendering and collider properties
    public void CubeVisible(GameObject GO)
    {
        DisableAllCube();
        
        GO.SetActive(true);
    }

    // Feedback of Cube on touch
    public void VisualFeeback(GameObject GO)
    {

        if (!startedQuestionnaire)
        {
            // disable both possible colliders
            GO.gameObject.GetComponent<SphereCollider>().enabled = false; // no collision with the cube anymore
            
            // three feedback conditions in total
            // 1: visual only
            GO.gameObject.GetComponent<Renderer>().material.color = Color.red;

            // calculate reaction time between stimulus onset and touching the stimulus cubes
            reaction_time = actualTime - reaction_start_time;
        }

        // 2: vibrotactile
        if (vibroFeedbackCondition){
            vibroFeedback = true;
            arduino.analogWrite(3, vibroStrength); // motor ON
            elapsed = 0;
            if (!startedQuestionnaire)
            {
                GUIControl.marker.Write("box:touched;condition:"+feedback_type+";vibroFeedback:on;reaction_time:"+reaction_time+";trial_nr:"+(((currentBlock-1)*100)+trialSeqCounter+1)+";normal_or_conflict:"+normConflict+";cube:"+GO+";isiTime:"+(isiTime+6)+";vibro_duration:"+vibroFeedbackDuration);
                Debug.Log("box:touched;condition:"+feedback_type+";vibroFeedback:on;reaction_time:"+reaction_time+";trial_nr:"+(((currentBlock-1)*100)+trialSeqCounter+1)+";normal_or_conflict:"+normConflict+";cube:"+GO+";isiTime:"+(isiTime+6)+";vibro_duration:"+vibroFeedbackDuration);
            }
        }
         
        // 3: EMS/FES
        if (emsFeedbackCondition){
            for (int i = 0; i < pulseCount; i++)
            {
                SinglePulse.sendSinglePulse(1, emsWidth, emsCurrent);
            }
            if (!startedQuestionnaire)
            {
                GUIControl.marker.Write("box:touched;condition:"+feedback_type+";emsFeedback:on;reaction_time:"+reaction_time+";trial_nr:"+(((currentBlock-1)*100)+trialSeqCounter+1)+";normal_or_conflict:"+normConflict+";cube:"+GO+";isiTime:"+(isiTime+6)+";emsCurrent:"+emsCurrent+";emsWidth:"+emsWidth+";pulseCount:"+pulseCount);
                Debug.Log("box:touched;condition:"+feedback_type+";emsFeedback:on;reaction_time:"+reaction_time+";trial_nr:"+(((currentBlock-1)*100)+trialSeqCounter+1)+";normal_or_conflict:"+normConflict+";cube:"+GO+";isiTime:"+(isiTime+6)+";emsCurrent:"+emsCurrent+";emsWidth:"+emsWidth+";pulseCount:"+pulseCount);
            }
        }
        else
        {
            if (!startedQuestionnaire)
            {
                // LSL marking feedback type and intensity, todo correct make this better
                GUIControl.marker.Write("box:touched;condition:"+feedback_type+";reaction_time:"+reaction_time+";trial_nr:"+(((currentBlock-1)*100)+trialSeqCounter+1)+";normal_or_conflict:"+normConflict+";cube:"+GO+";isiTime:"+(isiTime+6));
                Debug.Log("box:touched;condition:"+feedback_type+";reaction_time:"+reaction_time+";trial_nr:"+(((currentBlock-1)*100)+trialSeqCounter+1)+";normal_or_conflict:"+normConflict+";cube:"+GO+";isiTime:"+(isiTime+6));           
            }
        }

        if (!startedQuestionnaire){
            // continue with next trial
            nextTrial();
        }
    }

    // Assigned randomly Normal and Conflict condition
    public void AssignedCondition(int type, GameObject GO)
    {
        if (type == 0)
        {
            GO.GetComponent<SphereCollider>().enabled = true;
            GO.GetComponent<SphereCollider>().radius = 0.85f;
        }
        else
        {
            GO.GetComponent<SphereCollider>().enabled = true;
            GO.GetComponent<SphereCollider>().radius = 3f; // will be scaled by 3
        }
    }

    // Assigned randomly Smalle and Bigger Cube Size
    // not used in TU Berlin setup 04/2018
    public void AssignedCubeSize(int type, GameObject GO)
    {
        if (type == 0)  // Set Smaller 
        {
            GO.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
            GO.transform.position = new Vector3(GO.transform.position.x, 0.75f, GO.transform.position.z); //Adjust Y-position
            	
        }
        else // Set Bigger
        {
            GO.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        }
    }

     static string BoolToString(bool b)
    {
        return b ? "true":"false";
    }

    // Update is called once per frame
    void Update () {

        //Debug.Log(emsCurrent);     

        ControlState(); // run only once after hitting any key to start, in fact runs whenever key is hit
        if (flagStart) // flagStart is true after hitting any key to start at the beginning or after block end
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
