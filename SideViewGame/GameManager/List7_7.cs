// P233 List7-7 GameManager.cs
// プログラムでサウンドを再生／停止しよう

using UnityEngine;
using UnityEngine.UI;               // UIを使うのに必要
using UnityEngine.SceneManagement;  // シーンの切り替えに必要
using TMPro;                        // TextMeshProを使うのに必要

public class GameManager : MonoBehaviour
{
    public GameObject mainImage;        // 画像を持つImageゲームオブジェクト
    public Sprite gameOverSpr;          // GAME OVER画像
    public Sprite gameClearSpr;         // GAME CLEAR画像
    public GameObject panel;            // パネル
    public GameObject restartButton;    // RESTARTボタン
    public GameObject nextButton;       // NEXTボタン
    Image titleImage;                   // 画像を表示するImageコンポーネント

    GameState gamestate = GameState.InGame; // ゲームの状態

    public string nextSceneName;            // 次のシーン名

     // 時間制限追加
    public GameObject timeBar;          // 時間表示イメージ
    public GameObject timeText;         // 時間テキスト
    TimeController timeCnt;             // TimeController

    // スコア追加
    public GameObject scoreText;        // スコアテキスト
    public static int totalScore;       // 合計スコア
    public int stageScore = 0;          // ステージスコア

    // サウンド再生
    public AudioClip meGameClear;       // ゲームクリア
    public AudioClip meGameOver;        // ゲームオーバー

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("InactiveImage", 1.0f);  // 1秒後に画像を非表示にする
        panel.SetActive(false);         // パネルを非表示にする

        // 時間制限追加
        timeCnt = GetComponent<TimeController>();   // TimeControllerを取得
        if (timeCnt != null)
        {
            if (timeCnt.gameTime == 0.0f)
            {
                timeBar.SetActive(false);           // 制限時間なしなら隠す
            }
        }

        // スコア追加
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.gameState == GameState.GameClear)
        {
            // ゲームクリア
            gamestate = GameState.GameClear;
            mainImage.SetActive(true);  // 画像を表示する
            panel.SetActive(true);      // ボタン（パネル）を表示する
            // RESTARTボタンを無効化する
            Button bt = restartButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameClearSpr; // 画像を設定する
            PlayerController.gameState = GameState.GameEnd;

            // 時間制限追加
            if (timeCnt != null)
            {
                timeCnt.isTimeOver = true; // 時間カウント停止
                // スコア追加
                // 整数に代入することで小数を切り捨てる
                int time = (int)timeCnt.displayTime;
                totalScore += time * 10; // 残り時間をスコアに加える
            }
            // スコア追加
            totalScore += stageScore;
            stageScore = 0;
            UpdateScore();  //スコア更新

            // サウンド再生
            AudioSource soundPlayer = GetComponent<AudioSource>();  // AudioSourceを取得
            soundPlayer.Stop();     // BGM停止
            soundPlayer.PlayOneShot(meGameClear);
        }
        else if (PlayerController.gameState == GameState.GameOver)
        {
            // ゲームオーバー
            gamestate = GameState.GameOver;
            mainImage.SetActive(true);  // 画像を表示する
            panel.SetActive(true);      // ボタン（パネル）を表示する
            // NEXTボタンを無効化する
            Button bt = nextButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameOverSpr; // 画像を設定する
            PlayerController.gameState = GameState.GameEnd;

            // 時間制限追加
            if (timeCnt != null)
            {
                timeCnt.isTimeOver = true; // 時間カウント停止
            }

            // サウンド再生
            AudioSource soundPlayer = GetComponent<AudioSource>();  // AudioSourceを取得
            soundPlayer.Stop();     // BGM停止
            soundPlayer.PlayOneShot(meGameOver);
        }
        else if (PlayerController.gameState == GameState.InGame)
        {
            // ゲーム中
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            // PlayerControllerを取得する
            PlayerController playerCnt = player.GetComponent<PlayerController>();
            // 時間制限追加
            // タイムを更新する
            if (timeCnt != null)
            {
                if (timeCnt.gameTime > 0.0f)
                {
                    // 整数に代入することで小数を切り捨てる
                    int time = (int)timeCnt.displayTime;
                    // タイム更新
                    timeText.GetComponent<TextMeshProUGUI>().text = time.ToString();
                    // タイムオーバー
                    if (time == 0)
                    {
                        playerCnt.GameOver(); // ゲームオーバーにする
                    }
                }
            }
            // スコア追加
            if (playerCnt.score != 0)
            {
                stageScore += playerCnt.score;
                playerCnt.score = 0;
                UpdateScore();  //スコア更新
            }
        }
    }

    // 画像を非表示にする
    void InactiveImage()
    {
        mainImage.SetActive(false);
    }

    //リスタート
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //次へ
    public void Next()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    // スコア更新
    void UpdateScore()
    {
        int score = stageScore + totalScore;
        scoreText.GetComponent<TextMeshProUGUI>().text = score.ToString();
    }
}
