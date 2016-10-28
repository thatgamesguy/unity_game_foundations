using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GameFoundations
{
	public class CircularArray<T> : ICircularArray<T>, IEnumerable<T>
	{
		public int count { private set; get; }
		
		public int capacity { get { return _array.Length; } }
		
		public T this [int index] {
			get {
				if (index < 0 || index >= count)
					throw new ArgumentOutOfRangeException ("index", "not in range");
				
				return _array [(_tailIndex + index) % capacity];
			}
			set {
				if (index < 0 || index >= count)
					throw new ArgumentOutOfRangeException ("index", "not in range");
				
				_array [(_tailIndex + index) % capacity] = value;
			}
		}

		private T head { get { return _array [_headIndex]; } set { _array [_headIndex] = value; } }
		private T tail { get { return _array [_tailIndex]; } set { _array [_tailIndex] = value; } }

		private T[] _array;
		private int _headIndex;
		private int _tailIndex;

		public CircularArray (int capacity)
		{
			if (capacity < 1) {
				throw new ArgumentOutOfRangeException ("capacity", "must be positive");
			}

			_array = new T[capacity];

			Clear ();
		}



		public void Clear ()
		{
			_headIndex = capacity - 1;
			_tailIndex = 0;
			count = 0;
		}

		public int IndexOf (T obj)
		{
			for (var i = 0; i < count; ++i)
				if (Equals (obj, this [i]))
					return i;

			return -1;
		}

		public T Push (T obj)
		{
			IncrementHead ();

			var overwritten = head;
			head = obj;

			if (count == capacity)
				IncrementTail ();
			else
				count++;

			return overwritten;
		}

		public T Pop ()
		{
			if (count == 0)
				throw new InvalidOperationException ("queue empty");

			var popped = tail;
			tail = default (T);

			IncrementTail ();
			count--;

			return popped;
		}

		public void InsertAt (T obj, int index)
		{
			if (index < 0 || index > count)
				throw new ArgumentOutOfRangeException ("index", "not in range");
			
			if (count == index)
				Push (obj);
			else {
				var last = this [count - 1];

				for (var i = index; i < count - 2; ++i) {
					this [i + 1] = this [i];
				}

				this [index] = obj;
				Push (last);
			}
		}

		public void RemoveAt (int index)
		{
			if (index < 0 || index >= count)
				throw new ArgumentOutOfRangeException ("index", "not in range");
			
			for (var i = index; i > 0; --i)
				this [i] = this [i - 1];

			Pop ();
		}

		public IEnumerator<T> GetEnumerator ()
		{
			if (count == 0 || capacity == 0)
				yield break;
			
			for (var i = 0; i < count; ++i)
				yield return this [i];
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}

		private void IncrementHead ()
		{
			_headIndex = (_headIndex + 1) % _array.Length;
		}

		private void IncrementTail ()
		{
			_tailIndex = (_tailIndex + 1) % capacity;
		}
	}
}
