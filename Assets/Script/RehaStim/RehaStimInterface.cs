using UnityEngine;

#if UNITY_EDITOR

using System.IO.Ports;

#endif

/**
 * Sends messages to the RehaStim Device
 */

public class RehaStimInterface : MonoBehaviour
{
#if UNITY_EDITOR
    private static SerialPort Port;
#endif
    public string PortName = "COM4";
    public static RehaStimInterface instance;
    public static int maxPulseCurrent = 25; // the default maximum pulse_current in mA
    public static int maxPulseWidth = 400; // the default maximum pulse_width in microseconds

    // Use this for initialization
    public void Start()
    {
        instance = this;
        init();
    }

    private void OnDestroy()
    {
        close();
    }

    public bool enableEMS;

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            if (!enableEMS)
            {
                ChannelList.Stop();
            }

            init();

            enable = enableEMS;
        }
    }

    protected void init()
    {
#if UNITY_EDITOR
        if (enableEMS && RehaStimInterface.Port == null)
        {
            try
            {
                RehaStimInterface.Port = new SerialPort();
                RehaStimInterface.Port.PortName = PortName;
                RehaStimInterface.Port.BaudRate = 115200;
                RehaStimInterface.Port.Parity = Parity.None;
                RehaStimInterface.Port.DataBits = 8;
                RehaStimInterface.Port.StopBits = StopBits.Two;
                RehaStimInterface.Port.Open();
                //
                Debug.Log("Opening Serial Port: " + PortName);
            }
            catch (System.Exception e)
            {
                print(e.Message);
                enableEMS = false;
                enable = false;
            }
        }
#endif
    }

    protected void close()
    {
        ChannelList.Stop();
#if UNITY_EDITOR
        if (RehaStimInterface.Port != null)
        {
            RehaStimInterface.Port.Close();
            RehaStimInterface.Port.Dispose();
            //print("Closed Serial Port " + RehaStimInterface.Port.PortName);
            RehaStimInterface.Port = null;
        }
#endif
    }

    public static bool enable;

    public static bool sendMessage(byte[] message)
    {
        if (!enable)
            return false;

#if UNITY_EDITOR
        SerialPort sp = RehaStimInterface.Port;
        if (sp == null || !sp.IsOpen)
        {
            //print ("Error: Serial Port not open");
            return false;
        }
        sp.Write(message, 0, message.Length);
#endif
        //print(System.BitConverter.ToString(message));
        return true;
    }
}