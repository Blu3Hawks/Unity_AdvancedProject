using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float movingSpeed;
    [SerializeField] private NavMeshAgent agent;

    private void Start()
    {
        agent.speed = movingSpeed;
    }
}
