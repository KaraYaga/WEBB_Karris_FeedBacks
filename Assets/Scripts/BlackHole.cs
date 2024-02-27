using System.Collections;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public float shakeDuration = 2f;
    public float shakeMagnitude = 0.1f;
    public float shakeSpeed = 1.0f;
    public ParticleSystem destructionParticles;
    public ScoreManager scoreManager;

    private bool isDestroyed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Star") && !isDestroyed)
        {
            isDestroyed = true;
            StartCoroutine(ShakeAndDestroy());
        }
    }

    private IEnumerator ShakeAndDestroy()
    {
        float elapsed = 0.0f;
        Vector3 originalPos = transform.localPosition;
        Vector3 blackHolePos = transform.position;

        while (elapsed < shakeDuration)
        {
            float x = originalPos.x + Random.Range(-1f, 1f) * shakeMagnitude;
            float y = originalPos.y + Random.Range(-1f, 1f) * shakeMagnitude;
            float z = originalPos.z + Random.Range(-1f, 1f) * shakeMagnitude;

            transform.localPosition = new Vector3(x, y, z);

            elapsed += Time.deltaTime * shakeSpeed;

            yield return null;
        }

        transform.localPosition = originalPos;

        if (destructionParticles != null)
        {
            Instantiate(destructionParticles, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);

        int randomScore = Random.Range(100, 1001);
        scoreManager.AddPopupScore(randomScore, blackHolePos);
    }
}