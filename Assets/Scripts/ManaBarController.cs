using UnityEngine;
using UnityEngine.UI;

public class ManaBarController : MonoBehaviour
{
    public WizardController wizardController;
    public Image manaFill;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float percent = wizardController.currentMana / wizardController.maxMana;
        manaFill.fillAmount = percent;
    }
}
