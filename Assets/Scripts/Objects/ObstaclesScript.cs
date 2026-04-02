using System.Collections.Generic;
using UnityEngine;

public class ObstaclesScript : MonoBehaviour
{
    public List<HexTile> Tiles;

    public List<GameObject> DestroyObjects;

    ObjectManager objectManager;

    void Start()
    {
        objectManager = GameManager.Instance.objectManager;

        objectManager.RegisterObject(this);
    }

    public void DestroyObject()
    {
        foreach (GameObject obj in DestroyObjects)
        {
            Destroy(obj);
        }

        Destroy(this.gameObject);
    }
}
