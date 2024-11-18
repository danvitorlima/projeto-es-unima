using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtributoVitalidade : Atributo
{
    public override void EvoluirAtributo()
    {
        base.EvoluirAtributo();
        GameObject.FindGameObjectWithTag("Player").GetComponent<SistemaVida>().AumentarVidaMaxima(1.5f);
    }
}
