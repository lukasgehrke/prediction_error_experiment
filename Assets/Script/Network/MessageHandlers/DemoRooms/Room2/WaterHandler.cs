using UnityEngine;
using System;

namespace MuscleDeck
{
    [Serializable]
    public class WaterHandler : AbstractMessageHandler
    {
        [Header("WristL")]
        public int maxWidthWristL = -2;
        public int currentWristL = 15;

        [Header("WristR")]
        public int maxWidthWristR = -2;
        public int currentWristR = 15;
        protected WaterPayload payload;

        public override void HandleMessage(Message msg)
        {
            payload = JsonUtility.FromJson<WaterPayload>(msg.payload);
        }

        protected virtual void ApplyStimulation()
        {
            if (payload.direction <= 0)
            {
                int width = (int)Mathf.Round(
                    getPulseWidth(
                        payload.direction,
                        payload.side == Side.Left ? maxWidthWristL : maxWidthWristR
                    )
                 );

                ArmStimulation.StimulateArm(
                    Part.Wrist,
                    payload.side,
                    width,
                    payload.side == Side.Left ? currentWristL : currentWristR
                );
            }
            else
            {
                ChannelList.Stop();
                // normally apply to frist flexor
            }

            //stopTime = DateTime.Now.AddMilliseconds(500);
        }

        protected float getPulseWidth(float direction, int maxPulseWidth)
        {
            return Mathf.Min(maxPulseWidth, Mathf.Abs(direction * maxPulseWidth));
        }

        protected void Update()
        {
            
        }
    }
}