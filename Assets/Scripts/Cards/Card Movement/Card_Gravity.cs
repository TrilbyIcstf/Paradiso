using UnityEngine;

public class Card_Gravity : MonoBehaviour
{
    private const float MAX_DISTANCE = 1.5f;
    private const float SNAP_DISTANCE = 0.005f;
    private const float FLOAT_DISTANCE = 3.5f;

    /// <summary>
    /// The point of gravity this card will move towards
    /// </summary>
    private Vector2 gravityPoint = Vector2.zero;

    private CardMovementType movementType = CardMovementType.NONE;

    /// <summary>
    /// The strength of the gravity effect acting on this card
    /// </summary>
    private float gravityStrength = 8f;

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

    private void Update()
    {
        switch (this.movementType)
        {
            case CardMovementType.SNAP:
                this.SnapMove();
                break;
            case CardMovementType.LOOSESNAP:
                this.LooseSnapMove();
                break;
            case CardMovementType.FLOAT:
                this.FloatMove();
                break;
            default:
                break;
        }
    }

    public void SnapMove()
    {
        this.transform.position = this.gravityPoint;
    }

    public void LooseSnapMove()
    {
        float timeInc = Time.deltaTime;
        float gravityDistance = Vector2.Distance(this.transform.position, this.gravityPoint);
        if (gravityDistance >= 0.1f)
        {
            float snapForce = gravityDistance - MAX_DISTANCE;
            float distanceMargin = Mathf.Min(gravityDistance / MAX_DISTANCE, 1.0f);
            float gravityForce = Mathf.Pow(Mathf.Sin(distanceMargin * Mathf.PI / 2), 2) * this.gravityStrength * timeInc;
            float higherForce = Mathf.Max(snapForce, gravityForce);

            this.transform.position = Vector2.MoveTowards(this.transform.position, this.gravityPoint, higherForce);
        }
    }

    public void FloatMove()
    {
        float timeInc = Time.deltaTime;
        float gravityDistance = Vector2.Distance(this.transform.position, this.gravityPoint);
        if (gravityDistance <= SNAP_DISTANCE)
        {
            this.SnapMove();
            this.movementType = CardMovementType.NONE;
            return;
        }

        float distanceMargin = Mathf.Min(Mathf.Max(gravityDistance / FLOAT_DISTANCE, 0.2f), 1.0f);
        float distanceMultiplyer = Mathf.Min(Mathf.Max(gravityDistance / FLOAT_DISTANCE, 1.0f), 3.0f);
        float gravityForce = Mathf.Sin(distanceMargin * Mathf.PI / 2) * this.gravityStrength * distanceMultiplyer * timeInc;
        this.transform.position = Vector2.MoveTowards(this.transform.position, this.gravityPoint, gravityForce);
    }

    public void SetGravityPoint(Vector2 val)
    {
        this.gravityPoint = val;
    }

    public void SetPosition(Vector2 val)
    {
        this.initialPosition = val;
    }

    public void SetMovementType(CardMovementType movementType)
    {
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

public enum CardMovementType
{
    SNAP,
    LOOSESNAP,
    FLOAT,
    NONE
}