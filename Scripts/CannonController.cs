using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{

    public GameObject objPrefab;
    public float delayTime = 3.0f;
    public float fireSpeed = 4.0f;
    public float length = 8.0f;//範囲

    GameObject player;
    Transform gateTransform;
    float passedTimes = 0;

    //距離チェック
    bool CheckLength(Vector2 targetPos)
    {
        bool ret = false;
        float d = Vector2.Distance(transform.position, targetPos);//距離を確認
        if(length >= d)
        {
            ret = true;
        }
        return ret;
    }

    // Start is called before the first frame update
    void Start()
    {
        gateTransform = transform.Find("gate");//子オブジェクトの"gate"を探す
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        passedTimes += Time.deltaTime;
        //playeとの距離チェック
        if(CheckLength(player.transform.position))
        {
            if(passedTimes > delayTime)
            {
                passedTimes = 0;
                //砲弾をプレハブから作る
                Vector2 pos = new Vector2(gateTransform.position.x,gateTransform.position.y);//砲弾の生成場所
                GameObject obj = Instantiate(objPrefab,pos,Quaternion.identity);//砲弾プレハブを生成して代入
                //Quaternion 回転角度0

                //砲身が向いている方向に発射する
                Rigidbody2D rbody = obj.GetComponent<Rigidbody2D>();
                float angleZ = transform.localEulerAngles.z;//砲弾の角度をオイラー値で取得
                float x = Mathf.Cos(angleZ*Mathf.Deg2Rad);// オイラー角度から縦の比率を取得
                float y = Mathf.Sin(angleZ*Mathf.Deg2Rad);// オイラー角度から横の比率を取得
                Vector2 v = new Vector2(x,y)*fireSpeed;
                rbody.AddForce(v,ForceMode2D.Impulse);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position,length);
    }
}
