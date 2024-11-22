using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.PostProcessing;
using static Unity.Burst.Intrinsics.X86.Avx;

public class BotaoToggle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI tmp;
    public bool ativado;
    private Color cor;
    [SerializeField]
    private PostProcessVolume ppv;
    [SerializeField]
    private AudioSource sfx;

    private void Start()
    {
        ativado = PlayerPrefs.GetInt("Graficos", 0) == 1;
        cor = ativado ? new Color(0.6705882f, 1.0f, 0.0f) : new Color(0.4901961f, 0.4901961f, 0.4901961f);
        tmp = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        tmp.color = cor;
    }

    public void Toggle(bool a)
    {
        sfx.Play();
        ativado = !ativado;
        cor = ativado ? new Color(0.6705882f, 1.0f, 0.0f) : new Color(0.4901961f, 0.4901961f, 0.4901961f);
        tmp.color = cor;
        PlayerPrefs.SetInt("Graficos", ativado ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void OnDisable()
    {
        tmp.color = cor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tmp.color = new Color(0.9529412f, 0.7019608f, 0.4117647f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tmp.color = cor;
    }

    private void Update()
    {
        ppv.enabled = ativado;
    }
}
