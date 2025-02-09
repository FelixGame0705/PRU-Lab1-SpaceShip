using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private Dictionary<string, Queue<GameObject>> poolDictionary =
        new Dictionary<string, Queue<GameObject>>();
    private Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();

    /// <summary>
    /// Tạo Pool cho một loại GameObject.
    /// </summary>
    public void CreatePool(string tag, GameObject prefab, int size)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < size; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(tag, objectPool);
            prefabDictionary.Add(tag, prefab);
        }
    }

    /// <summary>
    /// Spawn một GameObject từ Pool.
    /// </summary>
    ///
    public GameObject SpawnFromPool(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool với tag {tag} không tồn tại!");
            return null;
        }

        GameObject objectToSpawn;

        if (poolDictionary[tag].Count == 0)
        {
            objectToSpawn = Instantiate(prefabDictionary[tag]); // Tạo mới nếu hết object
        }
        else
        {
            objectToSpawn = poolDictionary[tag].Dequeue(); // Lấy từ pool
        }
        objectToSpawn.SetActive(true);

        return objectToSpawn;
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool với tag {tag} không tồn tại!");
            return null;
        }

        GameObject objectToSpawn;

        if (poolDictionary[tag].Count == 0)
        {
            objectToSpawn = Instantiate(prefabDictionary[tag]); // Tạo mới nếu hết object
        }
        else
        {
            objectToSpawn = poolDictionary[tag].Dequeue(); // Lấy từ pool
        }

        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);

        return objectToSpawn;
    }

    /// <summary>
    /// Trả GameObject về Pool.
    /// </summary>
    public void ReturnToPool(string tag, GameObject obj)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool với tag {tag} không tồn tại!");
            return;
        }

        obj.SetActive(false);
        poolDictionary[tag].Enqueue(obj);
    }

    public void ReturnToPool(string tag, GameObject obj, float delay)
    {
        StartCoroutine(ReturnToPoolWithDelay(tag, obj, delay));
    }

    private IEnumerator ReturnToPoolWithDelay(string tag, GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnToPool(tag, obj);
    }
}
