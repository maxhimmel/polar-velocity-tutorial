using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Audio
{
	public class SfxClip : MonoBehaviour
	{
		[SerializeField] private SoundDatum m_soundData = default;

		public virtual void PlaySfx()
		{
			AudioManager.Instance.PlaySound( m_soundData );
		}
	}
}