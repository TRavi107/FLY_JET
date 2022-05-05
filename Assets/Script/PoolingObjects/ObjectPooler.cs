using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FJ
{
    public class ObjectPooler : MonoBehaviour
    {
        public Dictionary<string, Queue<GameObject>> poolDictionary;
        public List<Pool> pools;
        public static ObjectPooler singleton;

        void Awake()
        {
            singleton = this;
            poolDictionary = new Dictionary<string, Queue<GameObject>>();
            foreach (Pool pool in pools)
            {
                Queue<GameObject> objectsPool = new Queue<GameObject>();
                for (int i = 0; i < pool.totalSize; i++)
                {
                    GameObject ob = Instantiate(pool.prefab);
                    ob.SetActive(false);
                    objectsPool.Enqueue(ob);
                }
                poolDictionary.Add(pool.tag, objectsPool);
            }
        }

        void Start()
        {


        }
        public GameObject SpawnFromPool(string tag)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning("Not found" + tag);
                return null;
            }
            GameObject objectToSpawn = poolDictionary[tag].Dequeue();
            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = Vector3.zero;
            objectToSpawn.transform.rotation = Quaternion.identity;
            objectToSpawn.transform.parent = null;
            poolDictionary[tag].Enqueue(objectToSpawn);
            objectToSpawn.GetComponent<IPooledObj>().PooledObjStart();
            return objectToSpawn;
        }

        public GameObject SpawnFromPool(string tag, Vector3 pos, Quaternion angle, Transform parent = null)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning("Not found" + tag);
                return null;
            }
            GameObject objectToSpawn = poolDictionary[tag].Dequeue();
            if (objectToSpawn == null)
            {
                Debug.Log("Something is wrong");
                Debug.Log(tag);
                Debug.Log(poolDictionary[tag].Count);
                return null;
            }
            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = pos;
            objectToSpawn.transform.rotation = angle;
            objectToSpawn.transform.parent = parent;
            poolDictionary[tag].Enqueue(objectToSpawn);
            IPooledObj Ipol = objectToSpawn.GetComponent<IPooledObj>();
            if (Ipol != null)
            {
                Ipol.PooledObjStart();
            }
            return objectToSpawn;
        }

        [System.Serializable]
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int totalSize;
        }
    }
}
