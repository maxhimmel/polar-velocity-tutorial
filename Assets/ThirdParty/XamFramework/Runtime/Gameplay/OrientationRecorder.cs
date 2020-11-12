using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Gameplay
{
	public class OrientationRecorder : MonoBehaviour
	{
		public Vector3 RecordedPosition { get; private set; }
		public Quaternion RecordedRotation { get; private set; }

		/// <summary>
		/// Moves and rotates this gameobject to its recorded position and rotation.
		/// </summary>
		public void ResetOrientation()
		{
			transform.SetPositionAndRotation( RecordedPosition, RecordedRotation );
		}

		public void RecordOrientation()
		{
			RecordedPosition = transform.position;
			RecordedRotation = transform.rotation;
		}

		private void Awake()
		{
			RecordOrientation();
		}
	}
}