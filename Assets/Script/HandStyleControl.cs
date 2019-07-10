using UnityEngine;
using System.Collections;
using Leap.Unity;

public class HandStyleControl : MonoBehaviour {
    public HandPool HandPool;
    public string[] GroupNames;
    private int currentGroup;

    public GameObject dummy_hand_L;
    public GameObject dummy_hand_R;
    public GameObject handR;

    // Use this for initialization
    void Start () {
        HandPool = GetComponent<HandPool>();
		//HandPool.SetGroup("PhyHand");
		//disableAllGroups();
	}

	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Alpha1)){
            selectHand(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)){
            selectHand(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)){
            selectHand(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            //disableAllGroups();
        }
    }

    public void selectHand(int inputNumber)
    {
		if (inputNumber == 1)
        {
			disableArrow();
			HandPool.SetGroup("RealHand");
            handR.SetActive(false);
            //disableAllGroups();
            //HandPool.EnableGroup("RealHand");
        }
        if (inputNumber == 2)
        {
			disableArrow();
			HandPool.SetGroup("RobotHand");
            handR.SetActive(false);
			//disableAllGroups();
			//HandPool.EnableGroup("RobotHand");
		}
		if (inputNumber == 3)
        {
			HandPool.SetGroup("PhyHand");
            handR.SetActive(true);
			//enableArrow();
        }
    }

	private void enableArrow() {
		//HandPool.SetGroup("PhyHand");
		dummy_hand_L.SetActive(true);
		dummy_hand_R.SetActive(true);
	}

	private void disableArrow()
    {
        dummy_hand_L.SetActive(false);
        dummy_hand_R.SetActive(false);
    }
}
