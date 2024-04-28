using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace KooFrame.FrameTools
{
    //基于协程的定时器

    public partial class KNGameTimeTool
    {
        public static Coroutine WaitTime(float time, UnityAction callBack)
        {
            return MonoSystem.Start_Coroutine(TimeCoroutine(time, callBack));
        }

        public static void CancelWait(ref Coroutine coroutine)
        {
            if (coroutine == null) return;
            MonoSystem.Stop_Coroutine(coroutine);
            coroutine = null;
        }

        private static IEnumerator TimeCoroutine(float time, UnityAction callBack)
        {
            yield return CoroutineTool.WaitForSeconds(time);
            callBack?.Invoke();
        }
    }
}