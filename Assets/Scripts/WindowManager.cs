using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WindowManager : MonoBehaviour
{

    public static WindowManager Instance;

    [Header("Prefab Assignments")]
    [SerializeField] private GameObject _fileWindowPrefab;
    [SerializeField] private GameObject _shopWindowPrefab;
    [Header("Object Assignments")]
    [SerializeField] private Transform _windowParentTransform;

    private List<WindowHandler> _openWindowHandlers = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
    }

    /// <summary>
    /// Given a GameFile, displays its window onto the
    /// screen and marks it as open.
    /// </summary>
    public void OpenWindow(int gfIndex)
    {
        GameFile currFile = GameState.CreatedFiles[gfIndex];
        currFile.IsOpen = true;
        GameState.CreatedFiles[gfIndex] = currFile;
        GameObject windowObj = null;
        // Open different window depending on file type
        switch (currFile.FileType) {
            case FileType.FILE:
                windowObj = Instantiate(_fileWindowPrefab, _windowParentTransform);
                break;
            case FileType.SHOP:
                windowObj = Instantiate(_shopWindowPrefab, _windowParentTransform);
                break;
        }
        windowObj.GetComponent<WindowHandler>().Initialize(currFile);
        _openWindowHandlers.Add(windowObj.GetComponent<FileWindowHandler>());
    }

    public void CloseFileWindow(int gfIndex)
    {
        GameFile currFile = GameState.CreatedFiles[gfIndex];
        currFile.IsOpen = false;
        GameState.CreatedFiles[gfIndex] = currFile;
        // Find the handler to close
        int handlerIdx = _openWindowHandlers.FindIndex((wh) => wh.CurrentFileInfo.Equals(currFile));
        _openWindowHandlers[handlerIdx].DestroyWindow();
        _openWindowHandlers.RemoveAt(handlerIdx);
    }

    public void SaveAllFileContents()
    {
        foreach (WindowHandler window in _openWindowHandlers)
        {
            // Save file contents for all FileWindowHandlers
            if (window is FileWindowHandler fileWindow)
            {
                fileWindow.SaveFileContents();
            }
        }
    }

}
