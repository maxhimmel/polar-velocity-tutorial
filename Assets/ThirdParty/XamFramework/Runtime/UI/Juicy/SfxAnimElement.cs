using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Ui
{
	using Audio;
	using UnityEngine.EventSystems;

	public class SfxAnimElement : AnimEventElement
	{
		[Header( "SFX" )]
		[SerializeField] private SoundDatum m_enabledSfx = default;
		[SerializeField] private SoundDatum m_disabledSfx = default;

		protected override void OnElementEnabled( BaseEventData data )
		{
			base.OnElementEnabled( data );

			PlaySfx( m_enabledSfx );
		}

		protected override void OnElementDisabled( BaseEventData data )
		{
			base.OnElementDisabled( data );

			PlaySfx( m_disabledSfx );
		}

		private void PlaySfx( SoundDatum data )
		{
			AudioManager.Instance.PlaySound( data );
		}
	}
}