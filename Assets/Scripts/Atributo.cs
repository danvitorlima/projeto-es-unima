using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Atributo : MonoBehaviour
{
    protected XP xp;
    private List<GameObject> barraDeProgressao = new List<GameObject>();
    private int progresso;
    private bool zerado;

    protected virtual void Start()
    {
        xp = GameObject.FindGameObjectWithTag("Player").GetComponent<XP>();
        progresso = 0;
        foreach (Transform item in gameObject.GetComponentInChildren<Transform>())
        {
            if (item != gameObject.transform)
            {
                barraDeProgressao.Add(item.gameObject);
                var trigger = item.gameObject.GetComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.PointerClick
                };
                entry.callback.AddListener((data) => { GetComponent<Atributo>().EvoluirAtributo(); });
                trigger.triggers.Add(entry);
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
        if (xp)
        {
            zerado = xp.pontosDeAtributo == 0;
        }
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
            if (progresso < barraDeProgressao.Count && xp.pontosDeAtributo > 0)
            {
                var melhoriaPotencial = barraDeProgressao[progresso];
                melhoriaPotencial.GetComponent<Animator>().enabled = true;
                melhoriaPotencial.GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
                melhoriaPotencial.GetComponent<Animator>().Play(0);
                melhoriaPotencial.GetComponent<EventTrigger>().enabled = true;
            }
        }
    }

    public virtual void EvoluirAtributo()
    {
        xp.pontosDeAtributo--;
        progresso++;
        atualizarBarraDeProgresso();
    }
    private void Update()
    {
        if (!zerado && xp.pontosDeAtributo == 0)
        {
            atualizarBarraDeProgresso();
            zerado = true;
        }
    }
}
