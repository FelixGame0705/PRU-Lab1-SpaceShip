using System.Collections;
using TMPro;
using UnityEngine;

public class GameWaveManager : MonoBehaviour
{
    public static GameWaveManager Instance; // Singleton để dễ quản lý

    [SerializeField]
    private TextMeshProUGUI waveText;

    [SerializeField]
    private TextMeshProUGUI timerText;

    [SerializeField]
    private ScoreKeeper scoreKeeper;

    [Header("Weapons")]
    [SerializeField]
    GunSpawner gunSpawner;

    private int currentWave = 0;
    private float waveTimeRemaining = 0f;
    private bool isWaveActive = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }

    public void StartNewWave(float waveDuration, int count)
    {
        currentWave += count;
        waveTimeRemaining = waveDuration;
        isWaveActive = true;

        UpdateWaveUI();
        StartCoroutine(UpdateTimer());
    }

    public void Update()
    {
        SpawnGun();
    }

    private void UpdateWaveUI()
    {
        waveText.text = $"Wave: {currentWave}";
    }

    private IEnumerator UpdateTimer()
    {
        while (waveTimeRemaining > 0)
        {
            waveTimeRemaining -= Time.deltaTime;
            timerText.text = $"Time: {waveTimeRemaining:F1}s";
            yield return null;
        }

        isWaveActive = false;
        timerText.text = "Wave Over!";
    }

    void SpawnGun()
    {
        if (scoreKeeper.GetScore() >= 2000)
        {
            gunSpawner.SpawnNextGun();
        }
        else if (scoreKeeper.GetScore() >= 1000 && gunSpawner.GetCountGuns() < 1)
        {
            gunSpawner.SpawnNextGun();
        }
    }
}
