using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Utility.Extensions
{
	public static class MathExtensions
	{
		public static bool BitwiseAnd( int lhs, int rhs )
		{
			return (lhs & rhs) != 0;
		}

		/// <summary>
		/// Equivalent to % operator, but also wraps as expected when <paramref name="x"/> is negative.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="N"></param>
		/// <returns></returns>
		public static int Modulus( int x, int N )
		{
			return (x % N + N) % N;
		}
	}
}