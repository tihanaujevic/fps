using UnityEngine;

public class PlayerCoins : MonoBehaviour
{
    [SerializeField] private int Coins;
    public void AddCoin()
    {
        Coins++;
    }
}
