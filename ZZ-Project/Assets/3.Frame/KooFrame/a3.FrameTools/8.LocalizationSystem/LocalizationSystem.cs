//****************** 代码文件申明 ************************
//* 文件：LocalizationSystem                      
//* 作者：32867
//* 创建时间：2023年08月22日 星期二 17:03
//* 功能：
//*****************************************************


using System;
using UnityEngine;

namespace KooFrame
{
    public class LocalizationSystem : MonoBehaviour
    {
        private static LocalizationSystem instance;
        private const string OnUpdaterLanguage = "OnUpdaterLanguage";

        /// <summary>
        /// 访问或设置语言类型，设置时会自动分发语言修改事件
        /// </summary>
        public static LanguageType LanguageType
        {
            get
            {
                if (instance == null)
                {
                    instance = FrameRoot.RootTransform.GetComponentInChildren<LocalizationSystem>();
                }

                return instance.languageType;
            }
            set
            {
                instance.languageType = value;
                OnLanguageValueChanged();
            }
        }

        public static void Init()
        {
            instance = FrameRoot.RootTransform.GetComponentInChildren<LocalizationSystem>();
        }

        /// <summary>
        /// 全局的配置
        /// 可以运行时修改此配置
        /// </summary>
        [SerializeField] private LocalizationOdinConfig globalOdinConfig;

        [SerializeField] private LanguageType languageType;

        private void OnValidate()
        {
            OnLanguageValueChanged();
        }

        public static void OnLanguageValueChanged()
        {
            if (instance == null) return; // 应该没有运行
            EventBroadCastSystem.EventTrigger(OnUpdaterLanguage, instance.languageType);
        }

        /// <summary>
        /// 获取内容，如果不存在会返回Null
        /// </summary>
        /// <returns></returns>
        public static T GetContent<T>(string key, LanguageType languageType) where T : LocalizationDataBase =>
            instance.GetContentByKey<T>(key, languageType);

        public T GetContentByKey<T>(string key, LanguageType languageType) where T : LocalizationDataBase
        {
            if (globalOdinConfig == null)
            {
                Debug.LogWarning("缺少globalConfig");
                return null;
            }

            return globalOdinConfig.GetContent<T>(key, languageType);
        }

        public static LocalizationOdinConfig GetGlobalConfig()
        {
            if (instance != null)
            {
                return instance.globalOdinConfig;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据key获取当前语言的文本内容
        /// </summary>
        public static string GetGlobalStringData(string key)
        {
            //如果实例为空直接返回
            if (instance == null) return key;
            //如果不存在key直接返回
            if (!instance.globalOdinConfig.config.ContainsKey(key)) return key;
            //如果不是文本data直接返回
            if (!(instance.globalOdinConfig.config[key][LanguageType.SimplifiedChinese] is LocalizationStringData))
                return key;

            //如果条件都符合就返回当前语言的文本内容
            return ((LocalizationStringData)instance.globalOdinConfig.config[key][instance.languageType]).content;
        }

        public static void RegisterLanguageEvent(Action<LanguageType> action)
        {
            EventBroadCastSystem.AddEventListener(OnUpdaterLanguage, action);
        }

        public static void UnregisterLanguageEvent(Action<LanguageType> action)
        {
            EventBroadCastSystem.RemoveEventListener(OnUpdaterLanguage, action);
        }
    }
}