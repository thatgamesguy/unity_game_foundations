using UnityEngine;
using System.Collections;

namespace GameFoundations
{
	public interface ICircularArray<T>
	{
		int count { get; }
		int capacity { get; }

		void Clear ();
		T this [int index] {get;set;
		}
		int IndexOf (T obj);
		T Push (T obj);
		T Pop ();
		void InsertAt (T obj, int index);
		void RemoveAt (int index);

	}
}
