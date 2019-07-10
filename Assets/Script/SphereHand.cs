using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Leap;
using System;
using Leap.Unity;

public class SphereHand : MonoBehaviour {
	Controller controller;
	InteractionBox iBox;
	public GameObject lm_palm;


	void Start(){
		controller = new Controller ();
		//lm_palm= GameObject.Find("palm");
	}

	void Update(){	

		if (lm_palm.activeInHierarchy)
        {
            transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().enabled = true;
            transform.position = lm_palm.transform.position;
            transform.rotation = lm_palm.transform.rotation;
            transform.Rotate(0, -90, 0);
        }

	}

	void Update_old(){
		Frame frame = controller.Frame (); // controller is a Controller object
		InteractionBox iBox = frame.InteractionBox;
		if(frame.Hands.Count > 0){
			List<Hand> hands = frame.Hands;
			Hand firstHand = hands [0];
			transform.position = leapToWorld(firstHand.PalmPosition, iBox).ToVector3();
		}
	}

	Leap.Vector leapToWorld(Leap.Vector leapPoint, InteractionBox iBox)
	{
		leapPoint.z *= -1.0f; //right-hand to left-hand rule
		Leap.Vector normalized = iBox.NormalizePoint(leapPoint, false);
		normalized += new Leap.Vector(0.5f, 0f, 0.5f); //recenter origin
		return normalized; //scale
	}

}
