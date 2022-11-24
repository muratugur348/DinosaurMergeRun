using DG.Tweening;
using DinosaurMergeRun.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DinosaurMergeRun
{
    public class Meteor : MonoBehaviour
    {
        public GameObject meteorStone;
        public GameObject fireArea;
        public GameObject[] meteorStones;
        public float meteorMoveTime;
        public GameObject meteorHitExplosion;
        // Start is called before the first frame update
        void Start()
        {
            DOTween.Init();
            int rndForMeteorType = Random.Range(0, 3);
            switch (rndForMeteorType)
            {
                case 0:
                    meteorStone = Instantiate(meteorStones[0], transform);
                    break;
                case 1:
                    meteorStone = Instantiate(meteorStones[1], transform);
                    break;
                case 2:
                    meteorStone = Instantiate(meteorStones[2], transform);
                    break;
            }
            int rndForPosition = Random.Range(0, 2);
            switch(rndForPosition)
            {
                case 0:
                    meteorStone.transform.localPosition = new Vector3(Random.Range(-5, -1), 17, 0);
                    break;
                case 1:
                    meteorStone.transform.localPosition = new Vector3(Random.Range(2, 6), 17, 0);
                    break;
            }
            meteorStone.SetActive(false);
            SetChildsIsTrigger(true);
        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Dinosaur") && !LevelManager.Instance.isGameFinished)
            {
                Hit();
            }
            else if(other.CompareTag("Boss")&& LevelManager.Instance.isGameFinished)
            {
                Hit();

            }
        }
        public void SetChildsIsTrigger(bool value)
        {
            foreach (Transform childTransform in meteorStone.transform)
            {
                if (!childTransform.CompareTag("FireParticle"))
                {
                    childTransform.GetComponent<Collider>().isTrigger = value;
                    childTransform.GetComponent<Rigidbody>().useGravity = !value;
                }
            }
        }
        public void Hit()
        {
            GetComponent<Collider>().enabled = false;
            meteorStone.SetActive(true);
            meteorStone.transform.DOLocalMove(Vector3.zero, meteorMoveTime).SetEase(Ease.InSine).OnComplete(() => {
                Instantiate(meteorHitExplosion, meteorStone.transform.position, meteorStone.transform.rotation);
                fireArea.SetActive(true);
                SetChildsIsTrigger(false);
                //Destroy(meteorStone);
            });
        }
    }    
}