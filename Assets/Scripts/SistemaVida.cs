using UnityEngine;
using UnityEngine.UI;

public class SistemaVida : MonoBehaviour
{
    public float vidaMaxima = 100f;
    public float vidaAtual;
    public Image barraDeVidaUI;
    [SerializeField]
    GameObject telaGameOver;
    void Start()
    {
        vidaAtual = vidaMaxima;
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Inimigo"))
        {
            ReceberDano(10);
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
        if (gameObject.CompareTag("Player"))
        {
            telaGameOver.SetActive(true);
        }
        Destroy(gameObject);
    }
}
