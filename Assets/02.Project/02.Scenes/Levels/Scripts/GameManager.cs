using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LevelData Data;
    public List<int> listCoin = new List<int>();
    
    public bool[] luby = new bool[4];

    private void Update()
    {
        foreach (bool i in luby)
        {
            //Debug.Log(i.);
        }
    }

}
