using System;
using System.Collections;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class SistemaVida : MonoBehaviour
{
    [SerializeField]
    private float vidaMaxima = 100f;
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

    void Start()
    {
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

    public void Curar(float quantidade)
    {
        vidaAtual += quantidade;
        vidaAtual = Mathf.Clamp(vidaAtual, 0, vidaMaxima);
        AtualizarBarraDeVida();
    }

    private void AtualizarBarraDeVida()
    {
        if (barraDeVidaUI != null)
        {
            barraDeVidaUI.fillAmount = vidaAtual / vidaMaxima;
        }
    }
    private void Morrer()
    {
        if (gameObject.CompareTag("Inimigo"))
        {
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
