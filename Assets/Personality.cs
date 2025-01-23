using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AnimationAction
{
    public bool useBooleanAction;
    public RiveSpriteRenderer.BooleanActions booleanAction;
    public RiveSpriteRenderer.TriggerActions triggerAction;
    public string Text;

    // Direct references to the VFX GameObjects (optional)
    public GameObject startupVFX;   // Reference for the startup effect (optional)
    public float startupDuration = 1f;    // Duration for startup effect

    public GameObject activeVFX;    // Reference for the active effect (required if no startup)
    public float activeDuration = 0.5f;   // Duration for active effect

    public GameObject releaseVFX;   // Reference for the release effect (optional)
    public float releaseDuration = 2f;    // Duration for release effect

    // Method to trigger the animations and VFX sequences
    public void Play(RiveSpriteRenderer riveRenderer, VFXHandler vfxHandler, MoveToCenter2D ursula = null)
    {
        // Play the animation action
        if (useBooleanAction)
        {
            string animationName = RiveSpriteRenderer.GetBooleanActionString(booleanAction);
            riveRenderer.PlayAnimation(animationName);
        }
        else
        {
            string animationName = RiveSpriteRenderer.GetTriggerActionString(triggerAction);
            riveRenderer.PlayAnimation(animationName);
            Debug.Log(triggerAction.ToString());
        }

        // Optional text conversation with Ursula
        if (ursula != null && Text != null)
        {
            ursula.ShowConversation(Text);
        }

        // Trigger VFX sequence via VFXHandler
        if (vfxHandler != null)
        {
            vfxHandler.PlayVFXSequence(
                startupVFX != null ? startupVFX.name : null,
                startupDuration,
                activeVFX != null ? activeVFX.name : null,
                activeDuration,
                releaseVFX != null ? releaseVFX.name : null,
                releaseDuration
            );
        }
    }
}

[CreateAssetMenu(fileName = "NewPersonality", menuName = "Personality/Personality")]
public class Personality : ScriptableObject
{
    public string personalityName;

    // Use the custom class for each action type
    public AnimationAction idleAnimation;
    public AnimationAction moveAnimation;
    public AnimationAction showeringAnimation;
    public AnimationAction showerSurprisedAnimation;
    public AnimationAction teleportToBackground;
    public AnimationAction tapReactionAnimation;
    public AnimationAction tapAnnoyedAnimation;
    public AnimationAction specialActionAnimation;
    public AnimationAction dragAnimationLoop;
    public AnimationAction dragAnimationEnd;
    public AnimationAction hatGameStartAnimation;
    public AnimationAction equipHatAnimation;
    public AnimationAction teleportAnimation;

    // Methods to play selected animations
    public void PlayIdleAnimation(RiveSpriteRenderer riveRenderer, MoveToCenter2D ursula = null, VFXHandler vfxHandler = null)
    {
        idleAnimation.Play(riveRenderer, vfxHandler, ursula);
    }

    public void PlayMoveAnimation(RiveSpriteRenderer riveRenderer, MoveToCenter2D ursula = null, VFXHandler vfxHandler = null)
    {
        moveAnimation.Play(riveRenderer, vfxHandler, ursula);
    }

    public void PlayShoweringAnimation(RiveSpriteRenderer riveRenderer, MoveToCenter2D ursula = null, VFXHandler vfxHandler = null)
    {
        showeringAnimation.Play(riveRenderer, vfxHandler, ursula);
    }

    public void PlaySurprisedAnimation(RiveSpriteRenderer riveRenderer, MoveToCenter2D ursula = null, VFXHandler vfxHandler = null)
    {
        showerSurprisedAnimation.Play(riveRenderer, vfxHandler, ursula);
    }

    public void PlayTeleportToBackgroundScene(RiveSpriteRenderer riveRenderer, MoveToCenter2D ursula = null, VFXHandler vfxHandler = null)
    {
        showerSurprisedAnimation.Play(riveRenderer, vfxHandler, ursula);
    }

    public void PlayTapReactionAnimation(RiveSpriteRenderer riveRenderer, MoveToCenter2D ursula = null, VFXHandler vfxHandler = null)
    {
        tapReactionAnimation.Play(riveRenderer, vfxHandler, ursula);
    }

    public void PlayTapAnnoyedAnimation(RiveSpriteRenderer riveRenderer, MoveToCenter2D ursula = null, VFXHandler vfxHandler = null)
    {
        tapAnnoyedAnimation.Play(riveRenderer, vfxHandler, ursula);
    }

    public void PlaySpecialActionAnimation(RiveSpriteRenderer riveRenderer, MoveToCenter2D ursula = null, VFXHandler vfxHandler = null)
    {
        specialActionAnimation.Play(riveRenderer, vfxHandler, ursula);
    }
    public void PlayDragAnimation(RiveSpriteRenderer riveRenderer, MoveToCenter2D ursula = null, VFXHandler vfxHandler = null)
    {
        dragAnimationLoop.Play(riveRenderer, vfxHandler, ursula);
    }
    public void PlayDragEndAnimation(RiveSpriteRenderer riveRenderer, MoveToCenter2D ursula = null, VFXHandler vfxHandler = null)
    {
        dragAnimationEnd.Play(riveRenderer, vfxHandler, ursula);
    }

    public void PlayTeleportAnimation(RiveSpriteRenderer riveRenderer, MoveToCenter2D ursula = null, VFXHandler vfxHandler = null)
    {
        teleportAnimation.Play(riveRenderer, vfxHandler, ursula);
    }

    public void PlayHatGameAnimation(RiveSpriteRenderer riveRenderer, MoveToCenter2D ursula = null, VFXHandler vfxHandler = null)
    {
        hatGameStartAnimation.Play(riveRenderer, vfxHandler, ursula);
    }

    public void PlayEquipHatAnimation(RiveSpriteRenderer riveRenderer, MoveToCenter2D ursula = null, VFXHandler vfxHandler = null)
    {
        equipHatAnimation.Play(riveRenderer, vfxHandler, ursula);
    }
}
