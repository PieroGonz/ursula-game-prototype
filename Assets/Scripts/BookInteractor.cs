using UnityEngine;
using Lean.Touch;

public class BookInteractor : MonoBehaviour
{
    public GameObject menuCanvas;

    void Start()
    {
        menuCanvas.SetActive(false);
    }

    public void OnBookTapped()
    {
        menuCanvas.SetActive(!menuCanvas.activeSelf);
    }
}