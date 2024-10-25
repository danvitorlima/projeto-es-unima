using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using UnityEngine;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
    private int corridorLength = 14, corridorCount = 5;
    [SerializeField]
    [Range(0.1f, 1)]
    private float roomPercent = 0.8f;
    [SerializeField]
    private GameObject jogador;
    [SerializeField]
    private GameObject inimigo;

    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }
    private void Start()
    {
        GenerateDungeon();
    }
    private void CorridorFirstGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();
        Vector2Int escadaPos = new Vector2Int();
        Vector2Int spawnPos = new Vector2Int();

        CreateCorridors(floorPositions, potentialRoomPositions);

        var roomPosEPontosDePatrulha = CreateRooms(potentialRoomPositions);

        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>(roomPosEPontosDePatrulha.Item1);

        List<Vector2Int> deadEnds = FindAllDeadEnds(floorPositions);

        roomPosEPontosDePatrulha.Item2.UnionWith(CreateRoomsAtDeadEnd(deadEnds, roomPositions));
        
        float distMax = 0;
        float distMin = Mathf.Infinity;
        foreach (var position in potentialRoomPositions.Where(n => roomPositions.Contains(n)))
        {
            float dist = Vector2.Distance(position, Vector2.zero);
            if (dist < distMin)
            {
                distMin = dist;
                spawnPos = position;
            }
            if (dist > distMax)
            {
                distMax = dist;
                escadaPos = position;
            }
        }
        foreach (var pp in roomPosEPontosDePatrulha.Item2)
        {
            Instantiate(inimigo, (Vector3Int)pp, Quaternion.identity);
            tilemapVisualizer.ColocarPP(pp);
        }

        floorPositions.UnionWith(roomPositions);
        tilemapVisualizer.PaintFloorTiles(floorPositions);
        tilemapVisualizer.ColocarEscada(escadaPos);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
        jogador.transform.position = (Vector3Int)spawnPos;
        jogador.SetActive(true);
    }

    private HashSet<Vector2Int> CreateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors)
    {
        HashSet<Vector2Int> pontosDePatrulha = new HashSet<Vector2Int>();
        foreach (var position in deadEnds)
        {
            if (roomFloors.Contains(position) == false)
            {
                var room = RunRandomWalk(randomWalkParameters, position);
                for (int i = 0; i < 6; i++)
                {
                    int index = UnityEngine.Random.Range(0, room.Count);
                    pontosDePatrulha.Add(room.ToList()[index]);
                }
                roomFloors.UnionWith(room);
            }
        }
        return pontosDePatrulha;
    }

    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();
        foreach (var position in floorPositions)
        {
            int neighboursCount = 0;
            foreach (var direction in Direction2D.cardinalDirectionsList)
            {
                if (floorPositions.Contains(position + direction))
                    neighboursCount++;

            }
            if (neighboursCount == 1)
                deadEnds.Add(position);
        }
        return deadEnds;
    }

    private Tuple<HashSet<Vector2Int>, HashSet<Vector2Int>> CreateRooms(HashSet<Vector2Int> potentialRoomPositions)
    {
        HashSet<Vector2Int> pontosDePatrulha = new HashSet<Vector2Int>();
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercent);

        //selecionando posições aleatórias de salas potenciais
        List<Vector2Int> roomsToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();

        foreach (var roomPosition in roomsToCreate)
        {
            var roomFloor = RunRandomWalk(randomWalkParameters, roomPosition);
            for (int i = 0; i < 6; i++)
            {
                int index = UnityEngine.Random.Range(0, roomFloor.Count);
                pontosDePatrulha.Add(roomFloor.ToList()[index]);
            }
            roomPositions.UnionWith(roomFloor);
        }
        return Tuple.Create(roomPositions, pontosDePatrulha);
    }

    private void CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions)
    {
        var currentPosition = startPosition;
        //colocando inicio de corredores em posições potenciais de salas
        potentialRoomPositions.Add(currentPosition);

        for (int i = 0; i < corridorCount; i++)
        {
            var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, corridorLength);
            currentPosition = corridor[corridor.Count - 1];
            potentialRoomPositions.Add(currentPosition);
            floorPositions.UnionWith(corridor);
        }
    }
}
