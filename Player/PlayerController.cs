using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody;              // Rigidbody2D型の変数
    float axisH = 0.0f;             //入力
    //float axisY = 0.0f;             //入力

    public float speed = 3.0f;      // 移動速度

    public float jump = 9.0f;       // ジャンプ力
    public LayerMask groundLayer;   // 着地できるレイヤー
    bool goJump = false;            // ジャンプ開始フラグ

    // アニメーション対応
    Animator animator; // アニメーター
    public string stopAnime = "PlayerStop";
    public string moveAnime = "PlayerMove";
    public string jumpAnime = "PlayerJump";
    public string goalAnime = "PlayerGoal";
    public string deadAnime = "PlayerOver";
    public static string gameState = "playing";
    string nowAnime = "";
    string oldAnime = "";

    public int score = 0;

    //タッチ操作対応追加
    bool isMoving = false; //タッチ操作中かどうかのフラグ

    // Start is called before the first frame update
    void Start()
    {
        // 処理が始まる前に動くところ

        // Rigidbody2Dを取ってくる
        rbody = this.GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();// Animator を取ってくる
        nowAnime = stopAnime; // 停止から開始する
        oldAnime = stopAnime; //停止から開始する

        gameState = "playing";
    }

    // Update is called once per frame
    void Update()
    {
        if(gameState != "playing")
        {
            return;
        }

        // 入力処理はここで


        //水平方向の入力をチェックする
        if (isMoving == false)
        {
            //水平方向の入力をチェック
            axisH = Input.GetAxisRaw("Horizontal");
        }

        //axisY = Input.GetAxisRaw("Vertical");

        // 向きの調整
        if (axisH > 0.0f)
        {
            // 右移動
            //Debug.Log("右移動");
            transform.localScale = new Vector2(1, 1);
        }
        else if (axisH < 0.0f)
        {
            // 左移動
            //Debug.Log("左移動");
            transform.localScale = new Vector2(-1, 1); // 左右反転させる
        }
        // キャラクターをジャンプさせる
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

    }

    void FixedUpdate()
    {
        if (gameState != "playing")
        {
            return;
        }


        // 地上判定
        bool onGround = Physics2D.CircleCast(transform.position,   // 発射位置
                                            0.2f,               // 円の半径
                                            Vector2.down,       // 発射方向
                                            0.0f,               // 発射距離
                                            groundLayer);       // 検出するレイヤー

        //Debug.Log(onGround);

        if (onGround || axisH != 0)
        {
            // 地面の上 or 速度が 0 ではない
            // 速度を更新する
            rbody.velocity = new Vector2(speed * axisH, rbody.velocity.y);
        }

        if (onGround && goJump)
        {
            // 地面の上でジャンプキーが押された
            // ジャンプさせる
            Vector2 jumpPw = new Vector2(0, jump);        // ジャンプさせるベクトルを作る
            rbody.AddForce(jumpPw, ForceMode2D.Impulse);  // 瞬間的な力を加える
            goJump = false; // ジャンプフラグを下ろす
        }

        //アニメーション更新
        if(onGround)
        {
            //地面の上
            if(axisH == 0)
            {
                nowAnime = stopAnime;
            }
            else
            {
                nowAnime = moveAnime;
            }
        }
        else
        {
            //空中
            nowAnime = jumpAnime;
        }

        if(nowAnime != oldAnime)
        {
            oldAnime = nowAnime;
            animator.Play(nowAnime);
        }

    }

    // ジャンプ
    public void Jump()
    {
        goJump = true; // ジャンプフラグを立てる
    }

    //接触開始
    void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "Goal")
        {
            Goal();
        }
        else if (collision.gameObject.tag == "Dead")
        {
            GameOver();
        }
        else if(collision.gameObject.tag == "ScoreItem")
        {
            ItemData item = collision.gameObject.GetComponent<ItemData>();
            score = item.value;
            Destroy(collision.gameObject);
        }
    }

    // ゴール
    public void Goal()
    {
        animator.Play(goalAnime);

        gameState = "gameclear";

        GameStop();
    }

    // ゲームオーバー
    public void GameOver()
    {
        animator.Play(deadAnime);

        gameState = "gameover";

        GameStop();

        Debug.Log("デバック");
        GetComponent<CapsuleCollider2D>().enabled = false;
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
    }

    void GameStop()
    {
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        rbody.velocity = new Vector2(0, 0);
    }
    //タッチスクリーン対応追加
    //第一引数のhは水平（横）、第二引数のvは垂直（縦）を担当
    public void SetAxis(float h, float v)
    {
        //パッドの水平(横)方向の値を引数から拾う
        axisH = h;

        //もしパッドの水平(横)方向の値が0なら
        if (axisH == 0)
        {
            //パッドの水平の力が0だと、Updateメソッドにおいてキーボード操作が反応可
            isMoving = false;
        }
        else
        {
            //パッドの水平の力が入っていると、Updateメソッドにおいてキーボード操作が反応しない
            isMoving = true;
        }
    }
}