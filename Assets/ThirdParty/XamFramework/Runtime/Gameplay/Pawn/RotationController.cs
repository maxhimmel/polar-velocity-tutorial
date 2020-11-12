using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Gameplay
{
	public class RotationController : MonoBehaviour
	{
		public Quaternion CurrentRotation { get; private set; }

		public void SetRotation( Quaternion rotation )
		{
			CurrentRotation = rotation;
		}

		public void SetRotation( Vector3 forward, Vector3 upwards = default )
		{
			if ( upwards == default )
			{
				upwards = Vector3.up;
			}
			
			if ( !Mathf.Approximately( forward.magnitude, 0 ) )
			{
				Quaternion lookRot = Quaternion.LookRotation( forward, upwards );
				CurrentRotation = lookRot;
			}
		}

		private void LateUpdate()
		{
			transform.rotation = CurrentRotation;
		}
	}
}