using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PortalBehavior : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject self;
    public float interactionDistance = 1f;
    public GameObject FadePanel;
    

    public float fadeTime = 0.6f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Image img = FadePanel.GetComponent<Image>();

        img.color = new Color(img.color.r, img.color.g, img.color.b, 0.01f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Vector2.Distance(playerController.transform.position, self.transform.position) < interactionDistance)
            {
                Debug.Log("within portal collider and pressed interact button (F)");
                StartCoroutine(CollapsePortal());
                playerController.invincible = true;
            }
        }
    }

    IEnumerator CollapsePortal()
    {
        Image img = FadePanel.GetComponent<Image>();

        float elapsed = 0f;

        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;

            float alpha = Mathf.Clamp01(elapsed / fadeTime);

            img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);

            yield return null; // wait one frame
        }

    
        // SceneManager.LoadScene("CorruptedWorld1");
    

}

}
