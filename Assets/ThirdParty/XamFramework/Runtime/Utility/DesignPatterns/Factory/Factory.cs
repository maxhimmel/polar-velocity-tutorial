using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Gameplay.Patterns
{
	public abstract class Factory<T> : MonoBehaviour 
		where T : Object
	{
		[Header( "References" )]
		[SerializeField] protected T m_prefab = default;

		[Header( "Modifiers" )]
		[SerializeField] protected bool m_spawnOnAwake = true;
		[SerializeField] protected bool m_parentToFactory = true;

		public virtual T Create( Vector3 position = default, Quaternion rotation = default, Transform parent = null )
		{
			T newObj = Instantiate( m_prefab, position, rotation, parent );
			return newObj;
		}

		protected virtual void Awake()
		{
			if ( m_spawnOnAwake )
			{
				Transform parent = m_parentToFactory ? transform : null;
				Create( transform.position, transform.rotation, parent );
			}
		}
	}
}