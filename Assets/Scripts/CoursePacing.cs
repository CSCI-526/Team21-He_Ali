using UnityEngine;

public class CoursePacing : MonoBehaviour
{
    [SerializeField] private float distanceMultiplier = 1.5f;
    [SerializeField, Range(0f, 1f)] private float minimumSizeChance = 0.2f;
    [SerializeField] private Vector3 maximumObstacleScaleMultiplier =
        new Vector3(1.55f, 1.5f, 1.35f);

    private static readonly Vector3 StandardObstacleSize =
        new Vector3(1.8f, 1.2f, 1.8f);

    private void Awake()
    {
        StretchEnvironment();

        foreach (Obstacle obstacle in GetComponentsInChildren<Obstacle>(true))
        {
            StretchPosition(obstacle.transform);

            if (HasStandardObstacleSize(obstacle.transform))
            {
                obstacle.transform.localScale = Vector3.Scale(
                    StandardObstacleSize,
                    CreateRandomObstacleMultiplier()
                );
            }
        }

        foreach (FuelPickup fuel in GetComponentsInChildren<FuelPickup>(true))
        {
            StretchPosition(fuel.transform);
        }

        foreach (AmmoPickup ammo in GetComponentsInChildren<AmmoPickup>(true))
        {
            StretchPosition(ammo.transform);
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

    private static bool HasStandardObstacleSize(Transform obstacle)
    {
        return (obstacle.localScale - StandardObstacleSize).sqrMagnitude < 0.0001f;
    }

    private Vector3 CreateRandomObstacleMultiplier()
    {
        if (Random.value < minimumSizeChance)
        {
            return Vector3.one;
        }

        return new Vector3(
            Random.Range(1f, Mathf.Max(1f, maximumObstacleScaleMultiplier.x)),
            Random.Range(1f, Mathf.Max(1f, maximumObstacleScaleMultiplier.y)),
            Random.Range(1f, Mathf.Max(1f, maximumObstacleScaleMultiplier.z))
        );
    }
}
