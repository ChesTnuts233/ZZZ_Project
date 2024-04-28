// //****************** 代码文件申明 ************************
// //* 文件：AudioPlay                                       
// //* 作者：wheat
// //* 创建时间：2023/09/29 18:39:23 星期五
// //* 描述：音效播放
// //*****************************************************
// using UnityEngine;
// using System;
// using System.Collections.Generic;
// using UnityEngine.Audio;
// using Sirenix.OdinInspector;
//
// namespace KooFrame
// {
//     [RequireComponent(typeof(AudioSource))]
//     public class AudioPlay : MonoBehaviour
//     {
//         [LabelText("正在播放的音效的id"),AudioDescription]
//         public int PlayingAudioIndex;
//         public AudioSource m_AudioSource;
//         private event Action onVanish;
//         private event Action<AudioPlay> onVanishAudioPlay;
//
//         private void Awake()
//         {
//             InitAudio();
//         }
//         private void OnEnable()
//         {
//             //EventBroadCastSystem.AddEventListener(EventTags.UnloadSceneEvent, OnUnloadSceneEvent);
//         }
//         
//         
//         private void OnDisable()
//         {
//             //EventBroadCastSystem.RemoveEventListener(EventTags.UnloadSceneEvent, OnUnloadSceneEvent);
//         }
//         /// <summary>
//         /// 初始化
//         /// </summary>
//         private void InitAudio()
//         {
//             //获取AudioSource
//             if(m_AudioSource == null)
//             {
//                 m_AudioSource = GetComponent<AudioSource>();
//             }
//
//             //初始化属性
//             m_AudioSource.playOnAwake = false;
//             m_AudioSource.loop = false;
//             m_AudioSource.clip = null;
//         }
//         /// <summary>
//         /// 播放音效
//         /// </summary>
//         /// <param name="audioStack">音效的Stack</param>
//         /// <param name="callBack">回调函数</param>
//         /// <param name="modifyData">额外更改数据</param>
//         public void PlaySound(AudioStack audioStack,Action callBack =null,AudioModifyData? modifyData = null,Action<AudioPlay> unregister =null)
//         {
//             //如果什么都没有就return
//             if (audioStack == null||audioStack.Clips==null||audioStack.Clips.Count==0) return;
//
//             float pitch = 1f;
//
//             if(audioStack.RandomMaxPitch != audioStack.RandomMinPitch)
//             {
//                 pitch = UnityEngine.Random.Range(audioStack.RandomMinPitch, audioStack.RandomMaxPitch);
//             }
//
//             float volume = audioStack.Volume;
//
//             AudioClip clip;
//
//             PlayingAudioIndex = audioStack.AudioIndex;
//
//             //如果音源就一个
//             if(audioStack.Clips.Count==1)
//             {
//                 //那么直接选取
//                 clip = audioStack.Clips[0];
//             }
//             //如果音源有多个
//             else
//             {
//                 //那会随机播放，并且保证每个都会播放一次
//
//                 //如果播放id空了，那就生成一下
//                 if(audioStack.playIndexs==null||audioStack.playIndexs.Count==0)
//                 {
//                     if(audioStack.playIndexs == null)
//                         audioStack.playIndexs = new List<int>();
//
//                     for (int i = 0; i < audioStack.Clips.Count; i++)
//                     {
//                         audioStack.playIndexs.Add(i);
//                     }
//                 }
//                 //随机选取一个播放
//                 int k = UnityEngine.Random.Range(0, audioStack.playIndexs.Count);
//                 int id = audioStack.playIndexs[k];
//                 //然后把它从未播放的列表中去除
//                 audioStack.playIndexs.RemoveAt(k);
//
//                 //选取作为播放的音源
//                 clip = audioStack.Clips[id];
//             }
//
//             //如果有额外更改就应用更改
//             if(modifyData !=null)
//             {
//                 AudioModifyData data = (AudioModifyData)modifyData;
//                 if(data.pitch!= float.MinValue)
//                 {
//                     pitch = data.pitch;
//                 }
//                 if(data.volume!= float.MinValue)
//                 {
//                     volume = data.volume;
//                 }
//             }
//
//             pitch = Mathf.Clamp(pitch, -3f, audioStack.MaxPitch);
//
//             //播放音效
//             PlaySound(clip, audioStack.AudioGroup, volume, pitch, audioStack.Loop,audioStack.Is3D, callBack,unregister);
//         }
//         /// <summary>
//         /// 播放声音
//         /// (不推荐直接使用这个)
//         /// </summary>
//         /// <param name="audioClip">声音的Clip</param>
//         /// <param name="group">输出的Group</param>
//         /// <param name="pitch">音高</param>
//         /// <param name="loop">循环播放</param>
//         /// <param name="callBack">回调函数</param>
//         public void PlaySound(AudioClip audioClip,AudioMixerGroup group,float volume =1f,float pitch =1f,bool loop = false,bool is3D = false,
//             Action callBack = null, Action<AudioPlay> unregister = null)
//         {
//             if(m_AudioSource==null) InitAudio();
//
//             onVanish += callBack;
//             onVanishAudioPlay += unregister;
//
//             m_AudioSource.outputAudioMixerGroup = group;
//
//             m_AudioSource.clip = audioClip;
//
//             m_AudioSource.pitch = pitch;
//
//             m_AudioSource.volume = volume;
//
//             m_AudioSource.loop = loop;
//
//             //如果AudioSource被关了，或者gameobject被关了就不播放了
//             if (m_AudioSource.enabled == false || gameObject.activeSelf == false)
//             {
//                 Vanish();
//             }
//             //一切正常才播放
//             else
//             {
//                 m_AudioSource.Play();
//             }
//         }
//
//         private void Update()
//         {
//             if(!m_AudioSource.isPlaying&&m_AudioSource.clip!=null)
//             {
//                 //如果是循环播放的话那就重新开始播放
//                 if(m_AudioSource.loop)
//                 {
//                     m_AudioSource.Play();
//                 }
//                 //不是就消失
//                 else
//                 {
//                     Vanish();
//                 }
//             }
//         }
//         /// <summary>
//         /// 卸载场景事件 
//         /// </summary>
//         private void OnUnloadSceneEvent()
//         {
//             Vanish();
//         }
//         /// <summary>
//         /// 消失
//         /// </summary>
//         public void Vanish()
//         {
//             //如果有事件的话调用
//             onVanish?.Invoke();
//             onVanishAudioPlay?.Invoke(this);
//             PlayingAudioIndex = 0;
//             //清空事件
//             onVanish = null;
//             onVanishAudioPlay = null;
//             //清空clip
//             m_AudioSource.clip = null;
//             //恢复默认音高
//             m_AudioSource.pitch = 1f;
//             //推入对象池
//             PoolSystem.PushGameObject(gameObject);
//
//         }
//
//     }
// }
//
