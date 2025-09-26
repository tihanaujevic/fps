using UnityEngine;

public class PickupHealth : MonoBehaviour, IPickupable
{
    public void OnPickup(GameObject player)
    {
        player.gameObject.GetComponent<PlayerHealth>().AddPlayerHealth(10);
    }
}
