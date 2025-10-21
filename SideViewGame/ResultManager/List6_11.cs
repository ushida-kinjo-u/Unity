// P196 List6-11 ResultManager.cs

using UnityEngine;
using UnityEngine.SceneManagement;  // シーンの切り替えに必要
using TMPro;                        // TextMeshProを使うのに必要

public class ResultManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public string sceneName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreText.text = GameManager.totalScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // シーンを読み込む
    public void Load()
    {
        SceneManager.LoadScene(sceneName);
    }
}
