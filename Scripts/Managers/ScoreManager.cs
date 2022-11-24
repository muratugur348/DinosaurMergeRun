using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Patterns.Creational;
using DinosaurMergeRun.Controllers;

namespace DinosaurMergeRun.Managers
{
    public class ScoreManager : Singleton<ScoreManager>
    {
        public int coin;
        public int currentDinosaurLevel;
        public int playerHealth, bossHealth;
        public List<GameObject> dinosaurs = new List<GameObject>();
        public Color greenOutline, yellowOutline, redOutline;

        // Start is called before the first frame update
        private void Start()
        {
            coin = PlayerPrefs.GetInt("Coin");

        }

        // Update is called once per frame
        void Update()
        {
        }
        public void IncreaseCoin(int amount)
        {
            coin+=amount;
            UIManager.Instance.UpdateCoinText(coin);
            PlayerPrefs.SetInt("Coin", coin);
        }
        public void DecreaseBossHealth()
        {
            bossHealth--;
            UIManager.Instance.UpdateHealth(UIManager.Instance.bossHealthImg, bossHealth);
        }
        public void DecreasePlayerHealth()
        {
            playerHealth--;
            UIManager.Instance.UpdateHealth(UIManager.Instance.playerHealthImg, playerHealth);
        }
        public void AssignDinosaurs()
        {
            foreach(GameObject go in GameObject.FindGameObjectsWithTag("CollectableDinosaur"))
            {
                dinosaurs.Add(go);
            }
        }
        public void UpdateParticlesOnDinosaurs()
        {
            foreach (GameObject go in dinosaurs)
            {
                CollectableDinosaur dinoSc = go.GetComponent<CollectableDinosaur>();
                if (dinoSc.dinosaurLevel < currentDinosaurLevel)
                {
                    dinoSc.GetComponentInChildren<QuickOutline>().OutlineColor = yellowOutline;
                    /*dinoSc.glowRed.SetActive(false);
                    dinoSc.glowGreen.SetActive(false);
                    dinoSc.glowYellow.SetActive(true);*/
                }
                else if (dinoSc.dinosaurLevel == currentDinosaurLevel)
                {
                    dinoSc.GetComponentInChildren<QuickOutline>().OutlineColor = greenOutline;
                   /* dinoSc.glowRed.SetActive(false);
                    dinoSc.glowYellow.SetActive(false);
                    dinoSc.glowGreen.SetActive(true);*/
                }
                else
                {
                    dinoSc.GetComponentInChildren<QuickOutline>().OutlineColor = redOutline;
                   /* dinoSc.glowGreen.SetActive(false);
                    dinoSc.glowYellow.SetActive(false);
                    dinoSc.glowRed.SetActive(true);*/
                }
            }
        }
    }
}
