using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CSMoneyUpdater : MonoBehaviour
{

    [Header("Object Assignments")]
    [SerializeField] private TextMeshProUGUI _moneyText;

    private void Awake()
    {
        UpdateMoneyCount(GameState.Clicks);
        GameState.OnChangeBitcoin = UpdateMoneyCount;
    }

    public void UpdateMoneyCount(float newVal)
    {
        _moneyText.text = newVal.ToString("F5") + " BC";
    }

}
