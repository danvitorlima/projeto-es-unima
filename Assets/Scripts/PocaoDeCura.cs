using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PocaoDeCura : Item
{
    private void Start()
    {
        nome = "Po��o de Cura " + tier;
        icone = GetComponent<SpriteRenderer>().sprite;
    }
    public override bool Usar()
    {
        return GameObject.FindGameObjectWithTag("Player").GetComponent<SistemaVida>().Curar((int)(10 * Mathf.Pow(2, tier)));
    }
}
