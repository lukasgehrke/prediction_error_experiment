using UnityEngine;
using System.Collections;

public class CubeBigLeft : MonoBehaviour {

    public GUIControl GUIC;

    void OnTriggerEnter(Collider other) // in case of collision with leap motion tracked hand and cube gameObject
    {        
        //if (GUIControl.flagTouchEvent)
        if (GUIC.flagTouchEvent)
        {
            GUIC.VisualFeeback(gameObject);
            //GUIControl.flagTouchEvent = false; // Disable touch event action until next trial 
            GUIC.flagTouchEvent = false; // Disable touch event action until next trial 
        }             
    }
}
