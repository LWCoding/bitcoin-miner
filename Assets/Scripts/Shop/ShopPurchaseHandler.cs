using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPurchaseHandler : MonoBehaviour
{

    [Header("Object Assignments")]
    [SerializeField] private TextMeshProUGUI _purchaseNameText;
    [SerializeField] private TextMeshProUGUI _purchaseDescText;
    [SerializeField] private TextMeshProUGUI _purchaseBtnText;
    [SerializeField] private Button _purchaseBtn;

    private Purchase _purchase;
    private PurchaseInfo _pInfo;  // Cached purchase info for this object

    private void Awake()
    {
        _purchase = GetComponent<Purchase>();
    }

    public void Initialize(PurchaseInfo pInfo)
    {
        _pInfo = pInfo;
        _purchaseNameText.text = pInfo.PurchaseName;
        _purchaseDescText.text = pInfo.PurchaseDesc;
        if (pInfo.PurchaseCost != -1)
        {
            _purchaseBtnText.text = "Buy (" + pInfo.PurchaseCost.ToString("F5") + " BTC)";
        } else
        {
            _purchaseBtnText.text = "Maxed";
        }
        SetBuyButtonState(GameState.Clicks);
    }

    /// <summary>
    /// Set whether the purchase button is interactable or not. This
    /// should be updated whenever the player's money changes.
    /// </summary>
    public void SetBuyButtonState(float currMoney)
    {
        _purchaseBtn.interactable = (_pInfo.PurchaseCost != -1 && currMoney >= _pInfo.PurchaseCost);
    }

    /// <summary>
    /// Should be called by button `OnClick`. Renders logic when
    /// the buy button is pressed by the user.
    /// </summary>
    public void OnBuyButtonClicked()
    {
        _purchase.ClickBuyButton();
    }

    private void OnEnable()
    {
        GameState.OnChangeBitcoin += SetBuyButtonState;
    }

    private void OnDisable()
    {
        GameState.OnChangeBitcoin -= SetBuyButtonState;
    }

}
