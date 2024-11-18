using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AtaqueDoInimigo : MonoBehaviour
{
    [SerializeField]
    private GameObject projetil;
    [SerializeField]
    private float cooldown, duracaoDeTiro, lastAttackTime; // cooldown de ataque
    public int dano;
    [SerializeField]
    private AudioClip tiroSFX;
    private AudioSource audioSource;

    void Start()
    {
        lastAttackTime = cooldown+1;
        dano = 50;
        audioSource = GetComponent<AudioSource>();
    }

    public void AumentarDano(float fatorDeCrescimento)
    {
        dano = Mathf.RoundToInt(dano * fatorDeCrescimento);
    }

    public void AumentarVelocidadeDeAtaque(float fatorDeCrescimento)
    {
        cooldown /= fatorDeCrescimento;
    }

    void Update()
    {
        if (Time.time - lastAttackTime >= cooldown && GetComponent<PatrulhaInimigo>().patrulha == false)
        {
            lastAttackTime = Time.time;
            var bala = Instantiate(projetil, transform.position + transform.up / 1.6f, Quaternion.Euler(0, 0, transform.localEulerAngles.z + 90));
            bala.GetComponent<Rigidbody2D>().AddForce(transform.up * 400f);
            GetComponent<AudioSource>().Play();
        }
    }
}
