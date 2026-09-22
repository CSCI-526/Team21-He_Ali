using UnityEngine;

[DisallowMultipleComponent]
public sealed class PlayerFuel : MonoBehaviour
{
    [SerializeField, Min(0f)]
    private float startingFuel = 50f;

    [SerializeField, Min(0.01f)]
    private float maximumFuel = 100f;
    
    [SerializeField, Min(0f)]
    private float fuelDrainPerSecond = 2f;

    public float CurrentFuel { get; private set; }
    public float MaximumFuel => maximumFuel;

    private GUIStyle fuelLabelStyle;

    private void Awake()
    {
        maximumFuel = Mathf.Max(0.01f, maximumFuel);
        CurrentFuel = Mathf.Clamp(startingFuel, 0f, maximumFuel);
    }

    private void Update()
    {
        if (CurrentFuel > 0f)
        {
            CurrentFuel = Mathf.Max(0f, CurrentFuel - fuelDrainPerSecond * Time.deltaTime);
        }
    }
    public void AddFuel(float amount)
    {
        if (amount <= 0f)
        {
            return;
        }

        CurrentFuel = Mathf.Min(CurrentFuel + amount, maximumFuel);
        Debug.Log($"Fuel: {CurrentFuel:0}/{maximumFuel:0}", this);
    }

    private void OnGUI()
    {
        if (fuelLabelStyle == null)
        {
            fuelLabelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 28,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.UpperLeft
            };

            fuelLabelStyle.normal.textColor = Color.white;
        }

        GUI.Label(
            new Rect(20f, 20f, 300f, 50f),
            $"Fuel: {CurrentFuel:0} / {maximumFuel:0}",
            fuelLabelStyle
        );
    }
}
