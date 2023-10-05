using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    
    public float shakeDuration = 1.0f;
    public float shakeIntensity = 0.9f;

    private Vector3 originalPosition;
    private Coroutine shakeCoroutine;

    private void Start()
    {
        Debug.LogError("shake");
        originalPosition = transform.position;
    }
    public void StartShake()
    {
        Debug.LogError("shake2");
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine = StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            Vector3 randomOffset = Random.insideUnitSphere * shakeIntensity;
            transform.position = originalPosition + randomOffset;

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = originalPosition;
    }
}
