// P252 List7-13 PlayerController
// Submitアクションを受け取る

using UnityEngine;
using UnityEngine.InputSystem;  // InputSystemを使うのに必要

public enum GameState           // ゲームの状態
{
    InGame,                     // ゲーム中
    GameClear,                  // ゲームクリア
    GameOver,                   // ゲームオーバー
    GameEnd,                    // ゲーム終了
}

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody;              // Rigidbody2D型の変数
    float axisH = 0.0f;             // 入力
    public float speed = 3.0f;      // 移動速度
    public float jump = 9.0f;       // ジャンプ力
    public LayerMask groundLayer;   // 着地できるレイヤー
    bool goJump = false;            // ジャンプ開始フラグ
    bool onGround = false;          // 地面フラグ

    // アニメーション対応
    Animator animator; // アニメーター
    public string stopAnime = "PlayerStop";
    public string moveAnime = "PlayerMove";
    public string jumpAnime = "PlayerJump";
    public string goalAnime = "PlayerGoal";
    public string deadAnime = "PlayerOver";
    string nowAnime = "";
    string oldAnime = "";

    // ゲームの状態
    public static GameState gameState;

    //カメラ制御
    public float camLeft = 0.0f;        // カメラ左スクロールリミット
    public float camRight = 0.0f;       // カメラ右スクロールリミット
    public float camTop = 0.0f;         // カメラ上スクロールリミット
    public float camBottom = 0.0f;      // カメラ下スクロールリミット

    // 多重スクロール
    public GameObject subScreen;        // サブスクリーン

    // 強制スクロール
    public bool isForceScrollX = false;     // 強制スクロールフラグ
    public float forceScrollSpeedX = 0.5f;  // 1秒間で動かすX距離
    public bool isForceScrollY = false;     // Y軸強制スクロールフラグ
    public float forceScrollSpeedY = 0.5f;  // 1秒間で動かすY距離

    public int score = 0;                   // スコア

    InputAction moveAction;                 // Moveアクション
    InputAction jumpAction;                 // Jumpアクション

    // Submitアクションを受け取る
    void OnSubmit(InputValue value)
    {
        Debug.Log("OnSubmit");
        if (gameState != GameState.InGame)
        {
            GameManager gm = GameObject.FindFirstObjectByType<GameManager>();
            gm.GameEnd();
        }
    }

    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>();   // Rigidbody2Dを取ってくる
        animator = GetComponent<Animator>();        // Animator を取ってくる
        nowAnime = stopAnime;                       // 停止から開始する
        oldAnime = stopAnime;                       // 停止から開始する

        gameState = GameState.InGame;               // ゲーム中にする

        PlayerInput input = GetComponent<PlayerInput>();            // PlayerInput取得
        moveAction = input.currentActionMap.FindAction("Move");     // Moveアクション取得
        jumpAction = input.currentActionMap.FindAction("Jump");     // Jumpアクション取得
        InputActionMap uiMap = input.actions.FindActionMap("UI");   // UIマップ取得
        uiMap.Disable();                                            // UIマップ無効化
    }

    void Update()
    {
        if (gameState != GameState.InGame)
        {
            return;
        }
        // 地上判定
        onGround = Physics2D.CircleCast(transform.position,    // 発射位置
                                        0.2f,                  // 円の半径
                                        Vector2.down,          // 発射方向
                                        0.0f,                  // 発射距離
                                        groundLayer);          // 検出するレイヤー
                                                               // キャラクターをジャンプさせる
        //---- Input Manager ----
        //if (Input.GetButtonDown("Jump"))
        //{
        //    goJump = true; // ジャンプフラグを立てる
        //}
        //axisH = Input.GetAxisRaw("Horizontal");     //水平方向の入力をチェックする

        //---- Input System ----
        if (jumpAction.WasPressedThisFrame())
        {
            goJump = true; // ジャンプフラグを立てる
        }
        axisH = moveAction.ReadValue<Vector2>().x;


        if (axisH > 0.0f)                           // 向きの調整
        {
            transform.localScale = new Vector2(1, 1);   // 右移動
        }
        else if (axisH < 0.0f)
        {
            transform.localScale = new Vector2(-1, 1); // 左右反転させる
        }

        // アニメーション更新
        if (onGround)       // 地面の上
        {
            if (axisH == 0)
            {
                nowAnime = stopAnime; // 停止中
            }
            else
            {
                nowAnime = moveAnime; // 移動
            }
        }
        else                // 空中
        {
            nowAnime = jumpAnime;
        }
        if (nowAnime != oldAnime)
        {
            oldAnime = nowAnime;
            animator.Play(nowAnime); // アニメーション再生
        }

        //カメラ制御
        float x;
        float y;
        if (isForceScrollX)    // 横強制スクロール
        {
            x = Camera.main.transform.position.x + (forceScrollSpeedX * Time.deltaTime);
        }
        else
        {
            x = Mathf.Clamp(transform.position.x, camLeft, camRight);
        }
        if (isForceScrollY)    // 縦強制スクロール
        {
            y = Camera.main.transform.position.y + (forceScrollSpeedY * Time.deltaTime);
        }
        else
        {
            y = Mathf.Clamp(transform.position.y, camBottom, camTop);
        }
        Vector3 camPos = new Vector3(x, y, -10);        // カメラ位置のVector3を作る
        Camera.main.transform.position = camPos;        // カメラの更新座標

        // サブスクリーンスクロール
        if (subScreen != null)
        {
            y = subScreen.transform.position.y;
            Vector3 subpos = new Vector3(x / 2.0f, y, subScreen.transform.position.z);
            subScreen.transform.position = subpos;
        }

    }

    void FixedUpdate()
    {
        if (gameState != GameState.InGame)
        {
            return;
        }
        if (onGround || axisH != 0)     // 地面の上 or 速度が 0 ではない
        {
            //速度を更新する
            rbody.linearVelocity = new Vector2(axisH * speed, rbody.linearVelocity.y);
        }
        if (onGround && goJump)         // 地面の上でジャンプキーが押された
        {
            // ジャンプさせる
            Vector2 jumpPw = new Vector2(0, jump);          // ジャンプさせるベクトルを作る
            rbody.AddForce(jumpPw, ForceMode2D.Impulse);    // 瞬間的な力を加える
            goJump = false;                                 // ジャンプフラグを下ろす
        }
    }

    // 接触開始
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            Goal();         // ゴール！！
        }
        else if (collision.gameObject.tag == "Dead")
        {
            GameOver();     // ゲームオーバー
        }
        else if (collision.gameObject.tag == "ScoreItem")
        {
            // スコアアイテム
            ScoreItem item = collision.gameObject.GetComponent<ScoreItem>();  // ScoreItemを得る			
            score = item.itemdata.value;                // スコアを得る
            Destroy(collision.gameObject);              // アイテム削除する
        }
    }
    // ゴール
    public void Goal()
    {
        animator.Play(goalAnime);
        gameState = GameState.GameClear;
        GameStop();             // ゲーム停止
    }
    // ゲームオーバー
    public void GameOver()
    {
        animator.Play(deadAnime);
        gameState = GameState.GameOver;
        GameStop();             // ゲーム停止
        // ゲームオーバー演出
        GetComponent<CapsuleCollider2D>().enabled = false;      // 当たりを消す
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse); // 上に少し跳ね上げる
    }
    // ゲーム停止
    void GameStop()
    {
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();    // Rigidbody2Dを取ってくる
        rbody.linearVelocity = new Vector2(0, 0);           // 速度を0にして強制停止

        // Playerマップの無効化とUIマップの有効化
        PlayerInput input = GetComponent<PlayerInput>();    // PlayerInput取得
        input.currentActionMap.Disable();                   // Player無効化
        input.SwitchCurrentActionMap("UI");                 // アクションマップをUIに変更
        input.currentActionMap.Enable();                    // UIマップ有効化
    }
}
