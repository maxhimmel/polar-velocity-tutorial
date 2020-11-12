using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Xam.Utility.Patterns
{
	public class DynamicPool : SingletonMono<DynamicPool>
	{
		private Dictionary<System.Type, LinkedList<Component>> m_pooledObjectsByType = new Dictionary<System.Type, LinkedList<Component>>();

		public T GetFirstPooledObjectByType<T>() where T : Component
		{
			IEnumerable<T> pooledObjects = GetPooledObjectsByType<T>( out int count );

			return count > 0
				? pooledObjects.First()
				: null;
		}

		public IEnumerable<T> GetPooledObjectsByType<T>( out int count ) where T : Component
		{
			if ( m_pooledObjectsByType.TryGetValue( typeof( T ), out LinkedList<Component> result ) && result != null )
			{
				count = result.Count;
				return result.Cast<T>();
			}

			count = 0;
			return null;
		}

		public IEnumerable<T> GetPooledObjectsByType<T>() where T : Component
		{
			if ( m_pooledObjectsByType.TryGetValue( typeof( T ), out LinkedList<Component> result ) && result != null )
			{
				return result.Cast<T>();
			}
			return null;
		}

		public LinkedListNode<Component> AddPooledObjectByType<T>( Component newObj ) where T : Component
		{
			if ( newObj == null ) { return null; }

			System.Type type = typeof( T );
			if ( m_pooledObjectsByType.TryGetValue( type, out LinkedList<Component> result ) )
			{
				return result.AddLast( newObj );
			}

			result = new LinkedList<Component>();
			m_pooledObjectsByType[type] = result;

			return result.AddLast( newObj );
		}

		public void RemovePooledObjectByType<T>( LinkedListNode<Component> staleNode ) where T : Component
		{
			if ( staleNode == null ) { return; }
			
			if ( m_pooledObjectsByType.TryGetValue( typeof ( T ), out LinkedList<Component> result ) )
			{
				result.Remove( staleNode );
				return;
			}
		}
	}
}