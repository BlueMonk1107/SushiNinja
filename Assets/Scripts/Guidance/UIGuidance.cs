using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIGuidance : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        if(GuidanceBase.GuidanceMark>=3)
            gameObject.SetActive(false);
    }

}
