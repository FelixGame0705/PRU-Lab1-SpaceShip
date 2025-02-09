using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Wave Config Game", fileName = "NewWaveConfig")]
public class WaveConfigGame : ScriptableObject
{
    [System.Serializable]
    public class EnemySpawnData
    {
        public string enemyType; // Tên của enemy
        public int spawnWeight = 1; // Trọng số xuất hiện (càng cao, càng dễ xuất hiện)
    }

    [System.Serializable]
    public class SubWave
    {
        public string waveName = "Sub Wave";
        public List<EnemySpawnData> enemyTypes; // Danh sách enemy + trọng số
        public int minEnemies = 1;
        public int maxEnemies = 5;
        public float minTimeSpawn = 2f;
        public float maxTimeSpawn = 5f;
        public float minSpeed = 2f;
        public float maxSpeed = 5f;

        [Header("For item")]
        public string itemName;
        public int minItem = 1;
        public int maxItem = 5;
    }

    [System.Serializable]
    public class MajorWave
    {
        public string waveName = "Major Wave";
        public float waveDuration = 180f; // 3 phút
        public List<SubWave> subWaves;

        [Header("Special for ship enemies")]
        [SerializeField]
        public List<WaveConfigSO> waveConfigSO;

        public Transform GetStartingWaypoint(int i)
        {
            return waveConfigSO[i].GetStartingWaypoint();
        }

        public List<Transform> GetWaypoints(int i)
        {
            return waveConfigSO[i].GetWaypoints();
        }

        public GameObject GetRandomEnemyPrefab(int i)
        {
            int randomIndex = Random.Range(0, waveConfigSO[i].GetEnemyCount());
            return waveConfigSO[i].GetEnemyPrefab(randomIndex);
        }

        public float GetRandomSpawnTime(int i)
        {
            return waveConfigSO[i].GetRandomSpawnTime();
        }
    }

    public List<MajorWave> majorWaves;
    public bool allowEndlessMode = true; // Khi hết wave, có random tiếp không?
}
