    using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    public int totalCoins { get; private set; } = 0;

    public TextMeshProUGUI coinCounterText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateCoinUI();
    }
    public void AddCoin()
    {
        totalCoins++;
        Debug.Log("Coins: " + totalCoins);
        UpdateCoinUI();
        
    }

    public bool SpendCoins(int amount)
    {
        if(totalCoins < amount)
        {
            Debug.Log("Not enough coins to purchase");
        }
        if (totalCoins >= amount)
        {
            totalCoins -= amount;
            Debug.Log("Spent " + amount + " coins. Remaining: " + totalCoins);
            UpdateCoinUI();
            return true;
        }
        return false;
    }
    void UpdateCoinUI()
    {
        if (coinCounterText != null)
        {
            coinCounterText.text = "Coins: " + totalCoins;
        }
    }
}