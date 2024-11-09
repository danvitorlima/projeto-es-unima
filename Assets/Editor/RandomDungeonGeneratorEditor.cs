using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbstractDungeonGenerator), true)]
public class RandomDungeonGeneratorEditor : Editor
{
    AbstractDungeonGenerator generator;

    private void Awake()
    {
        generator = (AbstractDungeonGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Create Dungeon"))
        {
            foreach (var inimigo in GameObject.FindGameObjectsWithTag("Inimigo"))
            {
                DestroyImmediate(inimigo);
            }
            foreach (var background in GameObject.FindGameObjectsWithTag("Background"))
            {
                DestroyImmediate(background);
            }
            foreach (var item in GameObject.FindGameObjectsWithTag("Item"))
            {
                DestroyImmediate(item);
            }
            foreach (var xp in GameObject.FindGameObjectsWithTag("XP"))
            {
                DestroyImmediate(xp);
            }
            generator.GenerateDungeon();
        }
    }
}
