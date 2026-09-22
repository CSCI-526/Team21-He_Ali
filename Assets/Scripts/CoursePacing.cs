using UnityEngine;

public class CoursePacing : MonoBehaviour
{
    [SerializeField] private float distanceMultiplier = 1.5f;

    private void Awake()
    {
        StretchEnvironment();

        foreach (Obstacle obstacle in GetComponentsInChildren<Obstacle>(true))
        {
            StretchPosition(obstacle.transform);
        }

        foreach (FuelPickup fuel in GetComponentsInChildren<FuelPickup>(true))
        {
            StretchPosition(fuel.transform);
        }
    }

    private void StretchEnvironment()
    {
        Transform environment = transform.Find("Environment");

        if (environment == null)
        {
            return;
        }

        StretchPosition(environment);

        foreach (Transform part in environment)
        {
            Vector3 scale = part.localScale;
            scale.z *= distanceMultiplier;
            part.localScale = scale;
        }
    }

    private void StretchPosition(Transform item)
    {
        Vector3 position = item.localPosition;
        position.z *= distanceMultiplier;
        item.localPosition = position;
    }
}
