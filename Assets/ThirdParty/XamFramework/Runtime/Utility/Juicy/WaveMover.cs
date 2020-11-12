using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Utility.Juicy
{
	public class WaveMover : MonoBehaviour
	{
		[System.Serializable]
		public class WaveData
		{
			public bool isSine = true;
			public float Amplitude = 1;
			public float Frequency = 1;
			public float Phase = 0;

			private float m_timer = 0;

			public float Calculate() //System.Func<float, float> waveFunction )
			{
				//if ( waveFunction == null ) { return 0; }

				m_timer += Time.deltaTime;
				return Amplitude * (isSine//waveFunction( m_timer * Frequency + Phase );
					? Mathf.Sin( m_timer * Frequency + Phase )
					: Mathf.Cos( m_timer * Frequency + Phase )
				);
			}
		}

		[Header( "Modifiers" )]
		//[SerializeField, EnumFlag] private EOrientationMode m_updateMode = EOrientationMode.Everything;
		[SerializeField] private bool m_updatePosition = true;
		[SerializeField] private bool m_updateRotation = true;

		[SerializeField] private WaveData m_sinData = new WaveData();
		[SerializeField] private WaveData m_cosData = new WaveData();

		[Space]
		[SerializeField] private float m_angle = 30;
		[SerializeField] private float m_rotateSpeed = 1;
		[SerializeField] private float m_rotateDamping = 0.6f;

		[Header( "References" )]
		[SerializeField] private Transform m_target = default;

		private Vector3 m_initialPosition = Vector2.zero;
		private float m_rotateTimer = 0;
		private float m_rotateVelocity = 0;

		public void StopRotating()
		{
			//m_updateMode &= ~EOrientationMode.Rotation;
			//Debug.Log( $"WaveMover::StopRotate" );

			m_updateRotation = false;
		}

		public void StopPositioning()
		{
			//m_updateMode &= ~EOrientationMode.Position;
			//Debug.Log( $"WaveMover::StopPosition" );

			m_updatePosition = false;
		}

		public void StopScaling()
		{
			//m_updateMode &= ~EOrientationMode.Scale;
		}

		private void FixedUpdate()
		{
			// Update wave position ...
			//if ( (int)(m_updateMode & EOrientationMode.Position) >= 1 )
			if ( m_updatePosition )
			{
				float sin = m_sinData.Calculate();//( Mathf.Sin );
				float cos = m_cosData.Calculate();//( Mathf.Cos );

				Vector3 wave = new Vector3( cos, 0, sin );
				m_target.localPosition = m_initialPosition + wave;

				//Debug.Log( $"WaveMover::Update::Sin [{sin}]  |  Cos [{cos}]" );
			}

			// Update rotation ...
			//if ( (int)(m_updateMode & EOrientationMode.Rotation) >= 1 )
			if ( m_updateRotation )
			{
				m_rotateTimer += Time.deltaTime * m_rotateSpeed;
				float lerpValue = Mathf.PingPong( m_rotateTimer, 1 );
				float newAngle = Mathf.Lerp( -m_angle, m_angle, lerpValue );
				newAngle = Mathf.SmoothDampAngle( transform.localEulerAngles.z, newAngle, ref m_rotateVelocity, m_rotateDamping );

				//Debug.Log( $"WaveMover::Update::Angle [{newAngle}]  |  Timer [{m_rotateTimer}]" );

				transform.localEulerAngles = Vector3.forward * newAngle;
			}


			//Rigidbody2D body = GetComponent<Rigidbody2D>();
			//Debug.Log( $"WaveMover::Rigidbody2D::Kinematic [{body.isKinematic}]  |  Pos [{transform.position}]" );
		}

		private void Awake()
		{
			m_initialPosition = transform.localPosition;
		}
	}
}