List 4-2 アニメーションさせるためにスクリプトを変更しよう

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody;  // RigidBody2D型の変数
    float axisH = 0.0f; // 入力
    public float speed = 3.0f;  // 移動速度
    public float jump = 9.0f;   // ジャンプ力
    public LayerMask groundLayer;   // 着地できるレイヤー
    bool goJump = false;        // ジャンプ開始フラグ
    bool onGround = false;      // 地面に立っているフラグ

    // アニメーション対応

    Animator animator; // アニメーター

    public string stopAnime = "PlayerStop";
    public string moveAnime = "PlayerMove";
    public string jumpAnime = "PlayerJump";
    public string goalAnime = "PlayerGoal";
    public string deadAnime = "PlayerOver";
    string nowAnime = "";
    string oldAnime = "";
    
    // Start is called before the first frame update
    void Start()
    {
        // Rigidbody2Dを取ってくる
        rbody = this.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
	      nowAnime = stopAnime;
        oldAnime = stopAnime;
    }

    // Update is called once per frame
    void Update()
    {
        // 水平方向の入力をチェックする
        axisH = Input.GetAxisRaw("Horizontal");

        // 向きの調整
        if (axisH > 0.0f)
        {
            // 右移動
            Debug.Log("右移動");
            transform.localScale = new Vector2(1, 1);
        }
        else if (axisH < 0.0f)
        {
            // 左移動
            Debug.Log("左移動");
            transform.localScale = new Vector2(-1, 1);  // 左右反転させる
        }

        // キャラクターをジャンプさせる
        if (Input.GetButtonDown("Jump"))
        {
            Jump(); // ジャンプ
        }
    }

    void FixedUpdate()
    {
        // 地上判定
        onGround = Physics2D.Linecast(transform.position,
                                      transform.position - (transform.up * 1.0f),
                                      groundLayer);

        if (onGround || axisH != 0)
        {
            // 地面の上 or 速度が 0 ではない
            // 速度を更新する
            rbody.velocity = new Vector2(speed * axisH, rbody.velocity.y);
        }

        if (onGround && goJump)
        {
            // 地面の上でジャンプキーが押された
            // ジャンプさせる
            Debug.Log(" ジャンプ! ");

            Vector2 jumpPw = new Vector2(0, jump);        // ジャンプさせるベクトルを作る
            rbody.AddForce(jumpPw, ForceMode2D.Impulse);  // 瞬間的な力を加える
            goJump = false; // ジャンプフラグを下ろす
        }

        if (onGround)
        {
            // 地面の上
            if (axisH == 0)
            {
               nowAnime = stopAnime; // 停止中
            }
            else
            {
                nowAnime = moveAnime; // 移動
            }
        }
        else
        {
            // 空中
            nowAnime = jumpAnime;
        }

        if (nowAnime != oldAnime)
        {
            oldAnime = nowAnime;
            animator.Play(nowAnime); // アニメーション再生
        }
    }

    // ジャンプ
    public void Jump()
    {
        goJump = true;  // ジャンプフラグを立てる
        Debug.Log("ジャンプボタン押し！");
    }

    // 接触開始
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            Goal();        // ゴール！！
        }
        else if (collision.gameObject.tag == "Dead")
        {
            GameOver();     // ゲームオーバー
        }
    }

    // ゴール
    public void Goal()
    {
        animator.Play(goalAnime);
    }

    // ゲームオーバー
    public void GameOver()
    {
        animator.Play(deadAnime);
    }
}
