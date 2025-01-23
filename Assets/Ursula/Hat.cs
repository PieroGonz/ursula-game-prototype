using UnityEngine;

[CreateAssetMenu(fileName = "NewHat", menuName = "Hats/Hat")]
public class Hat : ScriptableObject
{
    public string hatName; // Name of the hat
    public string idleAnimation; // Idle animation name
    public string tapReactionAnimation; // Animation name when Ursula is tapped
    public string specialActionAnimation; // Special action animation name
    public string idleParticleEffectName; // Name of the particle effect in Ursula's hierarchy
    public string specialParticleEffectName; // Name of the special particle effect in Ursula's hierarchy

    public void PlayShowerDiscoveredAnim(RiveSpriteRenderer riveTexture, Transform ursulaTransform)
    {
        riveTexture.PlayAnimation(idleAnimation);
        ToggleParticleEffect(ursulaTransform, idleParticleEffectName, true);
    }
    public void PlayIdleAnimation(RiveSpriteRenderer riveTexture, Transform ursulaTransform)
    {
        riveTexture.PlayAnimation(idleAnimation);
        ToggleParticleEffect(ursulaTransform, idleParticleEffectName, true);
    }

    public void PlayTapReactionAnimation(RiveSpriteRenderer riveTexture)
    {
        riveTexture.PlayAnimation(tapReactionAnimation);
    }

    public void PlaySpecialActionAnimation(RiveSpriteRenderer riveTexture, Transform ursulaTransform)
    {
        riveTexture.PlayAnimation(specialActionAnimation);
        ToggleParticleEffect(ursulaTransform, specialParticleEffectName, true);
    }
    public void DisableIdleParticleEffect(Transform ursulaTransform)
    {
        ToggleParticleEffect(ursulaTransform, idleParticleEffectName, false);
    }

    public void ToggleParticleEffect(Transform ursulaTransform, string particleEffectName, bool isActive)
    {
        if (!string.IsNullOrEmpty(particleEffectName))
        {
            Transform particleEffectTransform = ursulaTransform.Find(particleEffectName);
            if (particleEffectTransform != null)
            {
                particleEffectTransform.gameObject.SetActive(isActive);
            }
        }
    }
}
