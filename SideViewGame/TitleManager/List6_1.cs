// P161 List6-1 TitleManager.cs
// タイトル画面からゲーム画面に遷移しよう

using UnityEngine;
using UnityEngine.SceneManagement;  // シーンの切り替えに必要
using UnityEngine.InputSystem;      // InputSystemを使うのに必要

public class TitleManager : MonoBehaviour
{
    public string sceneName;    // 読み込むシーン名

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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

    // On + [アクション名](InputValue) でメソッドを定義する
    void OnSubmit(InputValue value)
    {
        Load();
    }
}
