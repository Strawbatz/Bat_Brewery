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
    [SerializeField] GameObject mainCamera;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public Transform GetPlayerFeet() {return playerFeet;}
    public GameObject GetPlayerGraphics() {return playerGraphics;}
    public GameObject GetPlayerPhysics() {return playerPhysics;}
    public PlayerVisionController GetVisionController() {return GetComponent<PlayerVisionController>();}
    public PlayerInventory GetInventory() {return GetComponent<PlayerInventory>();}
    public SmellManager GetSmellManager() {return GetComponent<SmellManager>();}
    public PlayerTextbook GetTextbook() {return GetComponent<PlayerTextbook>();}
    public PlayerMovement GetPlayerMovement() {return playerPhysics.GetComponent<PlayerMovement>();}
    public GameObject GetMainCamera(){return mainCamera;}

    /// <summary>
    /// Teleport the player to a specific point.
    /// </summary>
    /// <param name="position"></param>
    public void TeleportPlayer(Vector2 position)
    {
        playerGraphics.transform.position = position;
        playerPhysics.transform.position = position;
        mainCamera.transform.position = new Vector3(position.x, position.y, mainCamera.transform.position.z);
    }
}
