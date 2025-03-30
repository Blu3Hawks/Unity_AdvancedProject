using UnityEngine;

namespace Managers
{
    public class GameplayAudioManager : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private GameManager gameManager;
        
        [Header("Audio Source")] 
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;
        
        [Header("Clips")] 
        [SerializeField] private AudioClip soundTrack1;
        [SerializeField] private AudioClip parrySfx;

        public void PlayParrySfx(float damageMultiplier)
        {
            sfxSource.PlayOneShot(parrySfx);
        }
    }
}