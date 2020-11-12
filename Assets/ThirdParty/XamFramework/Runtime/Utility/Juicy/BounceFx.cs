using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Utility.Juicy
{
	public class BounceFx : MonoBehaviour
	{
		[Header( "Modifiers" )]
		[SerializeField] private AnimationCurve m_bounceCurve = AnimationCurve.Linear( 0, 0, 1, 1 );

		[Header( "References" )]
		[SerializeField] private Transform m_target = default;

		private Coroutine m_bounceRoutine = null;

		public void Play( float timespan, float scalar = 1 )
		{
			if ( timespan <= 0 ) { return; }

			Stop();
			m_bounceRoutine = StartCoroutine( Bounce_Coroutine( timespan, scalar ) );
		}

		public void Stop()
		{
			if ( m_bounceRoutine != null )
			{
				StopCoroutine( m_bounceRoutine );
				m_bounceRoutine = null;
			}
		}

		private IEnumerator Bounce_Coroutine( float timespan, float scalar )
		{
			float timer = 0;
			while ( timer < 1 )
			{
				timer += Time.deltaTime / timespan;
				float bounce = m_bounceCurve.Evaluate( timer );

				m_target.localScale = Vector2.one + (Vector2.one * bounce * scalar);

				yield return null;
			}

			m_target.localScale = Vector2.one;
			m_bounceRoutine = null;
		}
	}
}