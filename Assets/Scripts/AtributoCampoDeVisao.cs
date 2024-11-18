using Cinemachine;
using System.Collections;
using UnityEngine;

public class AtributoCampoDeVisao : Atributo
{
    public override void EvoluirAtributo()
    {
        base.EvoluirAtributo();
        GameObject.FindAnyObjectByType<CinemachineVirtualCamera>().m_Lens.OrthographicSize *= 1.1f;
    }
}
