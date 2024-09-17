
using UnityEngine;
using UnityEngine.Analytics;
public class TilePointerController : MonoBehaviour
{
  GameObject previousHit = null;
  Color previousColor = new(255, 255, 255);

  Color lightColor = new(0.1f, 0.1f, 0.1f);
  SpriteRenderer spriteRenderRef;
  void Update()
  {
    LightTile();
  }

  //Light a tile if the mouse is over it and the user click it
  private void LightTile()
  {
    if (previousHit != null) { previousHit.GetComponent<SpriteRenderer>().color = previousColor; }

    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

    if (hit.collider != null && !hit.transform.CompareTag("Tile")) { return; }

    spriteRenderRef = hit.transform.GetComponent<SpriteRenderer>();
    previousHit = hit.transform.gameObject;
    previousColor = spriteRenderRef.color;

    if (hit.collider != null) { spriteRenderRef.color = previousColor + lightColor; }
  }
}
