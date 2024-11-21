using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    public float moveX = 0.0f;          // X移動距離
    public float moveY = 0.0f;          // Y移動距離
    public float times = 0.0f;
    public float wait = 0.0f;

    public bool isMoveWhenOn = false;   // 乗った時に動くフラグ
    public bool isCanMove = true;   // 動くフラグ
    Vector3 startPos;       //初期位置
    Vector3 endPos;         //移動位置
    bool isReverse = false; //反転フラグ
    float movep = 0;//移動補完値

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        endPos = new Vector2(startPos.x + moveX, startPos.y + moveY);

        if(isMoveWhenOn)
        {
            isCanMove = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isCanMove)
        {
            float distance = Vector2.Distance(startPos, endPos);
            float ds = distance / times;
            float df = ds * Time.deltaTime;
            movep += df / distance;

            if(isReverse)
            {
                //Lerp滑らかに動く
                transform.position = Vector2.Lerp(endPos,startPos,movep);
            }
            else
            {
                transform.position = Vector2.Lerp(startPos, endPos, movep);
            }

            if(movep >= 1.0f)
            {
                movep = 0.0f;
                isReverse = !isReverse;
                isCanMove = false;
                if(isMoveWhenOn == false)
                {
                    //乗った時に動くフラグOFF
                    Invoke("Move",wait);
                }
            }
        }
    }

    public void Move()
    {
        isCanMove = true;

    }

    public void Stop()
    {
        isCanMove= false;
    }

    //接触開始
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //プレイヤーを動くブロックの子属性にしてしまう
            collision.transform.SetParent(transform);
            if(isMoveWhenOn)
            {
                isCanMove = true;
            }
        }
    }

    //接触終了
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //プレイヤーを動くブロックの子属性にしたのを解除
            collision.transform.SetParent(null);
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector2 fromPos;

        //記録した初期地点が(0,0,0)だったら
        if(startPos == Vector3.zero)
        {
            //ブロックの位置がfromPos
            fromPos = transform.position;
        }
        else
        {
            //でなければ初期地点が描画上も初期地点
            fromPos = startPos;
        }

        //移動線を描画
        Gizmos.DrawLine(fromPos,new Vector2(fromPos.x + moveX,fromPos.y + moveY));

        Vector2 size = GetComponent<SpriteRenderer>().size;

        //移動前の場所を描画
        Gizmos.DrawWireCube(fromPos, new Vector2(size.x,size.y));

        Vector2 toPos = new Vector3(fromPos.x + moveX, fromPos.y + moveY);

        //移動後の場所を描画
        Gizmos.DrawWireCube(toPos, new Vector2(size.x, size.y));
    }
}
