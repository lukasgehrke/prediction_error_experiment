using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuscleDeck
{
    public class PunchCubeHandler : ElectroWallHandler
    {
        [Header("Wrist L")]
        public int maxWidthWristL = -1;
        [Header("Wrist R")]
        public int maxWidthWristR = -1;

        [Header("Biceps L")]
        public int maxWidthBicepsL = -1;
        [Header("Biceps R")]
        public int maxWidthBicepsR = -1;


        public override void InitParameters(string payloadString)
        {
            PunchCubePayload payload = JsonUtility.FromJson<PunchCubePayload>(payloadString);

            side = payload.side;

            Debug.Log("PunchCube Velocity:" + payload.velocity.magnitude);

            // Current
            int currentWrist = side == Side.Left ?
                                        currentWristL :
                                        currentWristR;
            int currentBiceps = side == Side.Left ?
                                        currentBicepsL :
                                        currentBicepsR;

            // Width
            int widthWrist = side == Side.Left ?
                    (int)Mathf.Lerp(widthWristL, maxWidthWristL, payload.velocity.magnitude):
                    (int)Mathf.Lerp(widthWristR, maxWidthWristR, payload.velocity.magnitude);
            int widthBiceps = side == Side.Left ?
                    (int)Mathf.Lerp(widthBicepsL, maxWidthBicepsL, payload.velocity.magnitude) :
                    (int)Mathf.Lerp(widthBicepsR, maxWidthBicepsR, payload.velocity.magnitude);


            widthWrist = (int)Mathf.Lerp(0, widthWrist, payload.velocity.magnitude);

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
