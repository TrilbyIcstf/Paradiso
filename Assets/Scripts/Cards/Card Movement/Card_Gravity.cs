using UnityEngine;

public class Card_Gravity : MonoBehaviour
{
    private const float MAX_DISTANCE = 2.5f;
    private const float SNAP_DISTANCE = 0.005f;
    private const float FLOAT_DISTANCE = 3.5f;
    private const float DRAG = 3.0f;

    /// <summary>
    /// The point of gravity this card will move towards
    /// </summary>
    private Vector2 gravityPoint = Vector2.zero;

    /// <summary>
    /// The force being applied to the card that moves the gravity point
    /// </summary>
    private Vector2 gravityPointForce = Vector2.zero;

    [SerializeField]
    private CardMovementType movementType = CardMovementType.FLOAT;

    /// <summary>
    /// The strength of the gravity effect acting on this card
    /// </summary>
    private float gravityStrength = 20f;

    /// <summary>
    /// The initial position of the card before being interacted with
    /// </summary>
    private Vector2 initialPosition = Vector2.zero;

    /// <summary>
    /// Determines if the card's position is locked
    /// </summary>
    private bool isLocked = false;

    private void Awake()
    {
        this.gravityPoint = this.transform.position;
        this.initialPosition = this.gravityPoint;
    }

    private void FixedUpdate()
    {
        switch (this.movementType)
        {
            case CardMovementType.SNAP:
                SnapMove();
                break;
            case CardMovementType.LOOSESNAP:
                LooseSnapMove();
                break;
            case CardMovementType.FLOAT:
                MoveGravityPoint();
                FloatMove();
                CalculateGravity();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Snaps card position to gravity point
    /// </summary>
    public void SnapMove()
    {
        this.transform.position = this.gravityPoint;
    }

    /// <summary>
    /// Moves card towards its gravity point, snapping it to a maximum distance if necessary
    /// </summary>
    public void LooseSnapMove()
    {
        float timeInc = Time.deltaTime;

        // Checks distance to gravity point
        float gravityDistance = Vector2.Distance(this.transform.position, this.gravityPoint);
        if (gravityDistance >= 0.15f)
        {
            // Calculates distance away from max distance
            float snapForce = gravityDistance - MAX_DISTANCE;
            
            // Card further from its gravity point should have a stronger float
            float distanceMargin = Mathf.Max(Mathf.Min(gravityDistance / MAX_DISTANCE, 1.0f), 0.1f);
            float gravityForce = Mathf.Pow(Mathf.Sin(distanceMargin * Mathf.PI / 2), 2) * this.gravityStrength * timeInc;
            
            // Determines if the snap or float would move the card further
            float higherForce = Mathf.Max(snapForce, gravityForce);

            this.transform.position = Vector2.MoveTowards(this.transform.position, this.gravityPoint, higherForce);
        }
    }

    /// <summary>
    /// Moves card toward gravity point
    /// </summary>
    public void FloatMove()
    {
        float timeInc = Time.deltaTime;

        // Checks distance to gravity point
        float gravityDistance = Vector2.Distance(this.transform.position, this.gravityPoint);

        // Simply snap to gravity point if close enough
        if (gravityDistance <= SNAP_DISTANCE)
        {
            this.SnapMove();
            return;
        }

        // Determines float strength based on distance from gravity point
        float distanceMargin = Mathf.Min(Mathf.Max(gravityDistance / FLOAT_DISTANCE, 0.2f), 1.0f);
        float distanceMultiplyer = Mathf.Min(Mathf.Max(gravityDistance / FLOAT_DISTANCE, 1.0f), 3.0f);
        float gravityForce = Mathf.Sin(distanceMargin * Mathf.PI / 2) * this.gravityStrength * distanceMultiplyer * timeInc;

        this.transform.position = Vector2.MoveTowards(this.transform.position, this.gravityPoint, gravityForce);
    }

    /// <summary>
    /// Calculates forces being applied to the card and adjusts the gravity point force
    /// </summary>
    private void CalculateGravity()
    {
        float timeInc = Time.deltaTime;

        // Apply drag
        if (this.gravityPointForce.magnitude >= 1.5f)
        {
            float currentVelocity = this.gravityPointForce.magnitude;
            Vector2 oppositeDirection = this.gravityPointForce.normalized * -1;
            Vector2 oppositeVelocityDrag = oppositeDirection * (0.5f) * Mathf.Pow(currentVelocity, 2) * DRAG;
            Vector2 oppositeForce = oppositeVelocityDrag * timeInc;
            if (oppositeForce.magnitude > currentVelocity)
            {
                this.gravityPointForce = Vector2.zero;
            } else
            {
                //Debug.Log($"Adding drag force {oppositeForce}");
                this.gravityPointForce += oppositeForce;
            }
        } else
        {
            this.gravityPointForce = Vector2.zero;
        }
    }

    /// <summary>
    /// Moves the gravity point based on forces applied to the card
    /// </summary>
    private void MoveGravityPoint()
    {
        float timeInc = Time.deltaTime;
        if (this.gravityPointForce.magnitude >= 0.01f)
        {
            this.gravityPoint += (this.gravityPointForce * timeInc);
        }
    }

    public void SetGravityPoint(Vector2 val)
    {
        this.gravityPoint = val;
    }

    public void SetPosition(Vector2 val)
    {
        this.initialPosition = val;
    }

    public void ApplyGravityForce(Vector2 val)
    {
        //Debug.Log($"Adding forces {val}");
        if (this.movementType != CardMovementType.FLOAT)
        {
            return;
        }
        this.gravityPointForce += val;
    }

    public void SetMovementType(CardMovementType movementType)
    {
        this.gravityPointForce = Vector2.zero;
        this.movementType = movementType;
    }

    public void AnchorCard()
    {
        this.gravityPoint = this.transform.position;
    }

    public void RegisterPosition()
    {
        this.initialPosition = this.transform.position;
    }

    /// <summary>
    /// Determines the closest point to the hand and returns the card to that point
    /// </summary>
    /// <param name="handBox">The object used to measure the hand area</param>
    public void ReturnToHand(GameObject handBox)
    {
        Combat_Area_Marker handBoxScript = handBox.GetComponent<Combat_Area_Marker>();

        this.movementType = CardMovementType.FLOAT;
        this.gravityPoint = handBoxScript.ClosestPoint(this.gameObject.transform.position);
    }

    public void ResetPosition()
    {
        this.movementType = CardMovementType.FLOAT;
        this.gravityPoint = this.initialPosition;
    }

    public void SetLocked(bool locked)
    {
        this.isLocked = locked;
    }

    public bool GetLocked()
    {
        return this.isLocked;
    }
}

/// <summary>
/// The different ways a card will move on the field
/// SNAP: Instantly snap to its gravity point
/// LOOSESNAP: Floats towards the gravity point, but will snap to a maximum distance
/// FLOAT: Floats towards the gravity point, speed increasing with distance
/// </summary>
public enum CardMovementType
{
    SNAP,
    LOOSESNAP,
    FLOAT,
    NONE
}