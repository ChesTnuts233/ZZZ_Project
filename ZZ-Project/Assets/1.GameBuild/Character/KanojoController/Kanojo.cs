using System;
using UnityEngine;

namespace GameBuild
{
	public class Kanojo : CharacterBase<Kanojo>
	{
		/// <summary>
		/// 模型GO
		/// </summary>
		[SerializeField]
		private GameObject ModelGO;


		protected override void RegisterIOC()
		{
			this.RegisterModel(new KanojoModel());
		}

		protected override Type inputClass => typeof(CharacterInput);
		protected override object[] inputArgs => new object[] { Invoker };
		protected override Type movementClass => typeof(CharacterMove);
		protected override object[] movementArgs => new object[] { this, this.GetComponent<Rigidbody>(), Camera.main.transform, ModelGO.transform, ModelGO.GetComponent<Animator>(), this };




		protected override void Awake()
		{
			base.Awake();
		}





	}
}