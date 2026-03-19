using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerLightController : MonoBehaviour
{
    public GameObject flashLightOrigin;
    public float rayDistance = 3.0f;
    private PlayerCore playerCore;
    private LightController lightController;
    private PlayerInputManager inputManager;

    public GameObject playerFlashLightObj;
    public GameObject lightObj;
    private void Start()
    {
        playerCore = GetComponent<PlayerCore>();
        lightController = GetComponent<LightController>();
        inputManager = GetComponent<PlayerInputManager>();
    }

    private void Update()
    {
        if (!playerCore.CanMove) return;
        MoveInput input = inputManager.CurrentInput;
        if (input.putPressed && playerCore.HasLight)
        {
            GameObject flashLight = Instantiate(flashLightOrigin, playerCore.cameraPivot.position + playerCore.cameraPivot.forward * 0.3f, playerCore.cameraPivot.rotation);
            LightController flashlight_lc = flashLight.GetComponent<LightController>();
            flashlight_lc.isShining = lightController.isShining;
            flashlight_lc.UpdateLight();
            SetFlashLight(false, false);
            
            flashlight_lc.lightObj.SetActive(flashlight_lc.isShining);
            
            playerFlashLightObj.SetActive(false);
        }
        if (input.takePressed && !playerCore.HasLight &&Physics.Raycast(playerCore.cameraPivot.position, playerCore.cameraPivot.forward, out RaycastHit hit, rayDistance) ){
            GameObject obj = hit.collider.gameObject;
            if (obj.CompareTag("FlashLight")){
                LightController flashlight_lc = obj.GetComponent<LightController>();
                SetFlashLight(true, flashlight_lc.isShining);
                flashlight_lc.isShining = false;
                flashlight_lc.UpdateLight();
                
                Destroy(obj);
                playerFlashLightObj.SetActive(true);
                LightCheck();
            }
        }
        if(input.switchPressed && playerCore.HasLight)
        {
            lightController.isShining = !lightController.isShining;
            LightCheck();
        }
    }
    void LightCheck()
    {
        if (lightController.isShining)
        {
            lightObj.SetActive(true);
        }
        else
        {
            lightObj.SetActive(false);
        }
    }

    // Update is called once per frame
    public void SetFlashLight(bool hasLight, bool isShining)
    {
        playerCore.HasLight = hasLight;
        lightController.isShining = isShining;
    }
}
