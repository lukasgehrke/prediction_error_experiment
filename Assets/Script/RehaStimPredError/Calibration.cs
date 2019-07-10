using UnityEngine;

public class Calibration : MonoBehaviour
{
    public int pulseCurrent = 15;
    public int pulseWidth = 100;
    public int pulseCount = 1;
    public Channels channel = Channels.Channel1;

    public void sendPulse()
    {
        print("test");
        for (int i = 0; i < pulseCount; i++)
        {
            SinglePulse.sendSinglePulse(channel, pulseWidth, pulseCurrent);
        }
    }
}