using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

public class TransformPosish : MonoBehaviour {

public Controller controller;
public GameObject toTrack;

	// Use this for initialization
	void Start () {
		controller = new Controller();
	}
	
	// Update is called once per frame
	void Update () {
		if(toTrack.activeInHierarchy){
			GetComponent<SkinnedMeshRenderer>().enabled = true;
			transform.position = toTrack.transform.position;
            transform.rotation = toTrack.transform.rotation;
			transform.Rotate(90, -90, 0);
		}

	// void Update_old(){
	// 	Frame frame = controller.Frame (); // controller is a Controller object
	// 	InteractionBox iBox = frame.InteractionBox;
	// 	if(frame.Hands.Count > 0){
	// 		List<Hand> hands = frame.Hands;
	// 		Hand firstHand = hands [0];
	// 		transform.position = leapToWorld(firstHand.PalmPosition, iBox).ToVector3();
	// 	}
	// }

	}

	Leap.Vector leapToWorld(Leap.Vector leapPoint, InteractionBox iBox) {
		leapPoint.z *= -1.0f; //right-hand to left-hand rule
		Leap.Vector normalized = iBox.NormalizePoint(leapPoint, false);
		normalized += new Leap.Vector(0.5f, 0f, 0.5f); //recenter origin
		return normalized; //scale
	}
}
