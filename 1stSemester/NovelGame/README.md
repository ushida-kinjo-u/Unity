# ノベルゲーム

List 1  TypingText.cs
```csharp
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
        for (int i = 0; i <= fullText.Length; i++)  // i を0からfullText.Length（文字数）まで1ずつ増やす
        {
            currentText = fullText.Substring(0, i);   //文字列の先頭からi文字目までを取り出す
            targetText.text = currentText;            //画面のテキストを更新
            yield return new WaitForSeconds(delay);   // delayの時間だけ待つ
        }
    }
}
```

List 2 TypingText.cs
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

List 3 TypingText.cs
```csharp
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TypingText : MonoBehaviour
{
    // セリフ1つ分を表すクラス（話者とセリフの組）
    [System.Serializable]
    public class Dialogue
    {
        public string speaker;           // 話者の名前（例："コロ" や "モモ"）
        [TextArea(2, 5)]
        public string sentence;          // 実際に表示するセリフ
    }

    public Dialogue[] dialogues;         // セリフのリスト（Inspectorで入力）

    public Text targetText;              // Legacy Text コンポーネント
    public Image koroImage;              // コロの立ち絵（Imageコンポーネント）
    public Image momoImage;              // モモの立ち絵（Imageコンポーネント）

    public float delay = 0.05f;          // 1文字あたりの表示速度（秒）

    private int currentIndex = 0;        // 現在表示しているセリフのインデックス
    private bool isWaitingForNext = false;  // 次の入力待ちかどうか

    void Start()
    {
        // 最初のセリフを表示
        ShowCurrentDialogue();
    }

    void Update()
    {
        // クリックされたら次のセリフへ（セリフ表示完了後のみ反応）
        if (isWaitingForNext && Input.GetMouseButtonDown(0))
        {
            currentIndex++; // 次のセリフへ進む

            if (currentIndex < dialogues.Length)
            {
                // 次のセリフがまだある場合は表示
                ShowCurrentDialogue();
            }
            else
            {
                // 全セリフ終了時の処理
                targetText.text = ""; // テキストを消す
                SetCharacterAlpha(koroImage, 0.3f);
                SetCharacterAlpha(momoImage, 0.3f);
            }

            isWaitingForNext = false; // 再入力防止
        }
    }

    // 現在のセリフをタイピング表示＋話者切り替え
    void ShowCurrentDialogue()
    {
        StartCoroutine(TypeSentence(dialogues[currentIndex].sentence));
        SwitchCharacter(dialogues[currentIndex].speaker);
    }

    // 1文字ずつ表示していくコルーチン
    IEnumerator TypeSentence(string sentence)
    {
        targetText.text = ""; // テキストを一旦クリア

        for (int i = 0; i <= sentence.Length; i++)
        {
            targetText.text = sentence.Substring(0, i); // i文字目まで表示
            yield return new WaitForSeconds(delay);     // 一定時間待つ
        }

        // 表示が終わったのでクリック待ち状態にする
        isWaitingForNext = true;
    }

    // 話者に応じて立ち絵の透明度を切り替える
    void SwitchCharacter(string speaker)
    {
        if (speaker == "コロ")
        {
            SetCharacterAlpha(koroImage, 1f);     // コロを表示
            SetCharacterAlpha(momoImage, 0.3f);   // モモを薄く
        }
        else if (speaker == "モモ")
        {
            SetCharacterAlpha(koroImage, 0.3f);
            SetCharacterAlpha(momoImage, 1f);
        }
    }

    // 指定されたImageの透明度を変更する関数
    void SetCharacterAlpha(Image image, float alpha)
    {
        Color c = image.color;
        c.a = alpha;
        image.color = c;
    }
}
```

