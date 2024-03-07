using System.Collections;
using System.Collections.Generic;
using AdvancedEditorTools.DataTypes;
using UnityEngine;
using UnityEngine.Tilemaps;
using AYellowpaper.SerializedCollections;

public class SoundManager : MonoBehaviour
{   
    [SerializedDictionary("Ground name", "Audio")]
    [SerializeField] SerializedDictionary<string, AudioClip> footstepSounds;
    [SerializeField] AudioClip defaultFootstepAudio;

    AudioSource footstepPlayer;
    Transform playerFeet;
    List<Tilemap> tilemaps = new List<Tilemap>();
    Grid grid;

    string previousGround = "";

    void Start()
    {
        footstepPlayer = GameObject.FindGameObjectWithTag("MainCamera").GetComponentInChildren<AudioSource>();
        playerFeet = GameObject.Find("PlayerFeet").transform;

        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
        Transform gridTrans = grid.transform;

        for(int i = 0; i < gridTrans.childCount; i++)
        {
            tilemaps.Add(gridTrans.GetChild(i).GetComponent<Tilemap>());
        }
    }

    void Update()
    {
        for(int i = tilemaps.Count-1; i >= 0; i--)
        {
            SiblingGroupTile tile = GetTile(tilemaps[i]);
            if(tile)
            {
                if(!previousGround.Equals(tile.siblingGroup))
                {
                    previousGround = tile.siblingGroup;
                    OnNewTile(tile);
                }
                return;
            }
        }
    }

    void OnNewTile(SiblingGroupTile tile)
    {
        Debug.Log(tile.siblingGroup);
        if(footstepSounds.ContainsKey(tile.siblingGroup))
        {
            footstepPlayer.clip = footstepSounds[tile.siblingGroup];
            footstepPlayer.Play();
        } else
        {
            footstepPlayer.clip = defaultFootstepAudio;
            footstepPlayer.Play();
        }
    }

    public SiblingGroupTile GetTile(Tilemap tilemap)
    {
        int posX = (int)((playerFeet.position.x+tilemap.transform.position.x)/grid.cellSize.x)-1;
        int posY = (int)((playerFeet.position.y+tilemap.transform.position.y)/grid.cellSize.y)-1;
        TileBase tile = tilemap.GetTile(new Vector3Int(posX,posY,0));
        var retTile = tile as SiblingGroupTile;
        return retTile;
    }
}
