using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PassiveIncomePurchase : Purchase
{

    public override PurchaseType GetPurchaseType() => PurchaseType.PASSIVE_INCOME;

    public override PurchaseInfo GetCurrentPurchaseInfo()
    {
        PurchaseInfo info = Levels[_currLevel];
        info.PurchaseDesc = info.PurchaseDesc.Replace("%cur", GameState.ClicksPerSecond.ToString("F5"));
        info.PurchaseDesc = info.PurchaseDesc.Replace("%new", (GameState.ClicksPerSecond + info.SpecialValue).ToString("F5"));
        return info;
    }

    public override void RenderPurchase(PurchaseInfo levelBought)
    {
        GameState.ClicksPerSecond += levelBought.SpecialValue;
    }

}
