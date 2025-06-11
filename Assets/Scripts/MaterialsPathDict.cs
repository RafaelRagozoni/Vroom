using UnityEngine;
using System.Collections.Generic;

public class MaterialsPathDict : MonoBehaviour
{
   [Header("Nomes dos Materiais (chaves)")]
    public List<string> MaterialsName = new List<string>();

    [Header("Caminhos dos Materiais (valores)")]
    public List<string> MaterialsPath = new List<string>();

    /// <summary>
    /// Retorna o caminho do material pelo nome, ou null se não encontrar.
    /// </summary>
    public string GetPathByName(string name)
    {
        int idx = MaterialsName.IndexOf(name);
        if (idx >= 0 && idx < MaterialsPath.Count)
            return MaterialsPath[idx];
        return null;
    }

}
