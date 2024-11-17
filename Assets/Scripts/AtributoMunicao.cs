using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtributoMunicao : Atributo
{
    int i = 2;
    public override void EvoluirAtributo()
    {
        base.EvoluirAtributo();
        GameObject.FindGameObjectWithTag("Player").GetComponent<AtaqueDoJogador>().AumentarMunicaoMaxima(50, i);
        i++;
    }
}
