using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Utility
{
	public abstract class Interpolator<T> : MonoBehaviour
	{
		public bool IsRunning { get { return m_interpRoutine != null; } }

		[SerializeField] protected T m_initialValue = default;
		[SerializeField] protected float m_lerpDuration = 1;

		protected Coroutine m_interpRoutine = null;

		public virtual void Init( T initialValue )
		{
			m_initialValue = initialValue;
		}

		public virtual void StartLerp( T goal, float duration = -1 )
		{
			StopLerp();

			if ( duration < 0 )
			{
				duration = m_lerpDuration;
			}

			bool isInstantaneous = duration <= 0;
			if ( isInstantaneous )
			{
				OnInstantaneousLerp( goal );
				return;
			}

			OnPreLerp( goal, duration );

			m_interpRoutine = StartCoroutine( Lerp_Coroutine( goal, duration ) );
		}

		public virtual void StopLerp()
		{
			if ( m_interpRoutine != null )
			{
				StopCoroutine( m_interpRoutine );
				m_interpRoutine = null;
			}
		}

		/// <summary>
		/// Called when the lerping duration is equal to zero.
		/// </summary>
		/// <param name="goal"></param>
		protected abstract void OnInstantaneousLerp( T goal );

		/// <summary>
		/// Called just before entering the interpolation loop.
		/// </summary>
		/// <param name="goal"></param>
		/// <param name="duration"></param>
		protected abstract void OnPreLerp( T goal, float duration );

		/// <summary>
		/// Called every frame while <paramref name="time"/> is less than zero.
		/// </summary>
		/// <param name="time"></param>
		/// <param name="goal"></param>
		protected abstract void Interpolate( float time, T goal );

		private IEnumerator Lerp_Coroutine( T goal, float duration )
		{
			float timer = 0;
			while ( timer < 1 )
			{
				timer += Time.deltaTime / duration;

				Interpolate( timer, goal );
				yield return null;
			}

			m_interpRoutine = null;
		}
	}
}