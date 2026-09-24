using UnityEngine;
using UnityEngine.VFX;

public class Obstacle : MonoBehaviour
{
    private bool hasSlowedPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (hasSlowedPlayer || other.GetComponentInParent<PlayerMovement>() == null)
        {
            return;
        }

        hasSlowedPlayer = true;
        ObstacleImpactVFX.Spawn(other.ClosestPoint(transform.position));

        PoliceMovement policeMovement =
            FindFirstObjectByType<PoliceMovement>();

        if (policeMovement != null)
        {
            policeMovement.ApplyPlayerObstacleSlowdown();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<PlayerMovement>() != null)
        {
            hasSlowedPlayer = false;
        }
    }

    public void DestroyObstacle()
    {
        Destroy(gameObject);
    }
}

public static class ObstacleImpactVFX
{
    private const string ResourceName = "ObstacleImpactSparks";

    private static VisualEffectAsset impactAsset;
    private static bool reportedMissingAsset;

    public static void Spawn(Vector3 position)
    {
        if (impactAsset == null)
        {
            impactAsset = Resources.Load<VisualEffectAsset>(ResourceName);
        }

        if (impactAsset == null)
        {
            if (!reportedMissingAsset)
            {
                Debug.LogWarning("Obstacle impact VFX Graph asset could not be loaded.");
                reportedMissingAsset = true;
            }

            return;
        }

        GameObject effectObject = new GameObject("Obstacle Impact VFX Graph");
        effectObject.transform.position = position + Vector3.up * 0.4f;

        VisualEffect effect = effectObject.AddComponent<VisualEffect>();
        effect.visualEffectAsset = impactAsset;

        ConfigureBurst(effect);
        effect.Reinit();
        effect.Play();

        Object.Destroy(effectObject, 2f);
    }

    private static void ConfigureBurst(VisualEffect effect)
    {
        SetVector2(effect, "Loop Duration Min/Max", new Vector2(10f, 10f));
        SetVector2(effect, "Burst Count Min/Max", new Vector2(40f, 55f));
        SetVector2(effect, "Lifetime Min/Max", new Vector2(0.45f, 0.9f));
        SetVector2(effect, "Size Min/Max", new Vector2(0.08f, 0.18f));
        SetFloat(effect, "Spawn Rate", 0f);
        SetFloat(effect, "Sparkle Width", 0.12f);
        SetFloat(effect, "Initial Velocity Divergence", 1.35f);
        SetFloat(effect, "Turbulence Intensity", 0.6f);
        SetVector3(effect, "Initial Position", Vector3.zero);
        SetVector3(effect, "Initial Velocity", Vector3.up * 7f);
        SetVector3(effect, "Gravity Vector", Vector3.down * 8f);

        Gradient orangeGradient = CreateOrangeGradient();
        SetGradient(effect, "Sparkle Fire Gradient", orangeGradient);
        SetGradient(effect, "Color Over Life", orangeGradient);
    }

    private static Gradient CreateOrangeGradient()
    {
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new[]
            {
                new GradientColorKey(new Color(1f, 1f, 0.7f) * 4f, 0f),
                new GradientColorKey(new Color(1f, 0.45f, 0.03f) * 3f, 0.35f),
                new GradientColorKey(new Color(1f, 0.1f, 0f) * 1.5f, 0.75f),
                new GradientColorKey(new Color(0.35f, 0.02f, 0f), 1f)
            },
            new[]
            {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 0.55f),
                new GradientAlphaKey(0f, 1f)
            }
        );

        return gradient;
    }

    private static void SetFloat(VisualEffect effect, string property, float value)
    {
        int id = Shader.PropertyToID(property);

        if (effect.HasFloat(id))
        {
            effect.SetFloat(id, value);
        }
    }

    private static void SetVector2(VisualEffect effect, string property, Vector2 value)
    {
        int id = Shader.PropertyToID(property);

        if (effect.HasVector2(id))
        {
            effect.SetVector2(id, value);
        }
    }

    private static void SetVector3(VisualEffect effect, string property, Vector3 value)
    {
        int id = Shader.PropertyToID(property);

        if (effect.HasVector3(id))
        {
            effect.SetVector3(id, value);
        }
    }

    private static void SetGradient(VisualEffect effect, string property, Gradient value)
    {
        int id = Shader.PropertyToID(property);

        if (effect.HasGradient(id))
        {
            effect.SetGradient(id, value);
        }
    }
}
