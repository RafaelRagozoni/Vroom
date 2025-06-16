using UnityEngine;

public class ActivateSaveAndLoadUI : MonoBehaviour
{
    public GameObject SaveUI;
    public GameObject LoadUI;

    public Camera sceneCamera;
    public float distanceFromCamera = 1.0f; // Distance from the camera to position the UI
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         distanceFromCamera = 1.0f; 
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
            SaveUI.transform.position = sceneCamera.transform.position + sceneCamera.transform.forward * distanceFromCamera;
            SaveUI.transform.LookAt(sceneCamera.transform, Vector3.up);
            // Correct the rotation so the UI does not appear mirrored
            SaveUI.transform.Rotate(0, 180, 0);
            DeactivateLoadUI(); // Deactivate Load UI when Save UI is activated
        }
    }

    public void ActivateLoadUI()
    {
        if (LoadUI != null)
        {
            GetComponent<SaveAndLoad>().PopulateLoadDropdown();
            // Ensure the dropdown is populated before activating the UI
            LoadUI.SetActive(true);
            LoadUI.transform.position = sceneCamera.transform.position + sceneCamera.transform.forward * distanceFromCamera;
            LoadUI.transform.LookAt(sceneCamera.transform, Vector3.up);
            // Correct the rotation so the UI does not appear mirrored
            LoadUI.transform.Rotate(0, 180, 0);
            DeactivateSaveUI(); // Deactivate Save UI when Load UI is activated
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
