
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UIElements;
public class MouseController : MonoBehaviour
{
  GameObject previousHit = null;
  List<GameObject> previousTiles = new();
  Color[] previousColors = new Color[100];

  Color previousColor = new(255, 255, 255);

  Color lightColor = new(0.1f, 0.1f, 0.1f);
  SpriteRenderer spriteRenderRef;
  void Update()
  {
    // LightTile();

    if (Input.GetMouseButtonDown(0))
    {
      // MovePlayerToPoint();
      PathFinding();
    }
  }

  //Light a tile if the mouse is over it and the user click it
  private void LightTile()
  {
    if (previousHit != null) { previousHit.GetComponent<SpriteRenderer>().color = previousColor; }

    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

    if (hit.collider == null || !hit.transform.CompareTag("Tile")) { return; }

    spriteRenderRef = hit.transform.GetComponent<SpriteRenderer>();
    previousHit = hit.transform.gameObject;
    previousColor = spriteRenderRef.color;

    if (hit.collider != null) { spriteRenderRef.color = previousColor + lightColor; }
  }

  List<TileNode> tiles;
  GameObject goalTile;
  private void PathFinding()
  {
    tiles = new();
    goalTile = GetTileObj(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    float goalTileDistance = GoalDistence(Player.Instance.gameObject.transform.position);
    Debug.Log(goalTileDistance);
    TileNode startTile = new(Player.Instance.gameObject, 0, goalTileDistance)
    {
      open = true
    };

    TileNode[] startLocalTiles = GetLocalTiles(startTile);

    foreach (TileNode tile in startLocalTiles)
    {
      if (tile == null) { continue; }
      tiles.Add(tile);
    }

    FindPath();
  }

  int cont = 0;
  private void FindPath()
  {
    float bestTraveledCost = 0;
    float bestHeuristicCost = 0;
    TileNode bestTile = null;

    foreach (TileNode tile in tiles)
    {
      if (tile == null) { continue; }
      if (tile.open) { continue; }
      if (tile.traveledCost + tile.heuristicCost < bestTraveledCost + bestHeuristicCost || bestTile == null)
      {
        bestTraveledCost = tile.traveledCost;
        bestHeuristicCost = tile.heuristicCost;
        bestTile = tile;
      }
    }

    if (bestTile.position == goalTile.transform.position)
    {
      colorPath();
      Debug.Log("Goal reached");
      return;
    }
    else
    {
      bestTile.open = true;
      TileNode[] localTiles = GetLocalTiles(bestTile);
      foreach (TileNode tile in localTiles)
      {
        if (tile == null) { continue; }
        if (tile.open) { continue; }
        tiles.Add(tile);
      }
      cont++;
      FindPath();
    }
  }

  private void colorPath()
  {
    TileNode[] localTiles = GetLocalTiles(new(Player.Instance.gameObject, 0, 0));
    TileNode minorTile = null;
    while (minorTile == null || minorTile.position != goalTile.transform.position)
    {
      minorTile = getMinorCost(localTiles);
      GetTileObj(minorTile.transform.position).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
      localTiles = GetLocalTiles(minorTile);
    }
  }

  private TileNode getMinorCost(TileNode[] localTiles)
  {
    TileNode minorTile = null;
    foreach (TileNode localTile in localTiles)
    {
      if (localTile == null) { continue; }
      if (minorTile == null || localTile.traveledCost + localTile.heuristicCost < minorTile.traveledCost + minorTile.heuristicCost)
      {
        minorTile = localTile;
      }
    }
    return minorTile;
  }

  private TileNode[] GetLocalTiles(TileNode tile)
  {
    TileNode[] localTiles = new TileNode[4];
    Vector3[] directions = new Vector3[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
    for (int i = 0; i < 4; i++)
    {
      TileNode localTile = GetTileNode(tile, tile.position + directions[i]);
      if (localTile == null) { continue; }
      localTiles[i] = localTile;
    }
    return localTiles;
  }

  private TileNode GetTileNode(TileNode tileNodeOrigin, Vector2 position)
  {
    GameObject tileObj = GetTileObj(position);
    if (tileObj == null) { return null; }

    float traveledCost = tileNodeOrigin.traveledCost + 1;
    float heuristicCost = GoalDistence(position);

    TileNode tileNode = tileObj.GetComponent<TileNode>();
    tileNode.traveledCost = traveledCost;
    tileNode.heuristicCost = heuristicCost;

    return tileNode;
  }
  private float GoalDistence(Vector2 start)
  {
    return Mathf.Abs(start.x - goalTile.transform.position.x) + Mathf.Abs(start.y - goalTile.transform.position.y);
  }


  private GameObject GetTileObj(Vector2 position)
  {
    RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);
    if (hit.collider == null || !hit.transform.CompareTag("Tile")) { return null; }
    if (!hit.transform.GetComponent<TileNode>().open)
    {
      hit.transform.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
    }
    hit.transform.GetComponent<TileNode>().position = hit.transform.position;
    return hit.transform.gameObject;
  }
}

