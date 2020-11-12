using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Initialization
{
	public class LevelInitializer : Utility.Patterns.SingletonMono<LevelInitializer>
	{
		public static bool IsInitialized { get { return Instance != null ? Instance.m_isInitialized : true; } }

		[Header( "References" )]
		[SerializeField] private GameManager m_gameManagerPrefab = default;

		[Header( "Modifiers" )]
		[SerializeField] private int m_initFrameDelay = 2;
		[SerializeField] private float m_fadeInDuration = 1.5f;

		private IInitialize m_initializer = null;
		private bool m_isInitialized = false;

		protected override void Awake()
		{
			base.Awake();

			m_initializer = GetComponentInChildren<IInitialize>();

			if ( !GameManager.Exists )
			{
				Instantiate( m_gameManagerPrefab );
			}

			Gameplay.TimeManager.Instance.ClearLevelTimer();
			Gameplay.TimeManager.Instance.SetTimeScale( 1, 0 );

			StartCoroutine( Initialize_Coroutine() );
		}

		private IEnumerator Initialize_Coroutine()
		{
			int frameDelay = m_initFrameDelay;
			while ( --frameDelay >= 0 ) { yield return null; }

			TransitionController.Instance.Open( m_fadeInDuration );

			m_isInitialized = false;
			{
				m_initializer.StartInitializing();
				while ( !m_initializer.IsInitializationComplete() ) { yield return null; }
			}
			m_isInitialized = true;
		}
	}
}