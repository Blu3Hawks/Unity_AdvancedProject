using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainHero : MonoBehaviour, IDataPersistence
{
    public Vector3 entryPointPosition;

    private void Start()
    {
        if (DataPersistenceManager.instance.GameData.PlayerPosition != Vector3.zero)
        {
            transform.position = DataPersistenceManager.instance.GameData.PlayerPosition;
        }
        else
        {
            transform.position = entryPointPosition;
        }

        Debug.Log(DataPersistenceManager.instance.GameData.PlayerPosition);
    }

    public void LoadData(GameData data)
    {
        if (data.PlayerPosition != Vector3.zero)
        {
            transform.position = data.PlayerPosition;
        }
        else
        {
            transform.position = entryPointPosition;
        }
    }

    public void SaveData(GameData data)
    {
        data.PlayerPosition = transform.position;
        DataPersistenceManager.instance.GameData.PlayerPosition = transform.position;
    }

    private void OnApplicationQuit()
    {
        DataPersistenceManager.instance.GameData.PlayerPosition = transform.position;
        Debug.Log(DataPersistenceManager.instance.GameData.PlayerPosition);
    }

    public void SetEntryPoint(Vector2 entryPoint)
    {
        entryPointPosition = new Vector3(entryPoint.x, transform.position.y, entryPoint.y);
        transform.position = entryPointPosition;
    }
}
