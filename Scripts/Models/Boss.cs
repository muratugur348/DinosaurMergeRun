using DG.Tweening;
using DinosaurMergeRun.Controllers;
using DinosaurMergeRun.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DinosaurMergeRun
{
    public class Boss : MonoBehaviour
    {
        private Animator _anim;
        private Avatar _avatar;
        private DinosaurController _dinoSc;
        public bool isHitting;
        public bool isTakingDamage;
        public float attackCounter;
        public float attackTime;
        public Material[] touchedMaterials;
        public GameObject particleChild;
        public GameObject meteor;
        public ParticleSystem hitParticle;
        // Start is called before the first frame update
        void Start()
        {
            DOTween.Init();
            _anim = GetComponent<Animator>();
            _avatar = _anim.avatar;
            _dinoSc = FindObjectOfType<DinosaurController>();
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        public void MoveFightPosition()
        {
            _anim.SetBool("isWalk", true);
            _anim.SetBool("isStay", false);
            transform.DOLocalMoveZ(transform.localPosition.z - 4, 1f).SetEase(Ease.Linear).OnComplete(()=> {
                _anim.SetBool("isStay", true);
                _anim.SetBool("isWalk", false);
            });
        }
        public IEnumerator TakeHit()
        {
            if (ScoreManager.Instance.currentDinosaurLevel == 2 || ScoreManager.Instance.currentDinosaurLevel==3)
            {
                isTakingDamage = true;
                attackCounter += 1;
                yield return new WaitForSeconds(1f);
                _anim.SetBool("isTakeHit", true);
                _anim.SetBool("isStay", false);
                yield return new WaitForSeconds(.8f);
                _anim.SetBool("isTakeHit", false);
                _anim.SetBool("isStay", true);
                isTakingDamage = false;
            }
            else
            {
                isTakingDamage = true;
                attackCounter += 1;
                yield return new WaitForSeconds(1.3f);
                _anim.avatar = null;
                _anim.SetBool("isTakeHit", true);
                _anim.SetBool("isStay", false);
                yield return new WaitForSeconds(1f);
                _anim.avatar = _avatar;
                _anim.SetBool("isTakeHit", false);
                _anim.SetBool("isStay", true);
                isTakingDamage = false;
            }
        }
        public IEnumerator StartFight()
        {
            if (LevelManager.Instance.isFighting)
            {
                yield return new WaitForEndOfFrame();
                if (!isTakingDamage)
                    attackCounter += Time.deltaTime;
                if (attackCounter >= attackTime && !isTakingDamage)
                {
                    attackCounter = 0;
                    StartCoroutine(Hit());
                }
                StartCoroutine(StartFight());
            }
        }
        public IEnumerator Hit()
        {
            isHitting = true;
            _anim.SetBool("isAttack", true);
            _anim.SetBool("isStay", false);
            ScoreManager.Instance.DecreasePlayerHealth();
            
            yield return new WaitForSeconds(0.9f);
            Destroy(Instantiate(hitParticle, particleChild.transform.position, transform.rotation),2);
            yield return new WaitForSeconds(0.2f);
            if (ScoreManager.Instance.playerHealth == 0)
            {
                LevelManager.Instance.isFighting = false;
                RUIPanel.Open("Lose");
                _dinoSc.Killself();
            }
            yield return new WaitForSeconds(.5f);
            _anim.SetBool("isAttack", false);
            _anim.SetBool("isStay", true);
            attackCounter = 0;
            isHitting = false;

        }
        public void Killself()
        {
            int random = Random.Range(0, 4);
            float moveBackMeter=10;
            switch (random)
            {
                case 0:
                    moveBackMeter = 50* UIManager.Instance.tapMeter.fillAmount;
                    break;
                case 1:
             moveBackMeter = 55* UIManager.Instance.tapMeter.fillAmount;
                    break;
                case 2:
             moveBackMeter = 63* UIManager.Instance.tapMeter.fillAmount;
                    break;
                case 3:
             moveBackMeter = 71* UIManager.Instance.tapMeter.fillAmount;
                    break;
            }
            float lastPositionZ = transform.position.z + moveBackMeter;
            _dinoSc.lastPositionZ = lastPositionZ - 15;
            _dinoSc.lastPositionZTime = moveBackMeter / 15;
            GameObject meteorGO=
                Instantiate(meteor, new Vector3(transform.position.x, transform.position.y, lastPositionZ), Quaternion.identity);
            meteorGO.transform.GetChild(0).gameObject.SetActive(false);
            _anim.SetBool("isRunAway",true);
            
            transform.DOLocalRotate(Vector3.zero, 0.5f);
            transform.DOMoveZ(lastPositionZ, moveBackMeter / 15).SetEase(Ease.Linear).OnComplete(()=> {
                if (LevelHelper.Instance.bossLevel == 2 && ScoreManager.Instance.currentDinosaurLevel == 2)
                {
                    _anim.avatar = null;
                }
                _anim.SetBool("isDeath", true);
            });
        }
       
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Wall")&& LevelManager.Instance.isGameFinished)
            {
                foreach(Rigidbody rb in other.GetComponentsInChildren<Rigidbody>())
                {
                    rb.useGravity = true;
                }
                other.transform.GetChild(0).GetComponent<Rigidbody>()
                    .AddExplosionForce(2, other.transform.GetChild(0).position, 2, 2, ForceMode.Impulse);
            }
            else if(other.CompareTag("LevelEndWay")&& LevelManager.Instance.isGameFinished)
            {
                other.GetComponent<MeshRenderer>().materials = touchedMaterials;
            }
        }
    }
}