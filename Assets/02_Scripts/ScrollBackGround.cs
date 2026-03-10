using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBackGround : MonoBehaviour
{
    public RawImage img;
    public float speed = 0.02f;

    void Start()
    {

    }

    void Update()
    {
        Rect r = img.uvRect;
        r.x += speed * Time.deltaTime;
        r.y += speed * Time.deltaTime;
        img.uvRect = r;
    }
}
