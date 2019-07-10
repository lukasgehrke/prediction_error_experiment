using UnityEngine;
using System.Collections;
using System;

namespace MuscleDeck
{
    public class RepulsionHandler : AbstractMessageHandler
    {
        protected Side side;
        public int duration = 300;


        /**
         * Initialize in InitParameters
         */
        protected StimulationInfo[] stimInfo;

        public virtual void InitParameters(string payloadString)
        {
            RepulsionPayload payload = JsonUtility.FromJson<RepulsionPayload>(payloadString);

            side = payload.side;
            
            stimInfo = new StimulationInfo[]
            {
                new StimulationInfo(
                Part.Wrist,
                side,
                defaultMaxWidth,
                15
                )
            };
        }

        public override void HandleMessage(Message msg)
        {
            InitParameters(msg.payload);

            ApplyStimulation();
        }

        protected virtual void ApplyStimulation()
        {
            ArmStimulation.StimulateArmBurst(
                stimInfo,
                duration
            );
        }
    }
}