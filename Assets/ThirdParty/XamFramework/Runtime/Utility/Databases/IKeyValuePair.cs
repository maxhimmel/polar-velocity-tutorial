using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Utility
{
	/// <summary>
	/// The implementing object is the value.
	/// </summary>
	/// <typeparam name="K">Key</typeparam>
	public interface IKeyValuePair<K>
	{
		K GetKey();
	}
}