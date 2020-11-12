using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Gameplay
{
	public class ProximityBucket2D<T> : MonoBehaviour
		where T : Component
	{
		public IEnumerable<T> Targets { get { return m_targets; } }
		public bool HasAvailableTargets { get { return Count > 0; } }
		public int Count { get { return m_targets.Count; } }

		public event System.Action<T> OnTargetEnterEvent;
		public event System.Action<T> OnTargetExitEvent;

		[SerializeField] private LayerMask m_gatherMask = -1;
		[SerializeField] private ContactFilter2D m_contactFilter = default;

		private List<T> m_targets = new List<T>();
		private Collider2D m_collider = null;
		private List<Collider2D> m_forceCheckedColliders = new List<Collider2D>();

		public void ClearBucket()
		{
			m_targets.Clear();
		}

		public void CheckProximity()
		{
			int numOverlaps = m_collider.OverlapCollider( m_contactFilter, m_forceCheckedColliders );
			for ( int idx = 0; idx < numOverlaps; ++idx )
			{
				Collider2D overlap = m_forceCheckedColliders[idx];
				OnTriggerEnter2D( overlap );
			}
		}

		private void OnTriggerEnter2D( Collider2D other )
		{
			if ( CanBeGathered( other ) )
			{
				T target = other.attachedRigidbody?.GetComponentInParent<T>();
				if ( IsInsideBucket( target ) ) { return; }

				AddTarget( target );
				OnTargetEnterEvent?.Invoke( target );
			}
		}

		private void OnTriggerExit2D( Collider2D other )
		{
			if ( CanBeGathered( other ) )
			{
				T target = other.attachedRigidbody?.GetComponentInParent<T>();
				if ( !IsInsideBucket( target ) ) { return; }
				
				RemoveTarget( target );
				OnTargetExitEvent?.Invoke( target );
			}
		}

		private bool CanBeGathered( Collider2D collider )
		{
			if ( !m_contactFilter.useTriggers && collider.isTrigger ) { return false; }

			int otherLayer = 1 << collider.gameObject.layer;
			return (otherLayer & m_gatherMask) != 0;
		}

		private bool IsInsideBucket( T other )
		{
			return m_targets.Contains( other );
		}

		private void AddTarget( T target )
		{
			m_targets.Add( target );
		}

		private void RemoveTarget( T target )
		{
			m_targets.Remove( target );
		}

		private void Awake()
		{
			m_collider = GetComponentInChildren<Collider2D>();
		}
	}
}