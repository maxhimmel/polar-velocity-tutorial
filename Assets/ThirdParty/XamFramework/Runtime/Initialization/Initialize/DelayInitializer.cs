using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Initialization
{
	public class DelayInitializer : MonoBehaviour, IInitialize
	{
		[SerializeField] private float m_delayDuration = 2;

		private float m_delayEndTime = 0;
		private bool m_initializationStarted = false;

		public void StartInitializing()
		{
			m_initializationStarted = true;
			m_delayEndTime = Time.timeSinceLevelLoad + m_delayDuration;
		}

		public bool IsInitializationComplete()
		{
			if ( !m_initializationStarted ) { return false; }

			return m_delayEndTime < Time.timeSinceLevelLoad;
		}
	}
}