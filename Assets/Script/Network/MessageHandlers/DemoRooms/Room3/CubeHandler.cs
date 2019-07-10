using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuscleDeck
{
    public class CubeHandler : SolidObjectHandler
    {
        [Header("TricepsL")]
        public int offsetTricepsL = -1;
        //public int maxWidthTricepsL = -1;
        public int currentTricepsL = 15;
        [Header("TricepsR")]
        public int offsetTricepsR = -1;
        //public int maxWidthTricepsR = -1;
        public int currentTricepsR = 15;

        /* Offset*/
        protected int offsetTriceps = 0;


        /* Width */
        //protected int maxWidthTriceps = 0;


        /* current */
        protected int currentTriceps = 0;

        public override void InitParameters(string payloadString)
        {
            base.InitParameters(payloadString);

            // Offset
            offsetTriceps = side == Side.Left ?
                                    offsetTricepsL :
                                    offsetTricepsR;

            // Max value, uniform weight for now so not used
            //maxWidthTriceps = side == Side.Left ?
            //                        maxWidthTricepsL :
            //                        maxWidthTricepsR;
            // current
            currentTriceps = side == Side.Left ?
                                    currentTricepsL :
                                    currentTricepsR;
        }


        public override void HandleMessage(Message msg)
        {
            switch (msg.type)
            {
                case MessageType.Cube:
                    base.HandleMessage(msg);

                    //ArmStimulation.StimulateArm(
                    //    Part.Triceps,
                    //    side,
                    //    offsetTriceps,
                    //    currentTriceps
                    //);
                    break;

                case MessageType.CubeGrab:
                    // Flat stimulation triceps only
                    ArmStimulation.StimulateArm(
                        Part.Triceps,
                        Side.Left,
                        offsetTricepsL,
                        currentTricepsL
                    );
                    ArmStimulation.StimulateArm(
                        Part.Triceps,
                        Side.Right,
                        offsetTricepsR,
                        currentTricepsR
                    );
                    break;
            }
        }
    }
}
