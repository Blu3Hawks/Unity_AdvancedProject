using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private DungeonLevelGenerator dungeonGenerator;

    public void BackToMainMenu()
    {
        dungeonGenerator.SaveValues();
        Application.Quit();
    }
}
