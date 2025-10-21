// P238 List7-9 TitleManager.cs
// Input Actionを使った入力の取得

using UnityEngine;
using UnityEngine.SceneManagement;  // シーンの切り替えに必要
using UnityEngine.InputSystem;      // InputSystemを使うのに必要

public class TitleManager : MonoBehaviour
{
    public string sceneName;            // 読み込むシーン名
    public InputAction submitAction;    // 決定のInputAction

    void OnEnable()
    {
        submitAction.Enable();      // Input Actionを有効化
    }

    void OnDisable()
    {
        submitAction.Disable();     // Input Actionを無効化
    }

    void Start()
    {
        
    }

    void Update()
    {
        // Input Actionを使った入力の取得
        if (submitAction.WasPressedThisFrame())
        {
            Load();
        }

    }

    // シーンを読み込む
    public void Load()
    {
        SceneManager.LoadScene(sceneName);
    }
}
