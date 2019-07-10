using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuscleDeck
{
    public class ApplyValues : MonoBehaviour
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

        [Header("TricepsL")]
        public int offsetTricepsL = -1;
        //public int maxWidthTricepsL = -1;
        public int currentTricepsL = 15;
        [Header("TricepsR")]
        public int offsetTricepsR = -1;
        //public int maxWidthTricepsR = -1;
        public int currentTricepsR = 15;

        [Header("ShoulderL")]
        public int offsetShoulderL = -1;
        public int maxWidthShoulderL = -1;
        public int currentShoulderL = 15;
        [Header("ShoulderR")]
        public int offsetShoulderR = -1;
        public int maxWidthShoulderR = -1;
        public int currentShoulderR = 15;

        [Header("References for handlers")]
        public ElectroProjectileHandler electroProjectileHandler;
        public ElectroWallHandler electroWallHandler;

        public ButtonHandler buttonHandler;
        public RockerHandler rockerHandler;

        public SliderHandler sliderHandler;
        public SolidWallHandler solidWallHandler;

        public SolidObjectHandler solidObjectHandler;
        public CubeHandler cubeHandler;
        public PunchCubeHandler punchCubeHandler;


        public void ApplyValuesToHandlers()
        {
            float repulsionBoost = 1.2f;

            // electro
            electroProjectileHandler.widthWristL = (int) (maxWidthWristL * repulsionBoost);
            electroProjectileHandler.currentWristL = currentWristL;

            electroProjectileHandler.widthWristR = (int) (maxWidthWristR * repulsionBoost);
            electroProjectileHandler.currentWristR = currentWristR;

            electroProjectileHandler.widthBicepsL = (int)(maxWidthBicepsL * repulsionBoost);
            electroProjectileHandler.currentBicepsL = currentBicepsL;

            electroProjectileHandler.widthBicepsR = (int)(maxWidthBicepsR * repulsionBoost);
            electroProjectileHandler.currentBicepsR = currentBicepsR;

            // electroWall
            electroWallHandler.widthWristL = (int)(maxWidthWristL * repulsionBoost);
            electroWallHandler.currentWristL = currentWristL;

            electroWallHandler.widthWristR = (int)(maxWidthWristR * repulsionBoost);
            electroWallHandler.currentWristR = currentWristR;

            electroWallHandler.widthBicepsL = (int)(maxWidthBicepsL * repulsionBoost);
            electroWallHandler.currentBicepsL = currentBicepsL;

            electroWallHandler.widthBicepsR = (int)(maxWidthBicepsR * repulsionBoost);
            electroWallHandler.currentBicepsR = currentBicepsR;


            //button
            buttonHandler.offsetWristL = offsetWristL;
            buttonHandler.maxWidthWristL = maxWidthWristL;
            buttonHandler.currentWristL = currentWristL;

            buttonHandler.offsetWristR = offsetWristR;
            buttonHandler.maxWidthWristR = maxWidthWristR;
            buttonHandler.currentWristR = currentWristR;

            buttonHandler.offsetBicepsL = offsetBicepsL;
            buttonHandler.maxWidthBicepsL = maxWidthBicepsL;
            buttonHandler.currentBicepsL = currentBicepsL;

            buttonHandler.offsetBicepsR = offsetBicepsR;
            buttonHandler.maxWidthBicepsR = maxWidthBicepsR;
            buttonHandler.currentBicepsR = currentBicepsR;

            // rocker

            rockerHandler.offsetWristL = offsetWristL;
            rockerHandler.maxWidthWristL = maxWidthWristL;
            rockerHandler.currentWristL = currentWristL;

            rockerHandler.offsetWristR = offsetWristR;
            rockerHandler.maxWidthWristR = maxWidthWristR;
            rockerHandler.currentWristR = currentWristR;

            rockerHandler.offsetBicepsL = offsetBicepsL;
            rockerHandler.maxWidthBicepsL = maxWidthBicepsL;
            rockerHandler.currentBicepsL = currentBicepsL;

            rockerHandler.offsetBicepsR = offsetBicepsR;
            rockerHandler.maxWidthBicepsR = maxWidthBicepsR;
            rockerHandler.currentBicepsR = currentBicepsR;

            // slider
            sliderHandler.offsetWristL = offsetWristL;
            sliderHandler.maxWidthWristL = maxWidthWristL;
            sliderHandler.currentWristL = currentWristL;

            sliderHandler.offsetWristR = offsetWristR;
            sliderHandler.maxWidthWristR = maxWidthWristR;
            sliderHandler.currentWristR = currentWristR;

            sliderHandler.offsetBicepsL = offsetBicepsL;
            sliderHandler.maxWidthBicepsL = maxWidthBicepsL;
            sliderHandler.currentBicepsL = currentBicepsL;

            sliderHandler.offsetBicepsR = offsetBicepsR;
            sliderHandler.maxWidthBicepsR = maxWidthBicepsR;
            sliderHandler.currentBicepsR = currentBicepsR;

            sliderHandler.offsetShoulderL = offsetShoulderL;
            sliderHandler.maxWidthShoulderL = maxWidthShoulderL;
            sliderHandler.currentShoulderL = currentShoulderL;

            sliderHandler.offsetShoulderR = offsetShoulderR;
            sliderHandler.maxWidthShoulderR = maxWidthShoulderR;
            sliderHandler.currentShoulderR = currentShoulderR;


            // solid wall
            solidWallHandler.offsetWristL = offsetWristL;
            solidWallHandler.maxWidthWristL = maxWidthWristL;
            solidWallHandler.currentWristL = currentWristL;

            solidWallHandler.offsetWristR = offsetWristR;
            solidWallHandler.maxWidthWristR = maxWidthWristR;
            solidWallHandler.currentWristR = currentWristR;

            solidWallHandler.offsetBicepsL = offsetBicepsL;
            solidWallHandler.maxWidthBicepsL = maxWidthBicepsL;
            solidWallHandler.currentBicepsL = currentBicepsL;

            solidWallHandler.offsetBicepsR = offsetBicepsR;
            solidWallHandler.maxWidthBicepsR = maxWidthBicepsR;
            solidWallHandler.currentBicepsR = currentBicepsR;

            // cube
            cubeHandler.offsetWristL = offsetWristL;
            cubeHandler.maxWidthWristL = offsetWristL + (int)((maxWidthWristL - offsetWristL) / 2);
            cubeHandler.currentWristL = currentWristL;

            cubeHandler.offsetWristR = offsetWristR;
            cubeHandler.maxWidthWristR = offsetWristR + (int)((maxWidthWristR - offsetWristR) / 2);
            cubeHandler.currentWristR = currentWristR;

            //cubeHandler.offsetBicepsL = offsetBicepsL;
            //cubeHandler.maxWidthBicepsL = maxWidthBicepsL;
            //cubeHandler.currentBicepsL = currentBicepsL;

            cubeHandler.offsetBicepsL = 0;
            cubeHandler.maxWidthBicepsL = 0;
            cubeHandler.currentBicepsL = 0;

            //cubeHandler.offsetBicepsR = offsetBicepsR;
            //cubeHandler.maxWidthBicepsR = maxWidthBicepsR;
            //cubeHandler.currentBicepsR = currentBicepsR;

            cubeHandler.offsetBicepsR = 0;
            cubeHandler.maxWidthBicepsR = 0;
            cubeHandler.currentBicepsR = 0;

            cubeHandler.offsetShoulderL = offsetShoulderL;
            cubeHandler.maxWidthShoulderL = maxWidthShoulderL;
            cubeHandler.currentShoulderL = currentShoulderL;

            cubeHandler.offsetShoulderR = offsetShoulderR;
            cubeHandler.maxWidthShoulderR = maxWidthShoulderR;
            cubeHandler.currentShoulderR = currentShoulderR;

            cubeHandler.offsetTricepsL = offsetTricepsL;
            cubeHandler.currentTricepsL = currentTricepsL;

            cubeHandler.offsetTricepsR = offsetTricepsR;
            cubeHandler.currentTricepsR = currentTricepsR;


            // punchCube
            punchCubeHandler.widthWristL = (int)(offsetWristL /** repulsionBoost*/);
            punchCubeHandler.maxWidthWristL = (int)(maxWidthWristL);
            punchCubeHandler.currentWristL = currentWristL;

            punchCubeHandler.widthWristR = (int)(offsetWristR /** repulsionBoost*/);
            punchCubeHandler.maxWidthWristR = (int)(maxWidthWristR);
            punchCubeHandler.currentWristR = currentWristR;

            punchCubeHandler.widthBicepsL = (int)(offsetBicepsL /** repulsionBoost*/);
            punchCubeHandler.maxWidthBicepsL = (int)(maxWidthBicepsL);
            punchCubeHandler.currentBicepsL = currentBicepsL;

            punchCubeHandler.widthBicepsR = (int)(offsetBicepsR /** repulsionBoost*/);
            punchCubeHandler.maxWidthBicepsR = (int)(maxWidthBicepsR);
            punchCubeHandler.currentBicepsR = currentBicepsR;

            //SolidObject (Cabinets)
            solidObjectHandler.offsetWristL = offsetWristL;
            solidObjectHandler.maxWidthWristL = maxWidthWristL;
            solidObjectHandler.currentWristL = currentWristL;

            solidObjectHandler.offsetWristR = offsetWristR;
            solidObjectHandler.maxWidthWristR = maxWidthWristR;
            solidObjectHandler.currentWristR = currentWristR;

            solidObjectHandler.offsetBicepsL = offsetBicepsL;
            solidObjectHandler.maxWidthBicepsL = maxWidthBicepsL;
            solidObjectHandler.currentBicepsL = currentBicepsL;

            solidObjectHandler.offsetBicepsR = offsetBicepsR;
            solidObjectHandler.maxWidthBicepsR = maxWidthBicepsR;
            solidObjectHandler.currentBicepsR = currentBicepsR;

            solidObjectHandler.offsetShoulderL = offsetShoulderL;
            solidObjectHandler.maxWidthShoulderL = maxWidthShoulderL;
            solidObjectHandler.currentShoulderL = currentShoulderL;

            solidObjectHandler.offsetShoulderR = offsetShoulderR;
            solidObjectHandler.maxWidthShoulderR = maxWidthShoulderR;
            solidObjectHandler.currentShoulderR = currentShoulderR;
        }
    }
}