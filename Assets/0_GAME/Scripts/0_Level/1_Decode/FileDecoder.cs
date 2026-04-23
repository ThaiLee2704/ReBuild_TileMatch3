using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class FileDecoder
{
    private static readonly CultureInfo InvariantCulture = CultureInfo.InvariantCulture;

    public static List<RawTileData> GetRawTileDatas(RawLevelData rawLevelData)
    {
        if (!rawLevelData.IsValid)
        {
            Debug.LogError("Invalid raw level data provided to decoder.");
            return new List<RawTileData>();
        }

        List<Vector3Int> positions = ParsePosLines(rawLevelData.PosText);
        List<int> tileIds = ParseIntLines(rawLevelData.IDText);
        List<int> rawVisualIds = ParseIntLines(rawLevelData.VisualIDText);

        //Check Raw Level Data is valid
        if (positions.Count == 0 || tileIds.Count == 0 || rawVisualIds.Count == 0)
        {
            Debug.LogError("Failed to parse one or more sections of raw level data.");
            return new List<RawTileData>();
        }

        if (positions.Count != tileIds.Count)
        {
            Debug.LogError($"Mismatch in counts: positions ({positions.Count}) vs ids ({tileIds.Count}).");
            return new List<RawTileData>();
        }

        //Checking unique Tile Ids equal than unique Visual Ids
        var uniqueTileIds = new HashSet<int>(tileIds);
        for (int i = 0; i < tileIds.Count; i++)
            uniqueTileIds.Add(tileIds[i]);

        var visualIds = new List<int>(rawVisualIds.Count);
        var uniqueRawVisualIds = new HashSet<int>(rawVisualIds);
        for (int i = 0; i < rawVisualIds.Count; i++)
        {
            int v = rawVisualIds[i];
            if (!uniqueRawVisualIds.Add(v))
                visualIds.Add(v);
        }

        if (visualIds.Count < uniqueTileIds.Count)
        {
            Debug.LogError(
                $"Decode failed: ListID does not contain enough UNIQUE visual IDs. " +
                $"UniqueVisual={visualIds.Count}, UniqueIDTile={uniqueTileIds.Count}. " +
                $"Fix Resources/.../ListID so it has >= {uniqueTileIds.Count} unique numbers.");
            return new List<RawTileData>();
        }

        ShuffleVisualIds(visualIds);

        var rawTileDatas = new List<RawTileData>(positions.Count);

        //Mapping from Tile Id to Visual Id
        var idToVisualId = new Dictionary<int, int>();
        var visualIndex = 0;

        for (int i = 0; i < positions.Count; i++)
        {
            int tileId = tileIds[i];

            if (!idToVisualId.TryGetValue(tileId, out int visualId))
            {
                if (visualIndex >= visualIds.Count)
                {
                    Debug.LogError("Ran out of visual IDs to assign. This should never happen due to the earlier check.");
                    return new List<RawTileData>();
                }

                visualId = visualIds[visualIndex++];
                idToVisualId.Add(tileId, visualId);
            }

            Vector3Int pos = positions[i];

            rawTileDatas.Add(new RawTileData
            {
                X = pos.x,
                Y = pos.y,
                OrderLayer = pos.z,
                Id = tileId,
                VisualId = visualId
            });
        }

        return rawTileDatas;
    }

    private static void ShuffleVisualIds(List<int> visualIds)
    {
        for (int i = 0; i < visualIds.Count; i++)
        {
            int temp = visualIds[i];
            int randomIndex = UnityEngine.Random.Range(i, visualIds.Count);
            visualIds[i] = visualIds[randomIndex];
            visualIds[randomIndex] = temp;
        }
    }


    #region Parsing Data Raw Level Data to List<RawTileData>
    private static List<Vector3Int> ParsePosLines(string text)
    {
        var result = new List<Vector3Int>(256);
        int i = 0;

        while (TryReadNextNonEmptyLine(text, ref i, out ReadOnlySpan<char> line))
        {
            //Cắt chuỗi thành 3 phần dựa trên dấu '-'.
            //Trả về 3 chuỗi a, b, c nếu thành công.
            if (!TrySplit3(line, '-', out var a, out var b, out var c))
            {
                Debug.LogError("Invalid Pos line: " + line.ToString());
                return new List<Vector3Int>();
            }

            //Sau khi tách chuỗi thành 3 phần
            //Phân tích từng phần thành số nguyên.
            //Trả về 3 int x, y, layer nếu thành công.
            if (!TryParseInt(a, out int x) ||
                !TryParseInt(b, out int y) ||
                !TryParseInt(c, out int layer))
            {
                Debug.LogError("Failed to parse Pos line: " + line.ToString());
                return new List<Vector3Int>();
            }

            result.Add(new Vector3Int(x, y, layer));
        }

        return result;
    }

    private static List<int> ParseIntLines(string text)
    {
        var result = new List<int>(128);
        int i = 0;

        while (TryReadNextNonEmptyLine(text, ref i, out ReadOnlySpan<char> line))
        {
            if (TryParseInt(line, out int value))
            {
                result.Add(value);
                continue;
            }

            Debug.LogError("Failed to parse int line: " + line.ToString());
            return new List<int>();
        }

        return result;
    }

    private static bool TryReadNextNonEmptyLine(string text, ref int index, out ReadOnlySpan<char> line)
    {
        line = default;

        int length = text.Length;

        while (index < length)
        {
            int lineStart = index;

            while (index < length && text[index] != '\n' && text[index] != '\r')
            {
                index++;
            }

            int lineEnd = index;

            while (index < length && (text[index] == '\n' || text[index] == '\r'))
            {
                index++;
            }

            ReadOnlySpan<char> candidate = text.AsSpan(lineStart, lineEnd - lineStart).Trim();
            if (candidate.Length == 0)
            {
                continue;
            }

            line = candidate;
            return true;
        }

        return false;
    }

    private static bool TrySplit3(ReadOnlySpan<char> s, char separator, out ReadOnlySpan<char> a, out ReadOnlySpan<char> b, out ReadOnlySpan<char> c)
    {
        a = default;
        b = default;
        c = default;

        int first = s.IndexOf(separator);
        if (first < 0) return false;

        int second = s.Slice(first + 1).IndexOf(separator);
        if (second < 0) return false;

        second += first + 1;

        a = s.Slice(0, first).Trim();
        b = s.Slice(first + 1, second - first - 1).Trim();
        c = s.Slice(second + 1).Trim();

        return a.Length > 0 && b.Length > 0 && c.Length > 0;
    }

    private static bool TryParseInt(ReadOnlySpan<char> s, out int value)
        => int.TryParse(s, NumberStyles.Integer, InvariantCulture, out value);
    #endregion
}