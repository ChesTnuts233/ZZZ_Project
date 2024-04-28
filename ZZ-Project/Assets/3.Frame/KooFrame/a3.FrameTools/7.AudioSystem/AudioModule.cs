using GameBuild;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace KooFrame
{
    public class AudioModule : MonoBehaviour
    {
        private static GameObjectPoolModule poolModule;

        //[SerializeField, LabelText("背景音乐播放器")] private AudioSource BGMAudioSource;
        //[SerializeField, LabelText("背景音乐播放器")] private BGMPlayer BGMPlayer;

        [SerializeField, LabelText("效果播放器预制体2D")]
        private GameObject EffectAudioPlayPrefab2D;
        [SerializeField, LabelText("效果播放器预制体3D")]
        private GameObject EffectAudioPlayPrefab3D;

        [SerializeField, LabelText("对象池预设播放器数量")]
        private int EffectAudioDefaultQuantity = 20;

        [SerializeField, LabelText("混音器")] private AudioMixer m_AudioMixer;

        // 场景中生效的所有特效音乐播放器
        private List<AudioSource> audioPlayList;
        /// <summary>
        /// 正在播放的音效的字典，用来处理高频播放的音效
        /// </summary>
        //private Dictionary<int, List<AudioPlay>> m_AudioPlayingDic;

        #region 音量、播放控制

        [SerializeField, Range(0, 1), OnValueChanged("UpdateMasterVolume"), LabelText("主音量")]
        private float masterVolume;

        public float MasterlVolume
        {
            get => masterVolume;
            set
            {
                if (masterVolume == value) return;
                masterVolume = value;
                UpdateMasterVolume();
            }
        }

        [SerializeField] [Range(0, 1)] [OnValueChanged("UpdateBGAudioPlay"), LabelText("音乐音量")]
        private float bgmVolume;

        public float BGMVolume
        {
            get => bgmVolume;
            set
            {
                if (bgmVolume == value) return;
                bgmVolume = value;
                UpdateBGAudioPlay();
            }
        }

        [SerializeField] [Range(0, 1)] [OnValueChanged("UpdateEffectAudioPlay"), LabelText("音效音量")]
        private float sfxVolume;

        public float SFXVolume
        {
            get => sfxVolume;
            set
            {
                if (sfxVolume == value) return;
                sfxVolume = value;
                UpdateEffectAudioPlay();
            }
        }

        [SerializeField] [OnValueChanged("UpdateMute")]
        private bool isMute = false;

        public bool IsMute
        {
            get => isMute;
            set
            {
                if (isMute == value) return;
                isMute = value;
                UpdateMute();
            }
        }

        [SerializeField] [OnValueChanged("UpdateLoop")]
        private bool isLoop = true;

        public bool IsLoop
        {
            get => isLoop;
            set
            {
                if (isLoop == value) return;
                isLoop = value;
                UpdateLoop();
            }
        }

        [SerializeField] [OnValueChanged("UpdatePause")]
        private bool isPause = false;

        public bool IsPause
        {
            get => isPause;
            set
            {
                if (isPause == value) return;
                isPause = value;
                UpdatePause();
            }
        }

        /// <summary>
        /// 更新全部播放器类型
        /// </summary>
        private void UpdateMasterVolume()
        {
            //设置SFXGroup的音量
            m_AudioMixer.SetFloat("MasterVolume", ConvertVolume(masterVolume));
            //UpdateBGAudioPlay();
            //UpdateEffectAudioPlay();
        }

        /// <summary>
        /// 更新背景音乐
        /// </summary>
        private void UpdateBGAudioPlay()
        {
            //设置SFXGroup的音量
            m_AudioMixer.SetFloat("BGMVolume", ConvertVolume(bgmVolume));
            //BGAudioSource.volume = bgVolume * globalVolume;
        }

        /// <summary>
        /// 更新特效音乐播放器
        /// </summary>
        private void UpdateEffectAudioPlay()
        {
            //设置SFXGroup的音量
            m_AudioMixer.SetFloat("SFXVolume", ConvertVolume(sfxVolume));
            /*
            if (audioPlayList == null) return;
            // 倒序遍历
            for (int i = audioPlayList.Count - 1; i >= 0; i--)
            {
                if (audioPlayList[i] != null)
                {
                    SetEffectAudioPlay(audioPlayList[i]);
                }
                else
                {
                    audioPlayList.RemoveAt(i);
                }
            }
            */
        }

        /// <summary>
        /// 设置特效音乐播放器
        /// </summary>
        private void SetEffectAudioPlay(AudioSource audioPlay, float spatial = -1)
        {
            audioPlay.mute = isMute;
            audioPlay.volume = sfxVolume * masterVolume;
            if (spatial != -1)
            {
                audioPlay.spatialBlend = spatial;
            }

            if (isPause)
            {
                audioPlay.Pause();
            }
            else
            {
                audioPlay.UnPause();
            }
        }

        /// <summary>
        /// 更新全局音乐静音情况
        /// </summary>
        private void UpdateMute()
        {
            //BGMAudioSource.mute = isMute;
            UpdateEffectAudioPlay();
        }

        /// <summary>
        /// 更新背景音乐循环
        /// </summary>
        private void UpdateLoop()
        {
            //BGMAudioSource.loop = isLoop;
        }

        /// <summary>
        /// 更新背景音乐暂停
        /// </summary>
        private void UpdatePause()
        {
            if (isPause)
            {
                //BGMAudioSource.Pause();
            }
            else
            {
                //BGMAudioSource.UnPause();
            }
        }

        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            Transform poolRoot = new GameObject("AudioPlayerPoolRoot").transform;
            poolRoot.SetParent(transform);
            poolModule = new GameObjectPoolModule();
            poolModule.Init(poolRoot);
            poolModule.InitGameObjectPool(EffectAudioPlayPrefab2D, -1, EffectAudioDefaultQuantity);
            poolModule.InitGameObjectPool(EffectAudioPlayPrefab3D, -1, EffectAudioDefaultQuantity);
            audioPlayList = new List<AudioSource>(EffectAudioDefaultQuantity);
            audioPlayRoot = new GameObject("audioPlayRoot").transform;
            audioPlayRoot.SetParent(transform);
            // m_AudioPlayingDic = new Dictionary<int, List<AudioPlay>>();
            // UpdateMasterVolume();
            // //防空
            // if(BGMPlayer==null)
            // {
            //     BGMPlayer = GetComponentInChildren<BGMPlayer>();
            //     if (BGMPlayer == null)
            //     {
            //         GameObject obj = ResSystem.InstantiateGameObject(transform, nameof(BGMPlayer));
            //         BGMPlayer = obj.GetComponent<BGMPlayer>();
            //     }
            // }
        }

        #region 背景音乐

        #region 旧的
        /*旧的
        private static Coroutine fadeCoroutine;

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="loop"></param>
        /// <param name="volume"></param>
        /// <param name="fadeOutTime"></param>
        /// <param name="fadeInTime"></param>
        public void PlayBGMAudio(AudioClip clip, bool loop = true, float volume = -1, float fadeOutTime = 0,
            float fadeInTime = 0)
        {
            IsLoop = loop;
            if (volume != -1)
            {
                BGMVolume = volume;
            }

            fadeCoroutine = StartCoroutine(DoVolumeFade(clip, fadeOutTime, fadeInTime));
            BGMAudioSource.Play();
        }

        /// <summary>
        /// 渐入渐出
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="fadeOutTime"></param>
        /// <param name="fadeInTime"></param>
        /// <returns></returns>
        private IEnumerator DoVolumeFade(AudioClip clip, float fadeOutTime, float fadeInTime)
        {
            float currTime = 0;
            if (fadeOutTime <= 0) fadeOutTime = 0.0001f;
            if (fadeInTime <= 0) fadeInTime = 0.0001f;

            // 降低音量，也就是淡出
            while (currTime < fadeOutTime)
            {
                yield return CoroutineTool.WaitForFrames();
                if (!isPause) currTime += Time.deltaTime;
                float ratio = Mathf.Lerp(1, 0, currTime / fadeOutTime);
                BGMAudioSource.volume = bgmVolume * ratio;
            }

            BGMAudioSource.clip = clip;
            BGMAudioSource.Play();
            currTime = 0;
            // 提高音量，也就是淡入
            while (currTime < fadeInTime)
            {
                yield return CoroutineTool.WaitForFrames();
                if (!isPause) currTime += Time.deltaTime;
                float ratio = Mathf.InverseLerp(0, 1, currTime / fadeInTime);
                BGMAudioSource.volume = bgmVolume * ratio;
            }

            fadeCoroutine = null;
        }

        private static Coroutine bgWithClipsCoroutine;

        /// <summary>
        /// 使用音效数组播放背景音乐，自动循环
        /// </summary>
        public void PlayBGMAudioWithClips(AudioClip[] clips, float volume = -1, float fadeOutTime = 0,
            float fadeInTime = 0)
        {
            bgWithClipsCoroutine = MonoSystem.Start_Coroutine(DoPlayBGAudioWithClips(clips, volume));
        }

        private IEnumerator DoPlayBGAudioWithClips(AudioClip[] clips, float volume = -1, float fadeOutTime = 0,
            float fadeInTime = 0)
        {
            if (volume != -1) BGMVolume = volume;
            int currIndex = 0;
            while (true)
            {
                AudioClip clip = clips[currIndex];
                fadeCoroutine = StartCoroutine(DoVolumeFade(clip, fadeOutTime, fadeInTime));
                float time = clip.length;
                // 时间只要还好，一直检测
                while (time > 0)
                {
                    yield return CoroutineTool.WaitForFrames();
                    if (!isPause) time -= Time.deltaTime;
                }

                // 到达这里说明倒计时结束，修改索引号，继续外侧While循环
                currIndex++;
                if (currIndex >= clips.Length) currIndex = 0;
            }
        }

        public void StopBGMAudio()
        {
            if (bgWithClipsCoroutine != null) MonoSystem.Stop_Coroutine(bgWithClipsCoroutine);
            if (fadeCoroutine != null) MonoSystem.Stop_Coroutine(fadeCoroutine);
            BGMAudioSource.Stop();
            BGMAudioSource.clip = null;
        }

        public void PauseBGMAudio()
        {
            IsPause = true;
        }

        public void UnPauseBGMAudio()
        {
            IsPause = false;
        }
        */
        #endregion

        // /// <summary>
        // /// 播放BGM
        // /// </summary>
        // /// <param name="bgmStack"><要播放的BGM的包/param>
        // public void PlayBGMAudio(BGMStack bgmStack)
        // {
        //     BGMPlayer.PlayBGM(bgmStack);
        // }
        // /// <summary>
        // /// 停止播放BGM
        // /// </summary>
        // /// <param name="immediate">立即停止</param>
        // [Button("停止播放BGM", 30), PropertySpace(5f, 5f), FoldoutGroup("测试按钮")]
        // public void StopBGM(bool immediate)
        // {
        //     BGMPlayer.StopBGMPlay(immediate);
        // }
        // /// <summary>
        // /// 切换播放的音轨
        // /// </summary>
        // /// <param name="trackId">音轨id</param>
        // [Button("切换播放BGM的音轨", 30), PropertySpace(5f, 5f), FoldoutGroup("测试按钮")]
        // public void ChangeSoundTrack(int trackId)
        // {
        //     BGMPlayer.ChangeSoundTrack(trackId);
        // }

        #endregion

        #region 特效音乐

        private Transform audioPlayRoot;

        /*
        /// <summary>
        /// 获取音乐播放器
        /// </summary>
        /// <returns></returns>
        private AudioSource GetAudioPlay(bool is3D = true)
        {
            // 从对象池中获取播放器
            GameObject audioPlay = poolModule.GetGameObject("AudioPlay", audioPlayRoot);
            if (audioPlay.IsNull())
            {
                audioPlay = GameObject.Instantiate(EffectAudioPlayPrefab, audioPlayRoot);
                audioPlay.name = EffectAudioPlayPrefab.name;
            }
            AudioSource audioSource = audioPlay.GetComponent<AudioSource>();
            SetEffectAudioPlay(audioSource, is3D ? 1f : 0f);
            audioPlayList.Add(audioSource);
            return audioSource;
        }
        */
        /// <summary>
        /// 回收播放器
        /// </summary>
        private void RecycleAudioPlay(AudioSource audioSource, AudioClip clip, bool autoReleaseClip, Action callBak)
        {
            StartCoroutine(DoRecycleAudioPlay(audioSource, clip, autoReleaseClip, callBak));
        }

        private IEnumerator DoRecycleAudioPlay(AudioSource audioSource, AudioClip clip, bool autoReleaseClip,
            Action callBak)
        {
            // 延迟 Clip的长度（秒）
            yield return CoroutineTool.WaitForSeconds(clip.length);
            // 放回池子
            if (audioSource != null)
            {
                audioPlayList.Remove(audioSource);
                poolModule.PushGameObject(audioSource.gameObject);
                if (autoReleaseClip) ResSystem.UnloadAsset(clip);
                callBak?.Invoke();
            }
        }

        // public AudioPlay PlayAudio(AudioStack audioStack, Vector2 pos, Transform parent = null, Action callBack = null)
        // {
        //     GameObject audioPlayer = PoolSystem.GetOrNewGameObject(audioStack.Is3D?EffectAudioPlayPrefab3D:EffectAudioPlayPrefab2D, parent);
        //
        //     if (pos != null)
        //     {
        //         audioPlayer.transform.position = pos;
        //     }
        //
        //     AudioPlay audioPlay = audioPlayer.GetComponent<AudioPlay>();
        //     audioPlay.PlayingAudioIndex = audioStack.AudioIndex;
        //     AudioModifyData? data = null;
        //     Action<AudioPlay> action = null;
        //
        //     if (audioStack.LimitContinuousPlay)
        //     {
        //         //注册
        //         data = RegisterPlayAudioDic(audioPlay);
        //
        //         //注销事件
        //         action = (audioPlay) => UnRegisterPlayAudioDic(audioPlay);
        //     }
        //
        //     //开始播放音效
        //     audioPlay.PlaySound(audioStack, callBack, data, action);
        //
        //     //返回AudioPlay
        //     return audioPlay;
        // }
        // /// <summary>
        // /// 注册音效播放字典
        // /// </summary>
        // /// <param name="audioPlay">要播放的音效</param>
        // /// <returns>返回一些修改参数，没有就为null</returns>
        // private AudioModifyData? RegisterPlayAudioDic(AudioPlay audioPlay)
        // {
        //     //防空
        //     if (audioPlay == null) return null;
        //
        //     int audioId = audioPlay.PlayingAudioIndex;
        //
        //     //防止无用信息
        //     if (audioId <= 0) return null;
        //
        //     AudioModifyData? modifyData = null;
        //
        //     //如果字典中不存在那就先创建一个list
        //     if (m_AudioPlayingDic.ContainsKey(audioId) == false)
        //     {
        //         m_AudioPlayingDic.Add(audioId, new List<AudioPlay>());
        //     }
        //
        //     //获取播放列表
        //     List<AudioPlay> playList = m_AudioPlayingDic[audioId];
        //
        //     //如果当前音效播放的数量大于最大数量，那就暂停前面播放的音效的
        //     if (playList.Count >= GameConfigs.MaxSameAudioPlayCount)
        //     {
        //
        //         //升高音调
        //         modifyData = new AudioModifyData(1)
        //         {
        //             pitch = playList[0].m_AudioSource.pitch + 0.05f,
        //         };
        //
        //         //把之前播放的音效给关掉
        //         playList[0].Vanish();
        //     }
        //
        //     //然后把新的播放的音效加入列表
        //     playList.Add(audioPlay);
        //
        //     //返回data
        //     return modifyData;
        // }
        // /// <summary>
        // /// 注销音效播放字典
        // /// </summary>
        // private void UnRegisterPlayAudioDic(AudioPlay audioPlay)
        // {
        //     //防空
        //     if (audioPlay == null) return;
        //
        //     int audioId = audioPlay.PlayingAudioIndex;
        //
        //     //如果字典中不存在那就返回
        //     if (m_AudioPlayingDic.ContainsKey(audioId) == false)
        //     {
        //         return;
        //     }
        //
        //     //注销
        //     List<AudioPlay> playList = m_AudioPlayingDic[audioId];
        //     playList.Remove(audioPlay);
        // }

        /*
        // 宿主销毁时，提前回收
        private void OnOwerDestory(GameObject go, AudioSource audioSource)
        {
            audioSource.transform.SetParent(audioPlayRoot);
        }

        // 播放结束时移除宿主销毁Action
        private void PlayOverRemoveOwnerDesotryAction(Component owner)
        {
            if (owner != null) owner.RemoveOnDestroy<AudioSource>(OnOwerDestory);
        }

        public void PlayOnShot(AudioClip clip, Vector3 position, bool autoReleaseClip = false, float volumeScale = 1, bool is3d = true, Action callBack = null)
        {
            // 初始化音乐播放器
            AudioSource audioSource = GetAudioPlay(is3d);
            audioSource.transform.position = position;

            // 播放一次音效
            audioSource.PlayOneShot(clip, volumeScale);
            // 播放器回收以及回调函数
            RecycleAudioPlay(audioSource, clip, autoReleaseClip, callBack);
        }
        */

        /// <summary>
        /// 将0~1的音量换算成-80f和[-50f,0f]
        /// </summary>
        /// <param name="v">0~1的音量</param>
        /// <returns>换算音量</returns>
        private float ConvertVolume(float v)
        {
            if (v > 0.02f)
            {
                return -50f + 50f * v;
            }
            else
            {
                return -80f;
            }
        }

        #endregion
    }
}
public struct AudioModifyData
{
    public float volume;
    public float pitch;

    public AudioModifyData(int init)
    {
        volume = float.MinValue;
        pitch = float.MinValue;
    }
}