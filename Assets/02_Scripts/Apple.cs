using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Apple : MonoBehaviour
{
    [Range(1f,10f)]
    public int number;

    public Vector2Int gridPos;

    public TextMeshPro numberTxt;

    public Outline outline;

    private float outlineWidth = 5f;
        

    //public SpriteRenderer spriteRender;

    private void Awake()
    {
        outline = GetComponentInChildren<Outline>();
        SetDefaultOutline();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetNumber();

        OffOutline();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDefaultOutline()
    {
        outline.OutlineColor = Color.yellow;
        outline.OutlineWidth = outlineWidth;
    }

    public void SetNumber()
    {
        int rnd = Random.Range(1, 10);
        number = rnd;
        numberTxt.text = number.ToString();
    }

    public MeshRenderer GetMeshRender()
    {
        MeshRenderer meshRen = null;
        foreach (Transform t in transform)        {
            if (t.gameObject.activeSelf && t.CompareTag("Apple"))
            {
                meshRen = t.GetComponent<MeshRenderer>();
            }
        }
        return meshRen;
    }

    public void OnOutline()
    {
        outline.OutlineWidth = outlineWidth;
    }

    public void OffOutline()
    {
        outline.OutlineWidth = 0f;
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.state == GameState.Remove)
        {
            GridManager.Instance.RemoveApple(this);
            GameManager.Instance.EndRemoveItem();
        } 
    }

    private void OnMouseOver()
    {
        if (GameManager.Instance.state != GameState.Remove)
            return;

        outline.OutlineColor = Color.blue;
        OnOutline();
    }
    private void OnMouseExit()
    {
        if (GameManager.Instance.state != GameState.Remove)
            return;

        SetDefaultOutline();
        OffOutline();
    }

}
