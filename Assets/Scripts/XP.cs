using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XP : MonoBehaviour
{
    [SerializeField]
    private float xpMaximo = 100f;
    [SerializeField]
    private float xpAtual;
    [SerializeField]
    private float nivel;
    private Image barraDeXPUI;
    [SerializeField]
    private AudioClip sfx;
    public int pontosDeAtributo;
    [SerializeField]
    private GameObject telaDeProgressaoDeAtributos;

    void Start()
    {
        pontosDeAtributo = 0;
        nivel = 1;
        xpAtual = 0;
        barraDeXPUI = GameObject.FindGameObjectWithTag("BarraDeXP").GetComponent<Image>();
        atualizarBarraXP();
    }
    private void ganharXP(int quantidade)
    {
        xpAtual += quantidade;
        while (xpAtual >= xpMaximo)
        {
            xpAtual -= xpMaximo;
            nivel++;
            pontosDeAtributo++;
            xpMaximo = 100 * Mathf.Pow(1.5f, nivel - 1);
        }
        atualizarBarraXP();
    }
    private void atualizarBarraXP()
    {
        barraDeXPUI.fillAmount = xpAtual / xpMaximo;
        barraDeXPUI.GetComponentInChildren<TextMeshProUGUI>().text = nivel.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("XP"))
        {
            ganharXP(100);
            GetComponent<AudioSource>().PlayOneShot(sfx);
            Destroy(collision.gameObject);
        }
    }

    private void AbrirProgressaoDeAtributos()
    {
        if (telaDeProgressaoDeAtributos.activeSelf)
        {
            telaDeProgressaoDeAtributos.SetActive(false);
            GetComponent<AtaqueDoJogador>().enabled = true;
            GetComponent<PolygonCollider2D>().enabled = true;
            Time.timeScale = 1;
        }
        else
        {
            telaDeProgressaoDeAtributos.SetActive(true);
            GetComponent<AtaqueDoJogador>().enabled = false;
            GetComponent<PolygonCollider2D>().enabled = false;
            Time.timeScale = 0;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && (!GameObject.FindGameObjectWithTag("Tela") || GameObject.FindGameObjectWithTag("Tela") == telaDeProgressaoDeAtributos))
        {
            AbrirProgressaoDeAtributos();
        }
    }
}
