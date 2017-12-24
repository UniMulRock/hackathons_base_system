using UnityEngine;
using System.Collections;

namespace Utility.System
{
    /// <summary>
    /// シーン遷移時のフェードイン・アウトを制御するためのクラス
    /// </summary>
    public class FadeManager : SingletonMonoBehaviour<FadeManager>
    {
        /// <summary>暗転用黒テクスチャ</summary>
        private Texture2D blackTexture;
        /// <summary>フェード中の透明度</summary>
        private float fadeAlpha = 0;
        /// <summary>フェード中かどうか</summary>
        static public bool isFading = false;

        new void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);

            //ここで黒テクスチャ作る
            blackTexture = new Texture2D(32, 32, TextureFormat.RGB24, false);
            blackTexture.SetPixel(0, 0, Color.white);
            blackTexture.Apply();
        }

        public void OnGUI()
        {
            if (!isFading) { return; }

            //透明度を更新して黒テクスチャを描画
            GUI.color = new Color(0, 0, 0, fadeAlpha);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackTexture);
        }

        /// <summary>
        /// 画面遷移
        /// </summary>
        /// <param name='scene'>シーン名</param>
        /// <param name='interval'>暗転にかかる時間(秒)</param>
        public void LoadLevel(string scene, float interval)
        {
            StartCoroutine(TransScene(scene, interval));
        }

        /// <summary>
        /// シーン遷移用コルーチン
        /// </summary>
        /// <param name='scene'>シーン名</param>
        /// <param name='interval'>暗転にかかる時間(秒)</param>
        private IEnumerator TransScene(string scene, float interval)
        {
            //だんだん暗く
            isFading = true;
            float time = 0;
            while (time <= interval)
            {
                fadeAlpha = Mathf.Lerp(0f, 1f, time / interval);
                time += Time.deltaTime;
                yield return 0;
            }

            //シーン切替
            UnityEngine.SceneManagement.SceneManager.LoadScene("Blank");
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene);

            //だんだん明るく
            time = 0;
            while (time <= interval)
            {
                fadeAlpha = Mathf.Lerp(1f, 0f, time / interval);
                time += Time.deltaTime;
                yield return 0;
            }

            isFading = false;
        }

        /// <summary>
        /// フェード用コルーチン
        /// </summary>
        /// <param name='interval'>暗転にかかる時間(秒)</param>
        public IEnumerator FadeOut(float interval)
        {
            //だんだん暗く
            isFading = true;
            float time = 0;
            while (time <= interval)
            {
                fadeAlpha = Mathf.Lerp(0f, 1f, time / interval);
                time += Time.deltaTime;
                yield return 0;
            }

        }

        /// <summary>
        /// フェード用コルーチン
        /// </summary>
        /// <param name='interval'>暗転にかかる時間(秒)</param>
        public IEnumerator FadeIn(float interval)
        {
            //だんだん明るく
            float time = 0;
            while (time <= interval)
            {
                fadeAlpha = Mathf.Lerp(1f, 0f, time / interval);
                time += Time.deltaTime;
                yield return 0;
            }

            isFading = false;
        }
    }
}