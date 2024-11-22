using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    private Image fadeImage;
    private AudioSource musica;

    private void Start()
    {
        fadeImage = GameObject.FindGameObjectWithTag("Fade").GetComponent<Image>();
        musica = GameObject.FindGameObjectWithTag("Musica").GetComponent<AudioSource>();
    }

    public void VoltarAoMenu()
    {
        PlayerPrefs.SetInt("Nivel", 1);
        PlayerPrefs.Save();
        Time.timeScale = 1;
        FadeOut("MainMenu", 1f);
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
            timer += Time.unscaledDeltaTime;
            fadeImage.color = new Color(0, 0, 0, timer / fadeDuration);
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 1);

        SceneManager.LoadScene(sceneName);
    }
    private IEnumerator FadeOutAudio(float fadeDuration)
    {
        float timer = fadeDuration;
        var volume = musica.volume;
        // Reduz o volume gradualmente
        while (timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            musica.volume = Mathf.Lerp(0, volume, timer / fadeDuration);
            yield return null;
        }
        musica.volume = 0;
    }
}
