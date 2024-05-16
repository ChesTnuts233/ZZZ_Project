//****************** 代码文件申明 ************************
//* 文件：RunAnimState                                       
//* 作者：Koo
//* 创建时间：2024/05/17 03:22:42 星期五
//* 描述：Nothing
//*****************************************************

using KooFrame;
using UnityEngine;

namespace GameBuild
{
	public class RunAnimState : StateMachineBehaviour
	{
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateEnter(animator, stateInfo, layerIndex);
			"进入Run".Log();
		}
	}
}
