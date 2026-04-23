using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FileReader
{
    public static RawLevelData GetRawLevelData(int majorLevel, int minorLevel, int idVariantIndex)
    {
        string folderPath = $"LevelDatas/map {majorLevel}.{minorLevel}/";

        string idFileName = idVariantIndex == 0 ? "ID" : $"ID {idVariantIndex}";

        TextAsset posFile = Resources.Load<TextAsset>(folderPath + "Pos");
        TextAsset visualIdFlie = Resources.Load<TextAsset>(folderPath + "ListID");
        TextAsset idFile = Resources.Load<TextAsset>(folderPath + idFileName);

        if (posFile == null || visualIdFlie == null || idFile == null)
        {
            Debug.LogError($"Failed to load files for level {majorLevel}.{minorLevel} variant {idVariantIndex}");
            return new RawLevelData { IsValid = false };
        }

        RawLevelData dataPackage = new RawLevelData
        {
            PosText = posFile.text,
            IDText = idFile.text,
            VisualIDText = visualIdFlie.text,
            IsValid = true
        };

        Resources.UnloadAsset(posFile);
        Resources.UnloadAsset(idFile);
        Resources.UnloadAsset(visualIdFlie);

        if (string.IsNullOrEmpty(dataPackage.PosText) ||
            string.IsNullOrEmpty(dataPackage.IDText) ||
            string.IsNullOrEmpty(dataPackage.VisualIDText))
        {
            Debug.LogError("Level data is empty at: " + folderPath);
            dataPackage.IsValid = false;
        }

        return dataPackage;
    }
}
