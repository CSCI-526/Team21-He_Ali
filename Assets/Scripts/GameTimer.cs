using UnityEngine;
using System.Collections;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private float gameDuration = 90f;
    [SerializeField] private float finishLineShowTime = 10f;
    [SerializeField] private float finishDuration = 3f;

    [SerializeField] private Transform player;
    [SerializeField] private GameObject finishLine;
    [SerializeField] private Transform finishPoint;

    private float timerRemaining;
    private bool finishing;
    private bool gameFinished;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timerRemaining = gameDuration;
        finishLine.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (finishing || gameFinished)
        {
            return;
        }

        timerRemaining -= Time.deltaTime;

        if(timerRemaining <= finishLineShowTime)
        {
            finishLine.SetActive(true);
        }

        if (timerRemaining <= finishDuration)
        {
            finishLine.SetActive(true);
            StartCoroutine(CrossFinishLine());
        }
    }

    private IEnumerator CrossFinishLine()
    {
        finishing = true;
        finishLine.SetActive(true);

        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<PlayerShooting>().enabled = false;
        player.GetComponent<PlayerFuel>().enabled = false;

        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.isKinematic = true;

        Vector3 startPosition = player.position;
        Vector3 endPosition = finishPoint.position;

        float elapsedTime = 0f;

        while(elapsedTime < finishDuration)
        {
            float t = elapsedTime / finishDuration;
            player.position = Vector3.Lerp(startPosition, endPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.position = endPosition;
        gameFinished = true;
        finishing = false;
    }

    private void OnGUI()
    {
        if (!gameFinished)
        {
            return;
        }

        GUIStyle finishStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 48,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        };

        finishStyle.normal.textColor = Color.white;

        GUI.Label(
            new Rect(0f, Screen.height / 2f - 40f, Screen.width, 80f),
            "FINISHED!",
            finishStyle
        );
    }
}
