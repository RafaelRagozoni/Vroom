using UnityEngine;
using TMPro;

public class ActivateSaveAndLoadUI : MonoBehaviour
{
    public GameObject SaveUI;
    public GameObject LoadUI;

    public GameObject deleteUI;

    // Save Feedback UI
    public GameObject saveFeedbackUI;
    public TMP_Text saveFeedbackText;

    // Error Save feedback UI
    public GameObject errorSaveFeedbackUI;

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
        InstantiatePrefabUI.Instance.DeactivateAddFurnitureMode();
        EditFurnitureManager.Instance.DeactivateEditFurnitureMode();
        InstantiateTexturesUI.Instance.DeactivateTextureEditMode();
        if (SaveUI != null)
        {
            SaveUI.SetActive(true);
            SaveUI.transform.position = sceneCamera.transform.position + sceneCamera.transform.forward * distanceFromCamera;
            SaveUI.transform.LookAt(sceneCamera.transform, Vector3.up);
            // Correct the rotation so the UI does not appear mirrored
            SaveUI.transform.Rotate(0, 180, 0);
            DeactivateLoadUI(); // Deactivate Load UI when Save UI is activated
            DeactivateDeleteUI(); // Deactivate Delete UI when Save UI is activated
            DeactivateSaveUIFeedBack(); // Deactivate Save Feedback UI when Save UI is activated
            DeactivateErrorSaveUIFeedBack(); // Deactivate Error Save Feedback UI when Save UI is activated
        }
    }

    public void ActivateDeleteUI()
    {
        if (deleteUI != null)
        {
             GetComponent<SaveAndLoad>().PopulateDeleteDropdown();
            deleteUI.SetActive(true);
            deleteUI.transform.position = sceneCamera.transform.position + sceneCamera.transform.forward * distanceFromCamera;
            deleteUI.transform.LookAt(sceneCamera.transform, Vector3.up);
            // Correct the rotation so the UI does not appear mirrored
            deleteUI.transform.Rotate(0, 180, 0);
            DeactivateLoadUI(); // Deactivate Load UI when Save UI is activated
            DeactivateSaveUI(); // Deactivate Save UI when Delete UI is activated
            DeactivateSaveUIFeedBack(); // Deactivate Save Feedback UI when Delete UI is activated
            DeactivateErrorSaveUIFeedBack(); // Deactivate Error Save Feedback UI when Delete UI
        }
    }

    public void ActivateLoadUI()
    {
        InstantiatePrefabUI.Instance.DeactivateAddFurnitureMode();
        EditFurnitureManager.Instance.DeactivateEditFurnitureMode();
        InstantiateTexturesUI.Instance.DeactivateTextureEditMode();
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
            DeactivateDeleteUI(); // Deactivate Delete UI when Load UI is activated
            DeactivateSaveUIFeedBack(); // Deactivate Save Feedback UI when Load UI is activated
            DeactivateErrorSaveUIFeedBack(); // Deactivate Error Save Feedback UI when Load UI is activated
        }
    }

    public void ActivateSaveFeedbackUI(string feedbackText)
    {
        if (saveFeedbackUI != null && saveFeedbackText != null)
        {
            saveFeedbackText.text = feedbackText;
            saveFeedbackUI.SetActive(true);
            errorSaveFeedbackUI.SetActive(false);
            saveFeedbackUI.transform.position = sceneCamera.transform.position + sceneCamera.transform.forward * distanceFromCamera;
            saveFeedbackUI.transform.LookAt(sceneCamera.transform, Vector3.up);
            // Correct the rotation so the UI does not appear mirrored
            saveFeedbackUI.transform.Rotate(0, 180, 0);
            DeactivateLoadUI();
            DeactivateSaveUI();
            DeactivateDeleteUI();
            DeactivateErrorSaveUIFeedBack();
        }
    }

    public void ActivateErrorSaveFeedbackUI()
    {
        if (errorSaveFeedbackUI != null)
        {
            errorSaveFeedbackUI.SetActive(true);
            saveFeedbackUI.SetActive(false);
            errorSaveFeedbackUI.transform.position = sceneCamera.transform.position + sceneCamera.transform.forward * distanceFromCamera;
            errorSaveFeedbackUI.transform.LookAt(sceneCamera.transform, Vector3.up);
            // Correct the rotation so the UI does not appear mirrored
            errorSaveFeedbackUI.transform.Rotate(0, 180, 0);
            DeactivateLoadUI();
            DeactivateSaveUI();
            DeactivateDeleteUI();
            DeactivateSaveUIFeedBack();
        }
    }

    public void DeactivateAllUI()
    {
        DeactivateSaveUI();
        DeactivateLoadUI();
        DeactivateDeleteUI();
        DeactivateSaveUIFeedBack();
        DeactivateErrorSaveUIFeedBack();
    }
    
    public void DeactivateSaveUI()
    {
        if (SaveUI != null)
        {
            SaveUI.SetActive(false);
        }
    }

    public void DeactivateDeleteUI()
    {
        if (deleteUI != null)
        {
            deleteUI.SetActive(false);
        }
    }

    public void DeactivateSaveUIFeedBack()
    {
        if (saveFeedbackUI != null)
        {
            saveFeedbackUI.SetActive(false);
        }
    }

    public void DeactivateErrorSaveUIFeedBack()
    {
        if (errorSaveFeedbackUI != null)
        {
            errorSaveFeedbackUI.SetActive(false);
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
