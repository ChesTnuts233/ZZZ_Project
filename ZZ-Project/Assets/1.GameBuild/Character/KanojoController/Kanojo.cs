using System;
using UnityEngine;

namespace GameBuild
{
	public class Kanojo : CharacterBase<Kanojo>
	{
		protected override void RegisterIOC()
		{
			this.RegisterModel(new KanojoModel());
		}
		protected override Type inputClass => typeof(KanojoInput);
		protected override object[] inputArgs => new object[] { Invoker };
		protected override Type movementClass => typeof(KanojoMove);
		protected override object[] movementArgs => new object[] { this, this.GetComponent<Rigidbody>(), Camera.main.transform, this.transform, this.GetComponent<Animator>(), this };

		protected override void Awake()
		{
			base.Awake();
		}





	}
}