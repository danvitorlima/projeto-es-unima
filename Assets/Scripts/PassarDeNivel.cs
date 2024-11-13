using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PassarDeNivel : MonoBehaviour
{
    private GameObject dungeonGenerator;

    void Start()
    {
        dungeonGenerator = GameObject.FindWithTag("GeradorDeNivel");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            dungeonGenerator.GetComponent<CorridorFirstDungeonGenerator>().GenerateDungeon();
            GameObject.FindGameObjectWithTag("Player").GetComponent<AtaqueDoJogador>().resetarMunicao();
        }
    }
}
