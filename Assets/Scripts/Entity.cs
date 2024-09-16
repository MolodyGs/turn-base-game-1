
using UnityEngine;

public class Entity : MonoBehaviour
{

  private int _health = 100;
  private int _baseAttack = 10;
  private string _name;

  public Entity(string name, int health, int baseAttack)
  {
    _name = name;
    _health = health;
    _baseAttack = baseAttack;
    Debug.Log("Entity constructor");
  }

  public bool GetAttack(int damage)
  {
    _health -= damage;
    if (_health <= 0)
    {
      return false;
    }
    return true;
  }
}
