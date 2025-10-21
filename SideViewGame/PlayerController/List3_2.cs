// P75 List3-2 PlayerController
// Unityエディターからパラメーターを変更しよう

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody;          // Rigidbody2D型の変数
    float axisH = 0.0f;         //入力
    public float speed = 3.0f;  // 移動速度
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Rigidbody2Dを取ってくる
        rbody = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //水平方向の入力をチェックする
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
            transform.localScale = new Vector2(-1, 1); // 左右反転させる
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //速度を更新する
        rbody.linearVelocity = new Vector2(axisH * speed, rbody.linearVelocity.y);
    }
}
