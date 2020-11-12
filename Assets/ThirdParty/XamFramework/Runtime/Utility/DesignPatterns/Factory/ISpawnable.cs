using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Gameplay.Patterns
{
	public interface ISpawnable
	{
		void OnSpawn<T>( Factory_Spawnable<T> creator ) where T : Object, ISpawnable;
	}
}