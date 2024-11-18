using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject painelMenuInicial;
    [SerializeField] private GameObject painelOpcoes;
    [SerializeField] private Slider volume;
    [SerializeField] private Slider sensibilidade;

    private void Start()
    {
        volume.value = PlayerPrefs.GetFloat("Volume", volume.value);
        sensibilidade.value = PlayerPrefs.GetFloat("Sensibilidade", sensibilidade.value);
    }

    public void SalvarConfiguracoes()
    {
        PlayerPrefs.SetFloat("Volume", volume.value);
        PlayerPrefs.SetFloat("Sensibilidade", sensibilidade.value);
        PlayerPrefs.Save();
    }

    public void Jogar()
    {
        SceneManager.LoadScene("Jogo");
    }

    public void AbrirOpcoes()
    {
        painelMenuInicial.SetActive(false);
        painelOpcoes.SetActive(true);
    }

    public void FecharOpcoes()
    {
        painelOpcoes.SetActive(false);
        painelMenuInicial.SetActive(true);
    }

    public void SairJogo()
    {
        Application.Quit();
    }
}
