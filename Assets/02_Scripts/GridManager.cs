using DG.Tweening;
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

    public GameObject deleteEffect;
    public GameObject appleSplitPrefab;

    private void Awake()
    {
        Instance = this;
        DOTween.SetTweensCapacity(800, 200);
    }
    // Start is called before the first frame update
    void Start()
    {
        CreateBoard();
        gameField.transform.position = new Vector3(-(columns - 1) * cellWidth / 2f + 0.3f, -(rows - 1) * cellHeight / 2f, 0);
    }

    public void CreateBoard()
    {
        appleGrid = new Apple[columns, rows];

        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Apple apple = Instantiate(applePrefab, gameField.transform).GetComponent<Apple>();

                apple.transform.localPosition = new Vector3(i * cellWidth, j * cellHeight, 0);

                apple.gridPos = new Vector2Int(i, j);
                appleGrid[i, j] = apple;
                AppleSpawnJuicy(apple.transform);
            }
        }

        if (!HasValidPackage())
        {
            RerollNumbers();
        }
    }

    public void ClearBoard()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (appleGrid[i, j] != null)
                {
                    appleGrid[i, j].transform.DOKill(); 
                    Destroy(appleGrid[i, j].gameObject);
                    appleGrid[i, j] = null;
                }
            }
        }
    }
    private IEnumerator ResetBoardRoutine()
    {
        Debug.Log("더 이상 맞출 사과가 없습니다. 판을 리셋합니다!");

        foreach(var apple in appleGrid)
            RemoveApple(apple);

        yield return new WaitForSeconds(1.0f);

        ClearBoard();

        yield return new WaitForSeconds(0.5f); 

        CreateBoard(); 
    }

    public void AppleSpawnJuicy(Transform appleTransform)
    {
        appleTransform.localScale = Vector3.zero;
        Vector3 originalScale = Vector3.one;

        Sequence spawnSeq = DOTween.Sequence();

        spawnSeq.Append(appleTransform.DOScale(new Vector3(0.7f, 1.5f, 0.7f), 0.15f).SetEase(Ease.OutQuad));

        spawnSeq.Append(appleTransform.DOScale(new Vector3(1.3f, 0.7f, 1.3f), 0.1f).SetEase(Ease.OutQuad));

        spawnSeq.Append(appleTransform.DOScale(originalScale, 0.2f).SetEase(Ease.OutBack));
    }

    public Apple GetApple(int x, int y)
    {
        if (x < 0 || x >= columns || y < 0 || y >= rows)
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
            RemoveApple(apple);
        }

        if (!HasValidPackage())
        {
            StartCoroutine(ResetBoardRoutine());
        }
    }

    public void ResetBoardTest()
    {
        StartCoroutine(ResetBoardRoutine());
    }

    public void RemoveApple(Apple apple)
    {
        if (apple == null)
            return;

        apple.transform.DOKill();

        appleGrid[apple.gridPos.x, apple.gridPos.y] = null;
        Destroy(apple.gameObject);
        SplitApple(apple);
        Instantiate(deleteEffect, apple.transform.position, Quaternion.identity);

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


            vaild = HasValidPackage();            
        }
    }

    public void SplitApple(Apple apple)
    {
        GameObject split = Instantiate(appleSplitPrefab, apple.transform.position, Quaternion.identity);

        Rigidbody[] rbs = split.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in rbs)
        {
            rb.isKinematic = false;

            Vector3 force = new Vector3(
                Random.Range(-1.5f, 1.5f),
                Random.Range(2f, 3f),
                Random.Range(-0.5f, 0.5f)
            );

            rb.AddForce(force, ForceMode.Impulse);

            Vector3 torque = Random.insideUnitSphere * 8f;
            rb.AddTorque(torque, ForceMode.Impulse);
        }

        Destroy(split, 3f);
    }
}
