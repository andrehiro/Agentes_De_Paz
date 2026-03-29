using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ColliderScaler : MonoBehaviour
{
    private const float BASE_WIDTH = 1920f;
    private const float BASE_HEIGHT = 1080f;

    private Vector3 basePosition;
    private Vector3 baseScale;
    private Camera mainCamera;
    private float baseCameraHeight;
    private float baseCameraWidth;

    void Awake()
    {
        mainCamera = Camera.main;
        baseCameraHeight = mainCamera.orthographicSize * 2f;
        baseCameraWidth = baseCameraHeight * (BASE_WIDTH / BASE_HEIGHT);

        string key = SceneManager.GetActiveScene().name + "_" + gameObject.name;

        if (Screen.width == 1920 && Screen.height == 1080)
        {
            basePosition = transform.position;
            baseScale = transform.localScale;

            PlayerPrefs.SetFloat(key + "_px", basePosition.x);
            PlayerPrefs.SetFloat(key + "_py", basePosition.y);
            PlayerPrefs.SetFloat(key + "_pz", basePosition.z);
            PlayerPrefs.SetFloat(key + "_sx", baseScale.x);
            PlayerPrefs.SetFloat(key + "_sy", baseScale.y);
            PlayerPrefs.SetFloat(key + "_sz", baseScale.z);
            PlayerPrefs.Save();
        }
        else if (PlayerPrefs.HasKey(key + "_px"))
        {
            basePosition = new Vector3(
                PlayerPrefs.GetFloat(key + "_px"),
                PlayerPrefs.GetFloat(key + "_py"),
                PlayerPrefs.GetFloat(key + "_pz")
            );
            baseScale = new Vector3(
                PlayerPrefs.GetFloat(key + "_sx"),
                PlayerPrefs.GetFloat(key + "_sy"),
                PlayerPrefs.GetFloat(key + "_sz")
            );
        }
        else
        {
            basePosition = transform.position;
            baseScale = transform.localScale;
        }
    }

    public void AdjustToResolution()
    {
        StartCoroutine(AdjustNextFrame());
    }

    private IEnumerator AdjustNextFrame()
    {
        yield return null;

        float currentCameraHeight = mainCamera.orthographicSize * 2f;
        float currentCameraWidth = currentCameraHeight * ((float)Screen.width / Screen.height);

        float ratioX = currentCameraWidth / baseCameraWidth;
        float ratioY = currentCameraHeight / baseCameraHeight;

        transform.position = new Vector3(
            basePosition.x * ratioX,
            basePosition.y * ratioY,
            basePosition.z
        );

        transform.localScale = new Vector3(
            baseScale.x * ratioX,
            baseScale.y * ratioY,
            baseScale.z
        );
    }
}