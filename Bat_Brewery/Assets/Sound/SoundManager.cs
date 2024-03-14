using System.Collections;
using System.Collections.Generic;
using AdvancedEditorTools.DataTypes;
using UnityEngine;
using UnityEngine.Tilemaps;
using AYellowpaper.SerializedCollections;
using AdvancedEditorTools.Attributes;
using UnityEditor;

/// <summary>
/// Controls the sound and music in the game scenes
/// </summary>
public class SoundManager : MonoBehaviour
{   
    #region Footstep sounds
    [BeginFoldout("Footstep Sounds")]
    [SerializedDictionary("Ground name", "Audio")]
    [SerializeField] SerializedDictionary<string, AudioSource> footstepSounds;
    [SerializeField] AudioSource defaultFootstepAudio;

    Transform playerFeet;
    List<Tilemap> tilemaps = new List<Tilemap>();
    Grid grid;
    AudioSource currentFootstepSource;
    PlayerMovement playerMovement;
    string previousGround = "";
    [EndFoldout]
    #endregion
    
    #region Drones
    [BeginFoldout("Drones")]
    [SerializeField] AudioSource[] droneSources;
    [SerializeField] float maxDroneTimer;
    float droneTimer;
    [EndFoldout]
    #endregion

    #region Background Music
    [BeginFoldout("Background Music")]
    [SerializeField] float maxVolume;
    [SerializeField] MusicZone[] musicZones =  new MusicZone[0];
    [SerializeField] float changeMusicTime;
    [EndFoldout]
    AudioSource currentMusic;
    float currentMaxVolume;
    #endregion


    void Start()
    {
        currentFootstepSource = defaultFootstepAudio;
        playerFeet = GameObject.Find("PlayerFeet").transform;

        playerMovement = FindAnyObjectByType<PlayerMovement>();

        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
        Transform gridTrans = grid.transform;

        for(int i = 0; i < gridTrans.childCount; i++)
        {
            tilemaps.Add(gridTrans.GetChild(i).GetComponent<Tilemap>());
        }

        defaultFootstepAudio.Play();
        defaultFootstepAudio.Pause();

        foreach(AudioSource source in footstepSounds.Values)
        {
            source.Play();
            source.Pause();
        }

        droneTimer = Random.Range(0, maxDroneTimer);
        foreach (MusicZone musicZone in musicZones)
        {
            musicZone.audioSource.volume = 0;
        }
        currentMusic = musicZones[0].audioSource;
        currentMaxVolume = maxVolume * musicZones[0].volumeScale;
        currentMusic.volume = currentMaxVolume;
    }

    void Update()
    {
        //Play drones sounds in the game
        droneTimer-=Time.deltaTime;
        if(droneTimer <= 0)
        {
            AudioSource droneSource = droneSources[Random.Range(0, droneSources.Length)];
            droneSource.Play();
            droneTimer = droneSource.clip.length + Random.Range(0, maxDroneTimer);
        }

        //Background music
        for (int i = 0; i < musicZones.Length; i++)
        {
            if(Utilities.InCircle(playerFeet.transform.position, (Vector2)transform.position+musicZones[i].centerOffset, musicZones[i].radius))
            {
                if(currentMusic != musicZones[i].audioSource) StartCoroutine(ChangeBackgroundMusic(musicZones[i].audioSource, musicZones[i].volumeScale));
                break;
            }
        }

        //Footstep sounds below this
        if(!playerMovement.IsMoving())
        {
            currentFootstepSource.Pause();
            return;
        }
        else if(!currentFootstepSource.isPlaying)
        {
            currentFootstepSource.UnPause();
        }

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

    /// <summary>
    /// Changes the footstep sound depending on what tile the player is on
    /// </summary>
    /// <param name="tile"></param>
    void OnNewTile(SiblingGroupTile tile)
    {
        if(footstepSounds.ContainsKey(tile.siblingGroup))
        {
            ChangeFootstepSource(footstepSounds[tile.siblingGroup]);
        } else
        {
            ChangeFootstepSource(defaultFootstepAudio);
        }
    }

    IEnumerator ChangeBackgroundMusic(AudioSource newMusic, float volumeScale)
    {
        AudioSource oldMusic = currentMusic;
        currentMusic = newMusic;
        float oldMaxVolume = currentMaxVolume;
        currentMaxVolume = maxVolume * volumeScale;
        float stepSize = .1f;
        while(currentMusic.volume < currentMaxVolume)
        {
            currentMusic.volume += currentMaxVolume*stepSize/changeMusicTime;
            oldMusic.volume -= oldMaxVolume*stepSize/changeMusicTime;

            if(currentMusic.volume >= currentMaxVolume)
            {
                currentMusic.volume = currentMaxVolume;
                oldMusic.volume = 0f;
            }

            yield return new WaitForSeconds(stepSize); 
        }

        yield return null;
    }

    /// <summary>
    /// Changes the footstep sound
    /// </summary>
    /// <param name="footstepSource"></param>
    private void ChangeFootstepSource(AudioSource footstepSource)
    {
        currentFootstepSource.Pause();
        currentFootstepSource = footstepSource;
        currentFootstepSource.UnPause();
    }

    /// <summary>
    /// Returns the current tile that the player is standing on
    /// </summary>
    /// <param name="tilemap"></param>
    /// <returns></returns>
    public SiblingGroupTile GetTile(Tilemap tilemap)
    {
        int posX = (int)(((playerFeet.position.x+tilemap.transform.position.x)/grid.cellSize.x));
        int posY = (int)(((playerFeet.position.y+tilemap.transform.position.y)/grid.cellSize.y));
        if(playerFeet.position.x < tilemap.transform.position.x) posX--;
        if(playerFeet.position.y < tilemap.transform.position.y) posY--;
        TileBase tile = tilemap.GetTile(new Vector3Int(posX,posY,0));
        var retTile = tile as SiblingGroupTile;
        return retTile;
    }

    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Handles.color = Color.yellow;
        for (int i = 0; i < musicZones.Length; i++)
        {
            Handles.DrawWireDisc(((Vector2)transform.position)+musicZones[i].centerOffset, Vector3.forward, musicZones[i].radius);
        }
    }
    #endif

    /// <summary>
    /// A small class for keeping track of the music zones
    /// </summary>
    [System.Serializable]
    struct MusicZone
    {
        public Vector2 centerOffset;
        public float radius;
        public AudioSource audioSource;
        public float volumeScale;
    }
}
