using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject applePrefab;
    public GameObject gameField;

    public int rows = 10;
    public int columns = 18;

    private float cellWidth = 0.7f;
    private float cellHeight = 0.7f;

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0;i<columns;i++)
        {
            for(int j=0;j<rows;j++)
            {
                Instantiate(applePrefab,new Vector3(i*cellWidth,j*cellHeight,0), Quaternion.identity, gameField.transform);
            }
        }
        gameField.transform.position = new Vector3(-(columns-1)*cellWidth/2f, -(rows-1)*cellHeight/2f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
