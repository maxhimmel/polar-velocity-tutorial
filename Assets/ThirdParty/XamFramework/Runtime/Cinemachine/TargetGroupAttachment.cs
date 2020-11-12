using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Xam.Cinemachine
{
	public class TargetGroupAttachment : MonoBehaviour
	{
		[SerializeField] private float m_weight = 1;
		[SerializeField] private float m_radius = 1;

		private void Start()
		{
			TargetGroupHelper.Instance.AddToTargetGroup( transform, m_weight, m_radius );
		}

		private void OnDestroy()
		{
			if ( TargetGroupHelper.Exists )
			{
				TargetGroupHelper.Instance.RemoveFromTargetGroup( transform );
			}
		}
	}
}