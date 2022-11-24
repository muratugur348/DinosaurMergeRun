using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DinosaurMergeRun
{
    public class CollectableDinosaur : MonoBehaviour
    {
        public int dinosaurLevel;
        public GameObject bloodExplosion;
        public GameObject hitParticle;
        public GameObject glowGreen, glowYellow, glowRed;
        private Animator _anim;
        // Start is called before the first frame update
        void Start()
        {
            _anim = GetComponentInChildren<Animator>();
            GameObject quickOutlineObject = GetComponentInChildren<SkinnedMeshRenderer>().gameObject;
            quickOutlineObject.AddComponent<QuickOutline>();
            quickOutlineObject.GetComponent<QuickOutline>().OutlineWidth = 3;
            quickOutlineObject.GetComponent<QuickOutline>().OutlineMode = QuickOutline.Mode.OutlineVisible;
            if (dinosaurLevel == 2)
            {
                glowGreen.transform.localScale *= 1.3f;
                glowRed.transform.localScale *= 1.2f;
                glowYellow.transform.localScale *= 1.3f;
            quickOutlineObject.GetComponent<QuickOutline>().OutlineWidth = 3.5f;
            }
            else if(dinosaurLevel==3)
            {
                glowGreen.transform.localScale *= 1.5f;
                glowRed.transform.localScale *= 1.5f;
                glowYellow.transform.localScale *= 1.5f;
            quickOutlineObject.GetComponent<QuickOutline>().OutlineWidth = 4.5f;
            }
            
        }

        // Update is called once per frame
        void Update()
        {

        }
        public IEnumerator KillDinosaur()
        {
            yield return new WaitForEndOfFrame();
            bloodExplosion.SetActive(true);
            _anim.SetBool("isDeath", true);
            _anim.speed = 2.5f;
        }
        public IEnumerator Attack()
        {
            _anim.SetBool("isStay", true);
            _anim.SetBool("isAttack", true);
            _anim.speed = 3;
            yield return new WaitForSeconds(.25f);
            hitParticle.SetActive(true);
            yield return new WaitForSeconds(.25f);
            _anim.speed = 1;
            _anim.SetBool("isAttack", false);
        }
        
    }
}