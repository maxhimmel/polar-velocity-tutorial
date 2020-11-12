using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Utility
{
	public class PositionLerper : Interpolator<Transform>
	{
		private Vector3 m_startPos = Vector3.zero;
		private Vector3 m_goalPos = Vector3.zero;

		protected override void OnInstantaneousLerp( Transform goal )
		{
			m_initialValue.position = goal.position;
		}

		protected override void OnPreLerp( Transform goal, float duration )
		{
			m_startPos = m_initialValue.position;
			m_goalPos = goal.position;
		}

		protected override void Interpolate( float time, Transform goal )
		{
			Vector3 newPos = Vector3.Lerp(m_startPos, m_goalPos, time);
			m_initialValue.position = newPos;
		}
	}
}