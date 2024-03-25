using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class maintains the player as a singelton and provides references to different player components.
/// </summary>
[RequireComponent(typeof(PlayerVisionController), typeof(PlayerInventory), typeof(SmellManager))]
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public const string PHYSICS_TAG = "PlayerPhysics";
    public const string GRAPHICS_TAG = "PlayerGraphics";

    [SerializeField] Transform playerFeet;
    [SerializeField] GameObject playerGraphics;
    [SerializeField] GameObject playerPhysics;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        } else
        {
            Debug.LogWarning("Player already exists in scene");
            Destroy(gameObject);
        }
    }

    public Transform GetPlayerFeet() {return playerFeet;}
    public GameObject GetPlayerGraphics() {return playerGraphics;}
    public GameObject GetPlayerPhysics() {return playerPhysics;}
    public PlayerVisionController GetVisionController() {return GetComponent<PlayerVisionController>();}
    public PlayerInventory GetInventory() {return GetComponent<PlayerInventory>();}
    public SmellManager GetSmellManager() {return GetComponent<SmellManager>();}
    public PlayerTextbook GetTextbook() {return GetComponent<PlayerTextbook>();}
    public PlayerMovement GetPlayerMovement() {return playerPhysics.GetComponent<PlayerMovement>();}
}
