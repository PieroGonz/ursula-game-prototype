using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BookClickHandler : MonoBehaviour
{
    public GameObject debugText;
    private Coroutine showCoroutine;

    void Start() {
        if (debugText != null) {
            debugText.SetActive(false);
        }
    }

    void OnMouseDown() {
        if (showCoroutine != null) StopCoroutine(showCoroutine);
        if (debugText != null) {
            debugText.SetActive(true);
            showCoroutine = StartCoroutine(HideDebugTextAfterDelay());
        }

        Debug.Log("event on");
    }

    IEnumerator HideDebugTextAfterDelay() {
        yield return new WaitForSeconds(2);
        if (debugText != null) {
            debugText.SetActive(false);
        }
    }
}