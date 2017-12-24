using System.Collections.Generic;

namespace Utility.SystemDefine
{
    public class WindowDefine
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
    }
}