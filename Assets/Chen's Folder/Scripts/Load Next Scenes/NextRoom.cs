using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextRoom : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadSceneAsync(GetNextSceneName());
        }
    }



    private string GetNextSceneName()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        string nextScene;
        if (currentSceneName == "Dungeon 1")
        {
            nextScene = "Dungeon 2"; 
        }
        else
        {
            nextScene = "Dungeon 1";
        }
        return nextScene;
    }
}
