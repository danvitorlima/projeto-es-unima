using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AtaqueRanged : MonoBehaviour
{
    public GameObject projetil;
    protected string fonte;
    public float cooldown, duracaoDeTiro, lastAttackTime; // cooldown de ataque
    public int dano;
    public AudioClip tiroSFX;
    protected virtual void Start()
    {
        fonte = gameObject.tag;
    }

    public void AumentarDistanciaDoTiro(float fatorDeCrescimento)
    {
        duracaoDeTiro *= fatorDeCrescimento;
    }

    public void AumentarDano(float fatorDeCrescimento)
    {
        dano = Mathf.RoundToInt(dano * fatorDeCrescimento);
    }

    public void AumentarVelocidadeDeAtaque(float fatorDeCrescimento)
    {
        cooldown /= fatorDeCrescimento;
    }
    public void Atirar()
    {
        var bala = Instantiate(projetil, transform.position + transform.up / 1.6f, Quaternion.Euler(0, 0, transform.localEulerAngles.z + 90));
        bala.GetComponent<Projetil>().determinarFonte(fonte);
        bala.GetComponent<Rigidbody2D>().AddForce(transform.up * 400f);
    }
}

