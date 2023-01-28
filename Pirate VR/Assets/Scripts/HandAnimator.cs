using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimator : MonoBehaviour
{
    public InputActionProperty pinchAnimationAction;
    public InputActionProperty gripAnimationAction;

    public Animator HandAnim;

    void Start()
    {
        HandAnim = GetComponent<Animator>();
    }

    void Update()
    {
        float triggerValue = pinchAnimationAction.action.ReadValue<float>();
        float gripValue = gripAnimationAction.action.ReadValue<float>();
        HandAnim.SetFloat("Trigger", triggerValue);
        HandAnim.SetFloat("Grip", gripValue);
    }
}
