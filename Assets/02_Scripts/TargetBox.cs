using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TargetBox : MonoBehaviour
{
    public List<Apple> applesInBox = new List<Apple>();

    private Vector2 startPos;
    private Vector2 endPos;
    private Rect selectRect;
    public LineRenderer lineRenderer; 
    
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

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {            
            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            lineRenderer.enabled = true;

        } else if (Input.GetMouseButton(0))
        {
            endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectRect = new Rect(
                Mathf.Min(startPos.x, endPos.x),
                Mathf.Min(startPos.y,endPos.y),
                Mathf.Abs(startPos.x - endPos.x),
                Mathf.Abs(startPos.y - endPos.y)
                );

            UpdateSelectionBox(startPos, endPos);
            CheckApplesInRect();

        }else if (Input.GetMouseButtonUp(0))
        {
            endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            CheckApplesSum();

            UpdateSelectionBox(Vector2.zero, Vector2.zero);

            lineRenderer.enabled = false;
        }
    }

    public void CheckApplesInRect()
    {
        Apple[] apples = FindObjectsOfType<Apple>();
        
        applesInBox.Clear();

        foreach (Apple apple in apples)
        {
            if (selectRect.Contains(apple.transform.position))
            {
                applesInBox.Add(apple);
                apple.onOutLine();
            }
            else
                apple.offOutLine();
        }
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
            GameManager.Instance.AddScore(10);
            foreach (Apple apple in applesInBox)
            {
                Destroy(apple.gameObject);
            }
            applesInBox.Clear();
        }
        
    }
}
