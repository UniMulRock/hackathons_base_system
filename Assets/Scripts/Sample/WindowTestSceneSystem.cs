using Utility.System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using DG.Tweening;

namespace System.Scene
{
    public class WindowTestSceneSystem : SingletonMonoBehaviour<WindowTestSceneSystem>
    {
        /// <summary>
        /// WindowRoot
        /// </summary>
        [SerializeField]
        private GameObject windowRoot;

        [SerializeField]
        private CanvasGroup canvasGroup;

        protected override void Awake()
        {
			base.Awake ();
			GameObject.DontDestroyOnLoad(this.gameObject);

            if (WindowManager.Validation())
            {
                WindowManager.Instance.WindowRoot = windowRoot;
            }
        }

        void Start()
        {
            if (canvasGroup != null)
            {
                canvasGroup.DOFade(1f, 1f).Complete();
            }
        }

        public void OnSetting()
        {
            if (WindowManager.Validation())
            {
                WindowManager.Instance.OpenWindow(WindowManager.WINDOW_TYPE.SETTING);
            }
        }

        public void OnClickCustomNextScene()
        {
            canvasGroup.DOFade(0, 1.0f);
            NextScene();
        }

        private void NextScene()
        {
            CustomSceneManager.Instance.ChangeState(CustomSceneManager.STATE.TITLE);
        }
    }
}