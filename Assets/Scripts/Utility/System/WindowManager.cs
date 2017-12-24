using UnityEngine;
using UnityEngine.Collections;
using System;
using System.Collections.Generic;
using Utility.Window;

namespace Utility.System
{
    public class WindowManager : SingletonMonoBehaviour<WindowManager>
    {
        public const string WINDOW_DIC = "Window/";

        public enum WINDOW_TYPE
        {
            NONE,
            BASE,
            SETTING,
            HELP,
        }

        public Dictionary<WINDOW_TYPE, string> WindowName = new Dictionary<WINDOW_TYPE, string>
        {
            { WINDOW_TYPE.BASE, "BaseWindow" },
            { WINDOW_TYPE.SETTING, "SettingWindow" },
            { WINDOW_TYPE.HELP, "HelpWindow" },
        };

        public Dictionary<WINDOW_TYPE, string> WindowClass = new Dictionary<WINDOW_TYPE, string>
        {
            { WINDOW_TYPE.BASE, "WindowBase" },
            { WINDOW_TYPE.SETTING, "SettingWindow" },
            { WINDOW_TYPE.HELP, "HelpWindow" },
        };

        public WINDOW_TYPE WindowType { get; set; }

        public GameObject WindowRoot{ get; set; }

        public WINDOW_TYPE NextWindowType{ get; set; }

        public WINDOW_TYPE CurrentWindowType{ get; set; }

        public bool IsExecuteWindow{ get; set; }

        new void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        public Transform CreateWindow(WINDOW_TYPE type)
        {
            if (WindowRoot == null || type == WINDOW_TYPE.NONE)
                return null;

            var window = Resources.Load(WINDOW_DIC + WindowName[type]) as GameObject;
            if (window != null)
            {
                var instantiateObject = Instantiate(window, WindowRoot.transform);
                instantiateObject.name = WindowName[type];
                return instantiateObject.transform;
            }
            else
            {
                Debug.LogWarning("【WindowCreateError】" + WINDOW_DIC + WindowName[type] + "の取得に失敗しました。");
            }
            return null;
        }

        public void OpenWindow(WINDOW_TYPE type, bool isForce = false)
        {
            if (WindowRoot == null || type == WINDOW_TYPE.NONE)
                return;
            
            Transform childTransform;
            if (WindowRoot.transform.childCount > 0)
            {
                childTransform = WindowRoot.transform.Find(WindowName[type]);
                if (childTransform == null)
                {
                    childTransform = CreateWindow(type);
                    if (childTransform == null)
                        return;
                }
            }
            else
            {
                childTransform = CreateWindow(type);
                if (childTransform == null)
                    return;
            }

            var windowClass = childTransform.gameObject.GetComponent(WindowClass[type]) as WindowBase;
            if (windowClass == null)
                return;

            if (!IsExecuteWindow)
            {
                if (isForce)
                {
                    if (CurrentWindowType != WINDOW_TYPE.NONE)
                    {
                        CloseWindow(CurrentWindowType);
                    }
                }
                else if (CurrentWindowType != WINDOW_TYPE.NONE)
                {
                    return;
                }

                switch (type)
                {
                    default:
                        WillOpenWindow(type);
                        windowClass.Open(() =>
                            {
                                OpenedWindow();
                            });
                        break;
                }
            }
        }

        private void WillOpenWindow(WINDOW_TYPE type)
        {
            IsExecuteWindow = true;
            NextWindowType = type;
        }

        private void OpenedWindow()
        {
            IsExecuteWindow = false;
            CurrentWindowType = NextWindowType;
        }

        public void CloseWindow(WINDOW_TYPE type)
        {
            if (WindowRoot == null || type == WINDOW_TYPE.NONE)
                return;
            
            var child = WindowRoot.transform.Find(WindowName[type]).gameObject;
            if (child == null)
                return;

            var windowClass = child.GetComponent(WindowClass[type]) as WindowBase;
            if (windowClass == null)
                return;

            if (!IsExecuteWindow)
            {
                switch (type)
                {
                    default:
                        WillCloseWindow();
                        windowClass.Close(() =>
                            {
                                ClosedWindow();
                            });
                        break;
                }
            }
        }

        private void WillCloseWindow()
        {
            IsExecuteWindow = true;
            NextWindowType = WINDOW_TYPE.NONE;
        }

        private void ClosedWindow()
        {
            IsExecuteWindow = false;
            CurrentWindowType = NextWindowType;
        }
    }
}
