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

    [SerializeField] private float speedIncreaseInterval = 30f;
    [SerializeField] private float speedIncreasePercent = 1f;

    [SerializeField] private float attackSlowPercent = 10f;
    [SerializeField] private float attackSlowDuration = 3f;

    private Rigidbody policeRigidbody;

    private float scheduledPoliceSpeed;
    private float currentPoliceSpeed;
    private float accelerationTimer;
    private float slowdownTimer;
    private float delayedPlayerX;

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

        if (accelerationTimer >= speedIncreaseInterval)
        {
            accelerationTimer = 0f;
            scheduledPoliceSpeed *=
                1f + speedIncreasePercent / 100f;
        }

        if (slowdownTimer > 0f)
        {
            slowdownTimer -= Time.fixedDeltaTime;
            currentPoliceSpeed =
                scheduledPoliceSpeed *
                (1f - attackSlowPercent / 100f);
        }
        else
        {
            currentPoliceSpeed = scheduledPoliceSpeed;
        }
    }

    private void UpdatePlayerHistory()
    {
        // Store the player's previous X positions.
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

        // Move using the speed difference.
        float relativeSpeed =
            currentPoliceSpeed - playerForwardSpeed;

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
        slowdownTimer = attackSlowDuration;
    }
}