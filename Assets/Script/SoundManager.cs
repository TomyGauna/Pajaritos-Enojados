using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;

    // Referencia al slider en la interfaz de usuario
    public Slider volumeSlider;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Busca el slider en la interfaz de usuario si no está asignado
        if (volumeSlider == null)
        {
            volumeSlider = FindObjectOfType<Slider>();
        }

        // Configura el valor inicial del slider al volumen actual solo si hay un slider asignado
        if (volumeSlider != null)
        {
            volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
            SetVolume(volumeSlider.value);
        }
    }

    public void SetVolume(float volume)
    {
        // Actualiza el volumen según el valor del slider
        if (volumeSlider != null)
        {
            volumeSlider.value = volume;
        }

        // Actualiza el volumen de la música
        PlayerPrefs.SetFloat("Volume", volume);
        AudioListener.volume = volume;
    }
}
