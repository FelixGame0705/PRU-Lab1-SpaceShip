using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 5f;
    Vector2 rawInput;

    [SerializeField]
    float paddingLeft;

    [SerializeField]
    float paddingRight;

    [SerializeField]
    float paddingTop;

    [SerializeField]
    float paddingBottom;

    [SerializeField]
    Animator animator;

    Vector2 minBounds;
    Vector2 maxBounds;

    Shooter shooter;

    void Awake()
    {
        shooter = GetComponent<Shooter>();
    }

    void Start()
    {
        InitBounds();
    }

    void Update()
    {
        Move();
    }

    void InitBounds()
    {
        Camera mainCamera = Camera.main;
        minBounds = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        maxBounds = mainCamera.ViewportToWorldPoint(new Vector2(1, 1.5f));
    }

    void Move()
    {
        Vector2 delta = rawInput * moveSpeed * Time.deltaTime;
        Vector2 newPos = new Vector2();
        newPos.x = Mathf.Clamp(
            transform.position.x + delta.x,
            minBounds.x + paddingLeft,
            maxBounds.x - paddingRight
        );
        newPos.y = Mathf.Clamp(
            transform.position.y + delta.y,
            minBounds.y + paddingBottom,
            maxBounds.y - paddingTop
        );
        if (rawInput.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, -30, 0);
        }
        else if (rawInput.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 30, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        transform.position = newPos;
    }

    void OnMove(InputValue value)
    {
        rawInput = value.Get<Vector2>();
    }

    void OnFire(InputValue value)
    {
        if (shooter != null)
        {
            shooter.isFiring = value.isPressed;
        }
    }
}
