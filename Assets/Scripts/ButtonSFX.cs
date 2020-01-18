using UnityEngine;
using UnityEngine.UI;

public class ButtonSFX : MonoBehaviour
{
    [SerializeField]
    AudioClip audioClip = null;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);    
    }

    void OnClick()
    {
        if (audioClip != null && SFXManager.Instance != null)
        {
            SFXManager.Instance.Play(audioClip);
        }
    }
}
