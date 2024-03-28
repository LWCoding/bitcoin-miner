using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PassiveIncomePurchase : Purchase
{

    public override PurchaseType GetPurchaseType() => PurchaseType.PASSIVE_INCOME;

    public override PurchaseInfo GetCurrentPurchaseInfo()
    {
        return Levels[_currLevel];
    }

    public override void RenderPurchase(PurchaseInfo levelBought)
    {
        GameState.ClicksPerSecond += levelBought.SpecialValue;
    }

}
