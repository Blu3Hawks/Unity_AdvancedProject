using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

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
