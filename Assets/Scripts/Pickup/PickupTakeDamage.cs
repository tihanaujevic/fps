using UnityEngine;

public class PickupTakeDamage : MonoBehaviour, IPickupable
{
    public void OnPickup(GameObject player)
    {
        player.gameObject.GetComponent<PlayerHealth>().Damage(10);
    }
}
