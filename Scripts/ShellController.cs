using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellController : MonoBehaviour
{

    public float deleteTime = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        //第２引数は時間差の指定
        Destroy(gameObject,deleteTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
