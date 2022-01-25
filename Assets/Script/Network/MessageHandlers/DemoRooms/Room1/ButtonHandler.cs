using UnityEngine;
using System;
using System.Collections;

namespace MuscleDeck
{
    public class ButtonHandler : DemoContinuousHandler
    {
        protected float minPenetration;

        public override void InitParameters(string payloadString)
        {
            ButtonPayload payload = JsonUtility.FromJson<ButtonPayload>(payloadString);

            side = payload.side;

            penetration     = payload.penetration;     // penetration
            minPenetration  = Mathf.Max(0.01f, payload.minPenetration);
            maxPenetration  = Mathf.Min(0.99f, payload.maxPenetration);  // maxPenetration until buttonPress
        }

        protected override void ApplyStimulation()
        {
            // how far the button was pressed
            float pen = Mathf.Max(0, penetration);
            // max push depth
            maxPenetration = Mathf.Abs(maxPenetration);
            int widthWrist, widthBiceps;
            

            int currentWrist    = side == Side.Left ? 
                                        currentWristL: 
                                        currentWristR;
            int currentBiceps   = side == Side.Left ? 
                                        currentBicepsL: 
                                        currentBicepsR;

            int offsetWrist     = side == Side.Left ?
                                        offsetWristL :
                                        offsetWristR;
            int offsetBicpes    = side == Side.Left ?
                                        offsetBicepsL :
                                        offsetBicepsR;

            if (pen < minPenetration)
            {
                // scale for initial contact
                int maxWidthWrist = side == Side.Left ?
                                        maxWidthWristL :
                                        maxWidthWristR;
                widthWrist = (int)Mathf.Lerp(
                                        0,
                                        offsetWrist, // stub
                                        (pen / minPenetration));

                int maxWidthBiceps = side == Side.Left ?
                                        maxWidthBicepsL :
                                        maxWidthBicepsR;
                widthBiceps = (int)Mathf.Lerp(
                                        0,
                                        offsetBicpes, // stub
                                        (pen / minPenetration));

            }
            else if (pen < maxPenetration)
            {
                // scale for moving
                // constant while able to push
                widthWrist = offsetWrist;
                widthBiceps = offsetBicpes;
            }
            else
            {
                // scale for moving
                // constant while able to push
                int maxWidthWrist = side == Side.Left ?
                                        maxWidthWristL :
                                        maxWidthWristR;
                widthWrist = (int)Mathf.Lerp(
                                        offsetWrist,
                                        maxWidthWrist,
                                        growthFactor * (pen - maxPenetration) / ( 1- maxPenetration));

                int maxWidthBiceps = side == Side.Left ?
                                        maxWidthBicepsL :
                                        maxWidthBicepsR;
                widthBiceps = (int)Mathf.Lerp(
                                        offsetBicpes,
                                        maxWidthBiceps,
                                        growthFactor * (pen - maxPenetration) / (1 - maxPenetration));
            }

            if (penetration <= 0)
            {
                widthWrist = 0;
                widthBiceps = 0;
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

            stopTime = DateTime.Now.AddMilliseconds(500);
        }

        public override void HandleMessage(Message msg)
        {
            if (!gameObject.activeInHierarchy)
                return;
            switch (msg.type)
            {
                case MessageType.ButtonContact:
                    /*
                    ContinuousPayload payload = JsonUtility.FromJson<ContinuousPayload>(msg.payload);

                    // send single pulse
                    ArmStimulation.StimulateArmSinglePulse(
                        Part.Wrist,
                        payload.side,
                        wristContactPW,
                        15
                    );
                    ArmStimulation.StimulateArmSinglePulse(
                        Part.Shoulder,
                        payload.side,
                        wristContactPW,
                        15
                    );
                    //*/
                    break;

                case MessageType.ButtonUpdate:
                    base.HandleMessage(msg);
                    break;
            }
        }
    }
}