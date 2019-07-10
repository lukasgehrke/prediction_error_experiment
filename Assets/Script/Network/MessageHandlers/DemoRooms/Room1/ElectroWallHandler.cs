using UnityEngine;
using System.Collections;

namespace MuscleDeck
{
    public class ElectroWallHandler : RepulsionHandler
    {
        [Header("Wrist L")]
        public int widthWristL = -1;
        public int currentWristL = 15;
        [Header("Wrist R")]
        public int widthWristR = -1;
        public int currentWristR = 15;
        
        [Header("Biceps L")]
        public int widthBicepsL= -1;
        public int currentBicepsL = 15;
        [Header("Biceps R")]
        public int widthBicepsR = -1;
        public int currentBicepsR = 15;

        public override void InitParameters(string payloadString)
        {
            RepulsionPayload payload = JsonUtility.FromJson<RepulsionPayload>(payloadString);

            side = payload.side;

            // Current
            int currentWrist    = side == Side.Left ?
                                        currentWristL :
                                        currentWristR;
            int currentBiceps   = side == Side.Left ?
                                        currentBicepsL :
                                        currentBicepsR;

            // Width
            int widthWrist  = side == Side.Left ?
                                        widthWristL :
                                        widthWristR;
            int widthBiceps = side == Side.Left ?
                                        widthBicepsL :
                                        widthBicepsR;


            stimInfo = new StimulationInfo[]
            {
                new StimulationInfo(
                    Part.Wrist,
                    side,
                    widthWrist,
                    currentWrist
                ),
                new StimulationInfo(
                    Part.Biceps,
                    side,
                    widthBiceps,
                    currentBiceps
                ),
            };
        }
    }
}