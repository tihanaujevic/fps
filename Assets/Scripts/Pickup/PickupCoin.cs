using UnityEngine;

public class PickupCoin : MonoBehaviour, IPickupable
{
    public void OnPickup(GameObject player)
    {
        player.GetComponent<PlayerCoins>().AddCoin();
    }
}
