シューティングゲーム Shooting Game

List 1 PlayerMovement.cs
```csharp
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // プレイヤーの移動速度

    void Update()
    {
        // 水平方向の入力（←→キー）
        float horizontal = Input.GetAxis("Horizontal");

        // プレイヤーを左右に移動させる
        transform.Translate(Vector3.right * horizontal * speed * Time.deltaTime);
    }
}
```


List 2 PlayerMovement.cs（移動制限を追加）
```csharp
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;  // インスペクターで調整可能

    private float xLimit;

    void Start()
    {
        // カメラの左端と右端のワールド座標を計算
        float halfShipWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
        float screenHalfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;

        // 画面端からプレイヤーの半分の幅を引いた位置を制限とする
        xLimit = screenHalfWidth - halfShipWidth;
    }

    void Update()
    {
        // 水平方向の入力（-1〜1）
        float horizontal = Input.GetAxis("Horizontal");

        // 現在位置を取得
        Vector3 position = transform.position;

        // 入力に応じて位置を更新
        position.x += horizontal * speed * Time.deltaTime;

        // 画面サイズに基づいた制限
        position.x = Mathf.Clamp(position.x, -xLimit, xLimit); 

        transform.position = position;
    }
}
```


List 3 EnemySpawner.cs
```csharp
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // 敵プレハブ（Prefab）をUnityエディタで指定
    public GameObject enemyPrefab;

    // 敵が出現する間隔（秒）
    public float spawnInterval = 2.0f;

    // 出現範囲のX方向の制限
    private float xLimit;

    // 時間計測用のタイマー
    private float timer = 0f;

    void Start()
    {
        // 画面の横幅の半分を取得（ワールド座標で）
        float screenHalfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;

        // 画面端ギリギリを避けるために、少し内側に制限（±xLimit）
        xLimit = screenHalfWidth - 1.0f;
    }

    void Update()
    {
        // 経過時間を加算
        timer += Time.deltaTime;

        // 一定時間経過したら敵を出現させる
        if (timer >= spawnInterval)
        {
            // タイマーをリセット
            timer = 0f;

            // -xLimit ～ +xLimit の間でランダムなX座標を取得
            float x = Random.Range(-xLimit, xLimit);

            // Y座標は画面の上（仮に4.6fと設定）
            Vector3 spawnPos = new Vector3(x, 4.6f, 0);

            // 敵プレハブを出現位置に生成する
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }
    }
}
```


List 4 EnemyMovement.cs（敵に追加するスクリプト）
```csharp
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2f; // 敵の移動速度

    void Update()
    {
        // 敵を下に移動させる
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // 画面外に出たら削除
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }
}
```


List 5 PlayerShooting.cs
```csharp
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject laserPrefab; // 弾のプレハブ（インスペクターで指定）
    public float laserSpeed = 10f; // 弾の移動速度
    public Transform firePoint;    // 発射位置（プレイヤーの位置など）

    void Update()
    {
        // スペースキーが押されたら
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 弾を生成（発射位置と同じ場所）
            GameObject laser = Instantiate(laserPrefab, firePoint.position, Quaternion.identity);

            // 弾を上方向に移動させるために Rigidbody2D を使う
            Rigidbody2D rb = laser.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.up * laserSpeed;
            }
        }
    }
}
```


List 6 Laser.cs
```csharp
using UnityEngine;

public class Laser : MonoBehaviour
{
    void Update()
    {
        // Y座標が高くなりすぎたら削除
        if (transform.position.y > 6f)
        {
            Destroy(gameObject);
        }
    }
}
```


List 7 Enemy.c
```csharp
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 弾が当たったときに呼ばれる
    void OnTriggerEnter2D(Collider2D other)
    {
        // 衝突相手のタグを確認（Laserに設定する）
        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject); // 弾を削除
            Destroy(gameObject);       // 自分（敵）を削除
        }
    }
}
```


List 8 GameManager.cs
```csharp
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // 他スクリプトからアクセスするための共通インスタンス

    public int score = 0;       // 現在のスコア
    public int life = 3;        // プレイヤーのライフ
    public Text scoreText;      // スコア表示用UI
    public Text lifeText;       // ライフ表示用UI

    void Awake()
    {
        // シングルトンパターン：1つだけ存在させる
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateUI(); // 初期表示を更新
    }

    // スコアを加算する関数
    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    // ライフを減らす関数
    public void ReduceLife(int amount)
    {
        life -= amount;
        UpdateUI();

        if (life <= 0)
        {
            GameOver();
        }
    }

    // UI表示を更新
    void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        lifeText.text = "Life: " + life;
    }

    // ゲームオーバー処理（ここではログ出力のみ）
    void GameOver()
    {
        Debug.Log("Game Over!");
        // 必要に応じてシーンをリロードなど
    }
}
```


List 9 Enemy.cs の一部を修正：
```csharp
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 弾が当たったときに呼ばれる
    void OnTriggerEnter2D(Collider2D other)
    {
        // 衝突相手のタグを確認（Laserに設定する）
        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject); // 弾を削除
            Destroy(gameObject);       // 自分（敵）を削除

            // スコア加算（10点）
            GameManager.Instance.AddScore(10);
        }
    }
}
```


