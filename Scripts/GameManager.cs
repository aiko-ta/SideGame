using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject mainimage;
    public Sprite gameOverSpr;
    public Sprite gameClearSpr;
    public GameObject panel;
    public GameObject restartButton;
    public GameObject nextButton;

    Image titleImage;

    //時間制限追加
    public GameObject timeBar;
    public GameObject timeText;
    TimeController timeCnt;

    //スコア
    public GameObject scoreText;
    public static int totalScore;
    public static int stageScore;

    //サウンド
    public AudioClip meGameOver;
    public AudioClip meGameClear;

    //+++プレイヤー操作+++
    public GameObject inputUI; //タッチ操作のUIパネル

    // Start is called before the first frame update
    void Start()
    {
        Invoke("InactiveImage",1.0f);//メソッドを指定した秒数遅らせて実行
        panel.SetActive(false);

        timeCnt = GetComponent<TimeController>();
        if (timeCnt != null)
        {
            if (timeCnt.gameTime == 0.0f)
            {
                timeBar.SetActive(false);//制限時間なしは隠す
            }
        }

        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerController.gameState == "gameclear")
        {
            mainimage.SetActive(true);
            panel.SetActive(true);

            Button bt = restartButton.GetComponent<Button>();
            bt.interactable = false;
            mainimage.GetComponent<Image>().sprite = gameClearSpr;
            PlayerController.gameState = "gameend";

            if (timeCnt != null)
            {
                timeCnt.isTimeOver = true;//時間停止

                //スコア追加
                int time = (int)timeCnt.displayTime;
                totalScore += time * 10;
            }

            totalScore += stageScore;
            stageScore = 0;
            UpdateScore();

            //サウンド再生
            AudioSource soundPlayer = GetComponent<AudioSource>();
            if (soundPlayer != null )
            {
                soundPlayer.Stop();
                soundPlayer.PlayOneShot(meGameClear);
            }

            //+++プレイヤー操作+++
            inputUI.SetActive(false); //タッチ操作UIを隠す

        }
        else if(PlayerController.gameState == "gameover")
        {
            mainimage.SetActive(true);
            panel.SetActive(true);

            Button bt = nextButton.GetComponent<Button>();
            bt.interactable = false;
            mainimage.GetComponent <Image>().sprite = gameOverSpr;
            PlayerController.gameState = "gameend";

            if (timeCnt != null)
            {
                timeCnt.isTimeOver = true;//時間停止
            }

            //サウンド再生
            AudioSource soundPlayer = GetComponent<AudioSource>();
            if (soundPlayer != null)
            {
                soundPlayer.Stop();
                soundPlayer.PlayOneShot(meGameOver);
            }

            //+++プレイヤー操作+++
            inputUI.SetActive(false); //タッチ操作UIを隠す

        }
        else if(PlayerController.gameState == "playing")
        {
            //ゲーム中
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            PlayerController playerCnt = player.GetComponent<PlayerController>();

            //タイム更新
            if (timeCnt != null)
            {
                if (timeCnt.gameTime > 0.0f)
                {
                    int time = (int)timeCnt.displayTime;
                    timeText.GetComponent<TextMeshProUGUI>().text = time.ToString();
                    if(time == 0)
                    {
                        playerCnt.GameOver();
                    }
                }
            }

            //スコア追加
            if (playerCnt.score != 0)
            {
                stageScore += playerCnt.score;
                playerCnt.score = 0;
                UpdateScore();
            }
        }
        
    }

    void InactiveImage()
    {
        mainimage.SetActive(false);
    }

    void UpdateScore()
    {
        int score = stageScore + totalScore;
        scoreText.GetComponent<TextMeshProUGUI>().text = score.ToString();
    }

    //+++プレイヤー操作+++
    //GameManager経由のジャンプメソッドの制作
    public void Jump()
    {
        //プレイヤーオブジェクトを変数playerに取得
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        //プレイヤーについているPlayerController.csを変数playerCntに取得
        PlayerController playerCnt = player.GetComponent<PlayerController>();

        //PlayerControllerのJumpメソッドを発動
        playerCnt.Jump();
    }
}
