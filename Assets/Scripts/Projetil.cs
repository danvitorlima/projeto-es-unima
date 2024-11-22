using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projetil : MonoBehaviour
{
    private Animator animator;
    public double duracao;
    public int dano;
    private double momentoDoDisparo;
    private AudioSource sfx;
    private string adversario;
    private void Start()
    {
        sfx = GameObject.FindGameObjectWithTag("SFX").GetComponent<AudioSource>();
        momentoDoDisparo = Time.time;
        animator = GetComponent<Animator>();
    }

    public void determinarFonte(string fonte)
    {
        duracao = GameObject.FindGameObjectWithTag(fonte).GetComponent<AtaqueRanged>().duracaoDeTiro;
        dano = GameObject.FindGameObjectWithTag(fonte).GetComponent<AtaqueRanged>().dano;
        if (fonte == "Player")
        {
            adversario = "Inimigo";
        }
        else
        {
            adversario = "Player";
        }
    }
    private void Update()
    {
        if (Time.time - momentoDoDisparo >= duracao)
        {
            DestruirProjetil();
        }
    }
    private void DestruirProjetil()
    {
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        animator.Play("explosao");
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Parede"))
        {
            DestruirProjetil();
        }
        if (collision.gameObject.CompareTag(adversario))
        {
            sfx.Play();
            collision.gameObject.GetComponent<SistemaVida>().ReceberDano(dano);
            DestruirProjetil();
        }
    }
}
