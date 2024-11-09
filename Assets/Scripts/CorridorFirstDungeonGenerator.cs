using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using Unity.Mathematics;
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
    [SerializeField]
    private GameObject estrelas;
    private GameObject[] itens;
    [SerializeField]
    private int maxItensPorSala;
    [SerializeField]
    private int minItensPorSala;
    [SerializeField]
    private int maxInimigosPorSala;
    [SerializeField]
    private int minInimigosPorSala;

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
        itens = Resources.LoadAll<GameObject>("Prefabs/Itens");
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();
        Vector2Int escadaPos = new Vector2Int();
        Vector2Int spawnPos = new Vector2Int();

        CreateCorridors(floorPositions, potentialRoomPositions);

        var roomPosPontosDePatrulhaESpawnDeItens = CreateRooms(potentialRoomPositions);

        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>(roomPosPontosDePatrulhaESpawnDeItens.Item1);

        List<Vector2Int> deadEnds = FindAllDeadEnds(floorPositions);

        var pontosDepatrulhaESpawnDeItens = CreateRoomsAtDeadEnd(deadEnds, roomPositions);

        roomPosPontosDePatrulhaESpawnDeItens.Item2.UnionWith(pontosDepatrulhaESpawnDeItens.Item1);
        roomPosPontosDePatrulhaESpawnDeItens.Item3.UnionWith(pontosDepatrulhaESpawnDeItens.Item2);

        float distMax = 0;
        float distMin = Mathf.Infinity;
        //iterando no centro de cada sala
        foreach (var position in potentialRoomPositions.Where(n => roomPositions.Contains(n)))
        {
            Instantiate(estrelas, (Vector3Int)position, Quaternion.identity);
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
        //instanciando pontos de patrulha
        foreach (var pp in roomPosPontosDePatrulhaESpawnDeItens.Item2)
        {
            Instantiate(inimigo, (Vector3Int)pp, Quaternion.identity);
            tilemapVisualizer.ColocarPP(pp);
        }
        //instanciando itens
        foreach (var item in roomPosPontosDePatrulhaESpawnDeItens.Item3)
        {
            //TODO fazer spawn aleatorio de itens baseado na probabilidade de spawn
            Instantiate(itens[0], (Vector3Int)item, Quaternion.identity);
        }




        floorPositions.UnionWith(roomPositions);
        tilemapVisualizer.PaintFloorTiles(floorPositions);
        tilemapVisualizer.ColocarEscada(escadaPos);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
        jogador.transform.position = (Vector3Int)spawnPos;
        jogador.SetActive(true);
    }

    private Tuple<HashSet<Vector2Int>, HashSet<Vector2Int>> CreateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors)
    {
        HashSet<Vector2Int> pontosDePatrulha = new HashSet<Vector2Int>();
        HashSet<Vector2Int> spawnDeItens = new HashSet<Vector2Int>();
        foreach (var position in deadEnds)
        {
            if (roomFloors.Contains(position) == false)
            {
                var room = RunRandomWalk(randomWalkParameters, position);
                for (int i = 0; i < UnityEngine.Random.Range(minInimigosPorSala, maxInimigosPorSala + 1); i++)
                {
                    int index = UnityEngine.Random.Range(0, room.Count);
                    pontosDePatrulha.Add(room.ToList()[index]);
                }
                for (int i = 0; i < UnityEngine.Random.Range(minItensPorSala, maxItensPorSala + 1); i++)
                {
                    int index = UnityEngine.Random.Range(0, room.Count);
                    spawnDeItens.Add(room.ToList()[index]);
                }
                roomFloors.UnionWith(room);
            }
        }
        return Tuple.Create(pontosDePatrulha, spawnDeItens);
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

    private Tuple<HashSet<Vector2Int>, HashSet<Vector2Int>, HashSet<Vector2Int>> CreateRooms(HashSet<Vector2Int> potentialRoomPositions)
    {
        HashSet<Vector2Int> pontosDePatrulha = new HashSet<Vector2Int>();
        HashSet<Vector2Int> spawnDeItens = new HashSet<Vector2Int>();
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercent);

        //selecionando posições aleatórias de salas potenciais
        List<Vector2Int> roomsToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();

        foreach (var roomPosition in roomsToCreate)
        {
            var roomFloor = RunRandomWalk(randomWalkParameters, roomPosition);
            for (int i = 0; i < UnityEngine.Random.Range(minInimigosPorSala, maxInimigosPorSala + 1); i++)
            {
                int index = UnityEngine.Random.Range(0, roomFloor.Count);
                pontosDePatrulha.Add(roomFloor.ToList()[index]);
            }
            for (int i = 0; i < UnityEngine.Random.Range(minItensPorSala, maxItensPorSala + 1); i++)
            {
                int index = UnityEngine.Random.Range(0, roomFloor.Count);
                spawnDeItens.Add(roomFloor.ToList()[index]);
            }
            roomPositions.UnionWith(roomFloor);
        }
        return Tuple.Create(roomPositions, pontosDePatrulha, spawnDeItens);
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
