using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DinosaurMergeRun.Controllers;
using DinosaurMergeRun.Managers;
using Core.UI;
using Cinemachine;
using UnityEngine.EventSystems;

namespace DinosaurMergeRun.UI
{
    public class StartButton : UIButton, IPointerDownHandler
    {
        private bool _isClicked;
        // Start is called before the first frame update
        private void Start()
        {


        }
        protected override void DoAction()
        {

        }

        private void StartCamera()
        {
            FindObjectOfType<LevelHelper>().StartGame();
        }
        private IEnumerator OpenGamePlay()
        {
            yield return new WaitForSeconds(0.5f);
            RUIPanel.Open("Gameplay");
            gameObject.SetActive(false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_isClicked)
            {
                _isClicked = true;
                StartCamera();
                StartCoroutine(OpenGamePlay());
                ScoreManager.Instance.AssignDinosaurs();
                ScoreManager.Instance.UpdateParticlesOnDinosaurs();
            }
        }
    }
}