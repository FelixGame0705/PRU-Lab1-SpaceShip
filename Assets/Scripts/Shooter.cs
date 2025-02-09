using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("General")]
    [SerializeField]
    GameObject projectilePrefab;

    [SerializeField]
    string projectileType;

    [SerializeField]
    float projectileSpeed = 10f;

    [SerializeField]
    float projectileLifetime = 5f;

    [SerializeField]
    float baseFiringRate = 0.2f;

    [Header("AI")]
    [SerializeField]
    bool useAI;

    [SerializeField]
    float firingRateVariance = 0f;

    [SerializeField]
    float minimumFiringRate = 0.1f;

    [HideInInspector]
    public bool isFiring;

    Coroutine firingCoroutine;
    AudioPlayer audioPlayer;

    void Awake()
    {
        audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    void Start()
    {
        if (useAI)
        {
            isFiring = true;
        }
    }

    void Update()
    {
        Fire();
    }

    void OnEnable()
    {
        ResetShooting();
    }

    public void ResetShooting()
    {
        if (firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }
    }

    void Fire()
    {
        if (isFiring && firingCoroutine == null)
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        else if (!isFiring && firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject instance = ObjectPooler.Instance.SpawnFromPool(
                projectileType,
                transform.position,
                Quaternion.identity
            );

            Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = transform.up * projectileSpeed;
            }

            // Đặt ID duy nhất cho viên đạn
            //int bulletID = instance.GetInstanceID();
            //StartCoroutine(
            //    HandleProjectileLifetime(instance, projectileType, projectileLifetime, bulletID)
            //);

            float timeToNextProjectile = Random.Range(
                baseFiringRate - firingRateVariance,
                baseFiringRate + firingRateVariance
            );
            timeToNextProjectile = Mathf.Clamp(
                timeToNextProjectile,
                minimumFiringRate,
                float.MaxValue
            );
            if (gameObject.tag.Equals("Player"))
                audioPlayer.PlayShootingClip();

            yield return new WaitForSeconds(timeToNextProjectile);
        }
    }

    // Quản lý lifetime với ID để tránh hủy sai viên đạn
    IEnumerator HandleProjectileLifetime(
        GameObject instance,
        string tag,
        float lifetime,
        int bulletID
    )
    {
        yield return new WaitForSeconds(lifetime);

        // Kiểm tra nếu viên đạn vẫn còn active và chưa bị tái sử dụng
        if (instance.activeInHierarchy && instance.GetInstanceID() == bulletID)
        {
            ObjectPooler.Instance.ReturnToPool(tag, instance);
        }
    }
}
