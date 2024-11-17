using System;
using System.Collections;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SistemaVida : MonoBehaviour
{
    [SerializeField]
    private int vidaMaxima = 100;
    [SerializeField]
    private float vidaAtual;
    [SerializeField]
    private Image barraDeVidaUI;
    [SerializeField]
    GameObject telaGameOver;
    Animator animator;
    private UnityEngine.Color corOriginal;
    [SerializeField]
    private Texture2D texturaCursor;
    private Vector2 cursorHotspot;
    [SerializeField]
    private GameObject xp;
    private TextMeshProUGUI contadorDeVida;

    void Start()
    {
        contadorDeVida = GameObject.FindGameObjectWithTag("Vida").GetComponent<TextMeshProUGUI>();
        corOriginal = gameObject.GetComponent<SpriteRenderer>().color;
        animator = GetComponent<Animator>();
        vidaAtual = vidaMaxima;
        AtualizarBarraDeVida();
    }

    private IEnumerator mudarCorTemporariamenteCorrotina()
    {
        GetComponent<SpriteRenderer>().color = UnityEngine.Color.red;
        yield return new WaitForSeconds(0.2f);
        GetComponent<SpriteRenderer>().color = corOriginal;
    }

    public void AumentarVidaMaxima(float fatorDeCrescimento)
    {
        vidaMaxima = Mathf.RoundToInt(vidaMaxima * fatorDeCrescimento);
        AtualizarBarraDeVida();
    }

    public void ReceberDano(float quantidade)
    {
        vidaAtual -= quantidade;
        vidaAtual = Mathf.Clamp(vidaAtual, 0, vidaMaxima);
        AtualizarBarraDeVida();
        if (vidaAtual <= 0)
        {
            Morrer();
        }
        else
        {
            StartCoroutine(mudarCorTemporariamenteCorrotina());
        }
        
    }

    public bool Curar(int quantidade)
    {
        if (quantidade + vidaAtual <= vidaMaxima)
        {
            vidaAtual += quantidade;
            vidaAtual = Mathf.Clamp(vidaAtual, 0, vidaMaxima);
            AtualizarBarraDeVida();
            return true;
        }
        return false;
    }

    private void AtualizarBarraDeVida()
    {
        if (barraDeVidaUI != null)
        {
            barraDeVidaUI.fillAmount = vidaAtual / vidaMaxima;
            contadorDeVida.text = vidaAtual.ToString()+"/"+vidaMaxima.ToString(); 
        }
    }
    private void Morrer()
    {
        if (gameObject.CompareTag("Inimigo"))
        {
            GetComponent<NavMeshAgent>().enabled = false;
            Instantiate(xp, gameObject.transform.position,Quaternion.identity).transform.parent = null;
        }
        gameObject.GetComponent<Rigidbody2D>().simulated = false;
        if (gameObject.CompareTag("Player"))
        {
            gameObject.GetComponent<MovimentacaoPersonagem>().enabled = false;
            gameObject.GetComponent<AtaqueDoJogador>().enabled = false;
        }
        animator.Play("morte");
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
        if(telaGameOver != null)
        {
            cursorHotspot = new Vector2(texturaCursor.width / 2, texturaCursor.height / 2);
            Cursor.SetCursor(texturaCursor, cursorHotspot, CursorMode.Auto);
            telaGameOver.SetActive(true);
        }
    }
}
