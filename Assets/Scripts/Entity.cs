
using UnityEngine;

public class Entity : MonoBehaviour
{

  private int _health = 100;
  public int baseAttack = 10;

  public Entity()
  {
    Debug.Log("Entity constructor");
  }

  public bool GetAttack(int damage)
  {
    Debug.Log(_health);
    _health -= damage;
    Debug.Log(_health);

    if (_health <= 0)
    {
      Destroy(gameObject);
      return false;
    }
    return true;
  }
}
