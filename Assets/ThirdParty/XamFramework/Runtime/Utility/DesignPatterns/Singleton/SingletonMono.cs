using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Utility.Patterns
{
	public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
	{
		public static bool Exists { get { return m_instance != null; } }
		public static T Instance
		{
			get
			{
				if ( m_applicationQuitting ) { return null; }

				if ( m_instance == null )
				{
					m_instance = FindObjectOfType<T>();

					// Still null ...
					if ( m_instance == null )
					{
						GameObject newObj = new GameObject( typeof(T).Name );
						m_instance = newObj.AddComponent<T>();
					}
				}

				return m_instance;
			}
		}

		private static T m_instance = null;
		private static bool m_applicationQuitting = false;

		[Header( "Singleton" )]
		[SerializeField] private bool m_destroyOnLoad = false;
		
		protected virtual void Awake()
		{
			if ( m_instance == null )
			{
				m_instance = GetComponent<T>();
			}

			if ( Instance != this )
			{
				Destroy( this );
				return;
			}

			if ( !m_destroyOnLoad )
			{
				DontDestroyOnLoad( gameObject );
			}
		}

		protected virtual void OnApplicationQuit()
		{
			m_instance = null;
			m_applicationQuitting = true;
		}
	}
}