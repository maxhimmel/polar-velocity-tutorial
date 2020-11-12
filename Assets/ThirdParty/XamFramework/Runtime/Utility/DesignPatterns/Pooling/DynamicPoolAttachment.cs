using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Utility.Patterns
{
	public abstract class DynamicPoolAttachment<T> : MonoBehaviour
		where T : Component
	{
		protected T m_objectInPool = null;
		protected LinkedListNode<Component> m_poolNode = null;

		private void Awake()
		{
			m_objectInPool = GetComponent<T>();
			m_poolNode = DynamicPool.Instance.AddPooledObjectByType<T>( m_objectInPool );
		}

		private void OnDestroy()
		{
			DynamicPool.Instance?.RemovePooledObjectByType<T>( m_poolNode );
		}
	}
}