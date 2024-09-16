
using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[Serializable]
public class EntityList
{
  public EntityData[] entities;
}

[Serializable]
public class EntityData
{
  public int id;
  public int health;
  public int baseAttack;
  public string entityName;
}

public class MapController : MonoBehaviour
{
  private List<string> _map = new();
  [SerializeField] public List<GameObject> entities = new(new GameObject[100]);
  [SerializeField] private GameObject _tilePrefab;
  [SerializeField] private GameObject _playerPrefab;
  [SerializeField] private GameObject[] _enemiesPrefab;
  [SerializeField] private GameObject _boxPrefab;
  [SerializeField] private GameObject _wallPrefab;
  private Entity[] entitiesData;
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
    LoadEntitiesFile();
    LoadMap();
  }

  private void LoadEntitiesFile()
  {
    string filePath = Application.dataPath + "/Resources/entitiesID.json";
    string data = File.ReadAllText(filePath);
    EntityList entityList = JsonUtility.FromJson<EntityList>(data);
    Entity[] entities = new Entity[100];
    foreach (EntityData entity in entityList.entities)
    {
      entities[entity.id] = new()
      {
        id = entity.id,
        health = entity.health,
        baseAttack = entity.baseAttack,
        entityName = entity.entityName
      };
    }
    entitiesData = entities;
  }

  private void LoadMap()
  {
    LoadWorldEgdes();
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

        GameObject tileObj = Instantiate(_tilePrefab, new Vector3(tileXIndex, tileYIndex, -5), Quaternion.identity);
        tileObj.transform.GetComponent<SpriteRenderer>().sortingOrder = -1;

        TileInterpretation(tile, tileXIndex, tileYIndex);
        tileXIndex += 1;
      }
      tileXIndex = 0;
      tileYIndex -= 1;
    }
    foreach (GameObject entity in entities)
    {
      if (entity != null)
      {
        entity.transform.GetComponent<Enemy>().ShowData();
      }
    }
  }

  private void LoadWorldEgdes()
  {
    for (int i = 0; i <= 11; i++)
    {
      Instantiate(_wallPrefab, new Vector2(i - 1, -1), Quaternion.identity);
      Instantiate(_wallPrefab, new Vector2(i - 1, 10), Quaternion.identity);
    }
    for (int i = 0; i < 10; i++)
    {
      Instantiate(_wallPrefab, new Vector2(-1, i), Quaternion.identity);
      Instantiate(_wallPrefab, new Vector2(10, i), Quaternion.identity);
    }
  }

  private void TileInterpretation(string tile, int x, int y)
  {
    string[] tileInfo = tile.Split(':');
    Entity entityData;
    GameObject entityObj;
    switch (tileInfo[0])
    {
      case "P":
        Debug.Log("Player");
        Instantiate(_playerPrefab, new Vector2(x, y), Quaternion.identity);
        break;
      case "e":
        entityData = entitiesData[int.Parse(tileInfo[2])];
        entityObj = Instantiate(_enemiesPrefab[int.Parse(tileInfo[2])], new Vector2(x, y), Quaternion.identity);
        entityObj.AddComponent<Enemy>();
        Enemy enemyComponent = entityObj.GetComponent<Enemy>();
        if (enemyComponent == null)
        {
          Debug.LogError("No se pudo encontrar el componente Enemy en el objeto instanciado.");
          return;
        }
        Debug.Log(entityData);
        entityObj.GetComponent<Enemy>().Init(entityData.id, entityData.health, entityData.baseAttack, entityData.entityName);
        entities[y * 10 + x] = entityObj;
        break;
    }
  }
}
