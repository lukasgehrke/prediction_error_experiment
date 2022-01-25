using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuscleDeck
{
    public class SolidObjectHandler : DemoContinuousHandler
    {
        [Header("ShoulderL")]
        public int offsetShoulderL = -1;
        public int maxWidthShoulderL = -1;
        public int currentShoulderL = 15;
        [Header("ShoulderR")]
        public int offsetShoulderR = -1;
        public int maxWidthShoulderR = -1;
        public int currentShoulderR = 15;
        
        /* Offset*/
        protected int offsetShoulder = 0;


        /* Width */
        protected int maxWidthShoulder = 0;


        /* current */
        protected int currentShoulder = 0;


        public override void InitParameters(string payloadString)
        {
            base.InitParameters(payloadString);

            // Offset
            offsetShoulder = side == Side.Left ?
                                    offsetShoulderL :
                                    offsetShoulderR;

            // Max value
            maxWidthShoulder = side == Side.Left ?
                                    maxWidthShoulderL :
                                    maxWidthShoulderR;
            // current
            currentShoulder = side == Side.Left ?
                                    currentShoulderL :
                                    currentShoulderR;
        }


        public override void HandleMessage(Message msg)
        {
            SolidObjectPayload payload = JsonUtility.FromJson<SolidObjectPayload>(msg.payload);

            InitParameters(msg.payload);

            side = payload.side;

            // higher cos => more aligned

            float rightCos = Mathf.Cos(
                Vector3.Angle(
                    payload.cameraDirections[0],
                    payload.handDirection) 
                * Mathf.PI / 180)
            ;

            float upCos = Mathf.Cos(
                Vector3.Angle(
                    payload.cameraDirections[1],
                    payload.handDirection)
                * Mathf.PI / 180)
            ;

            float frontCos =Mathf.Cos(
                Vector3.Angle(
                    payload.cameraDirections[1],
                    payload.handDirection)
                * Mathf.PI / 180)
            ;

            float max = Mathf.Max(
                Mathf.Abs(rightCos),
                Mathf.Abs(upCos),
                Mathf.Abs(frontCos)
            );

            // Check relative direction

            bool useWrist = false;
            bool useBiceps = false;
            bool useShoulder = false;

            if (max == Mathf.Abs(rightCos))
            {
                // From the side
                useWrist = true;
                useShoulder = true;
            }
            else if ( max == Mathf.Abs(upCos))
            {
                // Top or bottom
                useWrist = true;
                useBiceps = upCos > 0;
            }
            else
            {
                // Front or back
                useWrist = true;
                useBiceps = frontCos > 0;
            }

            // Apply stimulation

            float t = Mathf.InverseLerp(0, maxPenetration, penetration) * growthFactor;
          
            // Wrist
            if (useWrist)
            {
                // Alwways stimulate wrist
                int widthWrist = (int)Mathf.Lerp(
                                        offsetWrist,
                                        maxWidthWrist,
                                        t);

                if (penetration <= 0)
                {
                    widthWrist = 0;
                }

                ArmStimulation.StimulateArm(
                    Part.Wrist,
                    side,
                    widthWrist,
                    currentWrist
                );
            }

            // Biceps
            if (useBiceps)
            {
                int widthBiceps = (int)Mathf.Lerp(
                                    offsetBiceps,
                                    maxWidthBiceps,
                                    t);

                if (penetration <= 0)
                {
                    widthBiceps = 0;
                }

                ArmStimulation.StimulateArm(
                    Part.Biceps,
                    side,
                    widthBiceps,
                    currentBiceps
                );
            }

            // Shoulder
            if (useShoulder)
            {
                // Shoulder
                int widthShoulder = (int)Mathf.Lerp(
                                    offsetShoulder,
                                    maxWidthShoulder,
                                    t);

                if (penetration <= 0)
                {
                    widthShoulder = 0;
                }

                ArmStimulation.StimulateArm(
                    Part.Shoulder,
                    side,
                    widthShoulder,
                    currentShoulder
                );
            }
        }
    }
}
