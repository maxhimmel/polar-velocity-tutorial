using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Audio
{
	public class CompositeSfxClip : SfxClip
	{
		private List<SfxClip> m_childClips = new List<SfxClip>();

		public override void PlaySfx()
		{
			if ( m_childClips.Count <= 0 )
			{
				Debug.LogWarning( $"{name} cannot find any child SFX clips!", this );
				return;
			}

			int randIdx = Random.Range( 0, m_childClips.Count );
			SfxClip randSfx = m_childClips[randIdx];

			randSfx.PlaySfx();
		}

		private void Awake()
		{
			for ( int idx = 0; idx < transform.childCount; ++idx )
			{
				SfxClip sfxChild = transform.GetChild( idx ).GetComponent<SfxClip>();
				if ( sfxChild == null ) { continue; }

				m_childClips.Add( sfxChild );
			}
		}
	}
}