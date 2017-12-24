using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace Utility.System
{
    public class CustomSceneManager : SingletonMonoBehaviour<CustomSceneManager>
    {
        //遷移にかかる時間
        public const float TRANS_SECOND = 1f;

        public enum STATE
        {
            BLANK,
            TITLE,
            WINDOW_TEST,
        }

        Dictionary<STATE, string> SceneName = new Dictionary<STATE, string>
        {
            { STATE.BLANK, "Blank" },
            { STATE.TITLE, "TitleScene" },
            { STATE.WINDOW_TEST, "WindowTestScene" },
        };

        public STATE state { get; set; }

        new void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(gameObject);

            state = STATE.TITLE;
        }

        void Update()
        {
            //Android用処理
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
                Debug.Log("input escape");
            }
        }

        //状態遷移
        public void ChangeState(STATE st)
        {
            Instance._ChangeState(st);
        }

        void _ChangeState(STATE st)
        {
            StartCoroutine(TransState(st));
        }

        IEnumerator TransState(STATE st)
        {
            Resources.UnloadUnusedAssets();

            //フェードアウト
            yield return StartCoroutine(FadeManager.Instance.FadeOut(TRANS_SECOND / 2));

            state = st;
            Debug.Log("Scene to " + state);

            //memori release
            SceneManager.LoadScene(SceneName[STATE.BLANK]);

            SceneManager.LoadScene(SceneName[state]);

            //フェードインを待つ
            yield return StartCoroutine(FadeManager.Instance.FadeIn(TRANS_SECOND / 2));
        }
    }
}
