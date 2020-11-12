using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Gameplay
{
	/// <summary>
	/// Gathers colliders whose layer matches the <see cref="m_gatherMask"/>.
	/// </summary>
	public class ProximityBucket : MonoBehaviour
	{
		public bool HasAvailableTargets { get { return m_targets.Count > 0; } }

		public event System.Action<ProximityBucket, Collider> OnTargetEnterEvent;
		public event System.Action<ProximityBucket, Collider> OnTargetExitEvent;

		[SerializeField] private LayerMask m_gatherMask = -1;

		private List<Collider> m_targets = new List<Collider>();

		private void OnTriggerEnter( Collider other )
		{
			if ( CanEnterBucket( other ) && !IsInsideBucket( other ) )
			{
				AddTarget( other );
				OnTargetEnterEvent?.Invoke( this, other );
			}
		}

		private void OnTriggerExit( Collider other )
		{
			if ( CanEnterBucket( other ) && IsInsideBucket( other ) )
			{
				RemoveTarget( other );
				OnTargetExitEvent?.Invoke( this, other );
			}
		}

		private bool CanEnterBucket( Collider collider )
		{
			int otherLayer = 1 << collider.gameObject.layer;
			return (otherLayer & m_gatherMask) != 0;
		}

		private bool IsInsideBucket( Collider other )
		{
			return m_targets.Contains( other );
		}

		private void AddTarget( Collider target )
		{
			m_targets.Add( target );
		}

		private void RemoveTarget( Collider target )
		{
			m_targets.Remove( target );
		}
	}
}