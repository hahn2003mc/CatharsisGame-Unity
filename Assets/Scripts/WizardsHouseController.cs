using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WizardsHouseController : MonoBehaviour
{

    public GameObject Player;
    public GameObject self;

    public float interactionDistance = 1f;

    public int interactionCount;

    public GameControllerLaudos GameControllerLaudos;

    public bool canInteract = true;

    public string sceneID;

    public GameObject DialogueContainer;
    public GameObject BackPanel;
    public GameObject DialogueBox;
    public GameObject DialogueText;
    public GameObject SpeakerText;
    public GameObject Portrait;

    public Rigidbody2D playerController;
    public GameObject knight;
    public GameObject wizard;
    public KnightController knightController;
    public WizardController wizardController;
    public Animator knightAnimator;
    public Animator wizardAnimator;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactionCount = 0;
        canInteract = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Vector2.Distance(Player.transform.position, self.transform.position) < interactionDistance && canInteract)
            {
                if (interactionCount == 0)
                {
                    if (sceneID == "Laudos4")
                    {
                        if (playerController != null)
                        {
                            playerController.linearVelocity = Vector2.zero;

                            knightController = knight.GetComponent<KnightController>();
                            wizardController = wizard.GetComponent<WizardController>();
                            knightAnimator = knight.GetComponent<Animator>();
                            wizardAnimator = wizard.GetComponent<Animator>();


                            // Force stop movement completely
                            knightController.isWalking = false;
                            wizardController.isWalking = false;

                            // Reset animator parameters
                            knightAnimator.SetBool("isWalking", false);
                            wizardAnimator.SetBool("isWalking", false);
                            knightAnimator.ResetTrigger("Attack");
                            knightAnimator.ResetTrigger("HeavyAttack");
                            wizardAnimator.ResetTrigger("Attack");

                            // Play idle AFTER clearing params
                            knightAnimator.Play("Idle", 0, 0f);
                            wizardAnimator.Play("Idle", 0, 0f);
                        }

                        if (knightController != null)
                        {
                            knightController.setCanMoveFalse();
                        }
                        if (wizardController != null)
                        {
                            wizardController.setCanMoveFalse();
                        }

                        GameControllerLaudos.transferToWizardsHouseScene();
                    }
                }
            }
        }
    }

    public void updateInteractionCount(int value)
    {
        interactionCount = value;
    }

    public void setCanInteract(bool value)
    {
        canInteract = value;
    }

}
