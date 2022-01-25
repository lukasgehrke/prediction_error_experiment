using System;
using System.Collections.Generic;
using UnityEngine;
using MuscleDeck;

public class RehaStimListener : UDPServer
{
#if UNITY_EDITOR
    private static RehaStimListener _Instance;
    private static GameObject _Container;
    protected bool initialized = false;

    protected Dictionary<MessageType, AbstractMessageHandler> handlers;

    public static RehaStimListener Default
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType<RehaStimListener>();

                if (_Instance == null)
                {
                    _Container = new GameObject("RehaStimListener");
                    _Container.AddComponent<RehaStimListener>();

                    _Instance = _Container.GetComponent<RehaStimListener>();
                }
            }
            _Instance.Init();

            return _Instance;
        }
    }

    public void Init()
    {
        if (!initialized)
        {
            InitHandlers();
            initialized = true;
        }
    }

    protected void InitHandlers()
    {
        handlers = new Dictionary<MessageType, AbstractMessageHandler>();
        // Todo Change to real handler
        // Room 1
        handlers.Add(MessageType.ElectroProjectile, HandlerFactory<ElectroProjectileHandler>.Get());
        handlers.Add(MessageType.ElectroWall, HandlerFactory<ElectroWallHandler>.Get());
        handlers.Add(MessageType.ButtonUpdate, HandlerFactory<ButtonHandler>.Get());

        // Room 2
        handlers.Add(MessageType.SolidWall, HandlerFactory<SolidWallHandler>.Get());
        handlers.Add(MessageType.RockerButton, HandlerFactory<RockerHandler>.Get());
        handlers.Add(MessageType.Slider, HandlerFactory<SliderHandler>.Get());
        handlers.Add(MessageType.Water, HandlerFactory<WaterHandler>.Get());
        handlers.Add(MessageType.SolidObject, HandlerFactory<SolidObjectHandler>.Get());

        //Room 3
        handlers.Add(MessageType.Cube, HandlerFactory<CubeHandler>.Get());
        handlers.Add(MessageType.CubeGrab, HandlerFactory<CubeHandler>.Get());
        handlers.Add(MessageType.PunchCube, HandlerFactory<PunchCubeHandler>.Get());
    }

    protected override void Start()
    {
        base.Start();
        Init();
    }

    public static void DebugHandle(string msg)
    {
        RehaStimListener.Default.processMessage(msg);
    }

    public bool printDebug = false;

    protected override void processMessage(string raw)
    {
        //if (!isActiveAndEnabled)
        //    return;

        if (printDebug)
            Debug.Log("Got message: " + raw);

        try
        {
            Message msg = JsonUtility.FromJson<Message>(raw);
            AbstractMessageHandler handler;
            switch (msg.type)
            {
                // Room1
                case MessageType.ElectroWall:
                case MessageType.ButtonUpdate:
                case MessageType.ElectroProjectile:

                // Room2
                case MessageType.SolidWall:
                case MessageType.RockerButton:
                case MessageType.Slider:
                case MessageType.Water:
                case MessageType.SolidObject:

                //Room3
                case MessageType.Cube:
                case MessageType.CubeGrab:
                case MessageType.PunchCube:
                    if (handlers.TryGetValue(msg.type, out handler)
                        && handler.isActiveAndEnabled)
                        handler.HandleMessage(msg);
                    else
                        print("no handler " + msg.type);
                    break;

                case MessageType.Stop:
                    ChannelList.Stop();
                    break;

                default:
                    print("Unknown Message: " + raw);
                    break;
            }
        }
        catch (Exception e) { }
    }

#endif
}