using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField]
    private GameObject telaPause;
    private GameObject jogador;
    [SerializeField] private CursorManager cursorManager;

    void Start()
    {
        jogador = GameObject.FindGameObjectWithTag("Player");
    }

    public void Pausar(bool pausando)
    {
        telaPause.SetActive(pausando);
        jogador.GetComponent<AtaqueDoJogador>().enabled = !pausando;
        Time.timeScale = pausando ? 0 : 1;
        cursorManager.FecharTela();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && (!GameObject.FindGameObjectWithTag("Tela") || GameObject.FindGameObjectWithTag("Tela") == telaPause))
        {
            if (telaPause.activeSelf)
            {
                telaPause.SetActive(false);
                jogador.GetComponent<AtaqueDoJogador>().enabled = true;
                Time.timeScale = 1;

            }
            else
            {
                telaPause.SetActive(true);
                jogador.GetComponent<AtaqueDoJogador>().enabled = false;
                Time.timeScale = 0;
            }
        }
    }
}
