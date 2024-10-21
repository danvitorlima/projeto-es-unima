using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassarDeNivel : MonoBehaviour
{
    private GameObject dungeonGenerator;
    void Start()
    {
        dungeonGenerator = GameObject.FindWithTag("GeradorDeNivel");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.transform.position = Vector3.zero;
        dungeonGenerator.GetComponent<CorridorFirstDungeonGenerator>().GenerateDungeon();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
