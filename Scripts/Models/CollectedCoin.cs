using DG.Tweening;
using DinosaurMergeRun.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DinosaurMergeRun
{
    public class CollectedCoin : MonoBehaviour
    {
        private GameObject coinUI;
        public int amount;
        // Start is called before the first frame update
        void Start()
        {
            DOTween.Init();
            
            transform.SetParent(GameObject.FindGameObjectWithTag("GameplayCanvas").transform);
            coinUI = UIManager.Instance.coinText.gameObject;
            MoveToCoin();
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void MoveToCoin()
        {
            transform.DOMove(coinUI.transform.position, (float)Random.Range(5,14)/10).SetEase(Ease.InSine).OnComplete(() =>
            {
                ScoreManager.Instance.IncreaseCoin(amount);
                Destroy(gameObject);
            });
        }
    }
}