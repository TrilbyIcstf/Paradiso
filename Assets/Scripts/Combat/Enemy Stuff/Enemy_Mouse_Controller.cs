using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Mouse_Controller : ManagerBehavior
{
    public static bool testPause = false;

    [SerializeField]
    private MouseMovementState currentState = MouseMovementState.RESTING;
    private EnemyMouseIntent intent = EnemyMouseIntent.PLACE;

    // Variables for tracking resting state
    private float restingTime = 0;
    private float unrestTime = 1.0f;

    // Variables for tracking chasing state
    private Vector2 startPos = Vector2.zero;
    private float startDist = 0;
    private float chaseSpeed = 20.0f;
    private float catchDist = 0.05f;

    // Variables for tracking holding state
    private float holdTime = 0;
    private float holdDelayTime = 0.25f;

    // Variables for tracking placing state
    private float placeSpeed = 30.0f;
    private Vector2 placePos = Vector2.zero;
    private float placeDist = 0.05f;

    private GameObject targetCard = null;
    private int targetSpace = -1;

    [SerializeField]
    private Sprite openHand;
    [SerializeField]
    private Sprite closedHand;

    private SpriteRenderer sr;

    private void Awake()
    {
        this.sr = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (testPause) { return; }

        Vector2 currPos = gameObject.transform.position;
        float currDist;
        float speed;

        switch (this.currentState)
        {
            case MouseMovementState.RESTING:
                this.restingTime += Time.deltaTime;
                if (this.restingTime >= this.unrestTime)
                {
                    ChangeState(MouseMovementState.CHASING);
                }
                break;
            case MouseMovementState.CHASING:
                if (this.targetCard == null) { return; }

                Vector2 cardPos = this.targetCard.transform.position;
                Vector2 chaseDir = (cardPos - currPos).normalized;
                currDist = Vector2.Distance(currPos, cardPos);
                float speedMult = Mathf.Lerp(1, 2, currDist / this.startDist);

                speed = Time.deltaTime * this.chaseSpeed * speedMult;
                speed = Mathf.Min(speed, currDist);

                currPos += chaseDir * speed;
                gameObject.transform.position = currPos;

                if (Vector2.Distance(currPos, cardPos) <= this.catchDist)
                {
                    ChangeState(MouseMovementState.HOLDING);
                }
                break;
            case MouseMovementState.HOLDING:
                this.holdTime += Time.deltaTime;
                if (this.holdTime >= this.holdDelayTime)
                {
                    ChangeState(MouseMovementState.PLACING);
                }
                break;
            case MouseMovementState.PLACING:
                if (this.targetCard == null) { return; }

                Vector2 placeDir = (this.placePos - currPos).normalized;
                currDist = Vector2.Distance(currPos, this.placePos);

                speed = Time.deltaTime * this.placeSpeed;
                speed = Mathf.Min(speed, currDist);

                currPos += placeDir * speed;
                gameObject.transform.position = currPos;

                if (Vector2.Distance(currPos, this.placePos) <= this.placeDist)
                {
                    this.targetCard.GetComponent<Enemy_Card_Movement>().OnRelease(this.targetSpace);
                    ChangeState(MouseMovementState.RESTING);
                }
                break;
            default:
                break;
        }
    }

    private void ChangeState(MouseMovementState state)
    {
        switch(state)
        {
            case MouseMovementState.RESTING:
                this.currentState = MouseMovementState.RESTING;
                this.restingTime = 0;
                this.sr.sprite = this.openHand;
                this.targetCard = null;
                this.targetSpace = -1;
                return;
            case MouseMovementState.CHASING:
                (this.targetCard, this.targetSpace) = DecideNextPlay();
                if (this.targetCard != null)
                {
                    this.currentState = MouseMovementState.CHASING;
                    this.startPos = gameObject.transform.position;
                    this.startDist = Vector2.Distance(this.startPos, this.targetCard.transform.position);
                } else
                {
                    ChangeState(MouseMovementState.RESTING);
                }
                return;
            case MouseMovementState.HOLDING:
                this.currentState = MouseMovementState.HOLDING;
                this.holdTime = 0;
                this.sr.sprite = this.closedHand;
                this.targetCard.GetComponent<Enemy_Card_Movement>().OnHold(gameObject);
                return;
            case MouseMovementState.PLACING:
                this.currentState = MouseMovementState.PLACING;
                if (this.intent == EnemyMouseIntent.PLACE)
                {
                    this.placePos = GM.CUI.GetEnemyHolder(this.targetSpace).transform.position;
                } else
                {
                    this.placePos = GM.CUI.uiCoordinator.EnemyHandArea().RandomPoint();
                }
                return;
            default:
                return;
        }
    }

    private (GameObject, int) DecideNextPlay()
    {
        GameObject nextCard = GM.CEH.RandomCard();
        int nextSlot = GM.CF.NextFreeEnemySpace();
        if (nextSlot == -1)
        {
            this.intent = EnemyMouseIntent.SORT;
        } else
        {
            this.intent = EnemyMouseIntent.PLACE;
        }

        return (nextCard, nextSlot);
    }
}

public enum MouseMovementState
{
    RESTING,
    CHASING,
    HOLDING,
    PLACING,
    ORDERING
}

public enum EnemyMouseIntent
{
    PLACE,
    SORT
}