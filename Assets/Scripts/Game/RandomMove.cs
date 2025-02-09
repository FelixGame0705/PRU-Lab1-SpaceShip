using UnityEngine;

public class RandomMove : MonoBehaviour
{
    [SerializeField]
    public Vector2 startMove; // Vị trí bắt đầu

    [SerializeField]
    private float distanceDestroy = 20f; // Khoảng cách để hủy đối tượng

    [SerializeField]
    public float speed = 2f; // Tốc độ di chuyển

    [SerializeField]
    private float rotation = 30f; // Tốc độ quay

    [SerializeField]
    private float xDeviation = 1f; // Độ lệch ngang khi rơi

    [SerializeField]
    private GameObject spriteObject;

    [SerializeField]
    private GameObject fireTail;

    [SerializeField]
    private string tag;

    private Vector2 moveDirection; // Hướng di chuyển ban đầu
    private Vector2 initialPosition; // Lưu vị trí ban đầu để kiểm tra khoảng cách

    void OnEnable()
    {
        // Đặt vị trí bắt đầu
        transform.position = startMove;
        initialPosition = startMove;

        // Tạo hướng di chuyển chủ yếu rơi xuống với một chút lệch ngang
        float randomXOffset = Random.Range(-xDeviation, xDeviation); // Độ lệch ngang ngẫu nhiên
        moveDirection = new Vector2(randomXOffset, -1f).normalized; // Luôn có hướng xuống
        // Tính góc quay của fireTail theo hướng di chuyển
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        fireTail.transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
    }

    void Update()
    {
        // Di chuyển theo hướng xuống với tốc độ đã đặt
        transform.position += (Vector3)(moveDirection * speed * Time.deltaTime);

        // Xoay thiên thạch khi rơi
        spriteObject.transform.Rotate(Vector3.forward * rotation * Time.deltaTime);

        // Kiểm tra khoảng cách từ vị trí ban đầu, nếu vượt quá thì hủy đối tượng
        if (Vector2.Distance(initialPosition, transform.position) >= distanceDestroy)
        {
            ObjectPooler.Instance.ReturnToPool(tag, gameObject);
        }
    }
}
