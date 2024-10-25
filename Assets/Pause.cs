using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField]
    private GameObject telaPause;
    private GameObject jogador;
    void Start()
    {
        jogador = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
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
