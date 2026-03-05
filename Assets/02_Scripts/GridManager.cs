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

    public Apple[,] GetAllApple()
    {
        return appleGrid;
    }

    public Vector2Int GetGridPosition(Vector3 worldPos)
    {
        Vector3 localPos = worldPos - gameField.transform.position;

        int x = Mathf.RoundToInt(localPos.x / cellWidth);
        int y = Mathf.RoundToInt(localPos.y / cellHeight);
        return new Vector2Int(x, y);
    }

    public void RemoveApples(List<Apple> apples)
    {
        foreach (Apple apple in apples)
        {
            appleGrid[apple.gridPos.x, apple.gridPos.y] = null;
            Destroy(apple.gameObject);
        }
    }

    public void RemoveApple(Apple apple)
    {
        appleGrid[apple.gridPos.x, apple.gridPos.y] = null;
        Destroy(apple.gameObject);
    }

    public bool HasValidPackage()
    {
        return FindOneValidPackage() != null;
    }

    public List<Apple> FindOneValidPackage()
    {
        for (int startX = 0; startX < columns; startX++)
        {
            for (int startY = 0; startY < rows; startY++)
            {
                for (int endX = startX; endX < columns; endX++)
                {
                    for (int endY = startY; endY < rows; endY++)
                    {
                        int sum = 0;
                        List<Apple> selected = new List<Apple>();

                        for (int x = startX; x <= endX; x++)
                        {
                            for (int y = startY; y <= endY; y++)
                            {
                                Apple apple = appleGrid[x, y];

                                if (apple != null)
                                {
                                    sum += apple.number;
                                    selected.Add(apple);
                                }
                            }
                        }

                        if (sum == 10 && selected.Count > 0)
                            return selected;
                    }
                }
            }
        }

        return null;
    }

    public void RerollNumbers()
    {
        bool vaild = false;

        while (!vaild)
        {
            foreach (Apple apple in appleGrid)
            {
                if (apple != null)
                    apple.SetNumber();
            }

            vaild = FindOneValidPackage().Count > 0;
        }
    }
}
