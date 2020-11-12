using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Audio
{
	[System.Serializable]
	public class SoundDatum
	{
		public bool Is3d { get { return Attachment3d != null ? Attachment3d.Is3d : false; } }
		public float RandomPitch { get { return Random.Range( MinPitch, MaxPitch ); } }

		public AudioClip Clip = default;

		[Space]
		public bool IsLooping = false;

		[Space]
		[Range( 0, 1 )] public float Volume = 1;

		[Space]
		public float MinPitch = 0.95f;
		public float MaxPitch = 1.05f;

		[Space]
		public SoundAttachmentDatum Attachment3d = default;

		public bool IsValid()
		{
			return Clip != null;
		}

		public bool CanAttach()
		{
			return Is3d && Attachment3d.Attachment != null;
		}
	}

	[System.Serializable]
	public class SoundAttachmentDatum
	{
		public bool Is3d = true;
		public Transform Attachment = default;

		[Space]
		[Range( 0, 5 )] public float DopplerLevel = 1;
		[Range( 0, 360 )] public float Spread = 5;
		public AudioRolloffMode VolumeRolloff = AudioRolloffMode.Logarithmic;
		public float MinDistance = 1;
		public float MaxDistance = 10;
	}
}