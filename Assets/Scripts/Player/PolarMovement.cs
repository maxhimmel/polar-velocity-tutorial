using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moonshot.Gameplay
{
	using ReConsts;

	public class PolarMovement : MonoBehaviour
	{
		[Header( "Gravity" )]
		[SerializeField] private float m_gravityForce = 9.81f;
		[SerializeField] private Rigidbody2D m_orbitTarget = default;

		[Header( "Movement" )]
		[SerializeField] private float m_acceleration = 10;
		[SerializeField] private float m_maxSpeed = 5;
		[SerializeField] private float m_jumpHeight = 3.25f;

		private Rewired.Player m_input = null;

		private Rigidbody2D m_rigidbody = null;
		private Vector2 m_localVelocity = Vector2.zero;
		private Vector2 m_desiredVelocity = Vector2.zero;
		private bool m_isJumpDesired = false;

		private void Update()
		{
			Vector2 moveInput = GetMoveInput();

			Debug.DrawRay( m_rigidbody.position, moveInput, Color.magenta, 0.1f );

			SetDesiredVelocity( moveInput );

			if ( m_input.GetButtonDown( Action.Jump ) )
			{
				TryJump();
			}
		}

		private Vector2 GetMoveInput()
		{
			Vector2 rawInput = m_input.GetAxis2D( Action.MoveHorizontal, Action.MoveVertical );
			Vector2 moveInput = Vector2.ClampMagnitude( rawInput, 1 );

			return Vector3.ProjectOnPlane( moveInput, GetGravityNormal() );
		}

		private Vector2 GetGravityNormal()
		{
			return (m_rigidbody.position - m_orbitTarget.position).normalized;
		}

		private void SetDesiredVelocity( Vector2 moveDirection )
		{
			m_desiredVelocity = moveDirection * m_maxSpeed;
		}

		private bool TryJump()
		{
			m_isJumpDesired = true;
			return true;
		}

		private void FixedUpdate()
		{
			// UPDATE STATE ...
			m_localVelocity = m_rigidbody.velocity - m_orbitTarget.velocity;

			Vector2 gravityNormal = GetGravityNormal();
			Vector2 gravityTangent = Quaternion.Euler( 0, 0, -90 ) * gravityNormal;

			Debug.DrawRay( m_rigidbody.position, gravityNormal, Color.blue, 0.1f );
			Debug.DrawRay( m_rigidbody.position, gravityTangent, Color.green, 0.1f );

			// ACCELERATE ...
			float speedDelta = m_acceleration * Time.deltaTime;

			//Vector2 relativeVelocity = m_velocity - m_orbitTarget.velocity;

			//float angularSpeed = Vector2.Dot( relativeVelocity, gravityTangent );
			float angularSpeed = Vector2.Dot( m_localVelocity, gravityTangent );
			float targetAngularSpeed = Vector2.Dot( m_desiredVelocity, gravityTangent );
			float newLinearSpeed = Mathf.MoveTowards( angularSpeed, targetAngularSpeed, speedDelta );


			m_localVelocity += gravityTangent * (newLinearSpeed - angularSpeed);


			//float fallSpeed = Mathf.Max( 0, Vector2.Dot( m_localVelocity, gravityNormal ) );
			//float orbitFallSpeed = Vector2.Dot( m_orbitTarget.velocity, gravityNormal );
			//float newFallSpeed = Mathf.MoveTowards( fallSpeed, orbitFallSpeed, m_gravityForce * Time.deltaTime );

			////m_localVelocity -= gravityNormal * fallSpeed;
			//m_localVelocity += gravityNormal * (newFallSpeed - fallSpeed);


			//LOGGING();


			// JUMP ...
			if ( m_isJumpDesired )
			{
				Jump();
			}

			// GRAVITY ...
			//m_velocity.y -= m_gravityForce * Time.deltaTime;
			m_localVelocity -= gravityNormal * m_gravityForce * Time.deltaTime;


			// APPLY MOVEMENT ...
			m_rigidbody.velocity = m_localVelocity + m_orbitTarget.velocity;



			m_relativeVelocity = m_rigidbody.velocity - m_orbitTarget.velocity;
		}

		private Vector2 m_relativeVelocity = Vector2.zero;

		private void LOGGING()
		{
			//Vector2 gravityNormal = GetGravityNormal();
			//Vector2 gravityTangent = Quaternion.Euler( 0, 0, -90 ) * gravityNormal;
			//Debug.Log(
			//	$"Linear Speed: {Vector2.Dot( m_velocity, gravityTangent )}\n" +
			//	$"Gravity Speed: {Vector2.Dot( m_velocity, gravityNormal )}"
			//);


			float radius = (m_rigidbody.position - m_orbitTarget.position).magnitude;
			float theta = Mathf.Atan2( m_rigidbody.position.y, m_rigidbody.position.x ) * Mathf.Rad2Deg;
			Debug.Log( $"(r {radius}, t {theta})" );


			Debug.DrawLine( m_rigidbody.position, m_orbitTarget.position, Color.blue, 0.3f );
		}

		private void Jump()
		{
			m_isJumpDesired = false;
			m_localVelocity += GetGravityNormal() * GetJumpForce();
		}

		private float GetJumpForce()
		{
			float gravForce = m_gravityForce * m_rigidbody.gravityScale;
			return Mathf.Sqrt( 2.0f * gravForce * m_jumpHeight );
		}

		private void Start()
		{
			m_input = Rewired.ReInput.players.GetPlayer( 0 );
		}

		private void Awake()
		{
			m_rigidbody = GetComponent<Rigidbody2D>();
		}
	}
}