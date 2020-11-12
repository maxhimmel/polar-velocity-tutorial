using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Utility
{
	public static class AlgorithmUtility
	{
		public static void FisherYatesShuffle<T>( ref IList<T> array )
		{
			for ( int idx = array.Count - 1; idx > 0; --idx )
			{
				int randIdx = Random.Range( 0, idx + 1 );

				// Swap ...
				T temp = array[idx];
				array[idx] = array[randIdx];
				array[randIdx] = temp;
			}
		}
	}
}