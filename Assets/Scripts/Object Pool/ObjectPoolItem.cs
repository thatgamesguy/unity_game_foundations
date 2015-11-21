using UnityEngine;
using System.Collections;

namespace GameFoundations
{
	[System.Serializable]
	public class ObjectPoolItem
	{
		public GameObject ObjectPrefab;
		public int BufferAmount = 1;
	}
}
