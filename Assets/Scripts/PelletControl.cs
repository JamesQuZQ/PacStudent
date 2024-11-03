using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PelletControl : MonoBehaviour
{

    public List<Tilemap> tilemaps;
    public List<GameObject> pellets;


    // Start is called before the first frame update
    void Start()
    {
        tilemaps.Add(GameObject.Find("TopLeftQuadrant").GetComponent<Tilemap>());
        tilemaps.Add(GameObject.Find("TopRightQuadrant").GetComponent<Tilemap>());
        tilemaps.Add(GameObject.Find("BottomLeftQuadrant").GetComponent<Tilemap>());
        tilemaps.Add(GameObject.Find("BottomRightQuadrant").GetComponent<Tilemap>());
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < tilemaps.Count; i++)
        {
            if (tilemaps[i].GetTile(new Vector3Int(-7, 1, 0)) == null)
            {
                Destroy(pellets[i]);
            }
        }
    }
}
