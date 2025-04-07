using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blaster : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Vector2 _direction;

    public float speed = 20f;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        _direction.x = Input.GetAxis("Horizontal");
        _direction.y = Input.GetAxis("Vertical");
    }

    private void FixedUpdate() {
        Vector2 position = _rigidbody.position;
        position += _direction.normalized * speed * Time.fixedDeltaTime;

        _rigidbody.MovePosition(position);
    }



}
