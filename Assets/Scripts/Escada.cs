using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Escada : MonoBehaviour
{
    private CorridorFirstDungeonGenerator dungeonGenerator;
    private AudioSource[] audioSources;
    private Image fadeImage;

    void Start()
    {
        fadeImage = GameObject.FindGameObjectWithTag("Fade").GetComponent<Image>();
        Debug.Log(fadeImage.name);
        audioSources = GameObject.FindObjectsOfType<AudioSource>();
        dungeonGenerator = GameObject.FindWithTag("GeradorDeNivel").GetComponent<CorridorFirstDungeonGenerator>();
    }
    private void GerarNovoNivel()
    {
        dungeonGenerator.minInimigosPorSala *= 3;
        dungeonGenerator.maxInimigosPorSala *= 4;
        dungeonGenerator.minItensPorSala *= 2;
        dungeonGenerator.maxItensPorSala *= 2;
        dungeonGenerator.GenerateDungeon();
        Time.timeScale = 1;
        GameObject.FindGameObjectWithTag("Player").GetComponent<AtaqueDoJogador>().resetarMunicao();
        foreach (var inimigo in GameObject.FindGameObjectsWithTag("Inimigo"))
        {
            inimigo.GetComponent<SistemaVida>().AumentarVidaMaxima(2f);
            if (inimigo.GetComponent<AtaqueDoInimigo>())
            {
                inimigo.GetComponent<AtaqueDoInimigo>().AumentarDano(1.5f);
                inimigo.GetComponent<AtaqueDoInimigo>().AumentarVelocidadeDeAtaque(2f);
            }
            inimigo.GetComponent<NavMeshAgent>().speed *= 1.5f;
        }
    }
    private void PassarDeNivel()
    {
        StartCoroutine(FadeOutAudio(2f));
        StartCoroutine(FadeOutVideo(2f));
        //FadeOut(2f);
        PlayerPrefs.SetInt("Nivel", PlayerPrefs.GetInt("Nivel") + 1);
        PlayerPrefs.Save();
        Debug.Log("NIVEL: " + PlayerPrefs.GetInt("Nivel"));
    }
    private void FimDeJogo()
    {
        FadeOut(2f, "FimDeJogo");
    }

    private void FadeOut(float duracao, string sceneName = null)
    {
        StartCoroutine(FadeOutAudio(duracao));
        StartCoroutine(FadeOutVideo(duracao, sceneName));
    }
    private IEnumerator FadeOutVideo(float fadeDuration, string sceneName = null)
    {
        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            fadeImage.color = new Color(0, 0, 0, timer / fadeDuration);
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 1);

        if (sceneName != null)
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            GerarNovoNivel();
        }
    }
    private IEnumerator FadeOutAudio(float fadeDuration)
    {
        float timer = fadeDuration;
        var volumes = audioSources.Select(x => x.volume).ToArray();
        int i = 0;
        while (timer > 0)
        {
            i = 0;
            timer -= Time.unscaledDeltaTime;
            foreach (var audioSource in audioSources)
            {
                audioSource.volume = Mathf.Lerp(0, volumes[i], timer / fadeDuration);
                i++;
            }
            yield return null;
        }
        foreach (var audioSource in audioSources)
        {
            audioSource.volume = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (PlayerPrefs.GetInt("Nivel") == 3)
            {
                FimDeJogo();
            }
            else
            {
                Time.timeScale = 0;
                PassarDeNivel();
            }
        }
    }
}
