using UnityEngine;

public class KnightFormController : MonoBehaviour
{
    public enum KnightForm
    {
        Armor,
        Girl
    }

    public KnightForm currentForm;

    public bool formLocked = true;

    public Animator animator;

    public RuntimeAnimatorController armorAnimator;
    public RuntimeAnimatorController girlAnimator;

    void Start()
    {
        animator = GetComponent<Animator>();
        // SetForm(KnightForm.Armor);
        ApplyForm();
    }

    public void SetForm(KnightForm newForm)
    {
        if (formLocked) return;

        currentForm = newForm;
        ApplyForm();
    }

    void ApplyForm()
    {
        switch (currentForm)
        {
            case KnightForm.Armor:
                animator.runtimeAnimatorController = armorAnimator;
                break;

            case KnightForm.Girl:
                animator.runtimeAnimatorController = girlAnimator;
                break;
        }
    }

    public void LockForm(bool locked)
    {
        formLocked = locked;
    }
}