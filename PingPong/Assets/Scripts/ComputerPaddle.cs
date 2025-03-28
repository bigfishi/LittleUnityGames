using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerPaddle : Paddle
{
    public Rigidbody2D ball;
    void FixedUpdate()
    {
        if (ball.velocity.x > 0.0f) {
            if (ball.position.y > transform.position.y) {
                _rigidbody.AddForce(Vector2.up * speed);
            }
            else if (ball.position.y < transform.position.y) {
                _rigidbody.AddForce(Vector2.down * speed);
            }
        }
        else {
            if (transform.position.y > 0.0f) {
                _rigidbody.AddForce(Vector2.down * speed);
            }
            else if (transform.position.y < 0.0f) {
                _rigidbody.AddForce(Vector2.up * speed);
            }
        }
    }
}
