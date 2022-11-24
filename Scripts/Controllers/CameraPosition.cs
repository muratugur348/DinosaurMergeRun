using Cinemachine;
using DG.Tweening;
using DinosaurMergeRun.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DinosaurMergeRun.Controllers
{
    public class CameraPosition : MonoBehaviour
    {
        public CameraLookAt cameraLookAtSc;
        public float lerpSpeed;
        private GameObject _childForCameraPos;
        private Boss _bossSc;
        private DinosaurController _dinosaurController;
        private CinemachineVirtualCamera _cinemachineVirtualCamera;
        private CinemachineComposer _cinemachineComposer;
        // Start is called before the first frame update
        void Start()
        {
            _dinosaurController = FindObjectOfType<DinosaurController>();
            DOTween.Init();
            _childForCameraPos = transform.GetChild(0).gameObject;
            _cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            _cinemachineComposer = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineComposer>();
            _bossSc = FindObjectOfType<Boss>();
        }

        // Update is called once per frame
        void Update()
        {

        }
        public IEnumerator RotateY(float value, float rotateTime, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            transform.DOLocalRotate(new Vector3(transform.eulerAngles.x, value, transform.eulerAngles.z), rotateTime).SetEase(Ease.InSine);
                StartCoroutine(FollowDinosaur());
            yield return new WaitForSeconds(rotateTime + 0.1f);
                _dinosaurController.StartMove();
                cameraLookAtSc.StartFollowDinosaur();
                LevelManager.Instance.isGamePlayable = true;

        }
        private IEnumerator FollowDinosaur()
        {
            if (!LevelManager.Instance.isGameFinished)
            {
                yield return new WaitForEndOfFrame();
                //transform.position = Vector3.Lerp(transform.position, _dinosaurController.transform.position, lerpSpeed * Time.deltaTime);
                transform.position = _dinosaurController.transform.position;
                StartCoroutine(FollowDinosaur());
            }
        }
        public void IncreaseYandZPosition(float y,float z)
        {
            _childForCameraPos.transform.DOLocalMove(new Vector3 (_childForCameraPos.transform.localPosition.x, 
                _childForCameraPos.transform.localPosition.y + y,_childForCameraPos.transform.localPosition.z + z), .75f).SetEase(Ease.InSine);
        }
        public void ChangeTrackedObjectOffsetOfVC(Vector3 value)
        {
            DOTween.To(() => _cinemachineComposer.m_TrackedObjectOffset, x => _cinemachineComposer.m_TrackedObjectOffset = x,
               value, 1f).SetEase(Ease.InSine);
        }
        public void ChangePosition(Vector3 value)
        {
            _childForCameraPos.transform.DOLocalMove(value,1f).SetEase(Ease.InSine);
        }
        public IEnumerator FollowBoss()
        {
                yield return new WaitForEndOfFrame();
                transform.position = Vector3.Lerp(transform.position, 
                    new Vector3(0, _bossSc.transform.position.y, _bossSc.transform.position.z)
                    , lerpSpeed * Time.deltaTime);
                //transform.position = _dinosaurController.transform.position;
                StartCoroutine(FollowBoss());
        }
    }
}