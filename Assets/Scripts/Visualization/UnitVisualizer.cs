using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Birim görselleştirme - Birimler sahada görünür
/// </summary>
public class UnitVisualizer : MonoBehaviour
{
    private Dictionary<Unit, GameObject> unitGameObjects = new Dictionary<Unit, GameObject>();

    public void VisualizeUnit(Unit unit, Color tribeColor)
    {
        if (unit == null) return;

        // Birim prefab'ı oluştur
        GameObject unitObj = new GameObject($"Unit_{unit.GetUnitType()}_{unit.GetTribeId()}");
        unitObj.transform.position = new Vector3(unit.GetX(), 1f, unit.GetY());

        // Sphere oluştur (birim)
        var meshFilter = unitObj.AddComponent<MeshFilter>();
        var meshRenderer = unitObj.AddComponent<MeshRenderer>();
        var sphereCollider = unitObj.AddComponent<SphereCollider>();

        meshFilter.mesh = Resources.GetBuiltinResource<Mesh>("Sphere.fbx");

        Material mat = new Material(Shader.Find("Standard"));
        mat.color = tribeColor;
        meshRenderer.material = mat;

        unitObj.transform.localScale = Vector3.one * 0.5f;

        unitGameObjects[unit] = unitObj;
        Debug.Log($"[UnitVisualizer] Birim görselleştirildi: {unit.GetUnitType()}");
    }

    public void UpdateUnitPosition(Unit unit)
    {
        if (unitGameObjects.TryGetValue(unit, out GameObject unitObj))
        {
            unitObj.transform.position = new Vector3(unit.GetX(), 1f, unit.GetY());
        }
    }

    public void RemoveUnitVisual(Unit unit)
    {
        if (unitGameObjects.TryGetValue(unit, out GameObject unitObj))
        {
            Destroy(unitObj);
            unitGameObjects.Remove(unit);
        }
    }

    public Dictionary<Unit, GameObject> GetUnitGameObjects() => unitGameObjects;
}
