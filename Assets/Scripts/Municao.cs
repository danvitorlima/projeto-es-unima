using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Municao : Item
{
    private void Start()
    {
        nome = "Munição " + tier;
        icone = GetComponent<SpriteRenderer>().sprite;
    }
    public override bool Usar()
    {
        return GameObject.FindGameObjectWithTag("Player").GetComponent<AtaqueDoJogador>().AdicionarMunicao((int)(10 * Mathf.Pow(2, tier)));
    }

}
