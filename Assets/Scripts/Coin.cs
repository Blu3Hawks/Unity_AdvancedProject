using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public event Action<Coin> OnCollected;
    
    [SerializeField] private int scoreAmount = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        OnCollected?.Invoke(this);
        
        Destroy(gameObject);
    }

    public int GetScore()
    {
        return scoreAmount;
    }
}
