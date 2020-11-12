using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Xam.Ui
{
	[RequireComponent( typeof( EventTrigger ) )]
	public class ScalingAnimElement : AnimEventElement
	{
		[Header( "Scaling" )]
		[SerializeField] private Vector3 m_maxLocalScale = Vector3.one * 1.25f;

		protected override void OnElementEnabled( BaseEventData data )
		{
			transform.localScale = m_maxLocalScale;
		}

		protected override void OnElementDisabled( BaseEventData data )
		{
			transform.localScale = Vector3.one;
		}
	}
}