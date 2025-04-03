using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public Asteriod asteriodPrefab;
    // 产生小行星的偏差角度
    public float trajectoryVariance = 15.0f;
    public float spawnRate = 2.0f;
    public float spawnDistance = 15.0f;
    public int spawnAmount = 1;

    void Start()
    {
        InvokeRepeating(nameof(Spawn), this.spawnRate, this.spawnRate);
    }

    private void Spawn()
    {
        for (int i=0; i<this.spawnAmount; i++)
        {
            // insideUnitCircle 在一个圆里随机Vector2位置，normalized 将点设置在圆圈边缘
            Vector3 spawnDirection = Random.insideUnitCircle.normalized * this.spawnDistance;
            Vector3 spawnPoint = this.transform.position + spawnDirection;

            float variance = Random.Range(-this.trajectoryVariance, this.trajectoryVariance);
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);

            Asteriod asteriod = Instantiate(this.asteriodPrefab, spawnPoint, rotation);
            asteriod.size = Random.Range(asteriod.minSize, asteriod.maxSize);
            asteriod.SetTrajactory(rotation * -spawnDirection);
        }
    }
}
