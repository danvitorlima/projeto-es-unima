using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Atributo : MonoBehaviour
{
    private XP xp;
    private List<GameObject> barraDeProgressao = new List<GameObject>();
    private int progresso;
    private bool zerado;

    void Start()
    {
        xp = GameObject.FindGameObjectWithTag("Player").GetComponent<XP>();
        progresso = 0;
        foreach (Transform item in gameObject.GetComponentInChildren<Transform>())
        {
            if (item != gameObject.transform)
            {
                barraDeProgressao.Add(item.gameObject);
            }
        }
        atualizarBarraDeProgresso();
        zerado = xp.pontosDeAtributo == 0;
    }

    private void OnDisable()
    {
        foreach (var item in barraDeProgressao)
        {
            item.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        zerado = xp.pontosDeAtributo == 0;
        atualizarBarraDeProgresso();
    }

    private void atualizarBarraDeProgresso()
    {
        foreach (var item in barraDeProgressao)
        {
            if (barraDeProgressao.IndexOf(item) < progresso)
            {
                //roxo
                item.GetComponent<Image>().color = new Color(0.5098f, 0.2784f, 1f);
            }
            else
            {
                //cinza
                item.GetComponent<Image>().color = new Color(0.3686f, 0.3686f, 0.3686f);
                //Debug.Log(item.name + " foi pintado de cinza");
            }
            item.transform.GetChild(0).gameObject.SetActive(false);
            item.GetComponent<EventTrigger>().enabled = false;
            item.GetComponent<Animator>().enabled = false;
            //se tiver progresso disponivel e se tiver pontos
            if (progresso < 7 && xp.pontosDeAtributo > 0)
            {
                var melhoriaPotencial = barraDeProgressao[progresso];
                melhoriaPotencial.GetComponent<Animator>().enabled = true;
                melhoriaPotencial.GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
                melhoriaPotencial.GetComponent<Animator>().Play(0);
                melhoriaPotencial.GetComponent<EventTrigger>().enabled = true;
            }
        }
    }

    public void EvoluirAtributo()
    {
        xp.pontosDeAtributo--;
        progresso++;
        atualizarBarraDeProgresso();
    }
    private void Update()
    {
        if (!zerado && xp.pontosDeAtributo == 0)
        {
            Debug.Log("zerou");
            atualizarBarraDeProgresso();
            zerado = true;
        }
    }
}
