//****************** 代码文件申明 ************************
//* 文件：KeyTipSystem                                       
//* 作者：Koo
//* 创建时间：2024/03/08 15:39:00 星期五
//* 功能：多设备按键提示系统 属于本地化的子系统
//*****************************************************


using UnityEngine;

namespace KooFrame
{
    /// <summary>
    /// 按键提示系统
    /// </summary>
    public class KeyTipSystem : MonoBehaviour
    {
        private static KeyTipSystem instance;

        [SerializeField] private PlayerControllerDeviceType _deviceKeyType;

        private const string OnUpdaterDevice = "OnUpdaterDevice";

        [SerializeField] private KeyTipDataConfig globalKeyTipDataConfig;


        public static void Init()
        {
            instance = FrameRoot.RootTransform.GetComponentInChildren<KeyTipSystem>();
            if (instance.globalKeyTipDataConfig == null)
            {
                //从默认路径开始寻找配置
                instance.globalKeyTipDataConfig = ResSystem.LoadAsset<KeyTipDataConfig>("KeyTipDataConfig");

                //没有找到
                Debug.LogWarning("缺少按键提示的GlobalConfig");
            }
        }


        public PlayerControllerDeviceType DeviceKeyType
        {
            get => instance._deviceKeyType;
            set
            {
                instance._deviceKeyType = value;
                OnControlDeviceChanged();
            }
        }


        private void OnValidate()
        {
            OnControlDeviceChanged();
        }


        /// <summary>
        /// 当控制的设备发生了改变
        /// </summary>
        public static void OnControlDeviceChanged()
        {
            if (instance == null) return;
            EventBroadCastSystem.EventTrigger(OnUpdaterDevice, instance._deviceKeyType);
        }


        /// <summary>
        /// 返回按键提示的所有Sprite图片 如果不存在返回Null
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static KeyTipData GetKeyTip(PlayerActionType type)
        {
            //如果实例为空直接返回
            if (instance == null) return null;
            //如果不存在对应设备key直接返回空
            if (!instance.globalKeyTipDataConfig.GlobalConfig.ContainsKey(instance._deviceKeyType)) return null;

            if (instance.globalKeyTipDataConfig == null)
            {
                //从默认路径开始寻找配置
                instance.globalKeyTipDataConfig = ResSystem.LoadAsset<KeyTipDataConfig>("KeyTipDataConfig");

                //没有找到
                Debug.LogWarning("缺少按键提示的GlobalConfig");
                return null;
            }

            return instance.globalKeyTipDataConfig.GetKeyTipData(instance._deviceKeyType, type);
        }
    }
}