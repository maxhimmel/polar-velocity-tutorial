using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Initialization
{
	public interface IInitialize
	{
		void StartInitializing();
		bool IsInitializationComplete();
	}
}