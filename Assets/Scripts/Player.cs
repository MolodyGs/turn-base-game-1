
using UnityEngine;

public class Player : Entity
{

  private static Player _instance;

  public static Player Instance
  {
    get
    {
      if (_instance == null)
      {
        _instance = FindObjectOfType<Player>();
        if (_instance == null)
        {
          GameObject singleton = new("Player");
          _instance = singleton.AddComponent<Player>();
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

  public Player() : base()
  {
    Debug.Log("Player constructor");
  }
}
