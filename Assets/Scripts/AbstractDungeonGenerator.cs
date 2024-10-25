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
