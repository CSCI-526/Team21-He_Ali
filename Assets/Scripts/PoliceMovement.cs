using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PoliceMovement : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private float playerForwardSpeed = 5f;
    [SerializeField] private float basePoliceSpeed = 5f;

    [SerializeField] private float horizontalFollowSpeed = 4f;
    [SerializeField] private float horizontalDelay = 0.4f;

    [SerializeField] private float speedIncreaseInterval = 20f;
    [SerializeField] private float speedIncreasePercent = 1f;

    [SerializeField] private float slowSpeedBelowPlayer = 0.4f;
    [SerializeField] private float slowTimePerHit = 5f;
    [SerializeField] private int maxSlowdownHits = 3;

    [SerializeField] private float obstacleSlowdownDuration = 2f;
    [SerializeField] private float obstacleSpeedReduction = 0.25f;

    private Rigidbody policeRigidbody;

    private float scheduledPoliceSpeed;
    private float currentPoliceSpeed;
    private float accelerationTimer;
    private float slowdownTimer;
    private float playerObstacleSlowdownTimer;
    private float delayedPlayerX;

    private int slowdownHits;

    private readonly Queue<PlayerXSample> playerXHistory =
        new Queue<PlayerXSample>();

    private struct PlayerXSample
    {
        public float time;
        public float x;

        public PlayerXSample(float time, float x)
        {
            this.time = time;
            this.x = x;
        }
    }

    private void Awake()
    {
        policeRigidbody = GetComponent<Rigidbody>();

        scheduledPoliceSpeed = basePoliceSpeed;
        currentPoliceSpeed = basePoliceSpeed;

        if (player != null)
        {
            delayedPlayerX = player.position.x;
        }
    }

    private void FixedUpdate()
    {
        if (player == null)
        {
            return;
        }

        UpdateSpeed();
        UpdatePlayerHistory();
        MovePolice();
    }

    private void UpdateSpeed()
    {
        accelerationTimer += Time.fixedDeltaTime;

        if (playerObstacleSlowdownTimer > 0f)
        {
            playerObstacleSlowdownTimer = Mathf.Max(
                0f,
                playerObstacleSlowdownTimer - Time.fixedDeltaTime
            );
        }

        if (accelerationTimer >= speedIncreaseInterval)
        {
            accelerationTimer = 0f;

            scheduledPoliceSpeed *=
                1f + speedIncreasePercent / 100f;
        }

        if (slowdownTimer > 0f)
        {
            slowdownTimer -= Time.fixedDeltaTime;

            currentPoliceSpeed = Mathf.Max(
                0f,
                playerForwardSpeed - slowSpeedBelowPlayer
            );

            if (slowdownTimer <= 0f)
            {
                slowdownTimer = 0f;
                slowdownHits = 0;
                currentPoliceSpeed = scheduledPoliceSpeed;
            }
        }
        else
        {
            currentPoliceSpeed = scheduledPoliceSpeed;
        }
    }

    private void UpdatePlayerHistory()
    {
        playerXHistory.Enqueue(
            new PlayerXSample(
                Time.fixedTime,
                player.position.x
            )
        );

        float targetTime =
            Time.fixedTime - horizontalDelay;

        while (
            playerXHistory.Count > 0 &&
            playerXHistory.Peek().time <= targetTime
        )
        {
            delayedPlayerX =
                playerXHistory.Dequeue().x;
        }
    }

    private void MovePolice()
    {
        float newX = Mathf.MoveTowards(
            policeRigidbody.position.x,
            delayedPlayerX,
            horizontalFollowSpeed * Time.fixedDeltaTime
        );

        float effectivePlayerForwardSpeed = playerForwardSpeed;

        if (playerObstacleSlowdownTimer > 0f)
        {
            effectivePlayerForwardSpeed = Mathf.Max(
                0f,
                playerForwardSpeed - obstacleSpeedReduction
            );
        }

        float relativeSpeed =
            currentPoliceSpeed - effectivePlayerForwardSpeed;

        float newZ =
            policeRigidbody.position.z +
            relativeSpeed * Time.fixedDeltaTime;

        policeRigidbody.MovePosition(
            new Vector3(
                newX,
                policeRigidbody.position.y,
                newZ
            )
        );
    }

    public void ApplyAttackSlowdown()
    {
        if (slowdownTimer <= 0f)
        {
            slowdownTimer = 0f;
            slowdownHits = 0;
        }

        if (slowdownHits >= maxSlowdownHits)
        {
            return;
        }

        slowdownHits++;
        slowdownTimer += slowTimePerHit;
    }

    public void ApplyPlayerObstacleSlowdown()
    {
        playerObstacleSlowdownTimer = Mathf.Max(
            playerObstacleSlowdownTimer,
            obstacleSlowdownDuration
        );
    }
}
