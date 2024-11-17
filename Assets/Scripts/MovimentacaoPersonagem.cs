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

    public void AumentarVelocidade(float fatorDeCrescimento)
    {
        movSpeed = movSpeed * fatorDeCrescimento;
    }

    void FixedUpdate()
    {
        speedX = Input.GetAxisRaw("Horizontal");
        speedY = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(speedX, speedY).normalized * movSpeed;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = new Vector2(
                    mousePosition.x - transform.position.x,
                    mousePosition.y - transform.position.y
                );
        transform.up = direction;
    }
}
