using UnityEngine;
using UnityEngine.SceneManagement;

public class NextRoom : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetToNextLevel();
        }
    }

    private void GetToNextLevel()
    {
        Scene newScene = SceneManager.CreateScene("Level: " + 3.ToString()); //don't forget to also add an int here
        SceneManager.LoadScene(newScene.name);
    }
}
