using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CSMoneyUpdater : MonoBehaviour
{

    public static CSMoneyUpdater Instance;

    [Header("Object Assignments")]
    [SerializeField] private TextMeshProUGUI _moneyText;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
        UpdateMoneyCount(GameState.Clicks);
        GameState.OnChangeBitcoin = UpdateMoneyCount;
        ToggleTextVisibility(false);
    }

    public void UpdateMoneyCount(float newVal)
    {
        if (_moneyText.IsActive())
        {
            _moneyText.text = newVal.ToString("F5") + " BTC";
        }
    }

    public void ToggleTextVisibility(bool isEnabled)
    {
        _moneyText.gameObject.SetActive(isEnabled);
    }

}
