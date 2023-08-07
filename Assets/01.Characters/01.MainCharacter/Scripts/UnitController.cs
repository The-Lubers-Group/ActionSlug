using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitController : MonoBehaviour
{
    [Header("Data")]
    public UnitInfoData data;
    
    [Header("Movement")]
    [SerializeField] private GameInput gameInput;
    public Rigidbody2D RB { get; private set; }
    private Vector2 moveInput;
    
    [Header("Health Settings")]
    private bool unitIsAlive;

    [Header("Ability Settings")]

    [Header("Animation Settings")]

    [Header("Audio Settings")]

    [Header("Camera Stuff")]
    [SerializeField] private CameraFollowObject cameraFollowObject;
    [SerializeField] private GameObject cameraFollowObjectGO;

    [Header("Debug")]
    public bool initializeSelf;

    private void Awake()
    {
        //RB = GetComponent<Rigidbody2D>();
        RB = GetComponentInChildren<Rigidbody2D>();
        //AnimHandler = GetComponent<PlayerAnimator>();
        cameraFollowObject = cameraFollowObjectGO.GetComponent<CameraFollowObject>();
    }

    private void Start()
    {
        if(initializeSelf)
        {
            SetAlive();
        }
    }

    private void Update()
    {
        moveInput = gameInput.getMovementVectorNormalized();

    }

    public void SetAlive()
    {
        unitIsAlive = true;
    }
}