List 10 Enemy.cs の一部を修正：
```csharp
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 弾が当たったときに呼ばれる
    void OnTriggerEnter2D(Collider2D other)
    {
        // 衝突相手のタグを確認（Laserに設定する）
        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject); // 弾を削除
            Destroy(gameObject);       // 自分（敵）を削除

            // スコア加算（10点）
            GameManager.Instance.AddScore(10);
        }

        if (other.CompareTag("Player"))
        {
            Destroy(gameObject); // 敵を削除

            // プレイヤーのライフを1減らす
            GameManager.Instance.ReduceLife(1);
        }
    }
}
```

List 11 PlayerShooting.cs（AudioClip 追加版）
```csharp
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject laserPrefab;
    public Transform firePoint;
    public float laserSpeed = 10f;

    public AudioClip shootSound;  // 発射音
    private AudioSource audioSource; // 音を再生する装置

    void Start()
    {
        // AudioSource を取得（なければ追加）
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject laser = Instantiate(laserPrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = laser.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.up * laserSpeed;
            }

            // 効果音を再生
            audioSource.PlayOneShot(shootSound);
        }
    }
}
```

List 12 Enemy.cs （爆発音の追加版）
```csharp
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public AudioClip explosionSound; // 爆発音
    private AudioSource audioSource;

    void Start()
    {
        // AudioSource を取得 or 追加
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 爆発音を再生
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);

            // ライフ減らす処理など
            GameManager.Instance.ReduceLife(1);
            Destroy(gameObject);
        }

        if (other.CompareTag("Laser"))
        {
            GameManager.Instance.AddScore(10);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
```

List 13 GameManager.cs（ゲームオーバー追加版）
```csharp
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // ← シーンを再読み込みするのに必要

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score = 0;
    public int life = 3;

    public Text scoreText;
    public Text lifeText;

    public GameObject gameOverPanel; // ゲームオーバーUIを表示するため
    public Button restartButton;     // リスタート用ボタン

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
        gameOverPanel.SetActive(false); // 最初は非表示
        restartButton.onClick.AddListener(RestartGame); // ボタンに処理をつなぐ
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    public void ReduceLife(int amount)
    {
        life -= amount;
        UpdateUI();

        if (life <= 0)
        {
            GameOver();
        }
    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        lifeText.text = "Life: " + life;
    }

    void GameOver()
    {
        gameOverPanel.SetActive(true); // パネルを表示
        Time.timeScale = 0f; // 時間を止める（オプション）
    }

    void RestartGame()
    {
        Time.timeScale = 1f; // 時間を元に戻す
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 今のシーンをリロード
    }
}
```


List 14 RankingManager.cs
```csharp
using UnityEngine;
using UnityEngine.UI;

public class RankingManager : MonoBehaviour
{
    public Text[] rankTexts; // ランキング表示用のテキスト（上位3位など）
    private int maxRank = 5;

    // ゲームオーバー時に現在スコアを追加
    public void AddScoreToRanking(int newScore)
    {
        // 保存済みスコアを配列で取得
        int[] scores = new int[maxRank];
        for (int i = 0; i < maxRank; i++)
        {
            scores[i] = PlayerPrefs.GetInt("Rank" + i, 0);
        }

        // 新しいスコアを追加して並べ替え
        scores[maxRank - 1] = newScore;
        System.Array.Sort(scores);
        System.Array.Reverse(scores); // 高い順に

        // 上位だけ保存し直し
        for (int i = 0; i < maxRank; i++)
        {
            PlayerPrefs.SetInt("Rank" + i, scores[i]);
        }

        // 表示を更新
        UpdateRankingDisplay();
    }

    // ランキングを画面に表示
    public void UpdateRankingDisplay()
    {
        for (int i = 0; i < rankTexts.Length; i++)
        {
            int score = PlayerPrefs.GetInt("Rank" + i, 0);
            rankTexts[i].text = (i + 1) + "位： " + score + "点";
        }
    }
}
```

List 15 GameManager.cs の修正：
```csharp
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // ← シーンを再読み込みするのに必要

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score = 0;
    public int life = 3;

    public Text scoreText;
    public Text lifeText;

    public GameObject gameOverPanel; // ゲームオーバーUIを表示するため
    public GameObject rankingPanel; // ランキングパネルを表示するため
    public Button restartButton;     // リスタート用ボタン

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
        gameOverPanel.SetActive(false); // 最初は非表示
        rankingPanel.SetActive(false); // 最初は非表示
        restartButton.onClick.AddListener(RestartGame); // ボタンに処理をつなぐ
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    public void ReduceLife(int amount)
    {
        life -= amount;
        UpdateUI();

        if (life <= 0)
        {
            GameOver();
        }
    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        lifeText.text = "Life: " + life;
    }

    void GameOver()
{
    gameOverPanel.SetActive(true);
    rankingPanel.SetActive(true);     // ランキングUIを表示
    Time.timeScale = 0f;

    // ランキングに現在のスコアを追加
    FindFirstObjectByType<RankingManager>().AddScoreToRanking(score);
}


    void RestartGame()
    {
        Time.timeScale = 1f; // 時間を元に戻す
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 今のシーンをリロード
    }
}
```
