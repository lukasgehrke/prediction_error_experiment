using System;
using UnityEngine;

namespace MuscleDeck
{
    public class DemoContinuousHandler : ContinuousHandler
    {
        [Header("WristL")]
        public int offsetWristL = -1;
        public int maxWidthWristL = -1;
        public int currentWristL = 15;
        [Header("WristR")]
        public int offsetWristR = -1;
        public int maxWidthWristR = -1;
        public int currentWristR = 15;

        [Header("BicepsL")]
        public int offsetBicepsL = -1;
        public int maxWidthBicepsL = -1;
        public int currentBicepsL = 15;
        [Header("BicepsR")]
        public int offsetBicepsR = -1;
        public int maxWidthBicepsR = -1;
        public int currentBicepsR = 15;


        /* Offset*/
        protected int offsetWrist = 0;
        protected int offsetBiceps = 0;


        /* Width */
        protected int maxWidthWrist = 0;
        protected int maxWidthBiceps = 0;


        /* current */
        protected int currentWrist = 0;
        protected int currentBiceps = 0;

        public override void InitParameters(string payloadString)
        {
            base.InitParameters(payloadString);

            // Offset
            offsetWrist  = side == Side.Left ?
                                    offsetWristL :
                                    offsetWristR;
            offsetBiceps = side == Side.Left ?
                                    offsetBicepsL :
                                    offsetBicepsR;

            // Max value
            maxWidthWrist  = side == Side.Left ?
                                    offsetWristL :
                                    maxWidthWristR;
            maxWidthBiceps = side == Side.Left ?
                                    maxWidthBicepsL :
                                    maxWidthBicepsR;
            // current
            currentWrist  = side == Side.Left ?
                                    currentWristL :
                                    currentWristR;
            currentBiceps = side == Side.Left ?
                                    currentBicepsL :
                                    currentBicepsR;
        }

        protected override void ApplyStimulation()
        {
            float pen = Mathf.Abs(penetration);


            int widthWrist, widthBiceps;
            
            if (penetration <= 0)
            {
                widthWrist = 0;
                widthBiceps = 0;
            }
            else
            { 
                // Penetration adjusted value
                widthWrist = (int)Mathf.Lerp(
                                        offsetWrist,
                                        maxWidthWrist,
                                        pen * growthFactor);
                widthBiceps = (int)Mathf.Lerp(
                                        offsetBiceps,
                                        maxWidthBiceps,
                                        pen * growthFactor);
            }

            ArmStimulation.StimulateArm(
                Part.Wrist,
                side,
                widthWrist,
                currentWrist
            );

            ArmStimulation.StimulateArm(
                Part.Biceps,
                side,
                widthBiceps,
                currentBiceps
            );

            stopTime = DateTime.Now.AddMilliseconds(100);
            stimulating = true;
        }
    }
}
