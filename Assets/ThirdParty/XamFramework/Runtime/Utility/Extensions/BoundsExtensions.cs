using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Utility.Extensions
{
	public static class BoundsExtensions
	{
		public static Vector3 GetRandomPoint( this Bounds bounds )
		{
			Vector3 min = bounds.min;
			Vector3 max = bounds.max;

			return new Vector3()
			{
				x = Random.Range( min.x, max.x ),
				y = Random.Range( min.y, max.y ),
				z = Random.Range( min.z, max.z ),
			};
		}

		public static Vector3 GetRandomPoint( this Collider[] colliders )
		{
			int randColliderIdx = Random.Range( 0, colliders.Length );
			Collider randCollider = colliders[randColliderIdx];

			return randCollider.bounds.GetRandomPoint();
		}

		public static Vector3 GetRandomPoint( this Renderer[] renderers )
		{
			int randColliderIdx = Random.Range( 0, renderers.Length );
			Renderer randRenderer = renderers[randColliderIdx];

			return randRenderer.bounds.GetRandomPoint();
		}
	}
}