using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Button muteButton;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI muteButtonText;

    private bool isMuted = false;

    private void Start()
    {
        float volume = PlayerPrefs.GetFloat("volume", 1f);
        AudioListener.volume = volume;
        volumeSlider.value = volume;
        isMuted = volume == 0f;
        UpdateMuteButtonText();

        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        muteButton.onClick.AddListener(ToggleMute);
    }

    private void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
        isMuted = value == 0f;
        UpdateMuteButtonText();
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
    }

    private void ToggleMute()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0f : volumeSlider.value;
        volumeSlider.value = AudioListener.volume;
        UpdateMuteButtonText();
        PlayerPrefs.SetFloat("Volume", AudioListener.volume);
        PlayerPrefs.Save();
    }

    private void UpdateMuteButtonText()
    {
        if (muteButtonText != null)
            muteButtonText.text = isMuted ? "Unmute" : "Mute";
    }
}
