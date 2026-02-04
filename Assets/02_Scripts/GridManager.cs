using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject applePrefab;
    public GameObject gameField;

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0;i<10;i++)
        {
            for(int j=0;j<7;j++)
            {
                Instantiate(applePrefab,new Vector3(i,j,0), Quaternion.identity, gameField.transform);
            }
        }
        gameField.transform.position = new Vector3(-4.75f, -3f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
