using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public string nome { get; protected set; }
    public bool equipavel { get; protected set; } = false;
    private bool capturavel = false;
    public int stack { get; protected set; } = 5;
    public Sprite icone { get; protected set; }
    [SerializeField]
    private int _tier;
    public int tier
    {
        get { return _tier; }
        set
        {
            if (value >= 0 || value < 4)
            {
                _tier = value;
            }
        }
    }


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
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<Inventario>().AdicionarItem(this))
            {
                gameObject.SetActive(false);
            }
        }
    }
    public abstract bool Usar();

}
