using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Utility.Extensions
{
	public static class CameraExtensions
	{
		/// <summary>
		/// Returned bounds is centered at <paramref name="camera"/>'s transform position.
		/// </summary>
		/// <param name="camera"></param>
		/// <returns></returns>
		public static Bounds GetOrthoWorldBounds( this Camera camera )
		{
			float orthoHeight = camera.orthographicSize * 2;
			float orthoWidth = camera.aspect * orthoHeight;

			Vector3 orthoWorldSize = new Vector3( orthoWidth, orthoHeight );
			return new Bounds( camera.transform.position, orthoWorldSize );
		}

		/// <summary>
		/// Returned bounds is centered [<paramref name="distance"/>] units along <paramref name="camera"/>'s forward direction.
		/// </summary>
		/// <param name="camera"></param>
		/// <returns></returns>
		public static Bounds GetFrustumWorldBounds( this Camera camera, float distance )
		{
			float frustumHeight = 2 * distance * Mathf.Tan( camera.fieldOfView * 0.5f * Mathf.Deg2Rad );
			float frustuumWidth = frustumHeight * camera.aspect;

			Vector3 center = camera.transform.position + camera.transform.forward * distance;
			return new Bounds( center, new Vector3( frustuumWidth, frustumHeight, 0 ) );
		}
	}
}