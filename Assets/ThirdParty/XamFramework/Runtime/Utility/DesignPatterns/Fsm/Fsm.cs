using System;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Utility.Fsm
{
	public class Fsm<StateKey>
	{
		#region Properties
		public bool Enable { get; set; }
		public StateKey QueuedStateId { get { return GetStateId( m_queuedState ); } }
		public StateKey CurrentStateId { get { return GetStateId( m_currentState ); } }
		public StateKey PreviousStateId { get { return GetStateId( m_previousState ); } }
		#endregion

		#region Private
		private Dictionary<StateKey, IFsmState<StateKey>> m_registeredStates;
		private IFsmState<StateKey> m_queuedState;
		private IFsmState<StateKey> m_currentState;
		private IFsmState<StateKey> m_previousState;
		private object m_context = null;
		private object m_owner = null;
		private bool m_forceStateChange = false;
		private bool m_wasShutdown = false;
		private StateKey m_emptyStateID;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates an FSM. Takes an Owner and a state that should be assumed to be the default "empty" state.
		/// </summary>
		/// <param name="Owner"></param>
		/// <param name="emptyState">Note: this is the state transitioned from the first time you queue a state, and transitioned to when you shut down.</param>
		public Fsm( object Owner, StateKey emptyState )
		{
			Enable = true;
			this.m_owner = Owner;
			m_queuedState = null;
			m_currentState = null;
			m_previousState = null;
			m_registeredStates = new Dictionary<StateKey, IFsmState<StateKey>>();
			m_emptyStateID = emptyState;
		}
		#endregion

		#region Updaters
		public void Update()
		{
			if ( !Enable || m_wasShutdown ) { return; }

			// Do transition to another state
			if (CanEnterQueuedState())
			{
				if ( m_currentState != null && m_queuedState != null ) { m_currentState.ExitState( this, QueuedStateId ); }

				m_previousState = m_currentState;   // Store prev state ...
				m_currentState = m_queuedState;     // Set to next state ...
				m_queuedState = null;               // Remove queued state ...

				m_currentState.EnterState( this, PreviousStateId, m_context );
			}
			// Clear queued states which cannot be entered ...
			else { m_queuedState = null; }

			// Update current state ...
			if ( m_currentState != null ) { m_currentState.UpdateState( this ); }
		}

		public void LateUpdate()
		{
			if ( !Enable || m_wasShutdown ) { return; }

			if ( m_currentState != null ) { m_currentState.LateUpdateState( this ); }
		}

		public void FixedUpdate()
		{
			if ( !Enable || m_wasShutdown ) { return; }

			if ( m_currentState != null ) { m_currentState.FixedUpdateState( this ); }
		}
		#endregion

		#region Shutdown
		public virtual void Shutdown()
		{
			if ( m_registeredStates == null || m_registeredStates.Count <= 0 ) { return; }

			if ( m_currentState != null ) { m_currentState.ExitState( this, m_emptyStateID ); }

			foreach ( KeyValuePair<StateKey, IFsmState<StateKey>> kvp in m_registeredStates )
			{
				IFsmState<StateKey> thisState = GetStateById( kvp.Key );
				if ( thisState == null ) { continue; }

				thisState.ShutdownState( this );
			}

			m_registeredStates.Clear();
			m_registeredStates = null;

			m_wasShutdown = true;
			Enable = false;
		}
		#endregion

		#region Queue States
		public bool QueueState( StateKey id )
		{
			return QueueState( id, false );
		}

		public bool QueueState( StateKey id, object context )
		{
			return QueueState( id, context, false );
		}

		public bool QueueState( StateKey id, object context, bool forceStateChange )
		{
			if ( m_wasShutdown ) { return false; }

			IFsmState<StateKey> queuedState = m_registeredStates[id];
			if ( !queuedState.CanEnterState( CurrentStateId ) ) { return false; }

			this.m_context = context;
			this.m_forceStateChange = forceStateChange;
			this.m_queuedState = queuedState;

			return true;
		}
		#endregion

		#region Get States    
		public StateKey GetQueuedState()
		{
			return GetStateId( m_queuedState );
		}

		public StateKey GetCurrentState()
		{
			return GetStateId( m_currentState );
		}

		public IFsmState<StateKey> GetQueuedStateInstance()
		{
			return m_queuedState;
		}

		public IFsmState<StateKey> GetCurrentStateInstance()
		{
			return m_currentState;
		}

		public StateKey GetStateId( IFsmState<StateKey> stateToFind )
		{
			if ( m_wasShutdown )
			{
				Debug.Log( "Unable to get state " + stateToFind + " on a machine that was previously shut down." );
				return m_emptyStateID;
			}

			if ( m_registeredStates == null ) { return m_emptyStateID; }

			foreach ( KeyValuePair<StateKey, IFsmState<StateKey>> kvp in m_registeredStates )
			{
				if ( kvp.Value == stateToFind ) { return kvp.Key; }
			}

			return m_emptyStateID;
		}

		public IFsmState<StateKey> GetStateById( StateKey id )
		{
			if ( m_wasShutdown )
			{
				Debug.Log( "Unable to get state " + id + " on a machine that was previously shut down." );
				return null;
			}

			IFsmState<StateKey> result = null;
			if ( m_registeredStates != null ) { m_registeredStates.TryGetValue( id, out result ); }

			return result;
		}
		#endregion

		#region Register State
		public void RegisterState( StateKey id, IFsmState<StateKey> state )
		{
			if ( state == null || m_wasShutdown ) { return; }

			m_registeredStates[id] = state;
			state.InitializeState( this );
		}
		#endregion

		#region Other Helper
		public bool IsCurrentOrQueuedState( StateKey id )
		{
			if ( GetCurrentState().Equals( id ) || GetQueuedState().Equals( id ) ) { return true; }

			return false;
		}

		public object GetOwnerObject()
		{
			return m_owner;
		}

		public string CurrentStateName()
		{
			if ( m_currentState == null ) { return null; }

			return m_currentState.GetType().FullName;
		}

		private bool CanEnterQueuedState()
		{
			if ( m_queuedState == null ) { return false; }

			bool isNewStateRequested = (m_queuedState != m_currentState);
			if ( !isNewStateRequested && !m_forceStateChange ) { return false; }

			return true;
		}
		#endregion
	}
}