using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TargetBox : MonoBehaviour
{
    public List<Apple> applesInBox = new List<Apple>();

    private Vector2 startPos;
    private Vector2 endPos;

    private Vector2Int startGrid;
    private Vector2Int endGrid;

    //private Rect selectRect;
    public LineRenderer lineRenderer; 
    public SpriteRenderer boxSprite; 

    void UpdateSelectionBox(Vector2 startPos, Vector2 endPos) 
    { 
        Vector3[] corners = new Vector3[4]; 
        corners[0] = new Vector3(startPos.x, startPos.y, 0);
        corners[1] = new Vector3(endPos.x, startPos.y, 0); 
        corners[2] = new Vector3(endPos.x, endPos.y, 0); 
        corners[3] = new Vector3(startPos.x, endPos.y, 0); 
        lineRenderer.positionCount = 4; 
        lineRenderer.loop = true; 
        lineRenderer.SetPositions(corners);

        boxSprite.transform.position = new Vector3((startPos.x + endPos.x) / 2f, (startPos.y + endPos.y) / 2f, 0);
        boxSprite.transform.localScale = new Vector3(Mathf.Abs(endPos.x - startPos.x), Mathf.Abs(endPos.y - startPos.y), 1);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {            
            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lineRenderer.enabled = true;
            boxSprite.enabled = true;

        } else if (Input.GetMouseButton(0))
        {
            endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            endGrid = GridManager.Instance.GetGridPosition(endPos);

            /*
            selectRect = new Rect(
                Mathf.Min(startPos.x, endPos.x),
                Mathf.Min(startPos.y,endPos.y),
                Mathf.Abs(startPos.x - endPos.x),
                Mathf.Abs(startPos.y - endPos.y)
                );
            */

            UpdateSelectionBox(startPos, endPos);
            CheckApplesInRect();

        }else if (Input.GetMouseButtonUp(0))
        {
            endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CheckApplesSum();

            UpdateSelectionBox(Vector2.zero, Vector2.zero);

            lineRenderer.enabled = false;
            boxSprite.enabled = false;
        }
    }

    public void CheckApplesInRect()
    {
        //Apple[] apples = FindObjectsOfType<Apple>();
        
        applesInBox.Clear();

        startGrid = GridManager.Instance.GetGridPosition(startPos);
        endGrid = GridManager.Instance.GetGridPosition(endPos);

        int minX = Mathf.Min(startGrid.x, endGrid.x);
        int maxX = Mathf.Max(startGrid.x, endGrid.x);
        int minY = Mathf.Min(startGrid.y, endGrid.y);
        int maxY = Mathf.Max(startGrid.y, endGrid.y);

        foreach(Apple apple in GridManager.Instance.GetAllApple())
        {
            if (apple == null) continue;
            
            apple.OffOutline();
        }

        for (int x = minX; x<=maxX; x++)
        {
            for(int y = minY; y<=maxY; y++)
            {
                Apple apple = GridManager.Instance.GetApple(x, y);
                if(apple != null)
                {
                    applesInBox.Add(apple);
                    apple.OnOutline();
                }                
            }
        }

        /*
        foreach (Apple apple in apples)
        {
            if (selectRect.Contains(apple.transform.position))
            {
                applesInBox.Add(apple);
                apple.OnOutline();
            }
            else
                apple.OffOutline();
        }
        */
    }

    public void CheckApplesSum()
    {        
        int sum = 0;
        foreach(Apple apple in applesInBox)
        {
            sum += apple.number;
        }

        if(sum == 10)
        {
            float value = 10f * Mathf.Pow(applesInBox.Count - 1, 1.5f);
            GameManager.Instance.AddScore((int)value);
            GridManager.Instance.RemoveApples(applesInBox);
            applesInBox.Clear();

            return;
        }

        foreach(Apple apple in applesInBox)
        {
            apple.OffOutline();
        }        
    }
}
