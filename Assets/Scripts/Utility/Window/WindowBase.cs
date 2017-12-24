using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;

namespace Utility.Window
{
    public class WindowBase : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup ScreenRoot;

        private const float SCALE_DURATION = 0.5f;
        private const float FADE_IN_VALUE = 1f;
        private const float FADE_OUT_VALUE = 0f;
        private const float FADE_DURATION = 0.5f;

        private void Awake()
        {
            if (ScreenRoot != null)
            {
                if (ScreenRoot.gameObject.activeSelf)
                    ScreenRoot.gameObject.SetActive(false);
                ScreenRoot.transform.localScale = Vector3.zero;
            }
        }

        [ContextMenu("Open")]
        virtual public void Open(Action callback = null)
        {
            if (ScreenRoot == null)
                return;

            ScreenRoot.gameObject.SetActive(true);

            Sequence seq = DOTween.Sequence();
            seq.Append
            (
                ScreenRoot.transform.DOScale(Vector3.one, SCALE_DURATION)
            ).Join(
                ScreenRoot.DOFade(FADE_IN_VALUE, FADE_DURATION)
            ).OnComplete(() =>
                {
                    if (callback != null)
                    {
                        callback();
                    }
                });
        }

        [ContextMenu("Close")]
        virtual public void Close(Action callback = null)
        {
            if (ScreenRoot == null)
                return;
            
            Sequence seq = DOTween.Sequence();
            seq.Append
            (
                ScreenRoot.transform.DOScale(Vector3.zero, SCALE_DURATION)
            ).Join(
                ScreenRoot.DOFade(FADE_OUT_VALUE, FADE_DURATION)
            ).OnComplete(() =>
                {
                    ScreenRoot.gameObject.SetActive(false);
                    if (callback != null)
                    {
                        callback();
                    }
                });
        }
    }
}
