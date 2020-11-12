using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Xam.Audio
{
	public class AudioManager : Utility.Patterns.SingletonMono<AudioManager>
	{
		private const string k_sfxMixerName = "Sfx";
		private const string k_musicMixerName = "Music";

		[Header( "References" )]
		[SerializeField] private AudioMixer m_mainMixer = default;
		
		private AudioMixerGroup m_sfxMixerGroup = null;
		private AudioMixerGroup m_musicMixerGroup = null;
		private LinkedList<AudioSource> m_sources = new LinkedList<AudioSource>();

		public void PlaySound( SoundDatum data )
		{
			if ( data == null || !data.IsValid() ) { return; }

			AudioSource source = GetAudioSource();
			SetupAudioSource( source, data );

			source.Play();

			if ( data.Is3d )
			{
				StartCoroutine( Track3dAttachment_Coroutine( source, data.Attachment3d.Attachment ) );
			}
			else
			{
				StartCoroutine( TrackSoundCompletion_Coroutine( source ) );
			}
		}

		private AudioSource GetAudioSource()
		{
			if ( m_sources.Count <= 0 )
			{
				GameObject newObj = new GameObject();
				AudioSource source = newObj.AddComponent<AudioSource>();

				return source;
			}

			AudioSource firstSource = m_sources.First.Value;
			m_sources.RemoveFirst();

			firstSource.gameObject.SetActive( true );

			return firstSource;
		}

		private void SetupAudioSource( AudioSource source, SoundDatum data )
		{
			source.name = $"SFX_{data.Clip.name}";

			source.playOnAwake = false;
			source.clip = data.Clip;
			source.loop = data.IsLooping;
			source.volume = data.Volume;
			source.pitch = data.RandomPitch;
			source.spatialBlend = 0;

			if ( data.Is3d )
			{
				source.spatialBlend = 1;
				source.dopplerLevel = data.Attachment3d.DopplerLevel;
				source.spread = data.Attachment3d.Spread;
				source.rolloffMode = data.Attachment3d.VolumeRolloff;
				source.minDistance = data.Attachment3d.MinDistance;
				source.maxDistance = data.Attachment3d.MaxDistance;

				if ( data.CanAttach() )
				{
					source.transform.SetParent( null );
					source.transform.position = data.Attachment3d.Attachment.position;
				}
			}

			source.outputAudioMixerGroup = m_sfxMixerGroup;
		}

		private IEnumerator Track3dAttachment_Coroutine( AudioSource source, Transform attachment )
		{
			while ( source.isPlaying )
			{
				if ( attachment != null )
				{
					source.transform.position = attachment.position;
				}

				yield return null;
			}

			ReturnSourceToPool( source );
		}

		private IEnumerator TrackSoundCompletion_Coroutine( AudioSource source )
		{
			while ( source.isPlaying ) { yield return null; }

			ReturnSourceToPool( source );
		}

		private void ReturnSourceToPool( AudioSource source )
		{
			source.Stop();
			source.gameObject.SetActive( false );

			if ( source.transform.parent != transform )
			{
				source.gameObject.transform.SetParent( transform, false );
			}

			m_sources.AddLast( source );
		}

		protected override void Awake()
		{
			base.Awake();

			m_sfxMixerGroup = m_mainMixer.FindMatchingGroups( k_sfxMixerName )[0];
			m_musicMixerGroup = m_mainMixer.FindMatchingGroups( k_musicMixerName )[0];
		}
	}
}