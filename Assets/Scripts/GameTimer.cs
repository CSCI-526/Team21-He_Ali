
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private float gameDuration = 90f;
    [SerializeField] private float finishLineShowTime = 3f;
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
    private bool gameStarted;
    private PlayerFuel playerFuel;

    private void Start()
    {
        Time.timeScale = 0f;
        timerRemaining = gameDuration;
        playerFuel = player.GetComponent<PlayerFuel>();
        finishLine.SetActive(false);
        SetGameplayEnabled(false);
    }

    private void Update()
    {
        if (!gameStarted)
        {
            if (Keyboard.current != null &&
                Keyboard.current.enterKey.wasPressedThisFrame)
            {
                BeginGame();
            }

            return;
        }

        if (finishing || gameFinished || gameOver)
        {
            return;
        }

        CheckForGameOver();

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

    private void BeginGame()
    {
        if (gameStarted)
        {
            return;
        }

        gameStarted = true;
        SetGameplayEnabled(true);
        Time.timeScale = 1f;
    }

    private void CheckForGameOver()
    {
        float catchPosition =
            player.position.z - catchDistance;

        bool caughtByPolice = police.position.z >= catchPosition;
        bool ranOutOfFuel = playerFuel != null && playerFuel.CurrentFuel <= 0f;

        if (caughtByPolice || ranOutOfFuel)
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
        SetGameplayEnabled(false);

        Rigidbody playerRigidbody =
            player.GetComponent<Rigidbody>();

        if (playerRigidbody != null)
        {
            playerRigidbody.isKinematic = true;
        }
    }

    private void SetGameplayEnabled(bool isEnabled)
    {
        PlayerMovement playerMovement =
            player.GetComponent<PlayerMovement>();

        PlayerShooting playerShooting =
            player.GetComponent<PlayerShooting>();

        PoliceMovement policeMovement =
            police.GetComponent<PoliceMovement>();

        if (playerMovement != null)
        {
            playerMovement.enabled = isEnabled;
        }

        if (playerShooting != null)
        {
            playerShooting.enabled = isEnabled;
        }

        if (playerFuel != null)
        {
            playerFuel.enabled = isEnabled;
        }

        if (policeMovement != null)
        {
            policeMovement.enabled = isEnabled;
        }
    }

    private void OnGUI()
    {
        if (!gameStarted)
        {
            DrawStartScreen();
            return;
        }

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

    private void DrawStartScreen()
    {
        float panelWidth = 420f;
        float panelHeight = 330f;
        float panelX = (Screen.width - panelWidth) / 2f;
        float panelY = (Screen.height - panelHeight) / 2f;

        GUIStyle titleStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 46,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        };
        titleStyle.normal.textColor = Color.white;

        GUIStyle subtitleStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 22,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        };
        subtitleStyle.normal.textColor = Color.white;

        GUIStyle instructionStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 20,
            alignment = TextAnchor.MiddleCenter,
            wordWrap = true
        };
        instructionStyle.normal.textColor = Color.white;

        GUIStyle hintStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 16,
            alignment = TextAnchor.MiddleCenter
        };
        hintStyle.normal.textColor = Color.white;

        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = 30,
            fontStyle = FontStyle.Bold
        };

        GUI.Box(new Rect(panelX, panelY, panelWidth, panelHeight), GUIContent.none);
        GUI.Label(
            new Rect(panelX, panelY + 15f, panelWidth, 60f),
            "REDLINE RUN",
            titleStyle
        );
        GUI.Label(
            new Rect(panelX, panelY + 68f, panelWidth, 40f),
            "RUN, DODGE & ESCAPE",
            subtitleStyle
        );

        if (GUI.Button(
            new Rect(panelX + 90f, panelY + 120f, panelWidth - 180f, 65f),
            "START",
            buttonStyle
        ))
        {
            BeginGame();
        }

        GUI.Label(
            new Rect(panelX + 25f, panelY + 195f, panelWidth - 50f, 65f),
            "Dodge obstacles. Shoot to slow the cops.\nReach the finish line.",
            instructionStyle
        );
        GUI.Label(
            new Rect(panelX, panelY + 275f, panelWidth, 35f),
            "Click START or press Enter",
            hintStyle
        );
    }
}
