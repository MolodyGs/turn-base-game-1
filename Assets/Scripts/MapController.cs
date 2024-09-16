
using System;
using System.Collections.Generic;
using System.IO;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MapController : MonoBehaviour
{
  private List<string> _map = new();
  [SerializeField] public List<GameObject> entities = new(new GameObject[100]);
  [SerializeField] private GameObject _tilePrefab;
  [SerializeField] private GameObject _playerPrefab;
  [SerializeField] private GameObject[] _enemiesPrefab;
  [SerializeField] private GameObject _boxPrefab;

  private static MapController _instance;

  public static MapController Instance
  {
    get
    {
      if (_instance == null)
      {
        _instance = FindObjectOfType<MapController>();
        if (_instance == null)
        {
          GameObject singleton = new("MapController");
          _instance = singleton.AddComponent<MapController>();
        }
      }
      return _instance;
    }
  }

  private void Awake()
  {
    if (_instance == null)
    {
      _instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else if (_instance != this)
    {
      Destroy(gameObject);
    }
  }

  void Start()
  {
    LoadMap();
  }

  private void LoadMap()
  {
    Debug.Log("Loading map...");

    string filePath = Application.dataPath + "/Resources/map.csv";
    string[] lines = File.ReadAllLines(filePath);
    int tileYIndex = 9;
    int tileXIndex = 0;
    foreach (string line in lines)
    {
      string[] tiles = line.Split(',');
      foreach (string tile in tiles)
      {
        _map.Add(tile);

        GameObject tileObj = Instantiate(_tilePrefab, new Vector2(tileXIndex, tileYIndex), Quaternion.identity);
        tileObj.transform.GetComponent<SpriteRenderer>().sortingOrder = -1;

        TileInterpretation(tile, tileXIndex, tileYIndex);
        tileXIndex += 1;
      }
      tileXIndex = 0;
      tileYIndex -= 1;
    }
  }

  private void TileInterpretation(string tile, int x, int y)
  {
    string[] tileInfo = tile.Split(':');
    GameObject entity;
    switch (tileInfo[0])
    {
      case "P":
        Debug.Log("Player");
        Instantiate(_playerPrefab, new Vector2(x, y), Quaternion.identity);
        break;
      case "e":
        Debug.Log("Enemy");
        entity = Instantiate(_enemiesPrefab[int.Parse(tileInfo[2])], new Vector2(x, y), Quaternion.identity);
        entities[y * 10 + x] = entity;
        break;
    }
  }
}
