
using System.Collections.Generic;
using System.IO;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MapController : MonoBehaviour
{
  private List<string> _map = new();
  [SerializeField] private GameObject _tilePrefab;
  [SerializeField] private GameObject _playerPrefab;
  void Start()
  {
    LoadMap();
  }

  private void LoadMap()
  {
    Debug.Log("Loading map...");

    string filePath = Application.dataPath + "/Resources/map.csv";
    string[] lines = File.ReadAllLines(filePath);
    float tileYIndex = 4.5f;
    float tileXIndex = -4.5f;
    foreach (string line in lines)
    {
      string[] tiles = line.Split(',');
      foreach (string tile in tiles)
      {
        _map.Add(tile);

        GameObject tileObj = Instantiate(_tilePrefab, new Vector2(tileXIndex, tileYIndex), Quaternion.identity);
        tileObj.transform.GetComponent<SpriteRenderer>().sortingOrder = -1;

        TileInterpretation(tile, tileXIndex, tileYIndex);
        tileXIndex += 1f;
      }
      tileXIndex = -4.5f;
      tileYIndex -= 1f;
    }
  }

  private void TileInterpretation(string tile, float x, float y)
  {
    switch (tile)
    {
      case "P":
        Debug.Log("Player");
        Instantiate(_playerPrefab, new Vector2(x, y), Quaternion.identity);
        break;
    }
  }
}
