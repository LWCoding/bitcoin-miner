using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopWindowHandler : WindowHandler
{
    public override void DestroyWindow()
    {
        ObjectFactory.Instance.ReturnObjectToPool(gameObject, PoolableType.SHOP_WINDOW);
    }

}
