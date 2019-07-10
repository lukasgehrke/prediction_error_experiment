using System;
using UnityEngine;

[Serializable]
public class ContinuousPayload : EMSPayload
{
    public float penetration;
    public float maxPenetration;

    public ContinuousPayload(float penetration, float maxPenetration, Side side) : base(side)
    {
        this.penetration    = penetration;
        this.maxPenetration = maxPenetration;
    }
}