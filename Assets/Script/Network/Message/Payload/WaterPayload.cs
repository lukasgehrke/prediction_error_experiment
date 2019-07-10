using UnityEngine;
using System;

namespace MuscleDeck
{
    [Serializable]
    public class WaterPayload : EMSPayload
    {
        public float direction;
        //public float speed;

        public WaterPayload(Side side, float direction/*, float speed*/) : base(side)
        {
            this.direction = direction;
            //this.speed = speed;
        }
    }
}