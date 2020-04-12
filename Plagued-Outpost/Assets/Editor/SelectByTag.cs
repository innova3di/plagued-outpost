using UnityEngine;
using UnityEditor;

public class SelectByTag : MonoBehaviour
{
    private static string SelectedTag = "Wall";

    [MenuItem("Helpers/Select By Tag")]
    public static void SelectObjectsWithTag()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(SelectedTag);
        Selection.objects = objects;
    }
}