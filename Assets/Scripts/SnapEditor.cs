#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

[ExecuteInEditMode]
public class SnapEditor : MonoBehaviour
{
    public static Vector3 tileSizeInUnits = new Vector3(1.0f, 0.57f, 0.5f);
    Vector3 Snap(Vector3 localPosition)
    {
        // Calculate ratios for simple grid snap
        float xx = Mathf.Round(localPosition.y / tileSizeInUnits.y - localPosition.x / tileSizeInUnits.x);
        float yy = Mathf.Round(localPosition.y / tileSizeInUnits.y + localPosition.x / tileSizeInUnits.x);

        // Calculate grid aligned position from current position
        Vector3 position;
        position.x = (yy - xx) * 0.5f * tileSizeInUnits.x;
        position.y = (yy + xx) * 0.5f * tileSizeInUnits.y;
        position.z = 1.0f * position.y - 0.1f * position.x;

        return position;
    }

    void OnEnable()
    {
#if UNITY_EDITOR
        SceneView.onSceneGUIDelegate += OnSceneGui;
#endif
    }

    void OnDisable()
    {
#if UNITY_EDITOR
        SceneView.onSceneGUIDelegate -= OnSceneGui;
#endif
    }
#if UNITY_EDITOR
    void OnSceneGui(SceneView view)
    {
        GameObject tile = Selection.activeGameObject;
        if (tile != null)
        {
            if (Event.current.type == EventType.mouseUp)
            {
                tile.transform.position = Snap(tile.transform.position);
            }
        }
    }
#endif
}