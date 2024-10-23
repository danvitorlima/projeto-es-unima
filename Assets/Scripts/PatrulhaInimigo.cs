using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using static UnityEditor.Progress;

public class PatrulhaInimigo : MonoBehaviour
{
    [SerializeField]
    private bool patrulha;
    Rigidbody2D rb;
    Vector3[] pontosDePatrulha;
    Vector3 ppAtual;
    int index;
    void Start()
    {
        index = 0;
        pontosDePatrulha = EncontrarProximosPPs();
        ppAtual = pontosDePatrulha[index];
        rb = GetComponent<Rigidbody2D>();
        patrulha = true;
    }


    private void MoverInimigo(GameObject alvo)
    {
        gameObject.transform.up = (alvo.transform.position - gameObject.transform.position).normalized;
        rb.MovePosition(Vector3.MoveTowards(rb.position, alvo.transform.position, 2 * Time.deltaTime));
    }
    private void MoverInimigo(Collider2D alvo)
    {
        gameObject.transform.up = (alvo.transform.position - gameObject.transform.position).normalized;
        rb.MovePosition(Vector3.MoveTowards(rb.position, alvo.transform.position, 2 * Time.deltaTime));
    }
    private void MoverInimigo(Vector3 alvo)
    {
        gameObject.transform.up = (alvo - gameObject.transform.position).normalized;
        rb.MovePosition(Vector3.MoveTowards(rb.position, alvo, 2 * Time.deltaTime));
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.AddForce(-transform.up * 5000);

        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            patrulha = false;
            index = 0;
        }
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            patrulha = false;
            MoverInimigo(collision);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            patrulha = true;
            pontosDePatrulha = EncontrarProximosPPs();
            ppAtual = pontosDePatrulha[index];
        }
        
    }

    private GameObject EncontrarMenorDistancia(GameObject objeto, GameObject[] lista)
    {
        float menorDistancia = Mathf.Infinity;
        GameObject objetoProximo = lista[0];
        foreach (var item in lista)
        {
            var distanciaAtual = Vector3.Distance(item.transform.position, gameObject.transform.position);
            if (distanciaAtual < menorDistancia)
            {
                menorDistancia = distanciaAtual;
                objetoProximo = item;
            }
        }
        return objetoProximo;
    }
    private Vector3[] EncontrarProximosPPs()
    {
        var pps = GameObject.FindGameObjectsWithTag("PP");
        var listaDePPs = pps.ToList().OrderBy(p => Vector3.Distance(p.transform.position,transform.position)).ToList();
        string a = "A";
        string b = "B";
        foreach (var item in listaDePPs)
        {
            a += item.name + ",";
        }
        foreach (var item in pps)
        {
            b += item.name + ",";
        }
        Debug.Log(a);
        Debug.Log(b);
        return listaDePPs.Select(n => n.transform.position).ToArray();
        //Vector2 direcao = ppProximo.transform.position - gameObject.transform.position;
        //Debug.Log(direcao.magnitude);
        //rb.MovePosition(Vector3.MoveTowards(rb.position, ppProximo.transform.position, 2 * Time.deltaTime));
        //if (direcao.magnitude != 0)
        //{
        //    gameObject.transform.up = direcao.normalized;
        //}
    }
    void FixedUpdate()
    {
        //Debug.Log(index);
        if (patrulha)
        {
            MoverInimigo(ppAtual);
            if (rb.position.Equals(ppAtual))
            {
                if (index != pontosDePatrulha.Length-1)
                {
                    index++;
                    ppAtual = pontosDePatrulha[index];
                }
                else
                {
                    index = 0;
                    ppAtual = pontosDePatrulha[index];
                }
            }
        }
    } 
}
