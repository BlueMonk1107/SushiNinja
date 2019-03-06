using UnityEngine;
using System.Collections;
using UnityEditor;

public class ChangeAllRotation : MonoBehaviour
{
    [UnityEditor.MenuItem("Tools/Change rotation")]
    static void UpdatePrefabs()
    {
        ScriptableWizard.DisplayWizard<ApplyAllPrefabs>("Apply all Fruit prefabs", "NOW");
    }
}
