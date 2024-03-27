using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FileWindowHandler : WindowHandler
{

    [Header("Object Assignments")]
    [SerializeField] private TMP_InputField _windowFileContents;

    public override void Initialize(GameFile gf)
    {
        base.Initialize(gf);
        _windowFileContents.text = gf.FileContents;
    }

    public override void DestroyWindow()
    {
        SaveFileContents();
        ObjectFactory.Instance.ReturnObjectToPool(gameObject, PoolableType.FILE_WINDOW);
    }

    public void SaveFileContents()
    {
        int idx = GameState.CreatedFiles.FindIndex((gf) => gf.FileName == CurrentFileInfo.FileName);
        // Save contents of the file
        GameFile currFile = GameState.CreatedFiles[idx];
        currFile.FileContents = _windowFileContents.text;
        GameState.CreatedFiles[idx] = currFile;
    }

}
