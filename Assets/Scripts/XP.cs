using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
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
            ganharXP(10);
            Destroy(collision.gameObject);
        }
    }
    private void O(Collider2D collision)
    {

    }


    // Update is called once per frame
    void Update()
    {

    }
}
