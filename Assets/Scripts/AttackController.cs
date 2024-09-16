
using UnityEngine;

public class AttackController : MonoBehaviour
{
  private static AttackController _instance;

  public static AttackController Instance
  {
    get
    {
      if (_instance == null)
      {
        _instance = FindObjectOfType<AttackController>();
        if (_instance == null)
        {
          GameObject singleton = new("AttackController");
          _instance = singleton.AddComponent<AttackController>();
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

  public bool AttackEntity(Entity entity)
  {
    Debug.Log(entity);
    bool alive = entity.GetAttack(Player.Instance.baseAttack);
    if (alive)
    {
      Debug.Log("Entity is still alive");
      Player.Instance.GetAttack(entity.baseAttack);
      return true;
    }
    else
    {
      Debug.Log("Entity is dead");
      return false;
    }
  }
}
