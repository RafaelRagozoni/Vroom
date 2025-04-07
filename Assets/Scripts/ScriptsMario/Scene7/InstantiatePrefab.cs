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
        // Remove bot�es antigos, se houver
        foreach (Transform child in Container.transform)
        {
            Destroy(child.gameObject);
        }

        // Gera bot�es dinamicamente para cada prefab na lista
        for (int i = 0; i < Prefabs.Count; i++)
        {
            Debug.Log($"Criando bot�o para {Prefabs[i].name}"); // Loga o nome do prefab

            GameObject button = Instantiate(buttonPrefab, Container.transform, false); // Instancia bot�o corretamente
            button.name = "Button_" + Prefabs[i].name; // Nomeia o bot�o corretamente

            // Configura o texto do bot�o
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>(true); // O 'true' garante que busca at� em objetos inativos
            if (buttonText != null)
            {
                buttonText.text = Prefabs[i].name; // Define o nome correto do prefab no bot�o
            }
            else
            {
                Debug.LogError($"O bot�o {button.name} n�o tem um componente TMP_Text!");
            }

            // Corrige refer�ncia para evitar closures
            GameObject prefabCopy = Prefabs[i];

            // Adiciona a fun��o ao bot�o
            button.GetComponent<Button>().onClick.AddListener(() => InstantiateObject(prefabCopy));
        }
    }

    private bool canClick = true;
    private float clickCooldown = 0.1f;

    void InstantiateObject(GameObject prefab)
    {
        if (!canClick) return; // Se o clique j� foi registrado, ignora.

        canClick = false; // Bloqueia cliques adicionais por um curto tempo.
        Invoke(nameof(ResetClick), clickCooldown); // Reativa o clique ap�s cooldown.

        Debug.Log("Clique detectado!");

        Vector3 position = sceneCamera.transform.position + sceneCamera.transform.forward * 1.0f;
        Quaternion rotation = Quaternion.LookRotation(sceneCamera.transform.forward, Vector3.up);
        rotation *= Quaternion.Euler(0, 90, 0); // Ajusta rota��o

        Instantiate(prefab, position, rotation);
    }

    void ResetClick()
    {
        canClick = true;
    }
}
