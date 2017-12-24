using Utility.System;
using UnityEngine;

namespace Utility.Window
{
    public class SettingWindow : WindowBase
    {
        public void OnClose()
        {
            if (WindowManager.Validation())
            {
                WindowManager.Instance.CloseWindow(WindowManager.WINDOW_TYPE.SETTING);
            }
        }

        public void OnHelp()
        {
            if (WindowManager.Validation())
            {
                WindowManager.Instance.OpenWindow(WindowManager.WINDOW_TYPE.HELP,true);
            }
        }
    }
}