using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaqueDoJogador : MonoBehaviour
{
    [SerializeField]
    private GameObject projetil;
    void Start()
    {
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(projetil,transform.position,Quaternion.identity).GetComponent<Rigidbody2D>().AddForce(transform.up * 400f);
        }
    }
}
