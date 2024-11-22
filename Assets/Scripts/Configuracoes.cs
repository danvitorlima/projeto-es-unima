using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Configuracoes : MonoBehaviour
{
    [SerializeField] private Slider sfx, musica;
    [SerializeField] private AudioSource musicaSource;

    private void Start()
    {
        sfx.value = PlayerPrefs.GetFloat("Volume", sfx.value);
        musica.value = PlayerPrefs.GetFloat("Musica", musica.value);
    }
    public void SalvarConfiguracoes()
    {
        PlayerPrefs.SetFloat("Volume", sfx.value);
        PlayerPrefs.Save();
        PlayerPrefs.SetFloat("Musica", musica.value);
        PlayerPrefs.Save();
        foreach (var audioSource in GameObject.FindObjectsOfType<AudioSource>().Where(x => x.CompareTag("Musica") == false))
        {
            audioSource.volume = sfx.value;
        }
    }
    private void Update()
    {
        if (musica.isActiveAndEnabled)
        {
            musicaSource.volume = 0.5f * musica.value;
        }
    }
}
