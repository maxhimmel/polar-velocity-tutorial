using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Utility.Fsm
{
	public interface IFsmStateFactory<StateKey>
	{
		StateKey GetKey();
		IFsmState<StateKey> CreateState();
	}
}