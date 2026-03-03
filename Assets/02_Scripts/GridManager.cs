using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject applePrefab;
    public GameObject gameField;

    public int rows = 10;
    public int columns = 18;        

    public float cellWidth = 0.7f;
    public float cellHeight = 0.7f;

    public static GridManager Instance;

    public Apple[,] appleGrid;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        appleGrid = new Apple[columns, rows];

        for (int i=0;i<columns;i++)
        {
            for(int j=0;j<rows;j++)
            {
                Apple apple = Instantiate(applePrefab,new Vector3(i*cellWidth,j*cellHeight,0), Quaternion.identity, gameField.transform).GetComponent<Apple>();
                apple.gridPos = new Vector2Int(i,j);
                appleGrid[i, j] = apple;

            }
        }
        gameField.transform.position = new Vector3(-(columns-1)*cellWidth/2f, -(rows-1)*cellHeight/2f, 0);
    }

    public Apple GetApple(int x, int y)
    {
        if(x<0 || x>=columns || y<0 || y>=rows)
        {            
            return null;
        }   
        return appleGrid[x, y];
    }
}
