using UnityEngine;

namespace AE_SkillEditor_Plus.RunTime.Driver
{
    /// <summary>
    /// 播放行为
    /// </summary>
	public class AEPlayableBehaviour
    {
        public AEPlayableStateEnum State { get; protected set; }

        public AEPlayableBehaviour(StandardClip clip)
        {
        }

        public virtual void OnEnter(GameObject context, int currentFrameID)
        {
            State = AEPlayableStateEnum.Running;
            // Debug.LogWarning("OnEnter");
        }

        //更新帧的行为状态
        public virtual void Tick(int currentFrameID, int fps, GameObject context)
        {
            // if (State != AEPlayableStateEnum.Running) return;
            // Debug.Log("OnUpdate  "  + currentFrameID);
        }

        public virtual void OnExit(GameObject context, int currentFrameID)
        {
            State = AEPlayableStateEnum.Exit;
            // Debug.LogWarning("OnExit");
        }
    }
}