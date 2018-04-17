
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Com.Mygame;

public class GameModel : MonoBehaviour
{
    public float countDown = 3f;    
    public float timeToEmit;       
    private bool counting;          
    private bool shooting;          
    public bool isCounting() { return counting; }
    public bool isShooting() { return shooting; }

    private List<GameObject> disks = new List<GameObject>();    
    private List<int> diskIds = new List<int>();                
    private int diskScale;                  
    private Color diskColor;               
    private Vector3 emitPosition;           
    private Vector3 emitDirection;          
    private float emitSpeed;                
    private int emitNumber;                 
    private bool emitEnable;                

    private SceneController scene;

    void Awake()
    {
        scene = SceneController.getInstance();
        scene.setGameModel(this);
    }

    
    public void setting(int scale, Color color, Vector3 emitPos, Vector3 emitDir, float speed, int num)
    {
        diskScale = scale;
        diskColor = color;
        emitPosition = emitPos;
        emitDirection = emitDir;
        emitSpeed = speed;
        emitNumber = num;
    }

    
    public void prepareToEmitDisk()
    {
        if (!counting && !shooting)
        {
            timeToEmit = countDown;
            emitEnable = true;
        }
    }

 
    void emitDisks()
    {
        for (int i = 0; i < emitNumber; ++i)
        {
            diskIds.Add(DiskFactory.getInstance().getDisk());
            disks.Add(DiskFactory.getInstance().getDiskObject(diskIds[i]));
            disks[i].transform.localScale *= diskScale;
            disks[i].GetComponent<Renderer>().material.color = diskColor;
            disks[i].transform.position = new Vector3(emitPosition.x, emitPosition.y + i, emitPosition.z);
            disks[i].SetActive(true);
            disks[i].GetComponent<Rigidbody>().AddForce(emitDirection * Random.Range(emitSpeed * 5, emitSpeed * 10) / 10, ForceMode.Impulse);
        }
    }

    
    void freeADisk(int i)
    {
        DiskFactory.getInstance().free(diskIds[i]);
        disks.RemoveAt(i);
        diskIds.RemoveAt(i);
    }

    void FixedUpdate()
    {
        if (timeToEmit > 0)
        {
            counting = true;
            timeToEmit -= Time.deltaTime;
        }
        else
        {
            counting = false;
            if (emitEnable)
            {
                emitDisks(); 
                emitEnable = false;
                shooting = true;
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < disks.Count; ++i)
        {
            if (!disks[i].activeInHierarchy)
            {   
                scene.getJudge().scoreADisk();  
                freeADisk(i);
            }
            else if (disks[i].transform.position.y < 0)
            {   
                scene.getJudge().failADisk();   
                freeADisk(i);
            }
        }
        if (disks.Count == 0)
        {
            shooting = false;
        }
    }
}

