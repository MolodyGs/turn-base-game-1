
using Unity.Collections;
using UnityEngine;

public class MoveController : MonoBehaviour
{
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.W))
    {
      Vector2 newPosition = new(transform.position.x, transform.position.y + 1);
      transform.position = IsValidMove(newPosition) ? newPosition : transform.position;
    }
    else if (Input.GetKeyDown(KeyCode.S))
    {
      Vector2 newPosition = new(transform.position.x, transform.position.y - 1);
      transform.position = IsValidMove(newPosition) ? newPosition : transform.position;
    }
    else if (Input.GetKeyDown(KeyCode.A))
    {
      Vector2 newPosition = new(transform.position.x - 1, transform.position.y);
      transform.position = IsValidMove(newPosition) ? newPosition : transform.position;
    }
    else if (Input.GetKeyDown(KeyCode.D))
    {
      Vector2 newPosition = new(transform.position.x + 1, transform.position.y);
      transform.position = IsValidMove(newPosition) ? newPosition : transform.position;
    }
  }

  private bool IsValidMove(Vector2 newPosition)
  {
    int x = int.Parse(newPosition.x.ToString());
    int y = int.Parse(newPosition.y.ToString());
    if (MapController.Instance.entities[10 * y + x] != null)
    {
      return false;
    }
    return newPosition.x >= 0 && newPosition.x <= 9 && newPosition.y >= 0 && newPosition.y <= 9;
  }
}
