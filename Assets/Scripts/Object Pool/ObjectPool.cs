using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameFoundations
{
	public class ObjectPool : MonoBehaviour
	{

		public ObjectPoolItem[] ObjectPoolItems;

		/// <summary>
		/// The pooled objects currently available.
		/// </summary>
		public List<GameObject>[] pooledObjects;


		public int defaultBufferAmount = 3;

		/// <summary>
		/// The container object that we will keep unused pooled objects so we dont clog up the editor with objects.
		/// </summary>
		protected GameObject containerObject;

				
		private static ObjectPool _instance;
		public static ObjectPool instance {
			get {
				return _instance;
			}
		}
		
		void Awake ()
		{
			_instance = this;
		}

		// Use this for initialization
		void Start ()
		{
			containerObject = new GameObject ("ObjectPool");

			//Loop through the object prefabs and make a new list for each one.
			//We do this because the pool can only support prefabs set to it in the editor,
			//so we can assume the lists of pooled objects are in the same order as object prefabs in the array
			pooledObjects = new List<GameObject>[ObjectPoolItems.Length];

			int i = 0;
			
			foreach (var item in ObjectPoolItems) {
				pooledObjects [i] = new List<GameObject> (); 
		
				int bufferAmount = (i < item.BufferAmount) ? item.BufferAmount : defaultBufferAmount;

				for (int n = 0; n < bufferAmount; n++) {
					GameObject newObj = Instantiate (item.ObjectPrefab) as GameObject;
					newObj.name = item.ObjectPrefab.name;
					newObj.SetActive (false);
					PoolObject (newObj);
				}

				i++;
			}
								
		}

		/// <summary>
		/// Gets a new object for the name type provided.  If no object type exists or if onlypooled is true and there is no objects of that type in the pool
		/// then null will be returned.
		/// </summary>
		/// <returns>
		/// The object for type.
		/// </returns>
		/// <param name='objectType'>
		/// Object type.
		/// </param>
		/// <param name='onlyPooled'>
		/// If true, it will only return an object if there is one currently pooled.
		/// </param>
		public GameObject GetObjectForType (string objectType, bool onlyPooled)
		{
			for (int i = 0; i < ObjectPoolItems.Length; i++) {
				var prefab = ObjectPoolItems [i].ObjectPrefab;
				if (prefab.name == objectType) {

					if (pooledObjects [i].Count > 0) {
						GameObject pooledObject = pooledObjects [i] [0];
												
						if (pooledObject) {
							pooledObjects [i].RemoveAt (0);
							pooledObject.transform.SetParent (null, false);
						} else {
							Debug.LogError (objectType + ": not found in pool");
						}

						return pooledObject;

					} else if (!onlyPooled) {
						GameObject newObj = Instantiate (prefab) as GameObject;
						newObj.name = prefab.name;
						return newObj;
					}

					break;

				}
			}

			// No object of the specified type or none were left in the pool with onlyPooled set to true
			return null;
		}

		/// <summary>
		/// Pools the object specified.  Will not be pooled if there is no prefab of that type.
		/// </summary>
		/// <param name='obj'>
		/// Object to be pooled.
		/// </param>
		public void PoolObject (GameObject obj)
		{
			for (int i = 0; i < ObjectPoolItems.Length; i++) {
				if (ObjectPoolItems [i].ObjectPrefab.name == obj.name) {
					obj.SetActive (false);
					obj.transform.SetParent (containerObject.transform);
					pooledObjects [i].Add (obj);
					return;
				}
			}
						
			Debug.LogError (obj.name + ": not setup to use object pool");
		}



	}


}