List 4 TypingText改良版
```csharp
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TypingText : MonoBehaviour
{
    // セリフ1つ分を表すデータ（話者と内容）
    [System.Serializable]
    public class Dialogue
    {
        public string speaker;           // 話者の名前（例："コロ" や "モモ"、または "選択"）
        [TextArea(2, 5)]
        public string sentence;          // 実際のセリフ
    }

    // HeaderはInspectorで変数のグループに見出しを表示するためのもの
    [Header("全体ストーリー最初のセリフ群")]
    public Dialogue[] dialogues;        // 初期のセリフ一覧（Inspectorで編集）

    [Header("分岐Aのセリフ（例：調べに行く）")]
    public Dialogue[] pathA;            // 選択肢Aを選んだ場合のセリフ

    [Header("分岐Bのセリフ（例：のんびりする）")]
    public Dialogue[] pathB;            // 選択肢Bを選んだ場合のセリフ

    [Header("UI参照")]
    public Text targetText;             // Legacy Text（テロップ表示用）
    public Image koroImage;             // コロの立ち絵
    public Image momoImage;             // モモの立ち絵
    public Button option1Button;        // 選択肢ボタン1
    public Button option2Button;        // 選択肢ボタン2

    [Header("設定")]
    public float delay = 0.05f;         // 1文字あたりの表示間隔（秒）

    // 内部変数
    private int currentIndex = 0;       // 現在のセリフ番号
    private bool isWaitingForNext = false; // 表示後クリック待ち状態
    private bool isChoosing = false;    // 選択肢を表示中かどうか

    void Start()
    {
        ShowCurrentDialogue();          // 最初のセリフを表示
    }

    void Update()
    {
        // 通常のセリフ進行：セリフ表示後にクリックで次へ
        if (!isChoosing && isWaitingForNext && Input.GetMouseButtonDown(0))
        {
            currentIndex++;             // 次のセリフへ

            if (currentIndex < dialogues.Length)
            {
                ShowCurrentDialogue();  // 次のセリフを表示
            }
            else
            {
                // セリフがすべて終わったらキャラを非表示にする
                targetText.text = "";
                SetCharacterAlpha(koroImage, 0.3f);
                SetCharacterAlpha(momoImage, 0.3f);
            }

            isWaitingForNext = false;
        }
    }

    // 現在のセリフを表示（選択肢かどうかで処理を分ける）
    void ShowCurrentDialogue()
    {
        Dialogue current = dialogues[currentIndex];

        // 話者名が「選択」なら選択肢を表示
        if (current.speaker == "選択")
        {
            ShowChoices();
            return;
        }

        // 通常のセリフをタイピング表示
        StartCoroutine(TypeSentence(current.sentence));
        SwitchCharacter(current.speaker);
    }

    // 1文字ずつテキストを表示するコルーチン
    IEnumerator TypeSentence(string sentence)
    {
        targetText.text = "";

        for (int i = 0; i <= sentence.Length; i++)
        {
            targetText.text = sentence.Substring(0, i);
            yield return new WaitForSeconds(delay);
        }

        isWaitingForNext = true; // 表示が終わったのでクリックを受け付ける
    }

    // キャラクターの立ち絵の透明度を切り替える
    void SwitchCharacter(string speaker)
    {
        if (speaker == "コロ")
        {
            SetCharacterAlpha(koroImage, 1f);
            SetCharacterAlpha(momoImage, 0.3f);
        }
        else if (speaker == "モモ")
        {
            SetCharacterAlpha(koroImage, 0.3f);
            SetCharacterAlpha(momoImage, 1f);
        }
        else
        {
            // どちらでもない（選択肢など）場合は両方薄く
            SetCharacterAlpha(koroImage, 0.3f);
            SetCharacterAlpha(momoImage, 0.3f);
        }
    }

    // Imageの透明度（アルファ値）を設定する
    void SetCharacterAlpha(Image image, float alpha)
    {
        Color c = image.color;
        c.a = alpha;
        image.color = c;
    }

    // 選択肢ボタンを表示する処理
    void ShowChoices()
    {
        isChoosing = true;

        // 選択肢ボタンを表示する
        option1Button.gameObject.SetActive(true);
        option2Button.gameObject.SetActive(true);

        // ラベルを設定（ここは必要に応じて変更可能）
        option1Button.GetComponentInChildren<Text>().text = "調べに行く";
        option2Button.GetComponentInChildren<Text>().text = "のんびりする";

        // そのボタンに以前設定されていたクリック時の処理をすべて削除
        option1Button.onClick.RemoveAllListeners();
        option2Button.onClick.RemoveAllListeners();

        // ボタンが押されたときの動作を指定
        option1Button.onClick.AddListener(() => ChoosePath(pathA));
        option2Button.onClick.AddListener(() => ChoosePath(pathB));
    }

    // どちらかの選択肢が選ばれたときの処理
    void ChoosePath(Dialogue[] nextPath)
    {
        // ストーリーを次のパスに切り替える
        dialogues = nextPath;
        currentIndex = 0;
        isChoosing = false;

        // 選択肢ボタンを非表示に
        option1Button.gameObject.SetActive(false);
        option2Button.gameObject.SetActive(false);

        // 新しいストーリーを再開
        ShowCurrentDialogue();
    }
}
```

