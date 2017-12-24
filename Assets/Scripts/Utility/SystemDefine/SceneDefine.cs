using System.Collections.Generic;

namespace Utility.SystemDefine
{
    public class SceneDefine
    {
        //遷移にかかる時間
        public const float TRANS_SECOND = 1f;

        public enum STATE
        {
            BLANK,
            TITLE,
            WINDOW_TEST,
        }

        public Dictionary<STATE, string> SceneName = new Dictionary<STATE, string>
        {
            { STATE.BLANK, "Blank" },
            { STATE.TITLE, "TitleScene" },
            { STATE.WINDOW_TEST, "WindowTestScene" },
        };
    }
}