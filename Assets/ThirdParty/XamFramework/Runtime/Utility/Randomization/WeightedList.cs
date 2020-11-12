using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xam.Utility.Randomization
{
	[System.Serializable]
	public class WeightedList
	{
		public const int k_minWeight = 0;
		public const int k_maxWeight = 100;
	}

	[System.Serializable]
	public class WeightedList<T, K> : WeightedList
		where T : WeightedNode<K>
	{
		[SerializeField] private T[] m_items = default;

		private int m_maxWeight = 0;

		public void Init()
		{
			if ( m_items == null ) { return; }

			int weightSum = 0;
			foreach ( T node in m_items )
			{
				weightSum += node.NormalizedWeight;
				node.Weight = weightSum;
			}

			m_maxWeight = weightSum;
		}

		public K GetRandomItem()
		{
			if ( m_maxWeight <= 0 )
			{
				Debug.LogWarning( $"{typeof(T).Name} | Attempting to get random item without being initialized." );
				return default;
			}

			int roll = Random.Range( 0, m_maxWeight + 1 );
			if ( roll <= 0 ) { return default; }

			foreach ( T node in m_items )
			{
				if ( node.Weight > 0 && roll <= node.Weight )
				{
					return node.Item;
				}
			}

			return default;
		}
	}


	/* --- */


	[System.Serializable]
	public class WeightedNode
	{
		public int NormalizedWeight { get { return m_normalizedWeight; } }

		[Range( WeightedList.k_minWeight, WeightedList.k_maxWeight )]
		[SerializeField] private int m_normalizedWeight = 0;

		[HideInInspector] public int Weight = 0;
	}

	[System.Serializable]
	public class WeightedNode<T> : WeightedNode
	{
		public T Item;
	}
}
