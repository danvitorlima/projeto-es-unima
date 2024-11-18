using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtributoTempoDeRecarga : Atributo
{
    public override void EvoluirAtributo()
    {
        base.EvoluirAtributo();
        GameObject.FindGameObjectWithTag("Player").GetComponent<AtaqueDoJogador>().DiminuirTempoDeRecarga(1.4f);
    }
}
