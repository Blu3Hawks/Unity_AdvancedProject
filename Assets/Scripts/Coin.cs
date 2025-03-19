using System;
using Managers;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public event Action<Coin> OnCollected;
    
    [SerializeField] private int scoreAmount = 10;

    [SerializeField] private LevelManager levelManager;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        OnCollected?.Invoke(this);
        
        levelManager.RemoveCoin(this);
        
        Destroy(gameObject);
    }

    public int GetScore()
    {
        return scoreAmount;
    }
}
