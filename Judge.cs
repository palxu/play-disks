
using UnityEngine;
using System.Collections;
using Com.Mygame;

public class Judge : MonoBehaviour
{
    public int oneDiskScore = 10;
    public int oneDiskFail = 10;
    public int disksToWin = 4;

    private SceneController scene;

    void Awake()
    {
        scene = SceneController.getInstance();
        scene.setJudge(this);
    }

    void Start()
    {
        scene.nextRound(); 
    }

    
    public void scoreADisk()
    {
        scene.setPoint(scene.getPoint() + oneDiskScore);
        if (scene.getPoint() == disksToWin * oneDiskScore)
        {
            scene.nextRound();
        }
    }

    
    public void failADisk()
    {
        scene.setPoint(scene.getPoint() - oneDiskFail);
    }
}

