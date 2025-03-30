using UnityEngine;

namespace Managers
{
    public class MainMenuAudioManager : MonoBehaviour
    {
        [Header("Audio Source")] 
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("Clips")] 
        [SerializeField] private AudioClip menuMusic1;
        [SerializeField] private AudioClip buttonPress;

        private void Start()
        {
            musicSource.clip = menuMusic1;
            musicSource.Play();
        }

        public void OnButtonPress()
        {
            sfxSource.PlayOneShot(buttonPress);
        }
    }
}
