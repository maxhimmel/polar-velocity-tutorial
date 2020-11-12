using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Utility
{
	public abstract class Emancipator : MonoBehaviour
	{
		protected Coroutine m_lifetimeRoutine = null;
		
		public static void EmancipateAll( Emancipator[] emancipations )
		{
			foreach ( Emancipator child in emancipations )
			{
				child.Emancipate();
			}
		}

		public void Emancipate()
		{
			transform.SetParent( null );

			OnEmancipated();
			StartWaitingForExpiration();
		}

		protected abstract void OnEmancipated();

		private void StartWaitingForExpiration()
		{
			StopTrackingExpiration();
			m_lifetimeRoutine = StartCoroutine( WaitForExpiration_Coroutine() );
		}

		private void StopTrackingExpiration()
		{
			if ( m_lifetimeRoutine != null )
			{
				StopCoroutine( m_lifetimeRoutine );
				m_lifetimeRoutine = null;
			}
		}

		private IEnumerator WaitForExpiration_Coroutine()
		{
			while ( IsAlive() )
			{
				UpdateExpirationTick();
				yield return null;
			}

			Destroy( gameObject );
			m_lifetimeRoutine = null;
		}

		protected abstract bool IsAlive();

		protected virtual void UpdateExpirationTick()
		{
		}
	}
}