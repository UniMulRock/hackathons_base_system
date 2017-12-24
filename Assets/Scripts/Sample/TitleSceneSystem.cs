using Utility.System;
using GodTouches;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

using Utility.SystemDefine;

namespace System.Scene
{
    public class TitleSceneSystem : SingletonMonoBehaviour<TitleSceneSystem>
    {
        [SerializeField]
        private CanvasGroup canvasGroup;

        void Start()
        {
            if (canvasGroup != null)
            {
                canvasGroup.DOFade(1f, 1f).Complete();
            }
        }

        public void OnClickDefaultNextScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("WindowTestScene");
        }

        public void OnClickCustomNextScene()
        {
            canvasGroup.DOFade(0, 1.0f);
            NextScene();
        }

        private void NextScene()
        {
            CustomSceneManager.Instance.ChangeState(CustomSceneManager.STATE.WINDOW_TEST);
        }
    }
}
