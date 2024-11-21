using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float leftLimit = 0.0f;
    public float rightLimit = 0.0f;
    public float topLimit = 0.0f;
    public float bottomLimit = 0.0f;

    public GameObject subScreen;

    public bool isForceScrollX = false;//trueの時はスクロースするようにする
    public float forceScrollSpeedX = 0.5f;
    public bool isForceScrollY = false;
    public float forceScrollSpeedY = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null )
        {
            //カメラの更新座標
            float x = player.transform.position.x;
            float y = player.transform.position.y;
            float z = transform.position.z;



            //横同期
            if (isForceScrollX)
            {
                x = transform.position.x+(forceScrollSpeedX*Time.deltaTime);
            }

            //両端に移動制限をつける
            if (x < leftLimit)
            { 
                x = leftLimit;
            }
            else if (x > rightLimit)
            {
                x = rightLimit;
            }

            //横同期
            if (isForceScrollY)
            {
                y = transform.position.y + (forceScrollSpeedY * Time.deltaTime);
            }

            //Time.deltaTaime　掛け算するとマシン間の差をなくす 

            //上下に移動制限をつける
            if (y < bottomLimit)
            {
                y = bottomLimit;
            }
            else if (y > topLimit)
            {
                y = topLimit;
            }

            Vector3 v3 = new Vector3(x, y, z);
            transform.position = v3;

            //サブスクリーンスクロール
            if(subScreen != null)
            {
                y = subScreen.transform.position.y;
                x = subScreen.transform.position.x;
                Vector3 v = new Vector3(x/2.0f, y, z);
                subScreen.transform.position = v;
            }
        }
    }
}
