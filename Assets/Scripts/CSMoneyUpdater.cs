using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CSMoneyUpdater : MonoBehaviour
{

    public static CSMoneyUpdater Instance;

    [Header("Object Assignments")]
    [SerializeField] private Transform _bitcoinCanvasTransform;
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

    /// <summary>
    /// Animates `count` number of bitcoin prefabs going from a position
    /// to the bitcoin text at the top right.
    /// </summary>
    public void AnimateBTCFromPositionToText(Vector3 startPos, float count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 randomOffset = new(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));
            StartCoroutine(AnimateBTCFromPositionToTextCoroutine(startPos + randomOffset, i * 0.01f));
        }
    }

    private IEnumerator AnimateBTCFromPositionToTextCoroutine(Vector3 startPos, float waitBefore = 0)
    {
        yield return new WaitForSeconds(waitBefore);
        Vector3 finalPos = _moneyText.transform.position;
        GameObject btcObject = ObjectFactory.Instance.GetPooledObject(PoolableType.MONEY_PARTICLE, _bitcoinCanvasTransform);
        btcObject.transform.position = startPos;
        float currTime = 0;
        float timeToWait = 0.7f;
        yield return new WaitForSeconds(0.2f);
        while (currTime < timeToWait)
        {
            currTime += Time.deltaTime;
            btcObject.transform.position = Vector3.Lerp(startPos, finalPos, currTime / timeToWait);
            yield return null;
        }
        ObjectFactory.Instance.ReturnObjectToPool(btcObject, PoolableType.MONEY_PARTICLE);
    }

}
