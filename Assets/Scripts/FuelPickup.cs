using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider))]
public sealed class FuelPickup : MonoBehaviour
{
    [SerializeField, Min(0f)]
    private float fuelAmount = 10f;

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

        PlayerFuel playerFuel = other.GetComponentInParent<PlayerFuel>();

        if (playerFuel == null)
        {
            return;
        }

        isCollected = true;
        playerFuel.AddFuel(fuelAmount);
        Destroy(gameObject);
    }
}
