using UnityEngine;


public class MovingObstacle : MonoBehaviour
{
    [Header("Speed Variables")]
    [SerializeField] private float movingSpeed;
    [SerializeField] private float accelerationSpeed;

    [Header("Distance Variables")]
    [SerializeField] private float distanceToTravelXAxis;
    [SerializeField] private float distanceToTravelYAxis;

    [Header("Axis To Move On")]
    [SerializeField] private bool moveOnXAxis;
    [SerializeField] private bool moveOnYAxis;

    //differential numbers for moving the platformers

    private int _currentXDirectionModifier = 1;
    private Vector3 _minimumXPositionToReach;
    private Vector3 _maximumXPositionToReach;
    private Vector3 _minimumYPositionToReach;
    private Vector3 _maximumYPositionToReach;

    private void Awake()
    {
        if (moveOnXAxis)
        {
            _minimumXPositionToReach = new Vector3(transform.position.x + distanceToTravelXAxis, transform.position.y, transform.position.z);
            _maximumXPositionToReach = new Vector3(transform.position.x - distanceToTravelXAxis, transform.position.y, transform.position.z);
        }

        if (moveOnYAxis)
        {
            _minimumYPositionToReach = new Vector3(transform.position.x, transform.position.y + distanceToTravelXAxis, transform.position.z);
            _maximumYPositionToReach = new Vector3(transform.position.x, transform.position.y - distanceToTravelXAxis, transform.position.z);
        }
    }

    private void Update()
    {

    }

    private void LerpFromLeftToRight()
    {

    }
}
