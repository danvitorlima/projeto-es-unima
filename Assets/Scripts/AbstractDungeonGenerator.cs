using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDungeonGenerator : MonoBehaviour
{
    [SerializeField]
    protected TilemapVisualizer tilemapVisualizer = null;
    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero;

    public void GenerateDungeon()
    {
        foreach (var inimigo in GameObject.FindGameObjectsWithTag("Inimigo"))
        {
            Destroy(inimigo);
        }
        foreach (var background in GameObject.FindGameObjectsWithTag("Background"))
        {
            Destroy(background);
        }
        foreach (var item in GameObject.FindGameObjectsWithTag("Item"))
        {
            Destroy(item);
        }
        foreach (var xp in GameObject.FindGameObjectsWithTag("XP"))
        {
            Destroy(xp);
        }
        tilemapVisualizer.Clear();
        RunProceduralGeneration();
    }
    public void GenerateDungeon(TilemapVisualizer tilemapVisualizer)
    {
        tilemapVisualizer.Clear();
        RunProceduralGeneration();
    }

    protected abstract void RunProceduralGeneration();
}
