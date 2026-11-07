using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BladeScript : MonoBehaviour
{
    private float extdSpeed = 0.1F; // 1. 
    private bool isOn = true; // 2.
    private float scaleMin = 0; // 3. 
    private float scaleMax; // 4. Should be as large as the cylinder initially
    private float extendDelta; // 5. how much to scale per update
    private float scaleCurrent; // 6. current blade scale
    private float localScaleX; // 7.
    private float localScaleZ;
    public GameObject blade;

    [SerializeField] private AudioClip ignitionAudioClip;
    [SerializeField] private AudioClip closeAudioClip;
    private AudioSource ignitionAudioSource;
    private AudioSource closeAudioSource;

    private Light targetLight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Transform transform = GetComponent<Transform>();
        localScaleX = transform.localScale.x;
        localScaleZ = transform.localScale.z;
        scaleCurrent = scaleMax = transform.localScale.y;
        extendDelta = scaleMax / extdSpeed;
        isOn = true;

        ignitionAudioSource = GetComponent<AudioSource>();
        ignitionAudioSource.clip = ignitionAudioClip;
        closeAudioSource = GetComponent<AudioSource>();
        closeAudioSource.clip = closeAudioClip;

        targetLight = GetComponentInChildren<Light>();
        targetLight.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        // On Space, Make the cylinder grow in the opposite direction
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            extendDelta = isOn ? -Mathf.Abs(extendDelta) : Mathf.Abs(extendDelta);
            if (!isOn)
            {
                ignitionAudioSource.PlayOneShot(ignitionAudioClip);
                targetLight.enabled = true;
            } else
            {
                closeAudioSource.PlayOneShot(closeAudioClip);
                targetLight.enabled = false;
            }
        }

        scaleCurrent += extendDelta * Time.deltaTime;
        scaleCurrent = Mathf.Clamp(scaleCurrent, scaleMin, scaleMax); 
        transform.localScale = new Vector3(localScaleX, scaleCurrent, localScaleZ);
        isOn = scaleCurrent > 0;

        if (isOn && !blade.activeSelf)
        {
            blade.SetActive(true);
        } else if (!isOn && blade.activeSelf)
        {
            blade.SetActive(false);
        }
    }
}
