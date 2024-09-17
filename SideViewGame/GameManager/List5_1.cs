// 最初に作るGameManager

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI を使うのに必要

public class GameManager : MonoBehaviour
{
    public GameObject mainImage;        // 画像を持つ GameObject
    public Sprite gameOverSpr;          // GAME OVER 画像
    public Sprite gameClearSpr;         // GAME CLEAR 画像
    public GameObject panel;            // パネル
    public GameObject restartButton;    // RESTART ボタン
    public GameObject nextButton;       // ネクストボタン

    Image titleImage;                   // 画像を表示している Image コンポーネント

    // Start is called before the first frame update
    void Start()
    {
        // 画像を非表示にする
        Invoke("InactiveImage", 1.0f);
        // ボタン(パネル)を非表示にする
        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.gameState == "gameclear")
        {
            // ゲームクリア
            mainImage.SetActive(true); // 画像を表示する
            panel.SetActive(true); // ボタン(パネル)を表示する
            // RESTART ボタンを無効化する
            Button bt = restartButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameClearSpr;
            PlayerController.gameState = "gameend";

        }

        else if (PlayerController.gameState == "gameover")
        {
            // ゲームオーバー
            mainImage.SetActive(true);      // 画像を表示する
            panel.SetActive(true);          // ボタン(パネル)を表示する
            // NEXT ボタンを無効化する
            Button bt = nextButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameOverSpr;
            PlayerController.gameState = "gameend";
        }

        else if (PlayerController.gameState == "playing")
        {
            // ゲーム中
        }
    }

    // 画像を非表示にする
    void InactiveImage()
    {
        mainImage.SetActive(false);
    }
}
