using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public sealed class PlayerShooting : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform frontFirePoint;
    [SerializeField] private Transform backFirePoint;
    [SerializeField, Min(0f)] private float fireCooldown = 0.3f;

    [Header("Ammo")]
    [SerializeField, Min(0)] private int startingAmmo = 3;
    [SerializeField, Min(1)] private int maximumAmmo = 3;

    public int CurrentAmmo { get; private set; }
    public int MaximumAmmo => maximumAmmo;

    private float nextFireTime;
    private GUIStyle ammoLabelStyle;

    private void Awake()
    {
        maximumAmmo = Mathf.Max(1, maximumAmmo);
        CurrentAmmo = Mathf.Clamp(startingAmmo, 0, maximumAmmo);
    }

    private void Update()
    {
        if (Keyboard.current == null || Time.time < nextFireTime)
        {
            return;
        }

        if (Keyboard.current.upArrowKey.wasPressedThisFrame ||
            Keyboard.current.wKey.wasPressedThisFrame)
        {
            TryShoot(frontFirePoint, false);
        }
        else if (Keyboard.current.downArrowKey.wasPressedThisFrame ||
                 Keyboard.current.sKey.wasPressedThisFrame)
        {
            TryShoot(backFirePoint, true);
        }
    }

    public bool AddAmmo(int amount)
    {
        if (amount <= 0 || CurrentAmmo >= maximumAmmo)
        {
            return false;
        }

        CurrentAmmo = Mathf.Min(CurrentAmmo + amount, maximumAmmo);
        Debug.Log($"Ammo: {CurrentAmmo}/{maximumAmmo}", this);
        return true;
    }

    private void TryShoot(Transform firePoint, bool targetsPolice)
    {
        if (CurrentAmmo <= 0 || projectilePrefab == null || firePoint == null)
        {
            return;
        }

        GameObject newProjectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            firePoint.rotation
        );

        Projectile projectile = newProjectile.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.SetTargetsPolice(targetsPolice);
        }

        CurrentAmmo--;
        nextFireTime = Time.time + fireCooldown;
    }

    private void OnGUI()
    {
        if (ammoLabelStyle == null)
        {
            ammoLabelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 28,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.UpperLeft
            };

            ammoLabelStyle.normal.textColor = Color.white;
        }

        GUI.Label(
            new Rect(20f, 60f, 300f, 50f),
            $"Ammo: {CurrentAmmo} / {maximumAmmo}",
            ammoLabelStyle
        );
    }
}
