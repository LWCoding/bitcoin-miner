using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowHandler : MonoBehaviour, IDragHandler, IBeginDragHandler
{

    [Header("Object Assignments")]
    [SerializeField] private TextMeshProUGUI _windowName;

    public GameFile CurrentFileInfo;  // Cached info for current instance's file

    private Vector3 _mouseOffset;
    private Camera _mainCamera;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _rectTransform = GetComponent<RectTransform>();
    }

    public virtual void Initialize(GameFile gf)
    {
        _windowName.text = gf.FileName;
        CurrentFileInfo = gf;
        StartCoroutine(ShowCoroutine());
    }

    public void CloseFileWindow()
    {
        int idx = GameState.CreatedFiles.FindIndex((gf) => gf.FileName == CurrentFileInfo.FileName);
        WindowManager.Instance.CloseFileWindow(idx);
    }

    public virtual void DestroyWindow()
    {
        Destroy(gameObject);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 newPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition) + _mouseOffset;

        // Calculate the screen bounds
        float minX = _mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        float maxX = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        float minY = _mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;
        float maxY = _mainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;

        // Get the object's size
        Vector2 size = _rectTransform.sizeDelta * _rectTransform.lossyScale;

        // Calculate the bounds within which the object's center can move
        float allowedMinX = minX;
        float allowedMaxX = maxX - size.x;
        float allowedMinY = minY + size.y;
        float allowedMaxY = maxY;

        // Clamp the new position to these bounds
        Vector2 clampedPosition = new(
            Mathf.Clamp(newPos.x, allowedMinX, allowedMaxX),
            Mathf.Clamp(newPos.y, allowedMinY, allowedMaxY)
        );

        // Apply the clamped position
        transform.position = clampedPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _mouseOffset = transform.position - _mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    private IEnumerator ShowCoroutine()
    {
        Vector2 desiredScale = transform.localScale;
        float currTime = 0;
        float timeToWait = 0.1f;
        while (currTime < timeToWait)
        {
            currTime += Time.deltaTime;
            transform.localScale = Vector2.Lerp(Vector2.zero, desiredScale, currTime / timeToWait);
            yield return null;
        }
    }

}
