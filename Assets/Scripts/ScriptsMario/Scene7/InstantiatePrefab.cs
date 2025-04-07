using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class InstantiatePrefab : MonoBehaviour
{
    public Camera sceneCamera;
    public List<GameObject> Prefabs;
    public GameObject Container;
    public GameObject buttonPrefab;

    void Start()
    {
        GenerateButtons();
    }

    void GenerateButtons()
    {
        // Remove botões antigos, se houver
        foreach (Transform child in Container.transform)
        {
            Destroy(child.gameObject);
        }

        // Gera botões dinamicamente para cada prefab na lista
        for (int i = 0; i < Prefabs.Count; i++)
        {
            Debug.Log($"Criando botão para {Prefabs[i].name}"); // Loga o nome do prefab

            GameObject button = Instantiate(buttonPrefab, Container.transform, false); // Instancia botão corretamente
            button.name = "Button_" + Prefabs[i].name; // Nomeia o botão corretamente

            // Configura o texto do botão
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>(true); // O 'true' garante que busca até em objetos inativos
            if (buttonText != null)
            {
                buttonText.text = Prefabs[i].name; // Define o nome correto do prefab no botão
            }
            else
            {
                Debug.LogError($"O botão {button.name} não tem um componente TMP_Text!");
            }

            // Corrige referência para evitar closures
            GameObject prefabCopy = Prefabs[i];

            // Adiciona a função ao botão
            button.GetComponent<Button>().onClick.AddListener(() => InstantiateObject(prefabCopy));
        }
    }

    private bool canClick = true;
    private float clickCooldown = 0.1f;

    void InstantiateObject(GameObject prefab)
    {
        if (!canClick) return; // Se o clique já foi registrado, ignora.

        canClick = false; // Bloqueia cliques adicionais por um curto tempo.
        Invoke(nameof(ResetClick), clickCooldown); // Reativa o clique após cooldown.

        Debug.Log("Clique detectado!");

        Vector3 position = sceneCamera.transform.position + sceneCamera.transform.forward * 1.0f;
        Quaternion rotation = Quaternion.LookRotation(sceneCamera.transform.forward, Vector3.up);
        rotation *= Quaternion.Euler(0, 90, 0); // Ajusta rotação

        Instantiate(prefab, position, rotation);
    }

    void ResetClick()
    {
        canClick = true;
    }
}
