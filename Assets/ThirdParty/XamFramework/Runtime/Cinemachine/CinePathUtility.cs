using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Xam.Cinemachine
{
	public static class CinePathUtility
	{
		private const int k_vertsPerQuad = 4;
		private const int k_vertsPerTri = 3;
		private const int k_trisPerQuad = 2;

		public static int GetClosestPoint( this CinemachinePathBase path, Vector3 samplePosition )
		{
			float closestDistSqr = Mathf.Infinity;
			int closestPos = -1;

			for ( int pos = (int)path.MinPos; pos < path.MaxPos; ++pos )
			{
				Vector3 worldPos = path.EvaluatePosition( pos );
				float distSqr = (worldPos - samplePosition).sqrMagnitude;

				if ( distSqr < closestDistSqr )
				{
					closestDistSqr = distSqr;
					closestPos = pos;
				}
			}

			return closestPos;
		}

		public static void PopulateLineRenderPoints( this CinemachinePathBase path, LineRenderer renderer )
		{
			float maxStepLength = 1f / path.m_Resolution;
			int maxPoints = Mathf.CeilToInt( path.MaxPos / maxStepLength ) + 1;

			Vector3[] worldPoints = new Vector3[maxPoints];

			int idx = 0;
			for ( float tdx = 0; tdx < path.MaxPos; tdx += maxStepLength )
			{
				Vector3 worldPos = path.EvaluatePosition( tdx );
				worldPoints[idx++] = worldPos;
			}

			renderer.positionCount = maxPoints;
			renderer.SetPositions( worldPoints );

			renderer.useWorldSpace = true;
			renderer.loop = path.Looped;
		}

		public static MeshCollider GeneratePlanarCollision( this CinemachinePathBase path )
		{
			float halfWidth = path.m_Appearance.width / 2f;
			float maxStepLength = 1f / path.m_Resolution;
			int maxPoints = Mathf.CeilToInt( path.MaxPos / maxStepLength ) * k_vertsPerQuad;
			int maxTris = maxPoints / k_vertsPerQuad * (k_vertsPerTri * k_trisPerQuad);


			List<Vector3> vertices = new List<Vector3>( maxPoints );
			List<Vector3> normals = new List<Vector3>( maxPoints );
			List<int> triangles = new List<int>( maxTris );

			
			PathMeshNode prevNode = new PathMeshNode( path, path.MinPos );

			for ( float tdx = path.MinPos + maxStepLength; tdx < path.MaxPos; tdx += maxStepLength )
			{
				PathMeshNode nextNode = new PathMeshNode( path, tdx );
				{
					vertices.Add( prevNode.LeftPosition );
					vertices.Add( nextNode.RightPosition );
					vertices.Add( prevNode.RightPosition );
					vertices.Add( nextNode.LeftPosition );


					triangles.Add( vertices.Count - 4 );
					triangles.Add( vertices.Count - 3 );
					triangles.Add( vertices.Count - 2 );

					triangles.Add( vertices.Count - 4 );
					triangles.Add( vertices.Count - 1 );
					triangles.Add( vertices.Count - 3 );


					normals.Add( prevNode.Normal );
					normals.Add( nextNode.Normal );
					normals.Add( prevNode.Normal );
					normals.Add( nextNode.Normal );
				}
				prevNode = nextNode;
			}

			GameObject collisionObj = new GameObject( $"Collision_{path.name}" );
			collisionObj.transform.SetParent( path.transform, false );
			collisionObj.gameObject.layer = path.gameObject.layer;

			MeshCollider collider = collisionObj.AddComponent<MeshCollider>();
			collider.sharedMesh = new Mesh()
			{
				name = path.name,

				vertices = vertices.ToArray(),
				normals = normals.ToArray(),
				triangles = triangles.ToArray(),
			};

			return collider;
		}

		private struct PathMeshNode
		{
			public Vector3 Normal { get; }
			public Vector3 LeftPosition { get; }
			public Vector3 RightPosition { get; }

			public PathMeshNode( CinemachinePathBase path, float time )
			{
				Vector3 center = path.EvaluatePosition( time );
				Quaternion orientation = path.EvaluateOrientation( time );

				Normal = orientation * Vector3.up;

				float halfWidth = path.m_Appearance.width / 2f;
				Vector3 originOffset = path.transform.position;

				RightPosition = center + orientation * Vector3.right * halfWidth;
				RightPosition -= originOffset;

				LeftPosition = center + orientation * Vector3.left * halfWidth;
				LeftPosition -= originOffset;
			}
		}
	}
}