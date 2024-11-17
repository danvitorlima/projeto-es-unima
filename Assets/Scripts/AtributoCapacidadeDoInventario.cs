using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtributoCapacidadeDoInventario : Atributo
{
    public override void EvoluirAtributo()
    {
        base.EvoluirAtributo();
        GameObject.FindGameObjectWithTag("Player").GetComponent<Inventario>().AumentarSlots(2);
    }
}
