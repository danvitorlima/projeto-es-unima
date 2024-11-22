using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class VoidManager : MonoBehaviour
{
    public Image fadeImage;
    public AudioSource audioSource;
    public PostProcessVolume ppv;
    private List<KeyCode[]> cheats = new List<KeyCode[]>();
    private KeyCode[] boss = { KeyCode.B, KeyCode.O, KeyCode.S, KeyCode.S };
    private KeyCode[] menu = { KeyCode.M, KeyCode.E, KeyCode.N, KeyCode.U };
    private KeyCode[] vidaInfinita = { KeyCode.V, KeyCode.O, KeyCode.I, KeyCode.D };
    private KeyCode[] spoiler = { KeyCode.S, KeyCode.P, KeyCode.O, KeyCode.I, KeyCode.L, KeyCode.E, KeyCode.R };
    private KeyCode[][] teste;
    private float pressTime = 0;
    int i = 0;
    int x;
    [SerializeField] AudioClip sfxCheat;
    [SerializeField] private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer.url = Path.Combine(Application.streamingAssetsPath, "void.webm");
        teste = null;
        cheats.Add(boss);
        cheats.Add(menu);
        cheats.Add(vidaInfinita);
        cheats.Add(spoiler);
        ppv.enabled = PlayerPrefs.GetInt("Graficos") == 1;
        Cursor.visible = false;
        StartCoroutine(FadeInAudio(5f));
        StartCoroutine(FadeIn(4f));
    }
    private IEnumerator FadeIn(float fadeDuration)
    {
        float timer = fadeDuration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, timer / fadeDuration);
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 0);
    }
    private IEnumerator FadeInAudio(float fadeDuration)
    {
        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0,  PlayerPrefs.GetFloat("Musica"), timer / fadeDuration);
            yield return null;
        }
        audioSource.volume = PlayerPrefs.GetFloat("Musica");
    }

    private void CheatVidaInfinita()
    {
        PlayerPrefs.SetInt("VidaInfinita", PlayerPrefs.GetInt("VidaInfinita", 0) == 1 ? 0 : 1);
        PlayerPrefs.Save();
        Debug.Log("vida infinita: " + PlayerPrefs.GetInt("VidaInfinita"));
    }

    private void CheatFimDoJogo()
    {
        SceneManager.LoadScene("FimDeJogo");
    }

    private KeyCode[] Cheat()
    {
        //if (teste != null)
        //{
        //    Debug.Log(string.Join(", ", teste.Select(x => string.Join("", x))));
        //}
        if (teste != null && teste.Any())
        {
            if (i >= teste[0].Length)
            {
                if (teste.Length == 1 || Time.time - pressTime >= 1)
                {
                    audioSource.PlayOneShot(sfxCheat);
                    //cheat ativado
                    var resultado = teste[0];
                    i = 0;
                    teste = null;
                    return resultado;
                }
            }
        }
        else if (i > 0)
        {
            i = 0;
        }
        if (Input.anyKeyDown)
        {
            pressTime = Time.time;
            if (i == 0)
            {
                teste = cheats.Where(x => Input.GetKeyDown(x[0])).ToArray();
                if (teste.Any())
                {
                    i++;
                }
            }
            else
            {
                teste = teste.Where(x => x.Count() > i && Input.GetKeyDown(x[i])).ToArray();
                i++;
            }
        }
        return null;
    }

    private void CheatMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void Update()
    {
        x = cheats.IndexOf(Cheat());
        switch (x)
        {
            default:
                Debug.Log(x);
                break;
            case 1:
                CheatMenu();
                break;
            case 2:
                CheatVidaInfinita();
                break;
            case 3:
                CheatFimDoJogo();
                break;
            case -1:
                break;
        }
    }
}
