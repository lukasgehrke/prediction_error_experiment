using System.IO.Ports;
using System.Threading;
using UnityEngine;

public class SerialTest : PersistBehaviour
{
    public static SerialPort sp;

    // Use this for initialization
    private void Start()
    {
        Debug.Log("init serialtest");
    }

    public KeyCode startKey = KeyCode.UpArrow;
    public KeyCode stopKey = KeyCode.DownArrow;

    public int pulseLoopFrequency = 1;
    public int groupFrequency = 30;
    public int channelFrequency = 100; // 18*0.5ms +1ms => 10ms => ~ 100Hz
    public Channels channel = Channels.Channel1;

    [Range(0, 300)]
    public int pulseWidth = 100;

    [Range(0, 30)]
    public int milliAmps = 8;

    public Channels channel2 = Channels.Channel2;

    [Range(0, 300)]
    public int pulseWidth2 = 100;

    [Range(0, 30)]
    public int milliAmps2 = 8;

    private bool channel1Active = false;
    private bool channel2Active = false;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown("space"))
			TestSinglePulse();
			
        else if (Input.GetKeyDown(startKey))
            {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                startPulseThread();
            }
            else
            {
                ChannelMask channels = new ChannelMask();
                channels.setChannel((int)channel);
                channels.setChannel((int)channel2);

                ChannelList.InitCM(0
                        , channels
                        , new ChannelMask()
                        , groupFrequency
                        , channelFrequency);
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (channel1Active)
            {
                print("Stop Channel 1");
                ChannelList.StopChannel((int)channel);
            }
            else
            {
                print("Start Channel 1");
                ChannelList.UpdateChannel((int)channel, new UpdateInfo(UpdateInfo.MODE_SINGLE
                                                                        , pulseWidth
                                                                        , milliAmps));
            }

            channel1Active = !channel1Active;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (channel2Active)
            {
                print("Stop Channel 2");
                ChannelList.StopChannel((int)channel2);
            }
            else
            {
                print("Start Channel 2");
                ChannelList.UpdateChannel((int)channel2, new UpdateInfo(UpdateInfo.MODE_SINGLE
                                                                        , pulseWidth2
                                                                        , milliAmps2));
            }

            channel2Active = !channel2Active;
            //ChannelList.StopChannel((int)channel);
            //ChannelList.UpdateChannel((int)channel2, new UpdateInfo(0, pulseWidth2, milliAmps2));
        }
        else if (Input.GetKeyDown(stopKey))
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                stopPulseThread();
            }
            else
            {
                channel1Active = false;
                channel2Active = false;
                ChannelList.Stop();
            }

            //RehaStimInterface.sendMessage(ChannelList.GetStopCommand());
        }
        else if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            if (channel1Active)
            {
                pulseWidth += 5;
                ChannelList.UpdateChannel((int)channel, new UpdateInfo(UpdateInfo.MODE_SINGLE, pulseWidth, milliAmps));
            }
        }
        else if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            if (channel1Active)
            {
                pulseWidth -= 5;
                ChannelList.UpdateChannel((int)channel, new UpdateInfo(UpdateInfo.MODE_SINGLE, pulseWidth, milliAmps));
            }
            //RehaStimInterface.sendMessage(ChannelList.GetStopCommand());
        }
        else if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            if (channel1Active)
            {
                milliAmps += 1;
                ChannelList.UpdateChannel((int)channel, new UpdateInfo(UpdateInfo.MODE_SINGLE, pulseWidth, milliAmps));
            }
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            if (channel1Active)
            {
                milliAmps -= 1;
                ChannelList.UpdateChannel((int)channel, new UpdateInfo(UpdateInfo.MODE_SINGLE, pulseWidth, milliAmps));
            }
            //RehaStimInterface.sendMessage(ChannelList.GetStopCommand());
        }
        //StopCoroutine("ContinuousPulse");
    }

    private void TestSinglePulse()
    {
        SinglePulse.sendSinglePulse(Channels.Channel1, 200, 10);
        Debug.Log("send single pulse");
    }

    private Thread pulseThread;
    private bool isRunning = false;
    private UnityThreading.ActionThread myThread;

    private System.Diagnostics.Stopwatch stopwatch;

    private void startPulseThread()
    {
        try
        {
            // define the thread and assign function for thread loop
            pulseThread = new Thread(new ThreadStart(pulseThreadLoop));
            // Boolean used to determine the thread is running

            isRunning = true;

            pulseThread.Priority = System.Threading.ThreadPriority.Highest;
            // Start the thread
            pulseThread.Start();
            stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            Debug.Log("Pulse thread started");
        }
        catch (System.Exception ex)
        {
            // Failed to start thread
            Debug.Log("Error 3: " + ex.Message.ToString());
        }
    }

    private void pulseThreadLoop()
    {
        while (isRunning)
        {
            //send a single pulse
            if ((stopwatch.ElapsedMilliseconds >= 1000 / pulseLoopFrequency))
            {
                SinglePulse.sendSinglePulse(Channels.Channel1, 150, 15);
                //UnityEngine.Debug.Log(SinglePulse.stopwatch.ElapsedMilliseconds);
                stopwatch.Reset();
                stopwatch.Start();
            }
            //Thread.Sleep((int)(1000/pulseLoopFrequency));
            // UnityEngine.Debug.Log(SinglePulse.stopwatch.ElapsedMilliseconds);
        }
    }

    private void stopPulseThread()
    {
        // Set isRunning to false to let the while loop
        // complete and drop out on next pass
        isRunning = false;

        // Pause a little to let this happen
        Thread.Sleep(100);

        // If the thread still exists kill it
        // A bit of a hack using Abort :p
        if (pulseThread != null)
        {
            pulseThread.Abort();
            Thread.Sleep(100);
            pulseThread = null;
        }

        if (stopwatch != null)
            stopwatch.Stop();

        Debug.Log("Ended Pulse Loop thread");
    }

    //private void OnDestroy()
    //{
    //    Thread.Sleep(100);
    //    stopPulseThread();
    //}

    //private void OnApplicationQuit()
    //{
    //    Thread.Sleep(100);
    //    stopPulseThread();
    //}
}