
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

    Vector2 rayOrigin = new(transform.position.x, transform.position.y);
    Vector2 rayDirection = (new Vector2(x, y) - rayOrigin).normalized;
    Debug.DrawRay(rayOrigin, rayDirection, Color.blue, 5);

    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, 1);
    if (hit.collider != null && hit.transform.CompareTag("Wall"))
    {
      return false;
    }


    if (MapController.Instance.entities[10 * y + x] != null)
    {
      AttackController.Instance.AttackEntity(MapController.Instance.entities[10 * y + x].GetComponent<Entity>());
      return false;
    }

    return true;
  }
}
