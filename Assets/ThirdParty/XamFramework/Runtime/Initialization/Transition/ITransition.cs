using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Initialization
{
	public interface ITransition
	{
		void Open( float duration );
		void Close( float duration );
	}
}