List 5 TypingText.csの一部【 Start()メソッド 】
```csharp
void Start()
{
    // pathAをここで定義・代入する場合
    pathA = new Dialogue[]
    {
        new Dialogue { speaker = "コロ", sentence = "あっ、あの木の近くに何か落ちてるよ！" },
        new Dialogue { speaker = "モモ", sentence = "…これは…古い首輪？誰かの忘れ物かな？" },
        new Dialogue { speaker = "コロ", sentence = "もしかして…誰かがこの公園で迷子になったとか？" },
        new Dialogue { speaker = "モモ", sentence = "うーん、どうしようか？交番に届ける？それとももう少し探してみる？" },
        new Dialogue { speaker = "選択", sentence = "" }
    };

    ShowCurrentDialogue(); // 最初の表示
}
```

List 6 TypingText.csの一部【 public Dialogue[] pathA; の次の行に追加】
```csharp
public Dialogue[] pathA_1; // 交番に行く
public Dialogue[] pathA_2; // さらに探す
```

List 7 TypingText.csの一部【 Start()メソッド 】
```csharp
void Start()
{
    // pathAをここで定義・代入する場合
    pathA = new Dialogue[]
    {
        new Dialogue { speaker = "コロ", sentence = "あっ、あの木の近くに何か落ちてるよ！" },
        new Dialogue { speaker = "モモ", sentence = "…これは…古い首輪？誰かの忘れ物かな？" },
        new Dialogue { speaker = "コロ", sentence = "もしかして…誰かがこの公園で迷子になったとか？" },
        new Dialogue { speaker = "モモ", sentence = "うーん、どうしようか？交番に届ける？それとももう少し探してみる？" },
        new Dialogue { speaker = "選択", sentence = "" }
    };

    pathA_1 = new Dialogue[]
    {
        new Dialogue { speaker = "コロ", sentence = "そうだね、安全のためにも届けよう！" },
        new Dialogue { speaker = "モモ", sentence = "近くの交番なら10分くらいで行けるはずだよ。" },
        new Dialogue { speaker = "コロ", sentence = "うん、じゃあレッツゴー！" },
        new Dialogue { speaker = "モモ", sentence = "ちょっとした冒険になってきたね。" }
    };

    pathA_2 = new Dialogue[]
    {
        new Dialogue { speaker = "コロ", sentence = "うーん、もう少しだけ探してみようか。" },
        new Dialogue { speaker = "モモ", sentence = "気になるしね。あ、あっちに足跡が…！" },
        new Dialogue { speaker = "コロ", sentence = "ほんとだ！誰かが歩いていった跡みたい！" },
        new Dialogue { speaker = "モモ", sentence = "もしかして、その先に何かあるのかも…" }
    };

    ShowCurrentDialogue(); // 最初の表示
}
```

List 8 TypingText.csの一部【 ShowChoices() 】
```csharp
void ShowChoices()
{
    isChoosing = true;

    // ラベルを動的に変えたい場合はこちらで設定
    if (dialogues == pathA)
    {
        // Button内のTextコンポーネントのテキスト内容を変更する
        option1Button.GetComponentInChildren<Text>().text = "交番に届ける";
        option2Button.GetComponentInChildren<Text>().text = "さらに探す";

        // そのボタンに以前設定されていたクリック時の処理をすべて削除
        option1Button.onClick.RemoveAllListeners();
        option2Button.onClick.RemoveAllListeners();

        // option1Buttonがクリックされたときに、ChoosePath(pathA_1) を実行
        option1Button.onClick.AddListener(() => ChoosePath(pathA_1));
        // option2Buttonがクリックされたときに、ChoosePath(pathA_2) を実行
        option2Button.onClick.AddListener(() => ChoosePath(pathA_2));
    }
    else
    {
        // 1つ目の選択肢（最初の分岐用）
        option1Button.GetComponentInChildren<Text>().text = "調べに行く";
        option2Button.GetComponentInChildren<Text>().text = "のんびりする";

        option1Button.onClick.RemoveAllListeners();
        option2Button.onClick.RemoveAllListeners();

        option1Button.onClick.AddListener(() => ChoosePath(pathA));
        option2Button.onClick.AddListener(() => ChoosePath(pathB));
    }

    // option1Button, option2Button を画面に表示する（非表示から表示に切り替える）
    option1Button.gameObject.SetActive(true);
    option2Button.gameObject.SetActive(true);
}
```
