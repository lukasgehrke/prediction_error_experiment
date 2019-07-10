using System;
using UnityEngine;

namespace MuscleDeck
{
    [Serializable]
    public class PunchCubePayload : RepulsionPayload
    {
        public Vector3 velocity;

        public PunchCubePayload(Vector3 velocity, Side side) : base(side)
        {
            this.velocity = velocity;
        }
    }
}