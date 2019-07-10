using UnityEngine;
using System.Collections;
using Uniduino;

public class vibro_motor : MonoBehaviour {

	public Arduino arduino;
 
	void Start () {
		arduino = Arduino.global;
		arduino.Setup(ConfigurePins);
		StartCoroutine(BlinkLoop());
	}
	
	void ConfigurePins() {
		arduino.pinMode(13, PinMode.OUTPUT);
	}
	
	IEnumerator BlinkLoop() {
		while(true) {   
			arduino.digitalWrite(13, Arduino.HIGH); // led ON
			yield return new WaitForSeconds(1);
			arduino.digitalWrite(13, Arduino.LOW); // led OFF
			yield return new WaitForSeconds(1);
		}           
	}

	//     public Arduino arduino;
    // // Use this for initialization
    // void Start () {
    //     arduino = Arduino.global;
    //     arduino.Setup(ConfigurePins);   
    // }

    // void ConfigurePins( )
    // {
    //     arduino.pinMode (9, PinMode.PWM); //vibration motor
    // }

    // // Update is called once per frame
    // void Update () {
	// if (Input.GetKey(KeyCode.Q)){
	// 	arduino.analogWrite(9, 255);
	// } else if (Input.GetKey(KeyCode.W)){
	// 	arduino.analogWrite(9, 0);
	// }
    // }
}
