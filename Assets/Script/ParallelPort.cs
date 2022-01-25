using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;

public class ParallelPort : MonoBehaviour {

    // Use this for initialization
    [DllImport(@"C:\Users\CIBCI\Documents\VRExperiment\Assets\Plugins\x86_64\inpoutx64.dll", EntryPoint = "IsInpOutDriverOpen")]
    public static extern UInt32 IsInpOutDriverOpen_x64();
    [DllImport(@"C:\Users\CIBCI\Documents\VRExperiment\Assets\Plugins\x86_64\inpoutx64.dll", EntryPoint = "Out32")]
    public static extern void Output(int PortAddress, int Data);
    [DllImport(@"C:\Users\CIBCI\Documents\VRExperiment\Assets\Plugins\x86_64\inpoutx64.dll", EntryPoint = "Inp32")]
    public static extern char Inp32_x64(short PortAddress);

    void Start () {

       // Output(255, 956);

    }
	
	// Update is called once per frame
	void Update () {
	
	}


   
}
