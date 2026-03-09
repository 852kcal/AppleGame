using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleRotate : MonoBehaviour
{
    public float speed = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + speed * Time.deltaTime, 0);       
    }
}
