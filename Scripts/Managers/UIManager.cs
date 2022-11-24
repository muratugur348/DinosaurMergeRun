using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core.Patterns.Creational;
using DinosaurMergeRun.Controllers;
using TMPro;
using DG.Tweening;

namespace DinosaurMergeRun.Managers
{
    public class UIManager : Singleton<UIManager>
    {
        public TextMeshProUGUI levelText;
        private int _levelNumber = 1;
        public TextMeshProUGUI coinText;
        public bool hit = false;
        public GameObject winCanvas;
        public Image playerHealthImg,bossHealthImg,tapMeter;
        public float decreaseTapMeterMultiplier, increaseTapMeterMultiplier;
        public float clickCounter = 0.15f;
        public TextMeshProUGUI[] textsWillBeClosedEndGame;
        public Image[] imagesWillBeClosedEndGame;
        private bool _isIncreasing;
        // Start is called before the first frame update
        void Start()
        {
            _levelNumber = PlayerPrefs.GetInt("LevelNumber", 1);
            coinText.text = PlayerPrefs.GetInt("Coin").ToString();
            levelText.text =  "LEVEL " + _levelNumber.ToString();
        }

        // Update is called once per frame
        void Update()
        {
            if(LevelManager.Instance.isFighting)
            {
                clickCounter += Time.deltaTime;
                if(!_isIncreasing)
                {
                    tapMeter.fillAmount -= Time.deltaTime*decreaseTapMeterMultiplier;
                }
            }
        }
        public void IncreaseLevelNumberText()
        {
            _levelNumber++;
            PlayerPrefs.SetInt("LevelNumber", _levelNumber);
        }
        public void UpdateCoinText(int value)
        {
            coinText.text = value.ToString();
        }
        public void ChangeToBossUI()
        {
            RUIPanel.Open("Boss");
            RUIPanel.Close("Gameplay");
            /*runnerParent.SetActive(false);
            bossParent.SetActive(true);*/
        }
        public void UpdateHealth(Image targetHealth,int value)
        {
            DOTween.To(() => targetHealth.fillAmount, x => targetHealth.fillAmount = x,
               (float)value/LevelHelper.Instance.startHealth, 1f).SetEase(Ease.InSine);

        }
      /*  public IEnumerator DecreaseTapMeter()
        {
            if (LevelManager.Instance.isFighting)
            {
                print("1");
                yield return new WaitForSeconds(0.1f);
                
                float tapMeterAmount = tapMeter.fillAmount - 0.01f;
                if (tapMeterAmount > 0)
                {
                    DOTween.To(() => tapMeter.fillAmount, x => tapMeter.fillAmount = x,
                       tapMeterAmount, 0.1f).SetEase(Ease.InSine);
                }
                StartCoroutine(DecreaseTapMeter());
            }
        }*/
        public void IncreaseTapMeter()
        {
            if(clickCounter>=0.15f)
            {
                clickCounter = 0;
                _isIncreasing = true;
                float tapMeterAmount = tapMeter.fillAmount +0.1f* increaseTapMeterMultiplier;
                DOTween.To(() => tapMeter.fillAmount, x => tapMeter.fillAmount = x,
                   tapMeterAmount, 0.1f).SetEase(Ease.InSine).OnComplete(()=> {
                       _isIncreasing = false;
                   });
            }
        }
        public void OpenWinCanvas()
        {
            winCanvas.SetActive(true);
            RUIPanel.Open("Win");

        }
        public void CloseTextAndHealthBars()
        {
            foreach(Image image in imagesWillBeClosedEndGame)
            {
                DOTween.To(() => image.color, x => image.color= x,
                new Color(0,0,0,0), 1f).SetEase(Ease.InSine);
            }
            foreach (TextMeshProUGUI text in textsWillBeClosedEndGame)
            {
                DOTween.To(() => text.color, x => text.color = x,
                new Color(0, 0, 0, 0), 1f).SetEase(Ease.InSine);
            }
        }
    }
}