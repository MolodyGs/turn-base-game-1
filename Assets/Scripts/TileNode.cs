using UnityEngine;

public class TileNode : MonoBehaviour
{
  public float traveledCost = 0;
  public float heuristicCost = 0;
  public float value = 0;
  public readonly GameObject tileObj = null;
  [SerializeField] public Vector3 position;
  public bool open = false;

  public TileNode(GameObject tileObj, float traveledCost, float heuristicCost)
  {
    this.tileObj = tileObj;
    this.traveledCost = traveledCost;
    this.heuristicCost = heuristicCost;
    position = tileObj.transform.position;
    open = false;
  }
}