using UnityEngine;

public class Gravity : MonoBehaviour
{
    public float fallSpeed = 5.0f; // Tốc độ rơi cố định

    // Update được gọi mỗi khung hình
    void Update()
    {
        // Cập nhật vị trí của vật thể với tốc độ rơi cố định
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
    }
}
