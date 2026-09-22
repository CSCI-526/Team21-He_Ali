using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform frontFirePoint;
    [SerializeField] private Transform backFirePoint;
    [SerializeField] private float fireCooldown = 0.3f;

    private float nextFireTime;

    // Update is called once per frame
    private void Update()
    {
        if (Time.time >= nextFireTime)
        {

          if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            {
                Shoot(frontFirePoint, false);
            }
            else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            {
                Shoot(backFirePoint, true);
            }

        }
    }

     private void Shoot(Transform firePoint, bool targetsPolice)
  {
      GameObject newProjectile = Instantiate(
          projectilePrefab,
          firePoint.position,
          firePoint.rotation
      );

      Projectile projectile = newProjectile.GetComponent<Projectile>();
      projectile.SetTargetsPolice(targetsPolice);

      nextFireTime = Time.time + fireCooldown;
  }


}
