using UnityEngine;

public class AddCollidersEditor : MonoBehaviour
{
    [ContextMenu("Add mesh colliders to children")]
    void AddColliders()
    {
        foreach (var mf in GetComponentsInChildren<MeshFilter>())
        {
            if (!mf.gameObject.GetComponent<MeshCollider>())
                mf.gameObject.AddComponent<MeshCollider>();
        }
    }
}
