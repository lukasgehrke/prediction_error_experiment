using System.Collections;
using UnityEngine;

public class Globalstimulation : MonoBehaviour
{
    //[Header("EMS  Settings")]
    [Header("Wrist")]
    public bool useWrist;

    [Tooltip("max Value in mA")]
    public int wristCurrent = 15;

    [Tooltip("in ms")]
    public int wristWidthL = 100;

    [Tooltip("in ms")]
    public int wristWidthR = 100;

    [Header("Biceps")]
    public bool useBiceps;

    [Tooltip("max Value in mA")]
    public int bicepsCurrent = 15;

    [Tooltip("in ms")]
    public int bicepsWidthL = 100;

    [Tooltip("in ms")]
    public int bicepsWidthR = 100;

    [Header("Shoulder")]
    public bool useShoulder;

    [Tooltip("max Value in mA")]
    public int shoulderCurrent = 15;

    [Tooltip("in ms")]
    public int shoulderWidthL = 100;

    [Tooltip("in ms")]
    public int shoulderWidthR = 100;

    public Animation handL;
    public Animation handR;

    // Use this for initialization
    private void OnValidate()
    {
        // foreach (CubeStimulation stim in FindObjectsOfType<CubeStimulation>())
        // {
        //     stim.wristCurrent = wristCurrent;
        //     stim.wristWidthL = wristWidthL;
        //     stim.wristWidthR = wristWidthR;
        //     stim.bicepsCurrent = bicepsCurrent;
        //     stim.bicepsWidthL = bicepsWidthL;
        //     stim.bicepsWidthR = bicepsWidthR;
        //     stim.shoulderCurrent = shoulderCurrent;
        //     stim.shoulderWidthL = shoulderWidthL;
        //     stim.shoulderWidthR = shoulderWidthR;
        // }
        // foreach (SolidObjectColliderBehaviour stim in FindObjectsOfType<SolidObjectColliderBehaviour>())
        // {
        //     stim.wristCurrent = wristCurrent;
        //     stim.wristWidthL = wristWidthL;
        //     stim.wristWidthR = wristWidthR;
        //     stim.bicepsCurrent = bicepsCurrent;
        //     stim.bicepsWidthL = bicepsWidthL;
        //     stim.bicepsWidthR = bicepsWidthR;
        //     stim.shoulderCurrent = shoulderCurrent;
        //     stim.shoulderWidthL = shoulderWidthL;
        //     stim.shoulderWidthR = shoulderWidthR;
        // }
        // foreach (ElectroWall stim in FindObjectsOfType<ElectroWall>())
        // {
        //     stim.wristCurrent = wristCurrent;
        //     stim.wristWidthL = wristWidthL;
        //     stim.wristWidthR = wristWidthR;
        //     stim.bicepsCurrent = bicepsCurrent;
        //     stim.bicepsWidthL = bicepsWidthL;
        //     stim.bicepsWidthR = bicepsWidthR;
        // }
        // foreach (PunchColliderBehaviour stim in FindObjectsOfType<PunchColliderBehaviour>())
        // {
        //     stim.wristCurrent = wristCurrent;
        //     stim.wristWidthL = wristWidthL;
        //     stim.wristWidthR = wristWidthR;
        //     stim.bicepsCurrent = bicepsCurrent;
        //     stim.bicepsWidthL = bicepsWidthL;
        //     stim.bicepsWidthR = bicepsWidthR;
        //     stim.shoulderCurrent = shoulderCurrent;
        //     stim.shoulderWidthL = shoulderWidthL;
        //     stim.shoulderWidthR = shoulderWidthR;
        // }

        // foreach (ElectroWall wall in FindObjectsOfType<ElectroWall>())
        // {
        //     wall.handAnimationL = handL;
        //     wall.handAnimationR = handR;
        // }
    }

    // Update is called once per frame
    private void Update()
    {
    }
}