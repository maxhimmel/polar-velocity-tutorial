using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Initialization
{
	public class TransitionController : Utility.Patterns.SingletonMono<TransitionController>
	{
		private ITransition m_transition = null;

		public void Open( float duration )
		{
			m_transition.Open( duration );
		}

		public void Close( float duration )
		{
			m_transition.Close( duration );
		}

		protected override void Awake()
		{
			m_transition = GetComponentInChildren<ITransition>();

			base.Awake();
		}
	}
}