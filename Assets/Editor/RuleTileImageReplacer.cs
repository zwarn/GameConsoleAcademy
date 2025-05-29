using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using tilemaps;

public class RuleTileImageReplacer : EditorWindow
{
    private ColorRuleTile sourceTile;
    private List<Sprite> inputSprites = new List<Sprite>();
    private string newTileName = "NewColorRuleTile";

    [MenuItem("Tools/Replace RuleTile Sprites")]
    public static void ShowWindow()
    {
        GetWindow<RuleTileImageReplacer>("RuleTile Replacer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Create New RuleTile from Source", EditorStyles.boldLabel);
        sourceTile = (ColorRuleTile)EditorGUILayout.ObjectField("Source ColorRuleTile", sourceTile, typeof(ColorRuleTile), false);

        if (GUILayout.Button("Add Selected Sprites"))
        {
            foreach (var obj in Selection.objects)
            {
                if (obj is Sprite sprite && !inputSprites.Contains(sprite))
                {
                    inputSprites.Add(sprite);
                }
            }
        }

        newTileName = EditorGUILayout.TextField("New Tile Name", newTileName);

        if (GUILayout.Button("Create New Tile"))
        {
            CreateNewTile();
        }

        GUILayout.Space(10);
        GUILayout.Label("Loaded Sprites:", EditorStyles.boldLabel);
        foreach (var sprite in inputSprites)
        {
            GUILayout.Label(sprite.name);
        }
    }

    private void CreateNewTile()
    {
        if (sourceTile == null || inputSprites.Count == 0)
        {
            Debug.LogError("Source tile or sprites not set.");
            return;
        }

        // Duplicate the tile
        string sourcePath = AssetDatabase.GetAssetPath(sourceTile);
        string newPath = Path.GetDirectoryName(sourcePath) + "/" + newTileName + ".asset";
        AssetDatabase.CopyAsset(sourcePath, newPath);
        AssetDatabase.Refresh();

        ColorRuleTile newTile = AssetDatabase.LoadAssetAtPath<ColorRuleTile>(newPath);

        if (newTile == null)
        {
            Debug.LogError("Failed to load new tile.");
            return;
        }

        // Match sprites by index suffix (e.g., "grass_0")
        var spriteMap = new Dictionary<int, Sprite>();

        foreach (var sprite in inputSprites)
        {
            string name = sprite.name;
            if (name.Contains("_"))
            {
                string[] parts = name.Split('_');
                if (int.TryParse(parts.Last(), out int index))
                {
                    spriteMap[index] = sprite;
                }
            }
        }

        for (int i = 0; i < newTile.m_TilingRules.Count; i++)
        {
            if (spriteMap.TryGetValue(i, out Sprite replacement))
            {
                newTile.m_TilingRules[i].m_Sprites[0] = replacement;
            }
        }

        EditorUtility.SetDirty(newTile);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Created new tile: {newPath}");
    }
}
