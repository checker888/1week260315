using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    private bool canMove = false;//ƒƒjƒ…[ŠJ‚¢‚Ä‚é‚Æ‚«‚È‚Ç‚Éfalse
    private bool hasLight = false;
    public bool CanMove
    {
        get => canMove;
        set => canMove = value;
    }

    public bool HasLight
    {
        get => hasLight;
        set => hasLight = value;

    }
    public GameObject visual;
    public Transform cameraPivot;


}
