using UnityEngine;
using System.Collections;

public class CubeBigLeft : MonoBehaviour {

    GUIControl GUIC = new GUIControl();

    void OnTriggerEnter(Collider other) // in case of collision with leap motion tracked hand and cube gameObject
    {        
        if (GUIControl.flagTouchEvent)
        {
            GUIC.VisualFeeback(gameObject, GUIControl.eventMsg);
            GUIControl.flagTouchEvent = false; // Disable touch event action until next trial 
        }             
    }
}
