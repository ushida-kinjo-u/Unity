// P145 List5-2 GameManager.cs

using UnityEngine;
using UnityEngine.UI;               // UIを使うのに必要
using UnityEngine.SceneManagement;  // シーンの切り替えに必要

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("InactiveImage", 1.0f);  // 1秒後に画像を非表示にする
        panel.SetActive(false);         // パネルを非表示にする
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
        }
        else if (PlayerController.gameState == GameState.InGame)
        {
            // ゲーム中
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
}
