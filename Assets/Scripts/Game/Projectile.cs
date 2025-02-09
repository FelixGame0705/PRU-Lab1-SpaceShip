using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    string tag;

    [SerializeField]
    private float lifetime;
    private Coroutine despawnCoroutine;

    public void OnEnable()
    {
        ScheduleDespawn();
    }

    public void ScheduleDespawn()
    {
        if (despawnCoroutine != null)
            StopCoroutine(despawnCoroutine);
        despawnCoroutine = StartCoroutine(DespawnAfterTime());
    }

    IEnumerator DespawnAfterTime()
    {
        yield return new WaitForSeconds(lifetime);
        ObjectPooler.Instance.ReturnToPool(tag, gameObject);
    }
}
