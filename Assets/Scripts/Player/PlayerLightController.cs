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

    public AudioSource onSound;
    public AudioSource offSound;
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
            PutLight(playerCore.cameraPivot.position + playerCore.cameraPivot.forward * 0.3f, playerCore.cameraPivot.rotation, lightController.isShining);
        }
        if (input.takePressed && !playerCore.HasLight &&
            Physics.Raycast(playerCore.cameraPivot.position, playerCore.cameraPivot.forward,
            out RaycastHit hit, rayDistance))
        {
            GameObject obj = hit.collider.gameObject;
            if (obj.CompareTag("FlashLight"))
            {
                LightController flashlight_lc = obj.GetComponent<LightController>();
                SetFlashLight(true, flashlight_lc.isShining);

                // ① 先にSwitchBlock状態を引き継ぐ
                lightController.InheritSwitchState(flashlight_lc);

                // ② 設置物をisShining=falseにする前に、プレイヤー側のUpdateLightを即時実行
                flashlight_lc.isShining = false;
                flashlight_lc.isDynamic = true;

                // ③ 設置物のUpdateLightを呼ぶ前にプレイヤー側を更新してBarrierを消す
                lightController.UpdateLight();

                // ④ その後に設置物を処理（これでBarrierが復元されても即座に再消去済み）
                flashlight_lc.UpdateLight();

                Destroy(obj);
                playerFlashLightObj.SetActive(true);
                LightCheck();
            }
        }
        if (input.switchPressed && playerCore.HasLight)
        {
            lightController.isShining = !lightController.isShining;

            if (lightController.isShining)
            {
                onSound.Play();
            }
            else
            {
                offSound.Play();
            }
            LightCheck();
        }
    }

    public void PutLight(Vector3 pos, Quaternion rot, bool _isShining)
    {
        GameObject flashLight = Instantiate(flashLightOrigin, pos, rot);
        LightController flashlight_lc = flashLight.GetComponent<LightController>();
        flashlight_lc.isShining = _isShining;
        flashlight_lc.isDynamic = false;
        flashlight_lc.UpdateLight();
        SetFlashLight(false, false);

        flashlight_lc.lightObj.SetActive(flashlight_lc.isShining);

        playerFlashLightObj.SetActive(false);
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
