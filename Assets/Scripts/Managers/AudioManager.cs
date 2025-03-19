using System;
using UnityEngine;

namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private GameManager gameManager;

        [Header("Audio Source")] 
        [SerializeField] private AudioSource audioSource;

        [Header("SFX")] 
        [SerializeField] private AudioClip coinPickUp;

        private void Start()
        {
            gameManager.OnCoinPickUp += PlayCoinSfx;
        }

        private void PlayCoinSfx(int coin)
        {
            audioSource.PlayOneShot(coinPickUp);
        }
    }
}
