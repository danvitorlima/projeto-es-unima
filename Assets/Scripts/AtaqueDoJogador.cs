using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class AtaqueDoJogador : MonoBehaviour
{
    [SerializeField]
    private GameObject projetil;
    [SerializeField]
    private float cooldown; // cooldown de ataque
    private float lastAttackTime;
    [SerializeField]
    private int maxBalasPente = 10; // máximo de balas no pente
    [SerializeField]
    private int maxBalasTotal = 100;
    private int balasRestantes;
    private int balasAtuaisNoPente;
    [SerializeField]
    private TextMeshProUGUI contadorDeBalasPente, contadorDeBalasRestantes;
    [SerializeField]
    private UnityEngine.UI.Image barraDeBalasPente, barraDeBalasRestantes;
    [SerializeField]
    private AudioClip tiroSFX, semRecargaSFX, recargaSFX;
    private bool recarregando = false;
    [SerializeField]
    private float tempoDeRecarga = 2;
    private AudioSource audioSource;
    public float duracaoDeTiro;
    public int dano;
    [SerializeField] RectTransform qtdRestante, qtdPente, barra;

    void Start()
    {
        dano = 50;
        audioSource = GetComponent<AudioSource>();
        resetarMunicao();
        atualizarUI();
    }
    public bool AdicionarMunicao(int quantidade)
    {
        if (quantidade + balasAtuaisNoPente + balasRestantes <= maxBalasTotal)
        {
            balasRestantes += quantidade;
            atualizarUI();
            return true;
        }
        return false;
    }

    public void AumentarDano(float fatorDeCrescimento)
    {
        dano = Mathf.RoundToInt(dano * fatorDeCrescimento);
    }

    public void AumentarMunicaoMaxima(int fatorDeCrescimento = 1, int nivel = 1)
    {
        maxBalasTotal = 50 * (nivel - 1) + 100;
        atualizarUI();
        barra.offsetMin = new Vector2(2177.1f / (1 + ((float)nivel / 6)), 1318f);
    }

    public void AumentarVelocidadeDeAtaque(float fatorDeCrescimento)
    {
        cooldown /= fatorDeCrescimento;
    }
    public void DiminuirTempoDeRecarga(float fatorDeCrescimento)
    {
        tempoDeRecarga /= fatorDeCrescimento;
    }

    void atualizarUI(bool mudouBalasRestantes = true)
    {
        float porcentagemBalasRestantes = (float)balasRestantes / maxBalasTotal;
        float porcentagemBalasPente = porcentagemBalasRestantes + ((float)balasAtuaisNoPente / maxBalasTotal);
        barraDeBalasPente.fillAmount = porcentagemBalasPente;
        //Debug.Log(porcentagemBalasPente * 100 + "%%");
        //Debug.Log(porcentagemBalasRestantes * 100 + "%");

        if (balasAtuaisNoPente == 0)
        {
            contadorDeBalasPente.text = null;
        }
        else
        {
            contadorDeBalasPente.text = balasAtuaisNoPente.ToString();
        }
        if (mudouBalasRestantes)
        {
            //if (porcentagemBalasRestantes == 0)
            //{
            //    contadorDeBalasRestantes.rectTransform.anchoredPosition = new Vector3(-15.05f, contadorDeBalasRestantes.rectTransform.anchoredPosition.y, 0);
            //}
            //else
            //{
            //    contadorDeBalasRestantes.rectTransform.anchoredPosition = new Vector3(302.625f * porcentagemBalasRestantes - 296.5625f, contadorDeBalasRestantes.transform.localPosition.y, 0);
            //}
            //contadorDeBalasPente.rectTransform.anchoredPosition = new Vector3(587.78f * (((float)balasRestantes / maxBalasTotal) + ((float)maxBalasPente / maxBalasTotal)) - 324.98f, contadorDeBalasRestantes.rectTransform.anchoredPosition.y, 0);

            //ajustando texto com barra de munição
            barraDeBalasRestantes.fillAmount = porcentagemBalasRestantes;
            barraDeBalasPente.fillAmount = porcentagemBalasPente;

            if (porcentagemBalasRestantes < 0.3)
            {
                qtdRestante.anchorMax = new Vector2(1, 1);
            }
            else
            {
                qtdRestante.anchorMax = new Vector2(porcentagemBalasRestantes, 1);
            }


            qtdPente.anchorMin = new Vector2(porcentagemBalasRestantes, 0);
            qtdPente.anchorMax = new Vector2(((float)balasRestantes+maxBalasPente) / maxBalasTotal, 1);


            qtdPente.anchoredPosition = Vector2.zero;
            qtdRestante.anchoredPosition = Vector2.zero;

            contadorDeBalasRestantes.text = balasRestantes.ToString() + "/" + maxBalasTotal;
            barraDeBalasRestantes.fillAmount = porcentagemBalasRestantes;
        }
    }
    IEnumerator Recarregar()
    {
        var corOriginalBarra = barraDeBalasRestantes.GetComponent<UnityEngine.UI.Image>().color;
        var corOriginalTexto = contadorDeBalasRestantes.GetComponent<TextMeshProUGUI>().color;
        barraDeBalasRestantes.GetComponent<UnityEngine.UI.Image>().color = new Color(1.0f, 0.7569f, 0.0275f);
        contadorDeBalasRestantes.GetComponent<TextMeshProUGUI>().color = new Color(0.3059f, 0.3059f, 0.3059f);
        audioSource.PlayOneShot(recargaSFX);
        recarregando = true;
        audioSource.clip = tiroSFX;
        yield return new WaitForSeconds(tempoDeRecarga);
        contadorDeBalasRestantes.GetComponent<TextMeshProUGUI>().color = corOriginalTexto;
        barraDeBalasRestantes.GetComponent<UnityEngine.UI.Image>().color = corOriginalBarra;
        recarregando = false;
        if (balasRestantes >= maxBalasPente)
        {
            balasAtuaisNoPente = maxBalasPente;
        }
        else
        {
            balasAtuaisNoPente = balasRestantes;
        }
        balasRestantes -= balasAtuaisNoPente;
        atualizarUI();

    }
    public void resetarMunicao()
    {
        audioSource.clip = tiroSFX;
        balasAtuaisNoPente = maxBalasPente;
        balasRestantes = maxBalasTotal - maxBalasPente;
        atualizarUI();
    }




    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            //rectTransform.anchorMin = new Vector2(0f, 0.5f); // Âncora esquerda
            //rectTransform.anchorMax = new Vector2(0f, 0.5f); // Âncora fixa à esquerda
        }
        if (Input.GetKeyDown(KeyCode.R) && balasRestantes > 0 && !recarregando)
        {
            StartCoroutine(Recarregar());
        }
        if (Input.GetMouseButtonDown(0) && Time.time - lastAttackTime >= cooldown && !recarregando)
        {
            lastAttackTime = Time.time;
            if (balasAtuaisNoPente > 0)
            {
                var bala = Instantiate(projetil, transform.position + transform.up / 1.6f, Quaternion.Euler(0, 0, transform.localEulerAngles.z + 90));
                balasAtuaisNoPente--;
                atualizarUI(false);
                bala.GetComponent<Rigidbody2D>().AddForce(transform.up * 400f);
            }
            else if (balasRestantes > 0)
            {
                StartCoroutine(Recarregar());
                return;
            }
            else if (audioSource.clip != semRecargaSFX)
            {
                audioSource.clip = semRecargaSFX;
            }
            GetComponent<AudioSource>().Play();
        }
    }
}
