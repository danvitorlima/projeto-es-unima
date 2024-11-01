using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projetil : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Inimigo"))
        {
            collision.gameObject.GetComponent<SistemaVida>().ReceberDano(50);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Parede"))
        {
            Destroy(gameObject);
        }
    }
}
