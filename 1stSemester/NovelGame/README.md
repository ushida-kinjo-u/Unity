# ノベルゲーム

List 1  TypingText.cs
```csharp copy
using System.Collections;  // コルーチン（IEnumerator）を使うために必要
using UnityEngine;
using UnityEngine.UI;

public class TypingText : MonoBehaviour
{
    public Text targetText;           // 表示するテキスト（Legacy Text）
    public string fullText = "こんにちは、モモ！今日もいい天気だね。"; // 表示する全文
    public float delay = 0.05f;       // 1文字ごとの表示間隔（0.05秒、50ミリ秒）

    private string currentText = "";  // 現在表示中のテキスト（部分）を一時的に保存しておくための変数

    void Start()
    {
        StartCoroutine(ShowText()); //文字を順番に表示していく処理（ShowText関数）を時間をかけて実行
    }

    IEnumerator ShowText()
    {
        for (int i = 0; i <= fullText.Length; i++) 　// i を0からfullText.Length（文字数）まで1ずつ増やす
        {
            currentText = fullText.Substring(0, i);   //文字列の先頭からi文字目までを取り出す
            targetText.text = currentText;          //画面のテキストを更新
            yield return new WaitForSeconds(delay); // delayの時間だけ待つ
        }
    }
}
```

List 2
```csharp
using System.Collections;   // コルーチンを使うために必要
using UnityEngine;
using UnityEngine.UI;      // Text（UI）を使うために必要

public class TypingText : MonoBehaviour
{
    // 表示先のテキストUI（インスペクターで設定する）
    public Text targetText;

    // セリフの一覧。複数行の入力が可能（インスペクターで編集しやすくする属性付き）
    [TextArea(2, 5)]
    public string[] sentences;

    // 1文字ごとの表示間隔（秒）
    public float delay = 0.05f;

    // 現在のセリフのインデックス（何番目か）
    private int currentSentenceIndex = 0;

    // 文字を表示中かどうか（クリック無効化に使う）
    private bool isTyping = false;

    // 次のセリフを表示するのを待っている状態かどうか
    private bool isWaitingForNext = false;

    void Start()
    {
        // 最初のセリフを表示開始（コルーチン実行）
        StartCoroutine(ShowText());
    }

    void Update()
    {
        // 文字表示が完了していて、左クリックされたら次のセリフへ
        if (isWaitingForNext && Input.GetMouseButtonDown(0))  // 0は左クリック
        {
            isWaitingForNext = false;     // 次のセリフを表示する準備に入る
            currentSentenceIndex++;       // 次のセリフへ進む

            // まだ表示していないセリフが残っていれば、もう一度表示開始
            if (currentSentenceIndex < sentences.Length)
            {
                StartCoroutine(ShowText());
            }
            else
            {
                targetText.text = "";     // 全部表示し終わったので、テキストを空にする
            }
        }
    }

    // セリフを1文字ずつ表示する処理（コルーチン）
    IEnumerator ShowText()
    {
        isTyping = true;              // 表示中フラグON
        targetText.text = "";         // 表示をいったんクリア

        // 今表示するセリフ（全文）を取得
        string fullText = sentences[currentSentenceIndex];

        // 1文字ずつ順番に表示
        for (int i = 0; i <= fullText.Length; i++)  // i を0からfullText.Length（文字数）まで1ずつ増やす
        {
            targetText.text = fullText.Substring(0, i);  //文字列の先頭からi文字目までを取り出す
            yield return new WaitForSeconds(delay);      // 指定時間だけ待つ
        }

        isTyping = false;            // 表示完了
        isWaitingForNext = true;     // 次のクリック待ち状態へ
    }
}
```

List 
```csharp
abc
```

List 
```csharp
abc
```

List 
```csharp
abc
```

List 
```csharp
abc
```

List 
```csharp
abc
```
