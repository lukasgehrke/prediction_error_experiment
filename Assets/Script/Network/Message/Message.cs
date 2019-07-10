using System;
using UnityEngine;

namespace MuscleDeck
{
    [Serializable]
    public class Message
    {
        public MessageType type = MessageType.Unknown;
        public string payload;

        public Message(MessageType type)
        {
            this.type = type;
            payload = "";
        }

        public Message(MessageType type, string payload)
        {
            this.type = type;
            this.payload = payload;
        }

        public Message(MessageType type, BasicPayload payload)
        {
            this.type = type;
            this.payload = payload.ToString();
        }

        public override string ToString()
        {
            return JsonUtility.ToJson(this);
        }
    }

    public enum MessageType
    {
        Unknown = 0,
        Stop,
        // Room 1
        ButtonContact,
        ButtonUpdate,
        ElectroWall,
        ElectroProjectile,

        // Room 2
        Slider,
        RockerButton,
        SolidWall,    
        SolidObject,
        Water,

        // Room 3
        Cube,
        CubeGrab,
        PunchCube,
    }
}