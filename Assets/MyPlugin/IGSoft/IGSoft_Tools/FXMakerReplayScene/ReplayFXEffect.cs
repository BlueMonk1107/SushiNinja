using UnityEngine;

public class ReplayFXEffect : MonoBehaviour
{

    public void Awake()
    {
        NsEffectManager.SetReplayEffect(gameObject);
        NsEffectManager.PreloadResource(gameObject);
    }

    public void OnEnable()
    {
        NsEffectManager.RunReplayEffect(gameObject, false);
    }
}
