using System;
using UnityEngine;

namespace MuscleDeck
{
    [Serializable]
    public class ButtonPayload : ContinuousPayload {
        public float minPenetration;

	    public ButtonPayload(float penetration, float minPenetration, float maxPenetration, Side side) : base(penetration, maxPenetration, side)
        {
                this.minPenetration = minPenetration;
        }
    }
}
