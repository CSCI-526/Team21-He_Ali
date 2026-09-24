
using UnityEngine;

public class PoliceTarget : MonoBehaviour
{
    private PoliceMovement policeMovement;

    private void Awake()
    {
        policeMovement =
            GetComponentInParent<PoliceMovement>();
    }

    public void HitPolice()
    {
        if (policeMovement != null)
        {
            policeMovement.ApplyAttackSlowdown();
        }
    }
}