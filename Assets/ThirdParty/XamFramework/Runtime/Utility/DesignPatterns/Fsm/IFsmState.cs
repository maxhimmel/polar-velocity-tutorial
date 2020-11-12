namespace Xam.Utility.Fsm
{
	public interface IFsmState<StateKey>
	{
		void InitializeState( Fsm<StateKey> fsm );
		void ShutdownState( Fsm<StateKey> fsm );
		bool CanEnterState( StateKey currentState );
		void EnterState( Fsm<StateKey> fsm, StateKey prevState, object context = null );
		void ExitState( Fsm<StateKey> fsm, StateKey nextState );
		void UpdateState( Fsm<StateKey> fsm );
		void FixedUpdateState( Fsm<StateKey> fsm );
		void LateUpdateState( Fsm<StateKey> fsm );
	}
}