
using System.Collections;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private float gameDuration = 90f;
    [SerializeField] private float finishLineShowTime = 10f;
    [SerializeField] private float finishDuration = 3f;
    [SerializeField] private float catchDistance = 2f;

    [SerializeField] private Transform player;
    [SerializeField] private Transform police;
    [SerializeField] private GameObject finishLine;
    [SerializeField] private Transform finishPoint;

    private float timerRemaining;
    private bool finishing;
    private bool gameFinished;
    private bool gameOver;

    private void Start()
    {
        Time.timeScale = 1f;
        timerRemaining = gameDuration;
        finishLine.SetActive(false);
    }

    private void Update()
    {
        if (finishing || gameFinished || gameOver)
        {
            return;
        }

        CheckPoliceCatch();

        if (gameOver)
        {
            return;
        }

        timerRemaining -= Time.deltaTime;

        if (timerRemaining <= finishLineShowTime)
        {
            finishLine.SetActive(true);
        }

        if (timerRemaining <= finishDuration)
        {
            StartCoroutine(CrossFinishLine());
        }
    }

    private void CheckPoliceCatch()
    {
        float catchPosition =
            player.position.z - catchDistance;

        if (police.position.z >= catchPosition)
        {
            gameOver = true;

            StopPlayerAndPolice();

            Time.timeScale = 0f;
        }
    }

    private IEnumerator CrossFinishLine()
    {
        finishing = true;
        finishLine.SetActive(true);

        StopPlayerAndPolice();

        Vector3 startPosition = player.position;
        Vector3 endPosition = finishPoint.position;

        float elapsedTime = 0f;

        while (elapsedTime < finishDuration)
        {
            float t = elapsedTime / finishDuration;

            player.position = Vector3.Lerp(
                startPosition,
                endPosition,
                t
            );

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.position = endPosition;

        gameFinished = true;
        finishing = false;

        Time.timeScale = 0f;
    }

    private void StopPlayerAndPolice()
    {
        PlayerMovement playerMovement =
            player.GetComponent<PlayerMovement>();

        PlayerShooting playerShooting =
            player.GetComponent<PlayerShooting>();

        PlayerFuel playerFuel =
            player.GetComponent<PlayerFuel>();

        PoliceMovement policeMovement =
            police.GetComponent<PoliceMovement>();

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        if (playerShooting != null)
        {
            playerShooting.enabled = false;
        }

        if (playerFuel != null)
        {
            playerFuel.enabled = false;
        }

        if (policeMovement != null)
        {
            policeMovement.enabled = false;
        }

        Rigidbody playerRigidbody =
            player.GetComponent<Rigidbody>();

        if (playerRigidbody != null)
        {
            playerRigidbody.isKinematic = true;
        }
    }

    private void OnGUI()
    {
        if (!gameFinished && !gameOver)
        {
            return;
        }

        GUIStyle messageStyle =
            new GUIStyle(GUI.skin.label)
            {
                fontSize = 48,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };

        messageStyle.normal.textColor = Color.white;

        string message =
            gameFinished ? "FINISHED!" : "GAME OVER";

        GUI.Label(
            new Rect(
                0f,
                Screen.height / 2f - 40f,
                Screen.width,
                80f
            ),
            message,
            messageStyle
        );
    }
}