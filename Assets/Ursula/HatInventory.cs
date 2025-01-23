using UnityEngine;
using UnityEngine.UI;

public class HatInventory : MonoBehaviour
{
    public GameObject hatOption1;
    public GameObject hatOption2;
    public GameObject hatOption3;
    public GameObject hatOption4;
    public GameObject hatOption5;
    public GameObject hatOption6;

    public GameObject hatSprite1;
    public GameObject hatSprite2;
    public GameObject hatSprite3;
    public GameObject hatSprite4;
    public GameObject hatSprite5;
    public GameObject hatSprite6;

    public MoveToCenter2D moveToCenter2D;

    void Start()
    {
        if (hatOption1 != null)
            hatOption1.GetComponent<Button>().onClick.AddListener(() => OnHatSelected(hatSprite1.gameObject));

        if (hatOption2 != null)
            hatOption2.GetComponent<Button>().onClick.AddListener(() => OnHatSelected(hatSprite2.gameObject));

        if (hatOption3 != null)
            hatOption3.GetComponent<Button>().onClick.AddListener(() => OnHatSelected(hatSprite3.gameObject));

        if (hatOption4 != null)
            hatOption4.GetComponent<Button>().onClick.AddListener(() => OnHatSelected(hatSprite4.gameObject));

        if (hatOption5 != null)
            hatOption5.GetComponent<Button>().onClick.AddListener(() => OnHatSelected(hatSprite5.gameObject));

        if (hatOption6 != null)
            hatOption6.GetComponent<Button>().onClick.AddListener(() => OnHatSelected(hatSprite6.gameObject));

    }

    void OnHatSelected(GameObject hat)
    {
        // Handle hat selection logic here
        Debug.Log("Selected Hat: " + hat.name);

        StartCoroutine(moveToCenter2D.HandleHatCollision(hat.gameObject,true));
    }
}
