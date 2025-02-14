using System.Collections.Generic;
using UnityEngine;

public class GunSpawner : MonoBehaviour
{
    [SerializeField]
    List<GameObject> gunPrefabs; // Danh sách các prefab súng

    [SerializeField]
    List<Transform> gunMountPoints; // Danh sách các điểm gắn súng trên spaceship

    private List<GameObject> currentGuns = new List<GameObject>();
    private int currentGunIndex = 0; // Vị trí hiện tại trong danh sách mount points

    void Start()
    {
        ClearAllGuns();
    }

    public int GetCountGuns()
    {
        return currentGuns.Count;
    }

    // Spawn từng khẩu súng theo thứ tự
    public void SpawnNextGun()
    {
        if (currentGunIndex >= gunMountPoints.Count)
        {
            Debug.Log("Đã spawn hết tất cả súng!");
            return;
        }

        int gunIndex = currentGunIndex % gunPrefabs.Count; // Lặp lại súng nếu thiếu
        GameObject gun = Instantiate(
            gunPrefabs[gunIndex],
            gunMountPoints[currentGunIndex].position,
            gunMountPoints[currentGunIndex].rotation,
            gunMountPoints[currentGunIndex]
        );

        currentGuns.Add(gun);
        currentGunIndex++; // Tăng index để chuẩn bị cho lần spawn tiếp theo
    }

    // Xóa tất cả súng hiện tại
    public void ClearAllGuns()
    {
        foreach (GameObject gun in currentGuns)
        {
            Destroy(gun);
        }
        currentGuns.Clear();
        currentGunIndex = 0; // Reset lại chỉ số súng
    }
}
