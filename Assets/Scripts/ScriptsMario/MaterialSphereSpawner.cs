using UnityEngine;

public class MaterialSphereSpawner : MonoBehaviour
{
    public GameObject spherePrefab; // Prefab simples da esfera
    public float spacing = 2f;      // Espaçamento entre as esferas geradas

    void Start()
    {
        if (spherePrefab == null)
        {
            Debug.LogError("Sphere Prefab is not assigned.");
            return;
        }

        // Carrega todos os materiais da pasta Resources/Materials
        Material[] materials = Resources.LoadAll<Material>("Materials");

        for (int i = 0; i < materials.Length; i++)
        {
            GameObject newSphere = Instantiate(spherePrefab);
            newSphere.transform.position = transform.position + new Vector3(i * spacing, 0, 0);
            newSphere.name = "MaterialSphere_" + materials[i].name;

            Renderer renderer = newSphere.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = materials[i];
            }
        }
    }
}
