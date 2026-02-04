using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Apple : MonoBehaviour
{
    [Range(1f,10f)]
    public int number;

    public Transform[] sprites;

    public TextMeshPro numberTxt;
    public SpriteRenderer spriteRender;

    // Start is called before the first frame update
    void Start()
    {
        SetNumber(); 
                
        spriteRender = GetSpriteRender();
        if(spriteRender == null)
        {
            Debug.Log("spriteRender null!");
            return;
        }

        offOutLine();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNumber()
    {
        int rnd = Random.Range(1, 10);
        number = rnd;
        numberTxt.text = number.ToString();
    }

    public SpriteRenderer GetSpriteRender()
    {
        SpriteRenderer spriteRen = null;

        foreach (Transform t in sprites)
        {
            if (t.gameObject.activeSelf)
            {
                spriteRen = t.GetComponent<SpriteRenderer>();
            }
        }

        return spriteRen;
    }

    public void onOutLine()
    {
        spriteRender.material.SetFloat("_OutlineEnabled", 1);
    }

    public void offOutLine()
    {
        spriteRender.material.SetFloat("_OutlineEnabled", 0);
    }
}
