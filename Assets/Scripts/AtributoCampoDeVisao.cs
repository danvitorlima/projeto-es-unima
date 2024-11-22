using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class AtributoCampoDeVisao : Atributo
{
    public PostProcessVolume ppv;
    ChromaticAberration ca;
    public override void EvoluirAtributo()
    {
        base.EvoluirAtributo();
        GameObject.FindAnyObjectByType<CinemachineVirtualCamera>().m_Lens.OrthographicSize *= 1.1f;
        ppv.profile.TryGetSettings(out ca);
        ca.intensity.value *= 1.2f;
    }
}
