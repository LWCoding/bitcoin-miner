using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolableObject : MonoBehaviour
{

    public abstract void OnSpawn();
    public abstract void OnDespawn();

}
