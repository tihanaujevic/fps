using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider obj)
    {
        IPickupable coin = obj.GetComponent<IPickupable>();
        if (coin != null)
        {
            coin.OnPickup(gameObject);
            Destroy(obj.gameObject);
        }
            
    }
}
