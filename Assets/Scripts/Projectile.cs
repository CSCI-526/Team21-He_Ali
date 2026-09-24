
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifetime = 3f;

    private Rigidbody rb;
    private bool targetsPolice;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifetime);
    }

    public void SetTargetsPolice(bool police)
    {
        targetsPolice = police;
    }

    private void FixedUpdate()
    {
        Vector3 movement =
            transform.forward *
            speed *
            Time.fixedDeltaTime;

        rb.MovePosition(rb.position + movement);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (targetsPolice)
        {
            PoliceTarget police =
                other.GetComponentInParent<PoliceTarget>();

            if (police != null)
            {
                police.HitPolice();
                Destroy(gameObject);
            }
        }
        else
        {
            Obstacle obstacle =
                other.GetComponentInParent<Obstacle>();

            if (obstacle != null)
            {
                obstacle.DestroyObstacle();
                Destroy(gameObject);
            }
        }
    }
}