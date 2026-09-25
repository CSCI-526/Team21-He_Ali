using UnityEngine;
using UnityEngine.Rendering;

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
        ObstacleImpactVFX.Spawn(transform.position);
        Destroy(gameObject);
    }
}

public static class ObstacleImpactVFX
{
    public static void Spawn(Vector3 position)
    {
        GameObject effectObject = new GameObject("Obstacle Impact Particles");
        effectObject.transform.position = position + Vector3.up * 0.4f;

        ParticleSystem particles = effectObject.AddComponent<ParticleSystem>();
        particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        ConfigureParticles(particles);
        ConfigureRenderer(effectObject.GetComponent<ParticleSystemRenderer>());

        particles.Play();
        particles.Emit(50);
        Object.Destroy(effectObject, 2f);
    }

    private static void ConfigureParticles(ParticleSystem particles)
    {
        ParticleSystem.MainModule main = particles.main;
        main.loop = false;
        main.playOnAwake = false;
        main.duration = 1f;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.45f, 0.9f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(4f, 8f);
        main.startSize = new ParticleSystem.MinMaxCurve(0.08f, 0.2f);
        main.startRotation = new ParticleSystem.MinMaxCurve(0f, Mathf.PI * 2f);
        main.gravityModifier = 1f;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.stopAction = ParticleSystemStopAction.Destroy;

        ParticleSystem.EmissionModule emission = particles.emission;
        emission.enabled = false;

        ParticleSystem.ShapeModule shape = particles.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.25f;

        ParticleSystem.ColorOverLifetimeModule color = particles.colorOverLifetime;
        color.enabled = true;
        color.color = new ParticleSystem.MinMaxGradient(CreateOrangeGradient());

        ParticleSystem.SizeOverLifetimeModule size = particles.sizeOverLifetime;
        size.enabled = true;
        size.size = new ParticleSystem.MinMaxCurve(
            1f,
            AnimationCurve.EaseInOut(0f, 1f, 1f, 0f)
        );
    }

    private static void ConfigureRenderer(ParticleSystemRenderer renderer)
    {
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        renderer.alignment = ParticleSystemRenderSpace.View;
        renderer.shadowCastingMode = ShadowCastingMode.Off;
        renderer.receiveShadows = false;

        Shader shader = Shader.Find("Universal Render Pipeline/Particles/Unlit");
        if (shader == null)
        {
            shader = Shader.Find("Particles/Standard Unlit");
        }

        if (shader != null)
        {
            Material material = new Material(shader);
            material.name = "Obstacle Impact Orange Material";

            Color orange = new Color(1f, 0.35f, 0.02f, 1f);
            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", orange);
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", orange);
            }

            renderer.material = material;
            Object.Destroy(material, 2f);
        }
    }

    private static Gradient CreateOrangeGradient()
    {
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new[]
            {
                new GradientColorKey(new Color(1f, 1f, 0.55f), 0f),
                new GradientColorKey(new Color(1f, 0.4f, 0.02f), 0.35f),
                new GradientColorKey(new Color(0.9f, 0.05f, 0f), 0.75f),
                new GradientColorKey(new Color(0.2f, 0.01f, 0f), 1f)
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
}
