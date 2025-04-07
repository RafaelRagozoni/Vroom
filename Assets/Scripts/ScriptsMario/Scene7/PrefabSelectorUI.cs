using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefabSelectorUI : MonoBehaviour
{
    public List<GameObject> prefabs; // Lista de prefabs disponíveis
    public Transform spawnPoint; // Local onde os objetos serão instanciados
    public GameObject buttonPrefab; // Prefab do botão
    public Transform buttonContainer; // Container dos botões

    void Start()
    {
        GenerateButtons();
    }

    void GenerateButtons()
    {
        foreach (GameObject prefab in prefabs)
        {
            GameObject button = Instantiate(buttonPrefab, buttonContainer);
            button.GetComponentInChildren<Text>().text = prefab.name; // Define o nome do botão

            GameObject prefabCopy = prefab; // Criar uma cópia da referência

            button.GetComponent<Button>().onClick.AddListener(() => InstantiateObject(prefabCopy));
        }
    }

    void InstantiateObject(GameObject prefab)
    {
        Instantiate(prefab, spawnPoint.position, Quaternion.identity);
    }
}
