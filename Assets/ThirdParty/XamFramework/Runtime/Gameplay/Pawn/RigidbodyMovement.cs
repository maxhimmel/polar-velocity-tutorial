using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Gameplay
{
	public class RigidbodyMovement : MonoBehaviour
	{
		public Vector3 Velocity { get { return m_moveBody.velocity; } }

		[Header( "Modifiers" )]
		[SerializeField] private ForceMode m_moveMode = ForceMode.Force;

		[Space]
		[SerializeField] private float m_maxMoveSpeed = 30;
		[SerializeField] private float m_initialMoveBurstSpeed = 10;
		[SerializeField] private float m_moveSmoothTime = 1;

		[Header( "References" )]
		[SerializeField] private Rigidbody m_moveBody = default;
		
		private float m_currentAcceleration = 0;
		private float m_moveAccelerationDampVelocity = 0;

		public void UpdateMovement( Vector3 moveDirection )
		{
			float inputScalar = moveDirection.magnitude;

			SetSmoothedAcceleration( inputScalar );

			if ( inputScalar > 0 )
			{
				SetAccelerationBurst();
			}

			ApplyMovementForce( moveDirection );
		}

		private void SetSmoothedAcceleration( float inputScalar )
		{
			float targetSpeed = inputScalar * m_maxMoveSpeed;
			m_currentAcceleration = Mathf.SmoothDamp( m_currentAcceleration, targetSpeed, ref m_moveAccelerationDampVelocity, m_moveSmoothTime );
		}

		private void SetAccelerationBurst()
		{
			m_currentAcceleration = Mathf.Max( m_currentAcceleration, m_initialMoveBurstSpeed );
		}

		private void ApplyMovementForce( Vector3 moveDirection )
		{
			Vector3 force = GetMoveForce( moveDirection );
			m_moveBody.AddForce( force, m_moveMode );
		}

		private Vector3 GetMoveForce( Vector3 moveDirection )
		{
			return moveDirection * m_currentAcceleration;
		}
	}
}