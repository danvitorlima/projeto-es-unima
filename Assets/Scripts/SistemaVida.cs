using UnityEngine;
using UnityEngine.UI;

public class SistemaVida : MonoBehaviour
{
    public float vidaMaxima = 100f;
    public float vidaAtual;
    public Image barraDeVidaUI;

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
        Destroy(gameObject);
    }
}
