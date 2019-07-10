using System;

using UnityEngine;

[Serializable]
public class SolidObjectPayload : ContinuousPayload
{
    public Vector3[] cameraDirections; // right, up, front
    public Vector3 handPosition;
    public Vector3 handDirection; // points out from the palm
    public string source;

    public SolidObjectPayload(Vector3[] cameraDirections, Vector3 handPosition, Vector3 handDirection, Side side, string src = "UnknownSrc") : base(handPosition.magnitude, 1.41f, side)
    {
        this.cameraDirections = cameraDirections;
        this.handPosition = handPosition;
        this.handDirection = handDirection;
        source = src;
    }
}
