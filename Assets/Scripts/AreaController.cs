using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AreaController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject self;
    public string areaID;
    public GameObject areaAnimationUI; // UI component displayed when the player enters

    public float fadeTime = 0.8f;

    public bool canPlayAreaAnimation;

    void Start()
    {
        self.SetActive(true);
        areaAnimationUI.SetActive(false);
        canPlayAreaAnimation = true;
    }

    // Update is called once per frame
    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("something entered the collider");
        if (!canPlayAreaAnimation) { return; } // lock animation
        if (other.CompareTag("Player") ||
    (other.transform.parent != null && other.transform.parent.CompareTag("Player")))
        {
            Debug.Log("Player or direct child entered");
            StartCoroutine(playAreaAnimationUI());
        }
    }


    public IEnumerator playAreaAnimationUI() 
    {
        Debug.Log("player has entered collider and coroutine started");
        canPlayAreaAnimation = false; // lock animation until coroutine finishes
        // enable UI with alpha 0
        areaAnimationUI.SetActive(true);
        Image image = areaAnimationUI.GetComponent<Image>();

        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.01f);

        // UI fade in
        float elapsed = 0f;

        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;

            float alpha = Mathf.Clamp01(elapsed / fadeTime);

            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);

            yield return null; // wait one frame
        }
        yield return new WaitForSeconds(5f); // wait to display

        // UI fade out
        elapsed = 0f;

        while (elapsed > -fadeTime)
        {
            elapsed -= Time.deltaTime;

            float alpha = Mathf.Clamp01(elapsed / -fadeTime);

            image.color = new Color(image.color.r, image.color.g, image.color.b, 1-alpha);

            yield return null; // wait one frame
        }
        yield return new WaitForSeconds(5f); // wait to display

        // disable
        areaAnimationUI.SetActive(false);
        canPlayAreaAnimation = true; // unlock animation
    }
}
