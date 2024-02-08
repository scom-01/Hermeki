using System;
using UnityEngine;

namespace SCOM.UI
{
    public class UIManager : MonoBehaviour
    {
        public Canvas Canvas
        {
            get
            {
                if (canvas == null)
                {
                    canvas = GetComponent<Canvas>();
                }
                return canvas;
            }
        }
        private Canvas canvas;

        public Animator animator
        {
            get
            {
                if (anim == null)
                {
                    anim = GetComponent<Animator>();
                }
                return anim;
            }
        }
        private Animator anim;

        private Action UIUpdate;

        protected void Update()
        {
            InvokeAction(UIUpdate);
        }

        public void InvokeAction(Action action)
        {
            if (action == null)
            {
                Debug.Log("Action is Null");
                return;
            }
            action.Invoke();
        }

        public void AddAction(Action action)
        {
            UIUpdate -= action;
            UIUpdate += action;
        }
    }
}