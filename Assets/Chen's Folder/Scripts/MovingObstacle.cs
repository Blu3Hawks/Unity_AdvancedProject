using UnityEngine;


public class MovingObstacle : MonoBehaviour
{
    [Header("Speed Variables")]
    [SerializeField] private float movingSpeed;
    [SerializeField] private Vector3 movementVector;

    //differential numbers for moving the platformers
    private Vector3 startingPosition;


    private void Awake()
    {
        startingPosition = transform.position;
    }

    private void Update()
    {
        LerpMovement();
    }

    private void LerpMovement()
    {
        float cycles = Time.time * movingSpeed;

        const float tau = Mathf.PI * 2;
        float angle = cycles * tau;

        float newXPosition = Mathf.Cos(angle) * movementVector.x;
        float newYPosition = Mathf.Sin(angle) * movementVector.y;
        float newZPosition = Mathf.Sin(angle) * movementVector.z;

        // Apply offset
        transform.position = startingPosition + new Vector3(newXPosition, newYPosition, newZPosition);
    }
}
