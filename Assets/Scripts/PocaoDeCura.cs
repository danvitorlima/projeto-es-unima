using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocaoDeCura : Item
{
    private void Start()
    {
        icone = GetComponent<SpriteRenderer>().sprite;
        nome = "Poção de cura";
        equipavel = false;
        stack = 5;
    }
    
    public override void Usar()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<SistemaVida>().Curar(10);
    }
}
