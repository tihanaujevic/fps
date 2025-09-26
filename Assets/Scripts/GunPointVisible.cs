using UnityEngine;

public class GunPointVisible : MonoBehaviour
{
    [SerializeField] private GameObject gunPoint;
    void Update()
    {
        if (PickupWeapon.slotFull)
            gunPoint.SetActive(true);
        else
            gunPoint.SetActive(false);
    }
}
