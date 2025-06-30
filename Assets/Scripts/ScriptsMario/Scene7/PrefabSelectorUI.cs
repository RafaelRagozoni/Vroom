using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefabSelectorUI : MonoBehaviour
{
    public List<GameObject> prefabs; // Lista de prefabs dispon�veis
    public Transform spawnPoint; // Local onde os objetos ser�o instanciados
    public GameObject buttonPrefab; // Prefab do bot�o
    public Transform buttonContainer; // Container dos bot�es

    void Start()
    {
        GenerateButtons();
    }

    void GenerateButtons()
    {
        foreach (GameObject prefab in prefabs)
        {
            GameObject button = Instantiate(buttonPrefab, buttonContainer);
            button.GetComponentInChildren<Text>().text = prefab.name; // Define o nome do bot�o

            GameObject prefabCopy = prefab; // Criar uma c�pia da refer�ncia

            button.GetComponent<Button>().onClick.AddListener(() => InstantiateObject(prefabCopy));
        }
    }

    void InstantiateObject(GameObject prefab)
    {
        Instantiate(prefab, spawnPoint.position, Quaternion.identity);
    }
}
