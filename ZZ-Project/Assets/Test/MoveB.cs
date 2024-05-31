//****************** 代码文件申明 ************************
//* 文件：MonoA                                       
//* 作者：Koo
//* 创建时间：2024/05/22 15:24:59 星期三
//* 描述：Nothing
//*****************************************************
using UnityEngine;
using KooFrame.BaseSystem;

namespace GameBuild
{
	public class MonoB : MonoBehaviour
	{
		public int speed = 1;


		private void Update()
		{
			this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
		}
	}
}

