using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject painelMenuInicial, painelOpcoes;
    [SerializeField] private Slider sfx;
    [SerializeField] private Slider musica;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private PostProcessVolume ppv;
    [SerializeField] private AudioSource sfxAudioSource;
    public Image fadeImage;
    private bool emConfiguracoes;
    [SerializeField] private AudioClip sfxSair;

    private void Start()
    {
        Cursor.visible = true;
        ppv.enabled = PlayerPrefs.GetInt("Graficos", 0) == 1;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        sfx.value = PlayerPrefs.GetFloat("Volume", sfx.value);
        musica.value = PlayerPrefs.GetFloat("Musica", musica.value);
        audioSource.volume = musica.value;
        sfxAudioSource.volume = sfx.value;
    }


    public void SalvarConfiguracoes()
    {
        sfxAudioSource.Play();
        PlayerPrefs.SetFloat("Volume", sfx.value);
        PlayerPrefs.Save();
        PlayerPrefs.SetFloat("Musica", musica.value);
        PlayerPrefs.Save();
    }

    public void Jogar()
    {
        sfxAudioSource.Play();
        PlayerPrefs.SetInt("Nivel", 1);
        PlayerPrefs.Save();
        FadeOut("Jogo", 2f);
    }

    public void AbrirOpcoes()
    {
        sfxAudioSource.Play();
        emConfiguracoes = true;
        painelMenuInicial.SetActive(false);
        painelOpcoes.SetActive(true);

    }

    public void FecharOpcoes()
    {
        sfxAudioSource.Play();
        emConfiguracoes = false;
        painelOpcoes.SetActive(false);
        painelMenuInicial.SetActive(true);
    }

    public void SairJogo()
    {
        sfxAudioSource.PlayOneShot(sfxSair);
        FadeOut("void", 2f);
    }
    private void Update()
    {
        if (emConfiguracoes)
        {
            audioSource.volume = musica.value;
            sfxAudioSource.volume = sfx.value;
        }
    }
    private void FadeOut(string sceneName, float duracao)
    {
        StartCoroutine(FadeOutVideo(sceneName, duracao));
        StartCoroutine(FadeOutAudio(duracao));
    }
    private IEnumerator FadeOutVideo(string sceneName, float fadeDuration)
    {
        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, timer / fadeDuration);
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 1);

        SceneManager.LoadScene(sceneName);
    }
    private IEnumerator FadeOutAudio(float fadeDuration)
    {
        float timer = fadeDuration;

        // Reduz o volume gradualmente
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0, PlayerPrefs.GetFloat("Musica"), timer / fadeDuration);
            yield return null;
        }

        audioSource.volume = 0;
        audioSource.Stop();
    }
}
