using UnityEngine;

namespace Chen_s_Folder.Scripts.Load_Next_Scenes
{
    public class NextRoom : MonoBehaviour
    {
        public delegate void EnteringNextLevelHandler();
        public static event EnteringNextLevelHandler OnEnteringNextLevel;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("new level !");
                OnEnteringNextLevel?.Invoke();
            }
        }
    }
}
