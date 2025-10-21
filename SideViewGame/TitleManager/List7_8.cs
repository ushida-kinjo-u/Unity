// P237 List7-8 TitleManager.cs
// Input Deviceを使った入力の取得

using UnityEngine;
using UnityEngine.SceneManagement;  // シーンの切り替えに必要
using UnityEngine.InputSystem;      // InputSystemを使うのに必要

public class TitleManager : MonoBehaviour
{
    public string sceneName;            // 読み込むシーン名
    
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        //Input Deviceを使った入力の取得
        Keyboard kb = Keyboard.current;
        if (kb != null)
        {
            if (kb.enterKey.wasPressedThisFrame)
            {
                Load();
            }
        }
    }

    // シーンを読み込む
    public void Load()
    {
        SceneManager.LoadScene(sceneName);
    }
}
