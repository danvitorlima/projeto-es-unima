using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projetil : MonoBehaviour
{
    private Animator animator;
    private double duracao;
    private double momentoDoDisparo;
    private AudioSource sfx;
    private void Start()
    {
        sfx = GameObject.FindGameObjectWithTag("SFX").GetComponent<AudioSource>();
        duracao = GameObject.FindGameObjectWithTag("Player").GetComponent<AtaqueDoJogador>().duracaoDeTiro;
        momentoDoDisparo = Time.time;
        animator = GetComponent<Animator>();
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
        if (collision.gameObject.CompareTag("Inimigo"))
        {
            sfx.Play();
            collision.gameObject.GetComponent<SistemaVida>().ReceberDano(50);
            DestruirProjetil();
        }
        else if (collision.gameObject.CompareTag("Parede"))
        {
            DestruirProjetil();
        }
    }
}
