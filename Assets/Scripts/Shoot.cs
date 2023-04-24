using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [Header("General")]
    [SerializeField] GameObject projecttilePrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileLifetime = 5f;
    [SerializeField] float baseFiringRate = 0.2f;

    [Header("AI")]
    [SerializeField] bool useAI;
    [SerializeField] float firingRateVariance = 0f;
    [SerializeField] float minimumFiringRate = 0.1f;

    public bool isFiring;

    Coroutine firingCoroutine;
    AudioPlayer audioPlayer;

    void Awake() {
        audioPlayer = FindObjectOfType<AudioPlayer>();
    }
    void Start()
    {
        if (useAI) {
            isFiring = true;
        }
    }

    void Update()
    {
        Fire();
    }

    void Fire() {
        if (isFiring && firingCoroutine == null) {
            firingCoroutine = StartCoroutine(FireContinuously());  
            audioPlayer.PlayShootingClip();
        }
        else if(!isFiring && firingCoroutine != null) {
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }
    }
     IEnumerator FireContinuously() {
            while(true) {
                GameObject instance = Instantiate(projecttilePrefab, transform.position, Quaternion.identity);
                Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
                if (rb != null) {
                    rb.velocity = transform.up * projectileSpeed;
                }
                Destroy(instance, projectileLifetime);

                float timeToNextProjectile = Random.Range(baseFiringRate - firingRateVariance, baseFiringRate + firingRateVariance);
                timeToNextProjectile = Mathf.Clamp(timeToNextProjectile, minimumFiringRate, float.MaxValue);
                yield return new WaitForSeconds(timeToNextProjectile);
            }
     }
}
