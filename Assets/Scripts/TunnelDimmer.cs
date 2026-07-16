using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TunnelDimmer : MonoBehaviour
{
    public float fadeTime = 0.8f;
    public Light2D globalLight;

    private Coroutine currentFade;

    private float dimIntensity = 0.01f;
    private float brightIntensity = 0.5f;

    private void Start()
    {
        // Ensure starting brightness is correct
        if (globalLight != null)
        {
            globalLight.intensity = brightIntensity;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsPlayer(other))
        {
            StartFade(dimIntensity);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsPlayer(other))
        {
            StartFade(brightIntensity);
        }
    }

    private bool IsPlayer(Collider2D other)
    {
        return other.CompareTag("Player") ||
               (other.transform.parent != null && other.transform.parent.CompareTag("Player"));
    }

    private void StartFade(float targetIntensity)
    {
        // Stop any ongoing fade to prevent overlap
        if (currentFade != null)
        {
            StopCoroutine(currentFade);
        }

        currentFade = StartCoroutine(FadeLight(targetIntensity));
    }

    private IEnumerator FadeLight(float target)
    {
        float start = globalLight.intensity;
        float elapsed = 0f;

        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;

            // Smooth interpolation (nice easing)
            float t = Mathf.SmoothStep(0f, 1f, elapsed / fadeTime);

            globalLight.intensity = Mathf.Lerp(start, target, t);

            yield return null;
        }

        // Snap exactly to target at end
        globalLight.intensity = target;
    }
}