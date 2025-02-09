using System.Collections;
using UnityEngine;

public class ItemScore : MonoBehaviour
{
    [SerializeField]
    int score;

    [SerializeField]
    string tag;
    ScoreKeeper scoreKeeper;

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

    private void Start()
    {
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            scoreKeeper.ModifyScore(score);
            StopCoroutine(despawnCoroutine);
            ObjectPooler.Instance.ReturnToPool(tag, gameObject);
        }
    }
}
