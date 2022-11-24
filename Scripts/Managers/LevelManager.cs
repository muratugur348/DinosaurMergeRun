using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Patterns.Creational;
using UnityEngine.SceneManagement;
using DinosaurMergeRun.Controllers;
using DG.Tweening;
using GameAnalyticsSDK;

namespace DinosaurMergeRun.Managers
{
    public class LevelManager : Singleton<LevelManager>
    {
        public LevelHelper currentLevelHelper;
        public bool isGamePlayable;
        public bool isGameFinished = false;
        public bool isReachedBoss;
        public bool isFighting;
        private int _levelIndex;
        // Start is called before the first frame update
        void Start()
        {
            Application.targetFrameRate = 60;
            GameAnalytics.Initialize();
            _levelIndex = PlayerPrefs.GetInt("LevelIndex", 1);
            CreateLevelHelper();
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        private void CreateLevelHelper()
        {
            LevelHelper cloneLevel;
            if (_levelIndex <= LevelHelperRepository.LevelHelperCount())
            {
                cloneLevel = Instantiate(LevelHelperRepository.GetLevel(_levelIndex));
            }
            else
            {
                //cloneLevel = Instantiate(LevelHelperRepository.GetRandomLevel());
                _levelIndex = 1;
                cloneLevel = Instantiate(LevelHelperRepository.GetLevel(_levelIndex));

            }
            currentLevelHelper = cloneLevel;
        }
        public void GetNextLevel()
        {
            _levelIndex++;
            PlayerPrefs.SetInt("LevelIndex", _levelIndex);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
