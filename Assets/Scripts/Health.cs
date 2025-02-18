using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    bool isPlayer;

    [SerializeField]
    int health = 50;

    [SerializeField]
    int score = 50;

    [SerializeField]
    ParticleSystem hitEffect;

    [SerializeField]
    public string tag;

    [SerializeField]
    bool applyCameraShake;
    CameraShake cameraShake;

    AudioPlayer audioPlayer;
    ScoreKeeper scoreKeeper;
    LevelManager levelManager;
    int currentHealth = 0;

    void Awake()
    {
        cameraShake = Camera.main.GetComponent<CameraShake>();
        audioPlayer = FindObjectOfType<AudioPlayer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        levelManager = FindObjectOfType<LevelManager>();
        currentHealth = health;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.GetComponent<DamageDealer>();

        if (damageDealer != null)
        {
            TakeDamage(damageDealer.GetDamage());
            PlayHitEffect();
            audioPlayer.PlayDamageClip();
            ShakeCamera();
            damageDealer.Hit();
        }
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (!isPlayer)
        {
            currentHealth = health;
            scoreKeeper.ModifyScore(score);
        }
        else
        {
            levelManager.LoadGameOver();
        }
        if (!tag.Equals(null))
        {
            ObjectPooler.Instance.ReturnToPool(tag, gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void PlayHitEffect()
    {
        if (hitEffect != null)
        {
            GameObject instance = ObjectPooler.Instance.SpawnFromPool(
                "HitVFX",
                transform.position,
                Quaternion.identity
            );
            ParticleSystem p = instance.GetComponent<ParticleSystem>();
            ObjectPooler.Instance.ReturnToPool(
                "HitVFX",
                instance,
                p.main.duration + p.main.startLifetime.constantMax
            );
        }
    }

    void ShakeCamera()
    {
        if (cameraShake != null && applyCameraShake)
        {
            cameraShake.Play();
        }
    }
}
