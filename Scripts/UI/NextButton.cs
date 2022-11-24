using DinosaurMergeRun.Managers;
using System.Collections;
using System.Collections.Generic;
using Core.UI;
using UnityEngine;

namespace DinosaurMergeRun.UI
{
    public class NextButton : UIButton
    {
        protected override void DoAction()
        {
            Action();
        }

        private void Action()
        {
            LevelManager.Instance.GetNextLevel();
            UIManager.Instance.IncreaseLevelNumberText();
        }
    }
}