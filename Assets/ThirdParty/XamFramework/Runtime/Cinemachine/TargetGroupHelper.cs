using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Xam.Cinemachine
{
	using Utility.Patterns;

	public class TargetGroupHelper : SingletonMono<TargetGroupHelper>
	{
		private CinemachineTargetGroup m_targetGroup = null;

		public void AddToTargetGroup( Transform target, float weight, float radius )
		{
			m_targetGroup.AddMember( target, weight, radius );
		}

		public void RemoveFromTargetGroup( Transform target )
		{
			m_targetGroup.RemoveMember( target );
		}

		protected override void Awake()
		{
			base.Awake();

			m_targetGroup = GetComponent<CinemachineTargetGroup>();
		}
	}
}