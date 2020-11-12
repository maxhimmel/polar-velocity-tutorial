using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Gameplay.Patterns
{
	public abstract class Factory_Spawnable<T> : Factory<T> where T : Object, ISpawnable
	{
		public override T Create( Vector3 position = default, Quaternion rotation = default, Transform parent = null )
		{
			T newObj = base.Create( position, rotation, parent );

			ISpawnable spawnable = newObj as ISpawnable;
			spawnable.OnSpawn( this );

			return newObj;
		}
	}
}