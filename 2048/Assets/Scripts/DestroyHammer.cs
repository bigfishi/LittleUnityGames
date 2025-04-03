using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyHammer : MonoBehaviour
{
    public void PlayHitAnimation(Vector3 to)
    {
        StartCoroutine(MoveAnimate(to));
    }

    // 移动动画
    private IEnumerator MoveAnimate(Vector3 to)
    {
        float elapsed = 0f;
        float duration = 0.5f;

        Vector3 from = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = to;
        
        float angle = 45f;
        Quaternion rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, transform.forward);
        StartCoroutine(RotateAnimate(rotation));
    }

    // 旋转动画
    private IEnumerator RotateAnimate(Quaternion to)
    {
        float elapsed = 0f;
        float duration = 0.5f;

        Quaternion from = transform.rotation;

        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = to;
    }
}
