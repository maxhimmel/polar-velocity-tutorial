using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Utility.Extensions
{
	public static class MonoExtenstions
	{
		public static Coroutine StartWaitingForSeconds( this MonoBehaviour mono, float seconds, System.Action callback )
		{
			return mono.StartCoroutine( WaitForSeconds_Coroutine( seconds, callback ) );
		}

		public static IEnumerator WaitForSeconds_Coroutine( float seconds, System.Action callback )
		{
			if ( seconds > 0 ) { yield return new WaitForSeconds( seconds ); }

			callback?.Invoke();
		}

		public static Coroutine StartWaitingForUnscaledSeconds( this MonoBehaviour mono, float seconds, System.Action callback )
		{
			return mono.StartCoroutine( WaitForUnscaledSeconds_Coroutine( seconds, callback ) );
		}

		public static IEnumerator WaitForUnscaledSeconds_Coroutine( float seconds, System.Action callback )
		{
			if ( seconds > 0 ) { yield return new WaitForSecondsRealtime( seconds ); }

			callback?.Invoke();
		}

		public static Coroutine StartWaitingForFrames( this MonoBehaviour mono, int frames, System.Action callback )
		{
			return mono.StartCoroutine( WaitForFrames_Coroutine( frames, callback ) );
		}

		public static IEnumerator WaitForFrames_Coroutine( int frames, System.Action callback )
		{
			while ( frames > 0 )
			{
				--frames;
				yield return null;
			}

			callback?.Invoke();
		}
	}
}