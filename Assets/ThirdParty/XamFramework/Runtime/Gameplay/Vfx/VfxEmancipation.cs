using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Gameplay.Vfx
{
	using Utility;

	[RequireComponent( typeof( ParticleSystem ) )]
	public class VfxEmancipation : Emancipator
	{
		public enum EEmancipationAction
		{
			None,

			Play, Stop
		}

		[SerializeField] private EEmancipationAction m_action = EEmancipationAction.None;

		private ParticleSystem m_vfx = null;
		
		protected override void OnEmancipated()
		{
			PerformAction( m_action, m_vfx );
		}

		private void PerformAction( EEmancipationAction action, ParticleSystem vfx )
		{
			switch ( action )
			{
				default:
				case EEmancipationAction.None: break;

				case EEmancipationAction.Play:
					vfx.Play();
					break;

				case EEmancipationAction.Stop:
					vfx.Stop();
					break;
			}
		}

		protected override bool IsAlive()
		{
			return m_vfx != null && m_vfx.IsAlive();
		}

		private void Awake()
		{
			m_vfx = GetComponent<ParticleSystem>();
		}
	}
}