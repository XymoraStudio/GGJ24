using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public AnimationCurve curve;
    public float duration = 1f;
    [SerializeField] private bool start_on_awak = false;
    [SerializeField] private float shake_strength = 1;

    void Start()
    {
        if (start_on_awak)
            RestartShake(duration);
    }

    public void RestartShake(float strength)
    {
        StartCoroutine(Shake(strength));
    }

    public IEnumerator Shake(float strengthMuliplier)
    {
        Debug.Log("Starting shake");
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration) * shake_strength;
            transform.position = transform.position + Random.insideUnitSphere * strength * strengthMuliplier;
            yield return null;
        }
    }
}
