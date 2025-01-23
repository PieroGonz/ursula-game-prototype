using UnityEngine;
using System.Collections;

public class VFXHandler : MonoBehaviour
{
    public void PlayVFXSequence(string startupVFXName, float startupDuration, string activeVFXName, float activeDuration, string releaseVFXName, float releaseDuration)
    {
        if (!string.IsNullOrEmpty(startupVFXName))
        {
            StartCoroutine(PlayVFXCoroutine(startupVFXName, startupDuration, activeVFXName, activeDuration, releaseVFXName, releaseDuration));
        }
        else if (!string.IsNullOrEmpty(activeVFXName))
        {
            StartCoroutine(PlayVFXCoroutine(null, 0, activeVFXName, activeDuration, releaseVFXName, releaseDuration));
        }
    }

    private IEnumerator PlayVFXCoroutine(string startupVFXName, float startupDuration, string activeVFXName, float activeDuration, string releaseVFXName, float releaseDuration)
    {
        if (!string.IsNullOrEmpty(startupVFXName))
        {
            GameObject startupVFX = FindVFXByName(startupVFXName);
            if (startupVFX != null)
            {
                startupVFX.SetActive(true);
                yield return new WaitForSeconds(startupDuration);
                startupVFX.SetActive(false);
            }
        }

        if (!string.IsNullOrEmpty(activeVFXName))
        {
            GameObject activeVFX = FindVFXByName(activeVFXName);
            if (activeVFX != null)
            {
                activeVFX.SetActive(true);
                yield return new WaitForSeconds(activeDuration);
                activeVFX.SetActive(false);
            }
        }

        if (!string.IsNullOrEmpty(releaseVFXName))
        {
            GameObject releaseVFX = FindVFXByName(releaseVFXName);
            if (releaseVFX != null)
            {
                releaseVFX.SetActive(true);
                yield return new WaitForSeconds(releaseDuration);
                releaseVFX.SetActive(false);
            }
        }
    }

    private GameObject FindVFXByName(string vfxName)
    {
        Transform vfxTransform = transform.Find(vfxName);
        return vfxTransform != null ? vfxTransform.gameObject : null;
    }
}
