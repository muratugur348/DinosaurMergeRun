using DinosaurMergeRun.Controllers;
using System.Collections;
using System.Collections.Generic;
using Core.Patterns.Creational;
using UnityEngine;
using DinosaurMergeRun.Managers;

namespace DinosaurMergeRun
{
    public class LevelHelper : Singleton<LevelHelper>
    {
        public int levelIndex;
        public float startCameraRotateTime;
        public float startCameraWaitTime;
        public int bossLevel;
        public int startHealth;
        private void Start()
        {
            ScoreManager.Instance.playerHealth = startHealth;
            ScoreManager.Instance.bossHealth = startHealth;
        }
        public void StartGame()
        {
            FindObjectOfType<CameraPosition>().ChangePosition(new Vector3(0,7,9));
            StartCoroutine(FindObjectOfType<CameraPosition>().RotateY(180, startCameraRotateTime, startCameraWaitTime));


        }

    }
}