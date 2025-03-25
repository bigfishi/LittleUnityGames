using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    
    public BoxCollider2D _gridArea;

    private void Start()
    {
        RandomizePosition();
    }

    private void RandomizePosition()
    {
        Bounds bounds = _gridArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        transform.position = new Vector3(Mathf.Round(x), Mathf.Round(y), 0.0f);
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("trigger tag=" + other.tag);
        if (other.tag == "Player") {
            RandomizePosition();
        }
    }
}
