using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public string nome { get; protected set; }
    public bool equipavel { get; protected set; }
    private bool capturavel = false;
    public int stack { get; protected set; }
    public Sprite icone { get; protected set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            capturavel = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            capturavel = false;
        }
    }
    private void Update()
    {
        if (capturavel && Input.GetKeyDown(KeyCode.E))
        {
            //quantidade de 65 itens para testar
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<Inventario>().AdicionarItem(this))
            {
                gameObject.SetActive(false);
            }
        }
    }
    public abstract void Usar();

}
