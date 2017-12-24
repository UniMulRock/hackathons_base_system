using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Utility.System
{
    /// サウンド管理
    public class Sound
    {
        const int MAX_BGM = 1;
        const int MAX_SE = 5;
        const int MAX_VOICE = 5;
        const float DEFAULT_VOLUME_BGM = 0.3f;
        const float DEFAULT_VOLUME_SE = 0.5f;
        const float DEFAULT_VOLUME_VOICE = 0.5f;


        // シングルトン
        static Sound _singleton = null;
        // インスタンス取得
        public static Sound Instance
        {
            get
            {
                return _singleton ?? (_singleton = new Sound());
            }
        }

        // サウンド再生のためのゲームオブジェクト
        GameObject _object = null;
        // サウンドリソース
        AudioSource _sourceBgm = null;
        List<AudioSource> _sourceSeArray;
        // SE (チャンネル)
        List<AudioSource> _sourceVoiceArray;
        // VOICE (チャンネル)

        bool _isPause;

        public static bool isPause
        {
            get
            {
                return Instance._isPause;
            }
            set
            {
                Instance._isPause = value;
            }
        }

        // BGMにアクセスするためのテーブル
        Dictionary<string, _Data> _poolBgm = new Dictionary<string, _Data>();
        // SEにアクセスするためのテーブル
        Dictionary<string, _Data> _poolSe = new Dictionary<string, _Data>();
        // VOICEにアクセスするためのテーブル
        Dictionary<string, _Data> _poolVoice = new Dictionary<string, _Data>();

        /// 保持するデータ
        class _Data
        {
            /// アクセス用のキー
            public string Key;
            /// リソース名
            public string ResName;
            /// AudioClip
            public AudioClip Clip;
            ///追加時間
            public DateTime addTime;

            /// コンストラクタ
            public _Data(string key, string res, DateTime time = new DateTime())
            {
                Key = key;
                ResName = res;
                // AudioClipの取得
                Clip = Resources.Load("Sounds/" + ResName) as AudioClip;
                addTime = time;
            }
        }

        /// コンストラクタ
        public Sound()
        {
            // チャンネル確保
            _sourceSeArray = new List<AudioSource>();
            // チャンネル確保
            _sourceVoiceArray = new List<AudioSource>();
            //オブジェクトを生成
            _object = new GameObject("SoundSourceHolder");
            // 破棄しないようにする
            GameObject.DontDestroyOnLoad(_object);

            // AudioSourceを作成
            //BGMの設定をする
            _sourceBgm = _object.AddComponent<AudioSource>();
            _sourceBgm.loop = true;

            // (初期)ボリューム設定
            _SetVolume(eType.Bgm, DEFAULT_VOLUME_BGM);
        }

        public static void PichBgm(float pich)
        {
            Instance._sourceBgm.pitch = pich;
        }


        //SEの読み込み
        public static void LoadSe(string key)
        {
            Instance._LoadSe(key);
        }

        void _LoadSe(string key)
        {
            //すでに存在する
            if (_poolSe.ContainsKey(key))
            {
                return;
            }
            _poolSe.Add(key, new _Data(key, key, DateTime.Now));

            if (_poolSe.Count > MAX_SE)
            {
                _Data old = _poolSe.Last().Value;
                foreach (_Data d in _poolSe.Values)
                {
                    if (d.addTime < old.addTime)
                    {
                        old = d;
                    }
                }
                _poolSe.Remove(old.Key);
            }
        }

        /// SEの再生
        /// ※事前にLoadSeでロードしておくこと
        public static void PlaySe(string key = "Enter")
        {
            Instance._PlaySe("Se/" + key);
        }

        void _PlaySe(string key)
        {
            _LoadSe(key);

            // リソースの取得
            _Data _data = _poolSe[key];
            //最終士用時間を適用
            _data.addTime = DateTime.Now;

            //同じSEが使っている音源がれば利用
            foreach (AudioSource source in _sourceSeArray)
            {
                if (source.name == key)
                {
                    source.Play();
                    return;
                }
            }

            //再生されていない音源があれば利用
            foreach (AudioSource source in _sourceSeArray)
            {
                if (!source.isPlaying)
                {
                    source.clip = _data.Clip;
                    source.Play();
                    return;
                }
            }

            //使える音源がなかった場合追加
            _sourceSeArray.Add(_object.AddComponent<AudioSource>());
            _SetVolume(eType.Se, DEFAULT_VOLUME_SE);
            _sourceSeArray.Last().clip = _data.Clip;
            _sourceSeArray.Last().Play();
        }

        //BGMの読み込み
        public static void LoadBgm(string key)
        {
            Instance._LoadBgm(key);
        }

        void _LoadBgm(string key)
        {
            //すでに存在する
            if (_poolBgm.ContainsKey(key))
            {
                return;
            }
            _poolBgm.Add(key, new _Data(key, key, DateTime.Now));
            if (_poolBgm.Count > MAX_BGM)
            {
                _Data old = _poolBgm.Last().Value;
                foreach (_Data d in _poolBgm.Values)
                {
                    if (d.addTime < old.addTime)
                    {
                        old = d;
                    }
                }
                _poolBgm.Remove(old.Key);
            }
        }

        /// <summary>
        /// 指定したパスのBGMを再生する(事前のロードは不要)
        /// </summary>
        /// <param name="key">Resources/Sounds以下のパス</param>
        public static void PlayBgm(string key)
        {
            Instance._PlayBgm("Bgm/" + key);
        }

        void _PlayBgm(string key)
        {
            _LoadBgm(key);

            _Data _data = _poolBgm[key];

            //既に再生中の同じBGMがあれば何もしない
            if (_sourceBgm.isPlaying && _sourceBgm.clip.Equals(_data.Clip))
            {
                return;
            }

            // 再生
            _sourceBgm.clip = _data.Clip;
            _sourceBgm.Play();
        }

        public static void StopBgm()
        {
            Instance._StopBgm();
        }

        void _StopBgm()
        {
            _sourceBgm.Stop();
        }

        public static bool IsPlayingBgm()
        {
            return Instance._IsPlayingBgm();
        }

        bool _IsPlayingBgm()
        {
            return _sourceBgm.isPlaying;
        }

        //VOICEの読み込み
        public static void LoadVoice(string key)
        {
            Instance._LoadVoice(key);
        }

        void _LoadVoice(string key)
        {
            //すでに存在する
            if (_poolVoice.ContainsKey(key))
            {
                return;
            }
            _poolVoice.Add(key, new _Data(key, key, DateTime.Now));

            if (_poolVoice.Count > MAX_VOICE)
            {
                _Data old = _poolVoice.Last().Value;
                foreach (_Data d in _poolVoice.Values)
                {
                    if (d.addTime < old.addTime)
                    {
                        old = d;
                    }
                }
                _poolVoice.Remove(old.Key);
            }
        }

        /// Voiceの再生
        /// ※事前にLoadVoiceでロードしておくこと
        public static void PlayVoice(string key = "Enter")
        {
            Instance._PlayVoice("Se/" + key);
        }

        void _PlayVoice(string key)
        {
            _LoadVoice(key);

            // リソースの取得
            _Data _data = _poolVoice[key];
            //最終士用時間を適用
            _data.addTime = DateTime.Now;

            //同じVoiceが使っている音源がれば利用
            foreach (AudioSource source in _sourceVoiceArray)
            {
                if (source.name == key)
                {
                    source.Play();
                    return;
                }
            }

            //再生されていない音源があれば利用
            foreach (AudioSource source in _sourceVoiceArray)
            {
                if (!source.isPlaying)
                {
                    source.clip = _data.Clip;
                    source.Play();
                    return;
                }
            }

            //使える音源がなかった場合追加
            _sourceVoiceArray.Add(_object.AddComponent<AudioSource>());
            _SetVolume(eType.Voice, DEFAULT_VOLUME_VOICE);
            _sourceVoiceArray.Last().clip = _data.Clip;
            _sourceVoiceArray.Last().Play();
        }


        /// サウンド種別
        public enum eType
        {
            Bgm,
            Se,
            Voice

        }

        public static void SetVolume(eType type, float value)
        {
            Instance._SetVolume(type, value);
        }

        void _SetVolume(eType type, float value)
        {
            if (type == eType.Bgm)
            {
                _sourceBgm.volume = value;
                return;
            }
            if (type == eType.Se)
            {
                foreach (AudioSource source in _sourceSeArray)
                {
                    source.volume = value;
                }
                return;
            }
            if (type == eType.Voice)
            {
                foreach (AudioSource source in _sourceVoiceArray)
                {
                    source.volume = value;
                }
                return;
            }
        }

    }

    // 音量クラス
    public class Volume
    {
        public float Bgm;

        public void SetBgm(float value)
        {
            Bgm = value;
            Sound.SetVolume(Sound.eType.Bgm, value);
        }

        public float Se;

        public void SetSe(float value)
        {
            Se = value;
            Sound.SetVolume(Sound.eType.Se, value);
        }

        public float Voice;

        public void SetVoice(float value)
        {
            Voice = value;
            Sound.SetVolume(Sound.eType.Voice, value);
        }

        public Volume()
        {
            Bgm = Mathf.Pow(0.5f, 2);
            Se = Mathf.Pow(0.6f, 2);
            Voice = Mathf.Pow(0.9f, 2);
        }

        public void SetAllVolume()
        {
            Sound.SetVolume(Sound.eType.Bgm, Bgm);
            Sound.SetVolume(Sound.eType.Se, Se);
            Sound.SetVolume(Sound.eType.Voice, Voice);
        }
    }
}