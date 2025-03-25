using UnityEngine;
using UnityEngine.Audio;

namespace Managers
{
    public class SettingManager : MonoBehaviour
    {
        [Header("Managers")] 
        [SerializeField] private UiManager uiManager;
        
        [Header("References")]
        [SerializeField] private AudioMixer audioMixer;
        
        // Constants
        private const string SfxVolumeKey = "SfxVolume";
        private const string MusicVolumeKey = "MusicVolume";
        private const float DefaultVolume = 0.7f;
    
        void Start()
        {
            // Get the volume level from the PlayerPrefs or the default one if it's the first gameplay
            var sfxVolume = PlayerPrefs.GetFloat(SfxVolumeKey, DefaultVolume);
            var musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, DefaultVolume);
            
            // Set the sliders accordingly
            uiManager.SetSfxSlider(sfxVolume);
            uiManager.SetMusicSlider(musicVolume);
            
            // Set the actual volume in the mixer
            SetSfxVolume(sfxVolume);
            SetMusicVolume(musicVolume);
        }

        public void SetSfxVolume(float volume)
        {
            // Convert the volume to logarithmic form
            var dbVolume = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20;
            
            // Set the volume in the mixer
            audioMixer.SetFloat(SfxVolumeKey, dbVolume);
            
            // Saving the volume in his linear form (0-1)
            PlayerPrefs.SetFloat(SfxVolumeKey, volume);
        }

        public void SetMusicVolume(float volume)
        {
            // Convert the volume to logarithmic form
            var dbVolume = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20;
            
            // Set the volume in the mixer
            audioMixer.SetFloat(MusicVolumeKey, dbVolume);
            
            // Saving the volume in his linear form (0-1)
            PlayerPrefs.SetFloat(MusicVolumeKey, volume);
        }
    }
}
