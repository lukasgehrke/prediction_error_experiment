using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuscleDeck
{
    public class SliderHandler : SolidObjectHandler
    {
#if false
        public override void HandleMessage(Message msg)
        {
            SolidObjectPayload payload = JsonUtility.FromJson<SolidObjectPayload>(msg.payload);

            InitParameters(msg.payload);

            side = payload.side;

            float t = Mathf.InverseLerp(0, maxPenetration, penetration) * growthFactor;
            // Offset

            // Alwways stimulate wrist
            int widthWrist = (int)Mathf.Lerp(
                                    offsetWrist,
                                    maxWidthWrist,
                                    t);

            ArmStimulation.StimulateArm(
                Part.Wrist,
                side,
                widthWrist,
                currentWrist
            );

            float angle = Vector3.Angle(payload.cameraDirection, payload.handPosition);

            if (Mathf.Abs(Mathf.Sin(angle * Mathf.PI / 180)) > .5f)
            {
                // close to orthogonal => pushing sideways

                // Shoulder
                int widthShoulder = (int)Mathf.Lerp(
                                    offsetShoulder,
                                    maxWidthShoulder,
                                    t);

                ArmStimulation.StimulateArm(
                    Part.Shoulder,
                    side,
                    widthShoulder,
                    currentShoulder
                );
            }
            else
            {
                // pushing from the "front"
                // Biceps
                int widthBiceps = (int)Mathf.Lerp(
                                    offsetBiceps,
                                    maxWidthBiceps,
                                    t);

                ArmStimulation.StimulateArm(
                    Part.Biceps,
                    side,
                    widthBiceps,
                    currentBiceps
                );
            }
        }
#endif
    }
}
