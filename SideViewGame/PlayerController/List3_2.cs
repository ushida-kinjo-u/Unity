// List 3-2 Unityエディターからパラメーターを変更しよう

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody;  // RigidBody2D型の変数
    float axisH = 0.0f; // 入力
    public float speed = 3.0f;  // 移動速度

    // Start is called before the first frame update
    void Start()
    {
        // Rigidbody2Dを取ってくる
        rbody = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // 水平方向の入力をチェックする
        axisH = Input.GetAxisRaw("Horizontal");
        // 向きの調整
        if( axisH > 0.0f )
        {
            // 右移動
            Debug.Log("右移動");
            transform.localScale = new Vector2(1, 1);
        }
        else if(axisH < 0.0f )
        {
            // 左移動
            Debug.Log("左移動");
            transform.localScale = new Vector2(-1, 1);  // 左右反転させる
        }
    }

    private void FixedUpdate()
    {
        // 速度を更新する
        rbody.velocity = new Vector2(speed*axisH, rbody.velocity.y);
    }
}
