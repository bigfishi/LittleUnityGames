using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2 _direction = Vector2.right;

    private List<Transform> _segments = new List<Transform>();
    public Transform  _segmentPrefab;

    public int _initialSize = 5;

    void Start()
    {
        ResetState();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) {
            _direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.S)) {
            _direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.A)) {
            _direction  = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.D)) {
            _direction = Vector2.right;
        }
    }

    private void FixedUpdate()
    {
        for (int i=_segments.Count-1;i>0;i--) {
            _segments[i].position = _segments[i-1].position;
        }

        transform.position = new Vector3(
            Mathf.Round(this.transform.position.x + _direction.x),
            Mathf.Round(this.transform.position.y + _direction.y),
            0.0f
        );
    }

    private void Grow()
    {
        Transform segment = Instantiate(_segmentPrefab);
        segment.position = _segments[_segments.Count - 1].position;

        _segments.Add(segment);

    }

    private void ResetState()
    {
        for (int i=1;i<_segments.Count;i++) {
            Destroy(_segments[i].gameObject);
        }

        _segments.Clear();
        _segments.Add(transform);

        transform.position = Vector3.zero;
        _direction = Vector2.right;

        for (int i=1;i<this._initialSize;i++) {
            Grow();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Snake trigger tag=" + other.tag);
        if (other.tag == "Food") {
            Grow();
        }
        else if (other.tag == "Wall") {
            ResetState();
        }
    }
}
