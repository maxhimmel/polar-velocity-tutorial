using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Utility
{
	public class KvpScriptableObjectLookup<K, V> : ScriptableObject where V : IKeyValuePair<K>
	{
		[SerializeField] protected V[] m_data = default;

		protected Dictionary<K, V> m_dataLookup = default;

		public virtual V GetDatum( K key )
		{
			V result = default;
			if ( !m_dataLookup.TryGetValue( key, out result ) )
			{
				Debug.LogError( $"Cannott find value with key '{key}'. Returning default.", this );
			}

			return result;
		}

		protected virtual void OnEnable()
		{
			if ( m_data == null || m_data.Length <= 0 ) { return; }

			m_dataLookup = new Dictionary<K, V>( m_data.Length );
			foreach ( V value in m_data )
			{
				K key = value.GetKey();
				if ( m_dataLookup.ContainsKey( key ) )
				{
					Debug.LogWarning( $"Key '{key}' already exists. Overwriting previous value.", this );
				}

				m_dataLookup[key] = value;
			}
		}
	}
}