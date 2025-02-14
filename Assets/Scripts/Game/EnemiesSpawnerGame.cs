using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WaveConfigGame;

public class EnemiesSpawnerGame : MonoBehaviour
{
    [SerializeField]
    private WaveConfigGame waveConfig; // Gắn ScriptableObject chứa thông tin wave

    private int currentMajorWaveIndex = 0;
    private bool isEndlessMode = false;

    [SerializeField]
    float timeBetweenWaves = 0f;

    [SerializeField]
    bool isLooping;

    WaveConfigSO currentWave;

    void Start()
    {
        StartCoroutine(SpawnMajorWaves());
        StartCoroutine(SpawnEnemyWaves(waveConfig.majorWaves[currentMajorWaveIndex]));
    }

    public WaveConfigSO GetCurrentWave()
    {
        return currentWave;
    }

    IEnumerator SpawnEnemyWaves(MajorWave subWave)
    {
        do
        {
            foreach (WaveConfigSO wave in subWave.waveConfigSO)
            {
                currentWave = wave;
                for (int i = 0; i < currentWave.GetEnemyCount(); i++)
                {
                    ObjectPooler.Instance.SpawnFromPool(
                        currentWave.GetEnemyPrefab(i).GetComponent<Health>().tag,
                        currentWave.GetStartingWaypoint().position,
                        Quaternion.Euler(0, 0, 180)
                    );
                    Debug.Log("Spawn enemies ii " + i);
                    yield return new WaitForSeconds(currentWave.GetRandomSpawnTime());
                }
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        } while (isLooping);
    }

    IEnumerator SpawnMajorWaves()
    {
        while (true)
        {
            if (currentMajorWaveIndex >= waveConfig.majorWaves.Count)
            {
                if (waveConfig.allowEndlessMode)
                {
                    Debug.Log("🔁 Endless Mode: Random lại wave cuối.");
                    currentMajorWaveIndex = waveConfig.majorWaves.Count - 1;
                }
                else
                {
                    Debug.Log("🏆 Tất cả Major Waves đã kết thúc!");
                    yield break;
                }
            }

            WaveConfigGame.MajorWave currentWave = waveConfig.majorWaves[currentMajorWaveIndex];
            GameWaveManager.Instance.StartNewWave(currentWave.waveDuration, 1);

            float waveEndTime = Time.time + currentWave.waveDuration;

            while (Time.time < waveEndTime)
            {
                foreach (var subWave in currentWave.subWaves)
                {
                    yield return StartCoroutine(SpawnSubWave(subWave)); // Đảm bảo chỉ gọi 1 lần
                }
            }

            Debug.Log($"✅ Kết thúc {currentWave.waveName}.");
            currentMajorWaveIndex++;
        }
    }

    IEnumerator SpawnSubWave(WaveConfigGame.SubWave subWave)
    {
        int numberOfEnemies = Random.Range(subWave.minEnemies, subWave.maxEnemies);
        Debug.Log($"🚀 {subWave.waveName}: Spawn {numberOfEnemies} enemies.");

        for (int i = 0; i < numberOfEnemies; i++)
        {
            string selectedEnemyType = GetWeightedRandomEnemy(subWave.enemyTypes);
            GameObject enemy = ObjectPooler.Instance.SpawnFromPool(selectedEnemyType);
            GameObject item = ObjectPooler.Instance.SpawnFromPool(
                subWave.itemName,
                new Vector3(Random.Range(0, 10), Random.Range(0, 10), 1),
                Quaternion.identity
            );
            if (enemy != null && enemy.GetComponent<RandomMove>() != null)
            {
                RandomMove randomMove = enemy.GetComponent<RandomMove>();
                randomMove.startMove = new Vector2(Random.Range(-10, 10), 10);
                randomMove.speed = Random.Range(subWave.minSpeed, subWave.maxSpeed);
            }
            else
            {
                Debug.LogWarning("Failed to spawn enemy, object pool may be empty.");
            }

            // Đợi ngẫu nhiên giữa các lần spawn
            yield return new WaitForSeconds(subWave.minTimeSpawn);
        }

        float timeToNextSubWave = Random.Range(subWave.minTimeSpawn, subWave.maxTimeSpawn);
        Debug.Log($"⏳ Chờ {timeToNextSubWave} giây trước đợt spawn tiếp theo.");
        yield return new WaitForSeconds(timeToNextSubWave);
    }

    private string GetWeightedRandomEnemy(List<WaveConfigGame.EnemySpawnData> enemyTypes)
    {
        int totalWeight = 0;
        foreach (var enemy in enemyTypes)
        {
            totalWeight += enemy.spawnWeight;
        }

        int randomValue = Random.Range(0, totalWeight);
        int currentWeightSum = 0;

        foreach (var enemy in enemyTypes)
        {
            currentWeightSum += enemy.spawnWeight;
            if (randomValue < currentWeightSum)
            {
                return enemy.enemyType;
            }
        }

        return enemyTypes[0].enemyType; // Trường hợp lỗi, lấy enemy đầu tiên
    }

    public WaveConfigGame.SubWave GetCurrentSubWave()
    {
        if (currentMajorWaveIndex >= waveConfig.majorWaves.Count)
        {
            Debug.LogWarning("⚠ Không còn Major Wave nào!");
            return null;
        }

        WaveConfigGame.MajorWave currentWave = waveConfig.majorWaves[currentMajorWaveIndex];

        if (currentWave.subWaves.Count == 0)
        {
            Debug.LogWarning("⚠ Major Wave hiện tại không có sub-waves!");
            return null;
        }

        // Trả về sub-wave cuối cùng đã spawn hoặc sub-wave đầu tiên nếu chưa có cái nào spawn
        return currentWave.subWaves[^1];
    }
}
