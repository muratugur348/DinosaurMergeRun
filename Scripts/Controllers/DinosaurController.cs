using DG.Tweening;
using DinosaurMergeRun.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DinosaurMergeRun.Controllers
{
    public class DinosaurController : MonoBehaviour
    {
        public float moveForwardVelocity;
        public float moveHorizontalSpeed;
        public float rotateSpeed;
        public float rotateSpeedToStraighten;
        public float rotateValue;
        public GameObject currentChildDinosaur;
        public GameObject[] childDinosaurs;
        public GameObject collectedCoinGO;
        public Vector2 zoomOutValuePerUpgrade;
        public ParticleSystem dustWithLeaves;
        public ParticleSystem[] smokeExplosions;
        public ParticleSystem hitParticle;
        public GameObject headChild;
        private RaycastHit objectHit;
        private Rigidbody _rb;
        private Animator _anim;
        private bool _mouseDown = false;
        private Vector2 _secondPressPos;
        private Vector2 _currentSwipe;
        private Vector2 _firstPressPos;
        private Boss _bossSc;
        private CameraPosition _camPosSc;
        private bool _isHitting;
        public float lastPositionZ;
        public float lastPositionZTime;
        public AnimatorOverrideController iguanadonAnimatorOverride;
        public AnimatorOverrideController triceratopsAnimatorOverride;
        public Meteor[] meteors;
        private float _oldCurrX, _oldCurrY;
        private float _counterInMouseDown;
        private bool _isCheckingIngProgress;
        // Start is called before the first frame update
        void Start()
        {
            DOTween.Init();
            _anim = currentChildDinosaur.GetComponent<Animator>();
            _rb = GetComponent<Rigidbody>();
            _bossSc = FindObjectOfType<Boss>();
            _camPosSc = FindObjectOfType<CameraPosition>();
            meteors = FindObjectsOfType<Meteor>();
            _isCheckingIngProgress = false;
            /*moveForwardVelocity = 8;
            moveHorizontalSpeed = 400;
            rotateSpeed = 10;
            rotateValue = 500;*/
        }

        // Update is called once per frame
        void Update()
        {
            if (!LevelManager.Instance.isGameFinished)
            {
                GetMouseInput();
               /* if (transform.position.x < -4.3f)
                {
                    transform.position = new Vector3(-4.3f, transform.position.y, transform.position.z);
                }
                else if(transform.position.x>4.3f)
                { 
                    transform.position = new Vector3(4.3f, transform.position.y, transform.position.z);
                }*/
            }
        }
        private void FixedUpdate()
        {
            MoveHorizontal();
            /* if(!_isCheckingIngProgress)
             {
            */

            /*     _isCheckingIngProgress = true;
             }*/
        }
        private void GetMouseInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _mouseDown = true;
                _firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                _counterInMouseDown = 0;
                _oldCurrX = _firstPressPos.x;
                _oldCurrY = _firstPressPos.y;
               //StartCoroutine(HorizontalMovement(_firstPressPos));
                if (LevelManager.Instance.isFighting)
                {
                    StartCoroutine(Hit());
                    UIManager.Instance.IncreaseTapMeter();
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _mouseDown = false;
                _currentSwipe.x = 0;
                _currentSwipe.y = 0;
                _isCheckingIngProgress = false;

            }
        }
        private IEnumerator HorizontalMovement(Vector2 firstPos)
        {
            if (LevelManager.Instance.isGamePlayable)
            {
                yield return new WaitForFixedUpdate();
                if (_mouseDown)
                {
                    _secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y) / Screen.width;
                    _currentSwipe = new Vector2(_secondPressPos.x - firstPos.x, _secondPressPos.y - firstPos.y);
                    //print("First= " + firstPos.x + " second= " + _secondPressPos.x + " curr= " + _currentSwipe.x);
                    Vector3 rgt = transform.TransformDirection(Vector3.right);
                    Vector3 lft = transform.TransformDirection(-Vector3.right);
                    if (_currentSwipe.x > 0.05f)
                    {
                        if (Physics.Raycast(transform.position, rgt, out objectHit, 10))
                        {
                            if (objectHit.transform.CompareTag("Collider") && objectHit.distance < 0.5f)
                            {
                                RotateChildDinosaur(180, rotateSpeedToStraighten);
                                _rb.velocity = Vector3.forward * moveForwardVelocity;
                            }
                            else
                            {
                                _rb.AddForce(Vector3.right * moveHorizontalSpeed * _currentSwipe, ForceMode.Impulse);
                                RotateChildDinosaur(180 + _currentSwipe.x * rotateValue, 1);
                            }
                        }
                    }
                    else if (_currentSwipe.x < 0.05f)
                    {
                        if (Physics.Raycast(transform.position, lft, out objectHit, 10))
                        {
                            if (objectHit.transform.CompareTag("Collider") && objectHit.distance < 0.5f)
                            {
                                _rb.velocity = Vector3.forward * moveForwardVelocity;
                                RotateChildDinosaur(180, rotateSpeedToStraighten);
                            }
                            else
                            {
                                _rb.AddForce(Vector3.right * moveHorizontalSpeed * _currentSwipe, ForceMode.Impulse);
                                RotateChildDinosaur(180 + _currentSwipe.x * rotateValue, 1);
                            }
                        }
                    }/*
                    if ((Physics.Raycast(transform.position, rgt, out objectHit, 1) &&
                        objectHit.transform.CompareTag("Collider") && objectHit.distance < 0.5f)
                        || (Physics.Raycast(transform.position, lft, out objectHit, 1) &&
                        objectHit.transform.CompareTag("Collider") && objectHit.distance < 0.5f
                        ))
                    {
                        RotateChildDinosaur(180, rotateSpeedToStraighten);
                    }
                    else
                    {
                        //_rb.AddForce(Vector3.right * moveHorizontalSpeed * Mathf.Abs(_currentSwipe.x) / 1, ForceMode.Impulse);
                        _rb.AddForce(Vector3.right * moveHorizontalSpeed * _currentSwipe, ForceMode.Impulse);

                        RotateChildDinosaur(180 + _currentSwipe.x * rotateValue, 1);
                    }*/
                    StartCoroutine(HorizontalMovement(_secondPressPos));

                }
               
            }
        }
    
        private void MoveHorizontal()
        {
            if (LevelManager.Instance.isGamePlayable && !LevelManager.Instance.isReachedBoss)
            {
                if (_mouseDown)
                {
                    //_firstPressPos =  new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                    //_counterInMouseDown += Time.deltaTime;
                    //save ended touch 2d point
                    _secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                    //create vector from the two points
                    _currentSwipe = new Vector2(_secondPressPos.x - _firstPressPos.x, _secondPressPos.y - _firstPressPos.y);
                    //normalize the 2d vector
                    _currentSwipe /= Screen.width;

                    if (_currentSwipe.x > 0.3f)
                    {
                        //_rb.AddForce(Vector3.right * moveHorizontalSpeed * 0.3f / 1, ForceMode.Impulse);
                        Vector3 rgt = transform.TransformDirection(Vector3.right);
                        if (Physics.Raycast(transform.position, rgt, out objectHit, 10))
                        {
                            if (objectHit.transform.CompareTag("Collider") && objectHit.distance < 0.5f)
                            {
                                RotateChildDinosaur(180, rotateSpeedToStraighten);
                            }
                            else
                            {
                                _rb.AddForce(Vector3.right * moveHorizontalSpeed * 0.3f / 1, ForceMode.Impulse);
                                RotateChildDinosaur(180 + _currentSwipe.x * rotateValue,1);
                                    
                            }
                        }
                    }
                    else if (_currentSwipe.x > 0f)
                    {
                        Vector3 rgt = transform.TransformDirection(Vector3.right);
                        if (Physics.Raycast(transform.position, rgt, out objectHit, 10))
                        {
                            if (objectHit.transform.CompareTag("Collider") && objectHit.distance < 0.5f)
                            {
                                RotateChildDinosaur(180, rotateSpeedToStraighten);
                            }
                            else
                            {
                                _rb.AddForce(Vector3.right * moveHorizontalSpeed * Mathf.Abs(_currentSwipe.x) / 1, ForceMode.Impulse);
                                RotateChildDinosaur(180 + _currentSwipe.x * rotateValue,1);
                            }
                        }
                    }
                    else if (_currentSwipe.x < -0.3f)
                    {
                        Vector3 lft = transform.TransformDirection(-Vector3.right);
                        if (Physics.Raycast(transform.position, lft, out objectHit, 10))
                        {
                            if (objectHit.transform.CompareTag("Collider") && objectHit.distance < 0.5f)
                            {
                                RotateChildDinosaur(180, rotateSpeedToStraighten);
                            }
                            else
                            {
                                _rb.AddForce(-Vector3.right * moveHorizontalSpeed * 0.3f / 1, ForceMode.Impulse);
                                RotateChildDinosaur(180 + _currentSwipe.x * rotateValue,1);
                            }
                        }
                    }
                    else if (_currentSwipe.x < -0f)
                    {
                        Vector3 lft = transform.TransformDirection(-Vector3.right);
                        if (Physics.Raycast(transform.position, lft, out objectHit, 10))
                        {
                            if (objectHit.transform.CompareTag("Collider") && objectHit.distance < 0.5f)
                            {
                                RotateChildDinosaur(180, rotateSpeedToStraighten);
                            }
                            else
                            {
                                _rb.AddForce(-Vector3.right * moveHorizontalSpeed * Mathf.Abs(_currentSwipe.x) / 1, ForceMode.Impulse);
                                RotateChildDinosaur(180 + _currentSwipe.x * rotateValue,1);
                            }
                        }
                    }
                    else
                    {
                        RotateChildDinosaur(180, rotateSpeedToStraighten);
                    }
                    
                }
                else
                {
                    RotateChildDinosaur(180, rotateSpeedToStraighten);
                }
            }
        }
       /* private IEnumerator CheckSwerving(float previousXValue)
        {
            _isCheckingIngProgress = true;
            yield return new WaitForSeconds(0.1f);
            print(previousXValue + "previous. " + _currentSwipe.x + "currentx");
            if (_mouseDown && previousXValue == _currentSwipe.x)
            {
                if (_secondPressPos.x != _oldCurrX)
                {
                    _firstPressPos = _secondPressPos;
                    _oldCurrX = _firstPressPos.x;
                    _oldCurrY = _firstPressPos.y;

                }
                if (_secondPressPos.y != _oldCurrY)
                {
                    _firstPressPos = _secondPressPos;
                    _oldCurrY = _firstPressPos.y;
                    _oldCurrX = _firstPressPos.x;


                }
                StartCoroutine(CheckSwerving(_currentSwipe.x));
            }
            else if(_mouseDown)
                StartCoroutine(CheckSwerving(_currentSwipe.x));
            else
                _isCheckingIngProgress = false;

        }*/
        public void RotateChildDinosaur(float value,float speedMultiplier)
        {
            //print(value);
            currentChildDinosaur.transform.localEulerAngles = Vector3.Lerp(currentChildDinosaur.transform.localEulerAngles,
                    new Vector3(0, value, 0), Time.deltaTime * rotateSpeed*speedMultiplier);
        }
        public void StartMove()
        {
            StartCoroutine(MoveForward());
            dustWithLeaves.Play();
            //_rb.velocity = new Vector3(0, 0, moveForwardVelocity);
            _anim.SetBool("isRunning", true);
        }
        private IEnumerator MoveForward()
        {
            if (!LevelManager.Instance.isGameFinished && !LevelManager.Instance.isReachedBoss)
            {
                _rb.velocity = Vector3.forward * moveForwardVelocity;
                yield return new WaitForFixedUpdate();
                StartCoroutine(MoveForward());
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Coin"))
            {
                Instantiate(collectedCoinGO, Camera.main.WorldToScreenPoint(other.transform.position), Quaternion.identity).GetComponent<CollectedCoin>().amount=1;
                Destroy(other.gameObject);
            }
            else if (other.CompareTag("Trap"))
            {
                other.GetComponent<Collider>().enabled = false;
                if (ScoreManager.Instance.currentDinosaurLevel == 1)
                {
                    KillDinosaur(.5f);
                    other.GetComponent<Animator>().enabled = true;
                    transform.DOMove(other.gameObject.transform.position, 0.5f).SetEase(Ease.InSine);
                    StartCoroutine(TrapLayDown(other.gameObject));
                }
                else
                {
                    DowngradeDinosaur();
                }
            }
            else if (other.CompareTag("Meteor"))
            {
                other.GetComponent<Collider>().enabled = false;

                if (ScoreManager.Instance.currentDinosaurLevel == 1)
                {
                    KillDinosaur(.1f);
                }
                else
                {
                    DowngradeDinosaur();
                }
            }
            else if (other.CompareTag("CollectableDinosaur"))
            {
                other.GetComponent<Collider>().enabled = false;
                if (ScoreManager.Instance.currentDinosaurLevel > other.GetComponent<CollectableDinosaur>().dinosaurLevel)
                {
                    Instantiate(collectedCoinGO, Camera.main.WorldToScreenPoint(other.transform.position), Quaternion.identity).
                        GetComponent<CollectedCoin>().amount = other.GetComponent<CollectableDinosaur>().dinosaurLevel;
                    StartCoroutine(other.GetComponent<CollectableDinosaur>().KillDinosaur());
                }
                else if (ScoreManager.Instance.currentDinosaurLevel == other.GetComponent<CollectableDinosaur>().dinosaurLevel)
                {
                    ScoreManager.Instance.dinosaurs.Remove(other.gameObject);
                    Destroy(other.gameObject);
                    UpgradeDinosaur();
                }
                else if (ScoreManager.Instance.currentDinosaurLevel < other.GetComponent<CollectableDinosaur>().dinosaurLevel)
                {
                    if (ScoreManager.Instance.currentDinosaurLevel == 1)
                    {
                        KillDinosaur(.01f);
                        StartCoroutine(other.GetComponent<CollectableDinosaur>().Attack());
                    }
                    else
                    {
                        DowngradeDinosaur();
                        StartCoroutine(other.GetComponent<CollectableDinosaur>().Attack());
                    }
                }
            }
            else if (other.CompareTag("BossStage"))
            {
                if (ScoreManager.Instance.currentDinosaurLevel == LevelHelper.Instance.bossLevel)
                {
                    LevelManager.Instance.isReachedBoss = true;
                    
                    dustWithLeaves.Stop();
                    _bossSc.MoveFightPosition();
                    _rb.velocity = Vector3.zero;
                    _anim.transform.DOLocalRotate(new Vector3(0, 180, 0), 1f).SetEase(Ease.Linear);
                    if (ScoreManager.Instance.currentDinosaurLevel == 4)
                    {
                        _camPosSc.ChangeTrackedObjectOffsetOfVC(new Vector3(0, 1.5f, 3.5f));
                        _camPosSc.ChangePosition(new Vector3(-7, 10, 11));
                        transform.DOMove(other.transform.GetChild(0).position, 1f).SetEase(Ease.Linear).OnComplete(() =>
                        {
                        StartCoroutine(MakeRoar(2f));

                        });
                    }
                    else if (ScoreManager.Instance.currentDinosaurLevel == 3)
                    {
                        _camPosSc.ChangeTrackedObjectOffsetOfVC(new Vector3(0, 1.5f, 3.5f));
                        _camPosSc.ChangePosition(new Vector3(-7, 10, 11));
                        transform.DOMove(other.transform.GetChild(0).position, 1f).SetEase(Ease.Linear).OnComplete(() =>
                        {
                        _anim.runtimeAnimatorController = triceratopsAnimatorOverride;
                        StartCoroutine(MakeRoar(2f));

                        });
                    }
                    else if (ScoreManager.Instance.currentDinosaurLevel == 2)
                    {
                        _camPosSc.ChangeTrackedObjectOffsetOfVC(new Vector3(0, 1.5f, 2f));
                        _camPosSc.ChangePosition(new Vector3(-4.6f, 6.5f, 5));
                        transform.DOMove(other.transform.GetChild(0).position, 1f).SetEase(Ease.Linear).OnComplete(() =>
                        {
                            _anim.runtimeAnimatorController = iguanadonAnimatorOverride;
                            StartCoroutine(MakeRoar(2f));

                        });
                    }

                }
                else
                {
                    LevelManager.Instance.isReachedBoss = true;
                    _camPosSc.ChangeTrackedObjectOffsetOfVC(new Vector3(0, 1.5f, 3.5f));
                    _camPosSc.ChangePosition(new Vector3(-7, 10, 11));
                    dustWithLeaves.Stop();
                    _bossSc.MoveFightPosition();
                    _rb.velocity = Vector3.zero;
                    _anim.transform.DOLocalRotate(new Vector3(0, 180, 0), 1f).SetEase(Ease.Linear);
                    transform.DOMove(other.transform.GetChild(0).position, 1f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        _anim.SetBool("isStay", true);
                        _anim.SetBool("isRunning", false);
                        ScoreManager.Instance.playerHealth = 1;
                        StartCoroutine(_bossSc.Hit());
                    });
                }
            }
        }
        public void UpgradeDinosaur()
        {
            ScoreManager.Instance.currentDinosaurLevel++;
            moveForwardVelocity *= 1.1f;
            foreach(Meteor meteor in meteors)
            {
                meteor.meteorMoveTime /= 1.1f;
            }
            currentChildDinosaur = childDinosaurs[ScoreManager.Instance.currentDinosaurLevel - 1];
            currentChildDinosaur.transform.DOScale(currentChildDinosaur.transform.localScale * 1.5f, 1).SetEase(Ease.Linear);
            OpenChildDinosaur(ScoreManager.Instance.currentDinosaurLevel-1);
            _camPosSc.IncreaseYandZPosition(zoomOutValuePerUpgrade.x,zoomOutValuePerUpgrade.y);
            GetComponent<CapsuleCollider>().radius *= 1.35f;
            smokeExplosions[ScoreManager.Instance.currentDinosaurLevel-2].Play();
            var main = dustWithLeaves.main;
            main.startSize=main.startSizeMultiplier*1.5f;
            ScoreManager.Instance.UpdateParticlesOnDinosaurs();
            //print("2");

        }
        public void DowngradeDinosaur()
        {
            moveForwardVelocity /= 1.1f;
            foreach (Meteor meteor in meteors)
            {
                meteor.meteorMoveTime *= 1.1f;
            }
            smokeExplosions[ScoreManager.Instance.currentDinosaurLevel - 2].Play();
            currentChildDinosaur.transform.DOScale(currentChildDinosaur.transform.localScale / 1.5f, 1).SetEase(Ease.Linear);
            ScoreManager.Instance.currentDinosaurLevel--;
            currentChildDinosaur = childDinosaurs[ScoreManager.Instance.currentDinosaurLevel - 1];
            OpenChildDinosaur(ScoreManager.Instance.currentDinosaurLevel - 1);
            _camPosSc.IncreaseYandZPosition(-zoomOutValuePerUpgrade.x, -zoomOutValuePerUpgrade.y);
            GetComponent<CapsuleCollider>().radius /= 1.35f;
            var main = dustWithLeaves.main;
            main.startSize = main.startSizeMultiplier / 1.5f;
            ScoreManager.Instance.UpdateParticlesOnDinosaurs();
        }
        public void KillDinosaur(float deathWaitTime)
        {
            LevelManager.Instance.isGameFinished = true;
            LevelManager.Instance.isGamePlayable = false;
            StartCoroutine(DeathAnimation(deathWaitTime));
            RUIPanel.Close("Gameplay");
            RUIPanel.Open("Lose");
            //_rb.velocity = new Vector3(0, 0, 3.5f);

        }
        private IEnumerator DeathAnimation(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            _rb.velocity = Vector3.zero;
            dustWithLeaves.Stop();
            _anim.SetBool("isDeath", true);
        }
        private IEnumerator TrapLayDown(GameObject trapGO)
        {
            yield return new WaitForSeconds(1f);
            trapGO.GetComponent<Animator>().SetBool("isLayDown", true);
        }
        private void OpenChildDinosaur(int index)
        {
            foreach(GameObject go in childDinosaurs)
            {
                go.SetActive(false);
            }
            childDinosaurs[index].SetActive(true);
            _anim = currentChildDinosaur.GetComponent<Animator>();
            _anim.SetBool("isRunning", true);
        }
        private IEnumerator MakeRoar(float roarTime)
        {
            _anim.SetBool("isRoar", true);
            _anim.SetBool("isRunning", false);
            yield return new WaitForSeconds(roarTime);
            _anim.SetBool("isRoar", false);
            _anim.SetBool("isStay", true);
            LevelManager.Instance.isFighting = true;
            StartCoroutine(StartBossFight());
        }
        private IEnumerator StartBossFight()
        {
            yield return new WaitForEndOfFrame();
            UIManager.Instance.ChangeToBossUI();
           // StartCoroutine(UIManager.Instance.DecreaseTapMeter());
            StartCoroutine(_bossSc.StartFight());
        }
        private IEnumerator Hit()
        {
            if (!_isHitting && !_bossSc.isHitting && !_bossSc.isTakingDamage && _bossSc.attackCounter<_bossSc.attackTime)
            {
                if (ScoreManager.Instance.currentDinosaurLevel == 2)
                {
                    if (ScoreManager.Instance.bossHealth > 1)
                    {
                        _isHitting = true;
                        StartCoroutine(_bossSc.TakeHit());
                        _anim.SetBool("isAttack", true);
                        _anim.SetBool("isStay", false);
                        yield return new WaitForSeconds(1f);
                        Destroy(Instantiate(hitParticle, headChild.transform.position, headChild.transform.rotation), 2);
                        yield return new WaitForSeconds(0.3f);
                        _anim.SetBool("isAttack", false);
                        _anim.SetBool("isStay", true);
                        _isHitting = false;
                        ScoreManager.Instance.DecreaseBossHealth();
                    }
                    else if (ScoreManager.Instance.bossHealth == 1)
                    {
                        LevelManager.Instance.isFighting = false;
                        LevelManager.Instance.isGameFinished = true;
                        _anim.speed = 0.6f;
                        StartCoroutine(_bossSc.TakeHit());
                        _anim.SetBool("isAttack", true);
                        _anim.SetBool("isStay", false);
                        yield return new WaitForSeconds(1f);
                        StartCoroutine(FindObjectOfType<CameraLookAt>().FinishMovement());
                        StartCoroutine(FindObjectOfType<CameraPosition>().FollowBoss());
                        FindObjectOfType<CameraPosition>().ChangePosition(new Vector3(0, 7f, 20));
                        yield return new WaitForSeconds(.3f);
                        Destroy(Instantiate(hitParticle, headChild.transform.position, headChild.transform.rotation), 2);
                        ScoreManager.Instance.DecreaseBossHealth();
                        yield return new WaitForSeconds(1f);
                        //FindObjectOfType<CameraLookAt>().ChangeCinemachineLookAt();
                        _bossSc.Killself();
                        _anim.SetBool("isAttack", false);
                        _anim.SetBool("isStay", true);
                        StartCoroutine(FollowBoss());
                        UIManager.Instance.CloseTextAndHealthBars();
                        _isHitting = false;
                    }
                }
                else if (ScoreManager.Instance.currentDinosaurLevel == 3)
                {
                    if (ScoreManager.Instance.bossHealth > 1)
                    {
                        _isHitting = true;
                        StartCoroutine(_bossSc.TakeHit());
                        _anim.SetBool("isAttack", true);
                        _anim.SetBool("isStay", false);
                        yield return new WaitForSeconds(1f);
                        Destroy(Instantiate(hitParticle, headChild.transform.position, headChild.transform.rotation), 2);
                        yield return new WaitForSeconds(0.3f);
                        _anim.SetBool("isAttack", false);
                        _anim.SetBool("isStay", true);
                        _isHitting = false;
                        ScoreManager.Instance.DecreaseBossHealth();
                    }
                    else if (ScoreManager.Instance.bossHealth == 1)
                    {
                        LevelManager.Instance.isFighting = false;
                        LevelManager.Instance.isGameFinished = true;
                        _anim.speed = 0.6f;
                        StartCoroutine(_bossSc.TakeHit());
                        _anim.SetBool("isAttack", true);
                        _anim.SetBool("isStay", false);
                        yield return new WaitForSeconds(1f);
                        StartCoroutine(FindObjectOfType<CameraLookAt>().FinishMovement());
                        StartCoroutine(FindObjectOfType<CameraPosition>().FollowBoss());
                        FindObjectOfType<CameraPosition>().ChangePosition(new Vector3(0, 10f, 25));
                        yield return new WaitForSeconds(.3f);
                        Destroy(Instantiate(hitParticle, headChild.transform.position, headChild.transform.rotation), 2);
                        ScoreManager.Instance.DecreaseBossHealth();
                        yield return new WaitForSeconds(1f);
                        //FindObjectOfType<CameraLookAt>().ChangeCinemachineLookAt();
                        _bossSc.Killself();
                        _anim.SetBool("isAttack", false);
                        _anim.SetBool("isStay", true);
                        StartCoroutine(FollowBoss());
                        UIManager.Instance.CloseTextAndHealthBars();
                        _isHitting = false;
                    }
                }
                else if (ScoreManager.Instance.currentDinosaurLevel == 4)
                {
                    if (ScoreManager.Instance.bossHealth > 1)
                    {
                        _isHitting = true;
                        StartCoroutine(_bossSc.TakeHit());
                        _anim.SetBool("isAttack", true);
                        _anim.SetBool("isStay", false);
                        yield return new WaitForSeconds(1.3f);
                        Destroy(Instantiate(hitParticle, headChild.transform.position, headChild.transform.rotation), 2);
                        yield return new WaitForSeconds(0.3f);
                        _anim.SetBool("isAttack", false);
                        _anim.SetBool("isStay", true);
                        _isHitting = false;
                        ScoreManager.Instance.DecreaseBossHealth();
                    }
                    else if (ScoreManager.Instance.bossHealth == 1)
                    {
                        LevelManager.Instance.isFighting = false;
                        LevelManager.Instance.isGameFinished = true;
                        //_anim.speed = 0.6f;
                        StartCoroutine(_bossSc.TakeHit());
                        _anim.SetBool("isAttack", true);
                        _anim.SetBool("isStay", false);
                        yield return new WaitForSeconds(1f);
                        StartCoroutine(FindObjectOfType<CameraLookAt>().FinishMovement());
                        StartCoroutine(FindObjectOfType<CameraPosition>().FollowBoss());
                        FindObjectOfType<CameraPosition>().ChangePosition(new Vector3(0, 10f, 25));
                        yield return new WaitForSeconds(.3f);
                        Destroy(Instantiate(hitParticle, headChild.transform.position, headChild.transform.rotation), 2);
                        ScoreManager.Instance.DecreaseBossHealth();
                        yield return new WaitForSeconds(1f);
                        //FindObjectOfType<CameraLookAt>().ChangeCinemachineLookAt();
                        _bossSc.Killself();
                        _anim.SetBool("isAttack", false);
                        _anim.SetBool("isStay", true);
                        StartCoroutine(FollowBoss());
                        UIManager.Instance.CloseTextAndHealthBars();
                        _isHitting = false;
                    }
                }
            }
        }
        public void Killself()
        {
            if(LevelHelper.Instance.bossLevel==2 && ScoreManager.Instance.currentDinosaurLevel==2)
            {
                _anim.avatar = null;
            }
            _anim.SetBool("isDeath", true);
            
        }
        public IEnumerator FollowBoss()
        {
                _anim.SetBool("isRunning", true);
                _anim.SetBool("isStay", false);
            transform.DOMoveZ(lastPositionZ, lastPositionZTime).SetEase(Ease.Linear).OnComplete(() => {
                _anim.SetBool("isRunning", false);
                _anim.SetBool("isRoar", true);
            });
                yield return new WaitForSeconds(2+lastPositionZTime);
                UIManager.Instance.OpenWinCanvas();
            _anim.SetBool("isRoar", false);
            _anim.SetBool("isStay", true);
        }
    }
}