using System;
using System.Collections;
using System.Collections.Generic;
using _Hop._Scripts.ElementRelated;
using _Hop._Scripts.GameplayRelated;
using DG.Tweening;
using Dreamteck.Splines;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileSpawner : MonoBehaviour
{
    public static TileSpawner instance;
    
    [SerializeField] private HopPathGenerator _pathGenerator;
    [SerializeField] private GameObject tilePrefab;   // Tile prefab
    public Transform ball;         // Reference to the ball
    public int tilesAhead = 10;    // Number of tiles to spawn ahead
    private Vector3 nextTilePosition; // Position for the next tile
    private List<Vector3> _tilePositions = new List<Vector3>();

    [SerializeField] private Color currentColor;
    
    [SerializeField] private List<Color> tileColors;
    private int _colorCounter;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        nextTilePosition = Vector3.up * -0.5f;
        _tilePositions.Add(nextTilePosition);
        GameObject firstTile = Instantiate(tilePrefab, nextTilePosition, Quaternion.identity);
        firstTile.GetComponent<Collider>().enabled = false;
        SetCurrentTileColor();
        _zPosAdder += 7;
        StartCoroutine(Spawner());
        _pathGenerator.GenerateHopPath(tilesAhead - 1, true);
    }

    IEnumerator Spawner()
    {
        for (int i = 0; i < tilesAhead - 1; i++)
        {
            yield return new WaitForSeconds(0.1f);
            SpawnTile();
        }
    }

    void Update()
    {
        /*if (Vector3.Distance(ball.position, nextTilePosition) < 5f)
        {
            SpawnTile();
        }*/
    }

    public void SetCurrentTileColor()
    {
        if (_colorCounter < tileColors.Count)
            currentColor = tileColors[_colorCounter++];
        else _colorCounter = 0;
        Debug.Log("Color Set");
    }

    public Color GetCurrentColor()
    {
        return currentColor;
    }
    private float _zPosAdder;
    void SpawnTile()
    {
        int xAxis = Random.Range(0, 3);
        int multiplier = 0;
        if (xAxis > 0)
            multiplier = xAxis == 2 ? -1 : 1;
        nextTilePosition = new Vector3(multiplier * 3.5f, -.5f, _zPosAdder);
        Transform newTile = Instantiate(tilePrefab, nextTilePosition + Vector3.up * 5, Quaternion.identity).transform;
        Debug.Log("Tile Spawn");
        newTile.GetComponent<TileElement>().SetColor(currentColor);
        newTile.DOMove(nextTilePosition, 0.3f).SetEase(Ease.OutBack);
        _zPosAdder += 7;
    }

    public void AddMoreTiles()
    {
        tilesAhead++;
        SpawnTile();
        //_pathGenerator.GenerateHopPath(2, false);
    }
}