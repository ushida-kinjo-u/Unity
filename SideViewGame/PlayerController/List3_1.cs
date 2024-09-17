// List 3-1 ゲームオブジェクトをキー入力で操作するスクリプトファイルを書こう

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody;  // RigidBody2D型の変数
    float axisH = 0.0f; // 入力

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
    }

    private void FixedUpdate()
    {
        // 速度を更新する
        rbody.velocity = new Vector2(axisH * 3.0f, rbody.velocity.y);
    }
}
