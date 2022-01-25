using UnityEngine;
using System.Collections;
using System;

namespace MuscleDeck
{
    public class ContinuousHandler : AbstractMessageHandler
    {
        /**
         * Safety Check incase the stop packet was lost
         */
        protected DateTime stopTime;

        [Header("Scaling Stuff")]
        [Tooltip("Usually overwritten by child class")]
        public int offsetDefault = -2;
        public float growthFactor = 1;

        /**
         * Parameters
         */
        protected Side side;
        protected float penetration;
        protected float maxPenetration;
        protected bool stimulating;

        public virtual void InitParameters(string payloadString)
        {
            ContinuousPayload payload = JsonUtility.FromJson<ContinuousPayload>(payloadString);

            side = payload.side;

            penetration = payload.penetration;
            maxPenetration = payload.maxPenetration;
        }

        public override void HandleMessage(Message msg)
        {
            InitParameters(msg.payload);

            ApplyStimulation();
        }

        /**
         * Overwrite me
         */ 
        protected virtual void ApplyStimulation()
        {
            penetration = Mathf.Abs(penetration);
            maxPenetration = Mathf.Abs(maxPenetration);

            int width = (int)Mathf.Round(getPulseWidth(penetration, maxPenetration, defaultMaxWidth));

            ArmStimulation.StimulateArm(
                Part.Wrist,
                side,
                width,
                15
            );

            stopTime = DateTime.Now.AddMilliseconds(500);
            stimulating = true;
        }

        protected virtual void Update()
        {
            if (stimulating && stopTime < DateTime.Now)
            {
                stimulating = false;
                Stop();
            }
        }

        protected virtual void GetPart(float distance, float maxDistance)
        {
        }

        public virtual void Stop()
        {
            //Debug.Log("Stopping");
            ChannelList.Stop();
        }

        /**
         * distance = distance to center
         */ 
        protected virtual float getPulseWidth(float distance, float maxDistance, float maxValue)
        {
            float factor;

            factor = getLinearScaling(distance, maxDistance);

            return Mathf.Lerp(offsetDefault, maxValue, factor);
        }

        /**
         * @return a value between 0 and 1
         */


        protected virtual float getLinearScaling(float distance, float maxDistance)
        {
            return Mathf.Min(1, distance / maxDistance * growthFactor);
        }
    }
}