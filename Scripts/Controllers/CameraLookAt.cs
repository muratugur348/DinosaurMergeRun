using Cinemachine;
using DinosaurMergeRun.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DinosaurMergeRun.Controllers
{
    public class CameraLookAt : MonoBehaviour
    {
        public float lerpSpeed;
        private DinosaurController _dinosaurController;
        private Boss _bossSc;
        private CinemachineVirtualCamera _cinemachineVirtualCamera;
        // Start is called before the first frame update
        void Start()
        {
            _cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            _dinosaurController = FindObjectOfType<DinosaurController>();
            _bossSc = FindObjectOfType<Boss>();
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void StartFollowDinosaur()
        {
            StartCoroutine(FollowDinosaur());
        }
        private IEnumerator FollowDinosaur()
        {
            if (!LevelManager.Instance.isFighting)
            {
                yield return new WaitForFixedUpdate();
                //transform.position = Vector3.Lerp(transform.position, _dinosaurController.transform.position, lerpSpeed * Time.deltaTime);
                transform.position = _dinosaurController.transform.position;
                StartCoroutine(FollowDinosaur());
            }
        }
        public void ChangeCinemachineLookAt()
        {
            _cinemachineVirtualCamera.m_LookAt=gameObject.transform;
        }
        public IEnumerator FinishMovement()
        {
            yield return new WaitForEndOfFrame();
            transform.position = Vector3.Lerp(transform.position,
                    new Vector3(0, _bossSc.transform.position.y, _bossSc.transform.position.z)
                    , lerpSpeed * Time.deltaTime);
            StartCoroutine(FinishMovement());
        }

    }
}