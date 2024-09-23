using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentacaoPersonagem : MonoBehaviour
{
    public float movSpeed;
    float speedX, speedY;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        speedX = Input.GetAxisRaw("Horizontal");
        speedY = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(speedX, speedY).normalized * movSpeed;
    }
}
