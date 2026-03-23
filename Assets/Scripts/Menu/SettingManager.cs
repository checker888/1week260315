using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    
    public Button settingsButtton;
    public Button applyAndBackButton;
    public Slider BGMSlider;
    public Slider SESlider;
    public Slider sensitivitySlider;
    public Slider FOVSlider;
    public Toggle crosshairToggle;

    private float tempBGM;
    private float tempSE;
    private float tempSensitivity;
    private float tempFOV;
    private int   tempCrosshair;


    private MenuViewManager menuViewManager;
    private AudioManager audioManager;
    private PlayerSwitchManager playerSwitchManager;
    private PlayerLook playerLook;
    private CameraManager cameraManager;
    private CrosshairManager crosshairManager;
    // Start is called before the first frame update
    void Start()
    {
        settingsButtton.onClick.AddListener(PushSettingsButton);
        applyAndBackButton.onClick.AddListener(ApplyAndBackHome);

        menuViewManager = GetComponent<MenuViewManager>();
        audioManager = GetComponent<AudioManager>();
        playerSwitchManager = GetComponent<PlayerSwitchManager>();
        if(playerSwitchManager != null)playerLook = playerSwitchManager.firstPlayerObj.GetComponent<PlayerLook>();
        cameraManager = GetComponent<CameraManager>();
        crosshairManager = GetComponent<CrosshairManager>();

        tempBGM = PlayerPrefs.GetFloat("BGM", -40f);
        tempSE = PlayerPrefs.GetFloat("SE", -40f);
        tempSensitivity = PlayerPrefs.GetFloat("Sensitivity", 2f);
        tempFOV = PlayerPrefs.GetFloat("Sensitivity", 90f);
        tempCrosshair = PlayerPrefs.GetInt("Crosshair", 1);

        BGMSlider.minValue = -80f;
        BGMSlider.maxValue = 0f;
        SESlider.minValue = -80f;
        SESlider.maxValue = 0f;
        sensitivitySlider.minValue = 0.01f;
        sensitivitySlider.maxValue = 2f;
        FOVSlider.minValue = 60f;
        FOVSlider.maxValue = 110f;


        BGMSlider.onValueChanged.AddListener(ChangeBGM);
        SESlider.onValueChanged.AddListener(ChangeSE);
        sensitivitySlider.onValueChanged.AddListener(ChangeSensitivity);
        FOVSlider.onValueChanged.AddListener(ChangeFOV);
        crosshairToggle.onValueChanged.AddListener(ChangeCrosshair);

        BGMSlider.value = tempBGM;
        SESlider.value = tempSE;
        sensitivitySlider.value = tempSensitivity;
        FOVSlider.value = tempFOV;
        crosshairToggle.isOn = tempCrosshair == 1;



    }

    public void ChangeBGM(float value)
    {
        tempBGM = value;
        audioManager.SetBGMVolume(tempBGM);
    }
    public void ChangeSE(float value)
    {
        tempSE = value;
        audioManager.SetSEVolume(tempSE);
    }
    public void ChangeSensitivity(float value)
    {
        tempSensitivity = value;
        if(playerLook != null)playerLook.sensitivity = tempSensitivity;
    }
    public void ChangeFOV(float value)
    {
        tempFOV = value;
        if(cameraManager != null)cameraManager.mainCamera.fieldOfView = tempFOV;
    }
    public void ChangeCrosshair(bool value)
    {
        tempCrosshair = value ? 1 : 0;
        if (crosshairManager != null) crosshairManager.SetState(tempCrosshair);
    }

    public void ApplyAndBackHome()
    {
        
        PlayerPrefs.SetFloat("BGM", BGMSlider.value);
        PlayerPrefs.SetFloat("SE", SESlider.value);
        PlayerPrefs.SetFloat("Sensitivity", sensitivitySlider.value);
        PlayerPrefs.SetFloat("FOV", FOVSlider.value);
        PlayerPrefs.SetInt("Crosshair", crosshairToggle.isOn ? 1 : 0);
        menuViewManager.SetMenu("Home");
    }

    public void PushSettingsButton()
    {
        menuViewManager.SetMenu("Settings");
    }
}
