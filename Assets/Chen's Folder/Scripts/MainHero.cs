using UnityEngine;

public class MainHero : MonoBehaviour, IDataPersistence
{
    public void LoadData(GameData data)
    {
        // Load the character's position from the saved data
        transform.position = data.characterPosition;
    }

    public void SaveData(GameData data)
    {
        // Save the character's current position to the game data
        data.characterPosition = transform.position;
    }
}
