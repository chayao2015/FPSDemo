using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;

public class FBXToPrefab 
{
    private static string prefabExtension = ".prefab";

    [MenuItem("Example/FBX to Prefab")]
    public static void Generate()
    {
        GameObject selectedGameObject = Selection.activeGameObject;
        string selectedAssetPath = AssetDatabase.GetAssetPath(selectedGameObject);
        if(string.IsNullOrEmpty(selectedAssetPath))
        {
            return;
        }
		string path = StringUtility.getFilePath(selectedAssetPath);
        GameObject cloneObj = GameObject.Instantiate<GameObject>(selectedGameObject);
        cloneObj.name = cloneObj.name.Replace("(Clone)", string.Empty);
        string genPrefabFullName = string.Concat(path, "/", cloneObj.name, prefabExtension);
        Object prefabObj = PrefabUtility.CreateEmptyPrefab(genPrefabFullName);
        PrefabUtility.ReplacePrefab(cloneObj, prefabObj);
        GameObject.DestroyImmediate(cloneObj);
    }
}