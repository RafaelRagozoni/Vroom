using UnityEngine;

public class ActivateSaveAndLoadUI : MonoBehaviour
{
    public GameObject SaveUI;
    public GameObject LoadUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ActivateSaveUI()
    {
        if (SaveUI != null)
        {
            SaveUI.SetActive(true);
        }
    }

    public void ActivateLoadUI()
    {
        if (LoadUI != null)
        {
            GetComponent<SaveAndLoad>().PopulateLoadDropdown();
            // Ensure the dropdown is populated before activating the UI
            LoadUI.SetActive(true);
        }
    }
    public void DeactivateSaveUI()
    {
        if (SaveUI != null)
        {
            SaveUI.SetActive(false);
        }
    }
    public void DeactivateLoadUI()
    {
        if (LoadUI != null)
        {
            LoadUI.SetActive(false);
        }
    }
}
