using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private GameObject bulletEnemyPrefab;

    [SerializeField]
    private GameObject meteorsPrefab;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private GameObject Flying;

    [SerializeField]
    private GameObject Item;

    [SerializeField]
    private GameObject hitVFX;

    void Start()
    {
        // Tạo Pool cho các đối tượng khác nhau
        ObjectPooler.Instance.CreatePool("Enemy", enemyPrefab, 10);
        ObjectPooler.Instance.CreatePool("Meteors", meteorsPrefab, 10);
        ObjectPooler.Instance.CreatePool("Bullet", bulletPrefab, 20);
        ObjectPooler.Instance.CreatePool("BulletEnemy", bulletEnemyPrefab, 30);
        ObjectPooler.Instance.CreatePool("Flying", Flying, 5);
        ObjectPooler.Instance.CreatePool("Item", Item, 5);
        ObjectPooler.Instance.CreatePool("HitVFX", hitVFX, 5);
    }
}
