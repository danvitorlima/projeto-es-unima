using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtributoAtaque : Atributo
{
    public override void EvoluirAtributo()
    {
        base.EvoluirAtributo();
        GameObject.FindGameObjectWithTag("Player").GetComponent<AtaqueDoJogador>().AumentarDano(1.5f);
    }
}
