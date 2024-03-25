using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class FileWindowHandler : MonoBehaviour, IDragHandler, IBeginDragHandler
{

    [Header("Object Assignments")]
    [SerializeField] private TextMeshProUGUI _windowFileName;
    [SerializeField] private TMP_InputField _windowFileContents;

    private GameFile _currentFileInfo;  // Cached info for current instance's file

    private Vector3 mouseOffset;

    public void Initialize(GameFile gf)
    {
        _windowFileName.text = gf.FileName;
        _windowFileContents.text = gf.FileContents;
        _currentFileInfo = gf;
        StartCoroutine(ShowCoroutine());
    }

    public void CloseFileWindow()
    {
        int idx = GameState.CreatedFiles.FindIndex((gf) => gf.FileName == _currentFileInfo.FileName);
        // Save contents of the file
        GameFile currFile = GameState.CreatedFiles[idx];
        currFile.FileContents = _windowFileContents.text;
        GameState.CreatedFiles[idx] = currFile;
        // Close the window
        WindowManager.Instance.CloseFileWindow(idx);
        Destroy(gameObject);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 sizeDelta = GetComponent<RectTransform>().sizeDelta;
        Vector2 newPos = Input.mousePosition + mouseOffset;
        newPos = new (Mathf.Clamp(newPos.x, 0, Screen.width - sizeDelta.x), 
                      Mathf.Clamp(newPos.y, sizeDelta.y, Screen.height));
        transform.position = newPos;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        mouseOffset = transform.position - Input.mousePosition;
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
