using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtributoDistanciaDoTiro : Atributo
{
    public override void EvoluirAtributo()
    {
        base.EvoluirAtributo();
        GameObject.FindGameObjectWithTag("Player").GetComponent<AtaqueDoJogador>().AumentarDistanciaDoTiro(1.5f);
    }
}
