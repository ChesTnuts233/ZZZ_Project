//****************** 代码文件申明 ************************
//* 文件：AudioStack                                       
//* 作者：wheat
//* 创建时间：2023/09/29 18:54:33 星期五
//* 描述：用于存储音效信息
//*****************************************************
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace KooFrame
{
    public enum AudioSFXType
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,
        /// <summary>
        /// 攻击前摇
        /// </summary>
        AttackForward = 1,
        /// <summary>
        /// 攻击ATKFrame
        /// </summary>
        AttackATKFrame = 2,
        /// <summary>
        /// 攻击后摇
        /// </summary>
        AttackBack = 3,
        /// <summary>
        /// 环境音效
        /// </summary>
        EnvAudio = 4,
        /// <summary>
        /// 投掷音效
        /// </summary>
        Throw = 5,
        /// <summary>
        /// 摧毁物品音效
        /// </summary>
        Break = 6,
        /// <summary>
        /// BGM
        /// </summary>
        BGM = 7,
        /// <summary>
        /// 所有音效
        /// </summary>
        AllSFX =8,
    }
    [CreateAssetMenu(fileName ="AudioStack",menuName ="ScriptableObject/Audio/new AudioStack")]
    public class AudioStack:ScriptableObject
    {
        #region 字段(均为Private Set)
        [field:SerializeField,LabelText("id")] public int AudioIndex {  get; private set; }
        [field:SerializeField,LabelText("名称")] public string AudioName {  get; private set; }
        [field:SerializeField,LabelText("Clip")] public List<AudioClip> Clips {  get; private set; }
        [field:SerializeField,LabelText("分组")] public AudioMixerGroup AudioGroup {  get; private set; }
        [field:SerializeField,LabelText("音量"),Range(0,1f)] public float Volume {  get; private set; }
        [field:SerializeField,LabelText("音高"),Range(-3,3f)] public float Pitch {  get; private set; }
        [field:SerializeField,LabelText("随机最低音高"),Range(-3,3f)] public float RandomMinPitch {  get; private set; }
        [field: SerializeField, LabelText("随机最高音高"), Range(-3, 3f)] public float RandomMaxPitch { get; private set; }
        [field: SerializeField, LabelText("最高音高"), Range(-3, 3f)] public float MaxPitch { get; private set; }
        [field: SerializeField, LabelText("限制连续播放")] public bool LimitContinuousPlay { get; private set; }
        [field: SerializeField, LabelText("循环播放")] public bool Loop { get; private set; }
        [field:SerializeField,LabelText("3D音效")] public bool Is3D {  get; private set; }
        [field: SerializeField, LabelText("音效类型")] public AudioSFXType AudioType { get; private set; }

        #endregion

        #region 属性
        public List<int> playIndexs;

        #endregion

        #region 构造函数
        public AudioStack()
        {
            Clips = null;
            AudioGroup = null;
            Volume = 1f;
            Pitch = 1f;
            RandomMaxPitch = 1f;
            RandomMinPitch = 1f;
            MaxPitch = 1f;
            Loop = false;
            LimitContinuousPlay = false;
            Is3D = false;
            AudioType = AudioSFXType.None;
        }
        public AudioStack(int audioIndex,string audioName,List<AudioClip> clips, AudioMixerGroup audioGroup, float volume, float pitch,
            float randomMinPitch, float randomMaxPitch,float maxPitch, bool limitContinuousPlay,bool loop,bool is3D,AudioSFXType audioType)
        {
            AudioIndex = audioIndex;
            AudioName = audioName;
            Clips = clips;
            AudioGroup = audioGroup;
            Volume = volume;
            Pitch = pitch;
            RandomMinPitch = randomMinPitch;
            RandomMaxPitch = randomMaxPitch;
            MaxPitch = maxPitch;
            LimitContinuousPlay = limitContinuousPlay;
            Loop = loop;
            Is3D = is3D;
            AudioType = audioType;
        }

        #endregion

        #region 方法

#if UNITY_EDITOR
        /// <summary>
        /// 在AudioLibrary使用，其他地方尽量不要使用
        /// </summary>
        public void UpdateDatas(int audioIndex, string audioName, List<AudioClip> clips, AudioMixerGroup audioGroup, float volume, float pitch, 
            float randomMinPitch, float randomMaxPitch,float maxPitch, bool limitContinuousPlay, bool loop,bool is3D, AudioSFXType audioType)
        {
            AudioIndex = audioIndex;
            AudioName = audioName;
            Clips = clips;
            AudioGroup = audioGroup;
            Volume = volume;
            Pitch = pitch;
            RandomMinPitch = randomMinPitch;
            RandomMaxPitch = randomMaxPitch;
            MaxPitch= maxPitch;
            LimitContinuousPlay = limitContinuousPlay;
            Loop = loop;
            Is3D = is3D;
            AudioType = audioType;
        }

#endif
#endregion
    }
}

