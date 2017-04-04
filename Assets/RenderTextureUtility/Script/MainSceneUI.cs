using UnityEngine;
using UnityEngine.UI;

public class MainSceneUI : MonoBehaviour
{
    public Text content;
    public RawImage rawImage;

    private void Start()
    {
        content.text = string.Format("{0}: {1}x{2}", rawImage.texture.name, rawImage.texture.width, rawImage.texture.height);
    }
}