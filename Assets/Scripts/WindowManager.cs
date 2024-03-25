using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{

    public static WindowManager Instance;

    [Header("Prefab Assignments")]
    [SerializeField] private GameObject _windowPrefab;
    [Header("Object Assignments")]
    [SerializeField] private Transform _windowParentTransform;

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
    public void OpenFileWindow(int gfIndex)
    {
        GameFile currFile = GameState.CreatedFiles[gfIndex];
        currFile.IsOpen = true;
        GameState.CreatedFiles[gfIndex] = currFile;
        GameObject windowObj = Instantiate(_windowPrefab, _windowParentTransform);
        windowObj.GetComponent<FileWindowHandler>().Initialize(currFile);
    }

    public void CloseFileWindow(int gfIndex)
    {
        GameFile currFile = GameState.CreatedFiles[gfIndex];
        currFile.IsOpen = false;
        GameState.CreatedFiles[gfIndex] = currFile;
    }

}
