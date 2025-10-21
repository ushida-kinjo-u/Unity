// P245 List7-10 TitleManager.cs
// Input ActionアセットとPlayer Inputコンポーネントを使った入力の取得

using UnityEngine;
using UnityEngine.SceneManagement;  // シーンの切り替えに必要
using UnityEngine.InputSystem;      // InputSystemを使うのに必要

public class TitleManager : MonoBehaviour
{
    public string sceneName;            // 読み込むシーン名

    // On + [アクション名]でメソッドを定義する
    void OnSubmit(InputValue value)
    {
        Load();
    }

    void Start()
    {
        
    }

    void Update()
    {

    }

    // シーンを読み込む
    public void Load()
    {
        SceneManager.LoadScene(sceneName);
    }
}
