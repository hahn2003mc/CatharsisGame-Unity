using UnityEngine;
using UnityEngine.UI;

public class EnergyBarController : MonoBehaviour
{
    public KnightController knightController;
    public Image energyFill;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float percent = knightController.currentEnergy / knightController.maxEnergy;
        energyFill.fillAmount = percent;
    }
}
