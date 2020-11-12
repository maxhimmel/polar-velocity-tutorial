using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Xam.Ui
{
	public class RotatingAnimElement : AnimEventElement
	{
		[SerializeField] private float m_maxAngle = 15;
		[SerializeField] private float m_maxSpeed = 10;
		
		private Coroutine m_rotateRoutine = null;

		protected override void OnElementEnabled( BaseEventData data )
		{
			base.OnElementEnabled( data );

			StopRotating();
			m_rotateRoutine = StartCoroutine( UpdateRotation_Coroutine( m_maxAngle, m_maxSpeed ) );
		}

		private IEnumerator UpdateRotation_Coroutine( float maxAngle, float maxSpeed )
		{
			float counter = 0.5f;

			while ( IsAnimating )
			{
				counter += DeltaTime * maxSpeed;
				float pingPong = Mathf.PingPong( counter, 1 );

				float newAngle = Mathf.Lerp( -maxAngle, maxAngle, pingPong );
				transform.rotation = Quaternion.Euler( 0, 0, newAngle );

				yield return null;
			}
		}

		protected override void OnElementDisabled( BaseEventData data )
		{
			base.OnElementDisabled( data );

			StopRotating();
		}

		private void OnDisable()
		{
			StopRotating();
		}

		private void StopRotating()
		{
			if ( m_rotateRoutine != null )
			{
				StopCoroutine( m_rotateRoutine );
				m_rotateRoutine = null;
			}

			transform.rotation = Quaternion.identity;
		}
	}
}