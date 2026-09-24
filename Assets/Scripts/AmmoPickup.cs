using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider))]
public sealed class AmmoPickup : MonoBehaviour
{
    [SerializeField, Min(1)] private int ammoAmount = 1;

    private bool isCollected;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCollected)
        {
            return;
        }

        PlayerShooting playerShooting = other.GetComponentInParent<PlayerShooting>();

        if (playerShooting == null || !playerShooting.AddAmmo(ammoAmount))
        {
            return;
        }

        isCollected = true;
        Destroy(gameObject);
    }
}
