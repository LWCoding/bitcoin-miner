using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PurchaseType
{
    PASSIVE_INCOME = 0
}

[System.Serializable]
public struct PurchaseInfo
{
    public string PurchaseName;
    [TextArea(2, 4)]
    public string PurchaseDesc;
    public float PurchaseCost;  // Should be -1 for last upgrade
    public float SpecialValue;  // Optional value to store upgrade statistics
}

[RequireComponent(typeof(ShopPurchaseHandler))]
public abstract class Purchase : MonoBehaviour
{

    [Header("Level Assignments")]
    public List<PurchaseInfo> Levels;

    protected int _currLevel = 0;
    private ShopPurchaseHandler _shopPurchaseHandler;

    public abstract PurchaseType GetPurchaseType();
    public abstract void RenderPurchase(PurchaseInfo levelBought);
    public abstract PurchaseInfo GetCurrentPurchaseInfo();

    private void Awake()
    {
        _shopPurchaseHandler = GetComponent<ShopPurchaseHandler>();
    }

    /// <summary>
    /// Holds logic to check upgrade cost against user's balance.
    /// Calls the `RenderPurchase` function on success.
    /// </summary>
    public void ClickBuyButton()
    {
        PurchaseInfo currLevel = GetCurrentPurchaseInfo();
        // Don't let the user buy with insufficient funds
        if (currLevel.PurchaseCost > GameState.Clicks) { return; }
        // Don't let the user buy if the upgrade isn't purchaseable
        if (currLevel.PurchaseCost == -1) { return; }
        // Upgrade the GameState to reflect this change
        GameState.Clicks -= currLevel.PurchaseCost;
        RenderPurchase(currLevel);  // Render BEFORE upgrading
        GameState.Purchases[GetPurchaseType()] = ++_currLevel;
        GameState.PurchasesMade++;
        UpdateUpgradeInfo();
    }

    /// <summary>
    /// Updates text to reflect this current upgrade's state.
    /// </summary>
    public void UpdateUpgradeInfo()
    {
        PurchaseInfo currLevel = GetCurrentPurchaseInfo();
        _shopPurchaseHandler.Initialize(currLevel);
    }

    /// <summary>
    /// When this object is enabled, update its state depending
    /// on what is stored in GameState.
    /// </summary>
    public void OnEnable()
    {
        PurchaseType type = GetPurchaseType();
        // If we don't have a key stored, we're at level 0.
        if (!GameState.Purchases.ContainsKey(type))
        {
            _currLevel = 0;
        }
        else
        {
            // Or else, load in the appropriate level.
            _currLevel = GameState.Purchases[type];
        }
        // Then, update the information reflected on this
        UpdateUpgradeInfo();
    }

}
