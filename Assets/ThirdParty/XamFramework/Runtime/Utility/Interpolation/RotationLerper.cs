using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Utility
{
	public class RotationLerper : Interpolator<Transform>
	{
		private Quaternion m_startRot = Quaternion.identity;
		private Quaternion m_goalRot = Quaternion.identity;

		public void StartLerp( Quaternion goal, float duration = -1 )
		{
			m_goalRot = goal;

			base.StartLerp( null, duration );
		}

		private Dictionary<ERotationAxis, Vector3> m_axis = new Dictionary<ERotationAxis, Vector3>()
		{
			{ ERotationAxis.None, Vector3.zero },
			{ ERotationAxis.Pitch, Vector3.right },
			{ ERotationAxis.Yaw, Vector3.up },
			{ ERotationAxis.Roll, Vector3.forward },
		};

		protected override void OnInstantaneousLerp( Transform goal )
		{
			m_initialValue.rotation = goal.rotation;
		}

		protected override void OnPreLerp( Transform goal, float duration )
		{
			m_startRot = m_initialValue.rotation;
		}

		protected override void Interpolate( float time, Transform goal )
		{
			Quaternion newRot = Quaternion.Slerp( m_startRot, m_goalRot, time );
			m_initialValue.rotation = newRot;
		}

		private Vector3 GetAxis( ERotationAxis axis )
		{
			return m_axis[axis];
		}
	}
}