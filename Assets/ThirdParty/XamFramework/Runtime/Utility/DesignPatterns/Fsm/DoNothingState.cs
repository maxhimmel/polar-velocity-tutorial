namespace Xam.Utility.Fsm
{
	public class DoNothingState<StateKey> : IFsmState<StateKey>
	{
		public virtual void InitializeState( Fsm<StateKey> fsm ) { }
		public virtual void ShutdownState( Fsm<StateKey> fsm ) { }
		public virtual bool CanEnterState( StateKey currentState ) { return true; }
		public virtual void EnterState( Fsm<StateKey> fsm, StateKey prevState, object context = null ) { }
		public virtual void ExitState( Fsm<StateKey> fsm, StateKey nextState ) { }
		public virtual void UpdateState( Fsm<StateKey> fsm ) { }
		public virtual void FixedUpdateState( Fsm<StateKey> fsm ) { }
		public virtual void LateUpdateState( Fsm<StateKey> fsm ) { }
	}
}