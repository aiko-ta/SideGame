using System.Collections;
using System.Collections.Generic;
//using System.Drawing;
using UnityEngine;

public class GimmickBlock : MonoBehaviour
{
    public float length = 0.0f;
    public bool isDelete = false;
    public GameObject deadObj;//死亡あたり

    bool isFell = false;   //落下フラグ
    float fadeTime = 0.5f; //フェードアウト時間

    public bool isDraw = false;   //描画フラグ

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        rbody.bodyType = RigidbodyType2D.Static; //動きをしないようにする
        deadObj.SetActive(false);//死亡あたりを非表示
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float d = Vector2.Distance(transform.position,player.transform.position);//距離計測

            if(length >= d)
            {
                Rigidbody2D rbody = GetComponent<Rigidbody2D>();

                if(rbody.bodyType == RigidbodyType2D.Static)
                {
                    rbody.bodyType = RigidbodyType2D.Dynamic;
                    deadObj.SetActive(true);
                }
            }
        }
        if(isFell)
        {
            //落下時
            fadeTime -= Time.deltaTime;
            Color col = GetComponent<SpriteRenderer>().color;
            col.a = fadeTime;
            GetComponent<SpriteRenderer>().color = col;
            if(fadeTime <= 0.0f)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //トリガーがついてないやつ※ついてるのはonTriggerEnter2D(Collider2D)
        if(isDelete)
        {
            isFell = true;
        }
    }

    void OnDrawGizmosSelected()
    {
        if(isDraw)
        {
            //落下判定の範囲の描画
            Gizmos.DrawWireSphere(transform.position, length);
        }

    }
}
