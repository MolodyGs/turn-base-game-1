
using UnityEngine;
public class Entity : MonoBehaviour
{
  public int id;
  public int health;
  public int baseAttack;
  public string entityName;

  public void Init(int id, int health, int baseAttack, string entityName)
  {
    this.id = id;
    this.health = health;
    this.baseAttack = baseAttack;
    this.entityName = entityName;
  }
  public bool GetAttack(int damage)
  {
    health -= damage;

    if (health <= 0)
    {
      Destroy(gameObject);
      return false;
    }
    return true;
  }

  public void ShowData()
  {
    Debug.Log("Entity: " + entityName + " id: " + id + " health: " + health + " baseAttack: " + baseAttack);
  }
}
