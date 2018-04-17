打飞碟
===


## 编写一个简单的鼠标打飞碟（Hit UFO）游戏

*   游戏内容要求：
    1.  游戏有 n 个 round，每个 round 都包括10 次 trial；
    2.  每个 trial 的飞碟的色彩、大小、发射位置、速度、角度、同时出现的个数都可能不同。它们由该 round 的 ruler 控制；
    3.  每个 trial 的飞碟有随机性，总体难度随 round 上升；
    4.  鼠标点中得分，得分规则按色彩、大小、速度不同计算，规则可自由设定。
*   游戏的要求：
    *   使用带缓存的工厂模式管理不同飞碟的生产与回收，该工厂必须是场景单实例的！具体实现见参考资源 Singleton 模板类
    *   近可能使用前面 MVC 结构实现人机交互与游戏模型分离

![](https://img-blog.csdn.net/20160403202235357)

### 游戏预制
	 我的文件结构如下：
![](http://m.qpic.cn/psb?/V139yGGO35yTkp/UrXsfme96htRpvXdRoI4BmqyiNCQRRctNBMOdX8yUtQ!/b/dDIBAAAAAAAA&bo=5wCEAAAAAAADB0E!&rf=viewer_4&t=5)
	 
   
    预制总共有三个：
![](http://m.qpic.cn/psb?/V139yGGO35yTkp/Yvsr7x03l*ek7o0R9ZT2wvMgQFmB7MGAdJClDi4J0xE!/b/dDEBAAAAAAAA&bo=cAGnAAAAAAADB*Q!&rf=viewer_4&t=5)
	 
   
    首先做出bullet的预制：
![](http://m.qpic.cn/psb?/V139yGGO35yTkp/NmFr4lz1dfqdOLg9MRzauRbFTeRR4CrZOI5NYv5RyYg!/b/dEQBAAAAAAAA&bo=HgKSAgAAAAADB64!&rf=viewer_4&t=5)
	 
   
    然后是Disk的预制：
![](http://m.qpic.cn/psb?/V139yGGO35yTkp/7jqry3Av7.05wzMYE71qbfw.OWNcUDF8UFlEq.7nq*g!/b/dDMBAAAAAAAA&bo=HQKeAgAAAAADF7E!&rf=viewer_4&t=5)
	 
   
    最后是爆炸效果的预制：
![](http://m.qpic.cn/psb?/V139yGGO35yTkp/SELGn7GTgcCFvnI9HE1iJIlRCjGHDZUfadZ8qX8PdIs!/b/dDEBAAAAAAAA&bo=GQKYAgAAAAADF7M!&rf=viewer_4&t=5)
	 
   
    游戏对象列表如下：
![](http://m.qpic.cn/psb?/V139yGGO35yTkp/GTlWVcOh0MTcqhi2AsWotAVr*AU8Hcr9hAGVEL0wcg8!/b/dEEBAAAAAAAA&bo=4QCvAAAAAAADF3w!&rf=viewer_4&t=5)
	 
   
    创建三个text对象，按照下面的数据进行设置：
![](http://m.qpic.cn/psb?/V139yGGO35yTkp/mNwDrCJ8TK5W07prmf3R9hcv4tsHMuJ1TmVOfN*eoMw!/b/dEMBAAAAAAAA&bo=HQKdAgAAAAADF7I!&rf=viewer_4&t=5)
![](http://m.qpic.cn/psb?/V139yGGO35yTkp/3sImVGsordMt7rUiivLQplps0Qr4h.cOKyCdCDcVZI8!/b/dDABAAAAAAAA&bo=HQKWAgAAAAADF7k!&rf=viewer_4&t=5)
![](http://m.qpic.cn/psb?/V139yGGO35yTkp/42NcIk6XMSPNv7KjR5jetELM1U6z65Ti3DgWzV1wsAk!/b/dEABAAAAAAAA&bo=GQKiAgAAAAADF4k!&rf=viewer_4&t=5)
	 
   
    接下来就是代码的编写了：
![](http://m.qpic.cn/psb?/V139yGGO35yTkp/2VIqjuZ1qEnHBkghxq0cP5YmHDk50ORgZJAKAlYOwqM!/b/dEMBAAAAAAAA&bo=PAKnAAAAAAADF6s!&rf=viewer_4&t=5)
	

一、飞碟回收工厂类（DiskFactory）

        创建新的命名空间Com.Mygame，单例类DiskFactory和SceneController都定义其中。飞碟工厂类的目的是管理飞碟实例，同时对外屏蔽飞碟实例的的提取和回收细节，对于需要使用飞碟的其他对象，只能使用工厂类提供的3个函数，分别是getDisk()、getDiskObject()、free()。

```
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Com.Mygame;

namespace Com.Mygame
{
    public class DiskFactory : System.Object
    {
        private static DiskFactory _instance;
        private static List<GameObject> diskList;   
        public GameObject diskTemplate;            

        public static DiskFactory getInstance()
        {
            if (_instance == null)
            {
                _instance = new DiskFactory();
                diskList = new List<GameObject>();
            }
            return _instance;
        }

        
        public int getDisk()
        {
            for (int i = 0; i < diskList.Count; ++i)
            {
                if (!diskList[i].activeInHierarchy)
                {
                    return i;   
                }
            }
         
            diskList.Add(GameObject.Instantiate(diskTemplate) as GameObject);
            return diskList.Count - 1;
        }

      
        public GameObject getDiskObject(int id)
        {
            if (id > -1 && id < diskList.Count)
            {
                return diskList[id];
            }
            return null;
        }

        public void free(int id)
        {
            if (id > -1 && id < diskList.Count)
            {
                
                diskList[id].GetComponent<Rigidbody>().velocity = Vector3.zero;
                
                diskList[id].transform.localScale = diskTemplate.transform.localScale;
                diskList[id].SetActive(false);
            }
        }
    }
}

public class DiskFactoryBC : MonoBehaviour
{
    public GameObject disk;

    void Awake()
    {
        // 初始化预设对象
        DiskFactory.getInstance().diskTemplate = disk;
    }
}

```
二、用户界面类（UserInterface）

        用户界面有两大功能，一是处理用户键入，二是显示得分和倒计时等。用户键入有两种：鼠标左键和空格。左键发射子弹，空格发射飞碟。显示有三种：得分、回合和倒计时。

        子弹射击的思路：当用户点击鼠标时，从摄像机到鼠标创建一条射线，射线的方向即是子弹发射的方向，子弹采用刚体组件，因此发射子弹只需要给子弹施加一个力。子弹对象只有一个，下一次发射子弹时，必须改变子弹的位置（虽然有了刚体组件不建议修改transform，但也没有其它方法改变子弹位置了吧）。为了不让子弹继承上一次发射的速度，必须将子弹的速度归零重置。

        子弹的击中判断：采用射线而不是物理引擎，因为物理引擎在高速物体碰撞时经常不能百分百检测得到。

        发射飞碟的思路：调用用户接口。

        显示的思路：得分和回合直接通过查询接口获得。倒计时显示前通过查询接口判断是否正在倒计时，如果是，那么再通过查询接口获得倒计时时间。如果回合发生改变，则显示新的回合，直到用户按下空格。

```

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Com.Mygame;

public class UserInterface : MonoBehaviour
{
    public Text mainText;   
    public Text scoreText;   
    public Text roundText;  

    private int round;  

    public GameObject bullet;           
    public ParticleSystem explosion;    
    public float fireRate = .25f;       
    public float speed = 500f;         

    private float nextFireTime;         

    private IUserInterface userInt;    
    private IQueryStatus queryInt;     

    void Start()
    {
        bullet = GameObject.Instantiate(bullet) as GameObject;
        explosion = GameObject.Instantiate(explosion) as ParticleSystem;
        userInt = SceneController.getInstance() as IUserInterface;
        queryInt = SceneController.getInstance() as IQueryStatus;
    }

    void Update()
    {
        if (queryInt.isCounting())
        {
            
            mainText.text = ((int)queryInt.getEmitTime()).ToString();
        }
        else
        {
            if (Input.GetKeyDown("space"))
            {
                userInt.emitDisk();     
            }
            if (queryInt.isShooting())
            {
                mainText.text = "";     
            }
            // 发射子弹
            if (queryInt.isShooting() && Input.GetMouseButtonDown(0) && Time.time > nextFireTime)
            {
                nextFireTime = Time.time + fireRate;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    
                bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;                       
                bullet.transform.position = transform.position;                 
                bullet.GetComponent<Rigidbody>().AddForce(ray.direction * speed, ForceMode.Impulse);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag == "Disk")
                {
                    
                    explosion.transform.position = hit.collider.gameObject.transform.position;
                    explosion.GetComponent<Renderer>().material.color = hit.collider.gameObject.GetComponent<Renderer>().material.color;
                    explosion.Play();
                    
                    hit.collider.gameObject.SetActive(false);
                }
            }
        }
        roundText.text = "Round: " + queryInt.getRound().ToString();
        scoreText.text = "Score: " + queryInt.getPoint().ToString();
        
        if (round != queryInt.getRound())
        {
            round = queryInt.getRound();
            mainText.text = "Round " + round.ToString() + " !";
        }
    }
}


```
三、场景控制器类（SceneController）

        场景控制类主要实现接口定义和保存注入对象。另外它有两个私有变量round和point，分别记录游戏正在进行的回合，以及玩家目前的得分。

```

using UnityEngine;
using System.Collections;
using Com.Mygame;

namespace Com.Mygame
{
    public interface IUserInterface
    {
        void emitDisk();
    }

    public interface IQueryStatus
    {
        bool isCounting();
        bool isShooting();
        int getRound();
        int getPoint();
        int getEmitTime();
    }

    public interface IJudgeEvent
    {
        void nextRound();
        void setPoint(int point);
    }

    public class SceneController : System.Object, IQueryStatus, IUserInterface, IJudgeEvent
    {
        private static SceneController _instance;
        private SceneControllerBC _baseCode;
        private GameModel _gameModel;
        private Judge _judge;

        private int _round;
        private int _point;

        public static SceneController getInstance()
        {
            if (_instance == null)
            {
                _instance = new SceneController();
            }
            return _instance;
        }

        public void setGameModel(GameModel obj) { _gameModel = obj; }
        internal GameModel getGameModel() { return _gameModel; }

        public void setJudge(Judge obj) { _judge = obj; }
        internal Judge getJudge() { return _judge; }

        public void setSceneControllerBC(SceneControllerBC obj) { _baseCode = obj; }
        internal SceneControllerBC getSceneControllerBC() { return _baseCode; }

        // 操作接口
        public void emitDisk() { _gameModel.prepareToEmitDisk(); }

        // 查询接口
        public bool isCounting() { return _gameModel.isCounting(); }
        public bool isShooting() { return _gameModel.isShooting(); }
        public int getRound() { return _round; }
        public int getPoint() { return _point; }
        public int getEmitTime() { return (int)_gameModel.timeToEmit + 1; }

        // 得分接口
        public void setPoint(int point) { _point = point; }
        public void nextRound() { _point = 0; _baseCode.loadRoundData(++_round); }
    }



    public class SceneControllerBC : MonoBehaviour
    {
        private Color color;
        private Vector3 emitPos;
        private Vector3 emitDir;
        private float speed;

        void Awake()
        {
            SceneController.getInstance().setSceneControllerBC(this);
        }

        public void loadRoundData(int round)
        {
            switch (round)
            {
                case 1:     
                    color = Color.green;
                    emitPos = new Vector3(-2.5f, 0.2f, -5f);
                    emitDir = new Vector3(24.5f, 40.0f, 67f);
                    speed = 4;
                    SceneController.getInstance().getGameModel().setting(1, color, emitPos, emitDir.normalized, speed, 1);
                    break;
                case 2:     
                    color = Color.red;
                    emitPos = new Vector3(2.5f, 0.2f, -5f);
                    emitDir = new Vector3(-24.5f, 35.0f, 67f);
                    speed = 4;
                    SceneController.getInstance().getGameModel().setting(1, color, emitPos, emitDir.normalized, speed, 2);
                    break;
            }
        }
    }


}


```
五、游戏规则类（Judge）

        游戏规则单独作为一个类，有利于日后修改。这里需要处理的规则无非就两个，得分和失分。另外，得分需要判断是否能晋级下一关。能就调用接口函数nextRound()。

```

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


```
六、游戏场景类（GameModel）

        场景类是整个飞碟射击游戏的核心类，主要负责飞碟动作的处理。我是这样设计的：首先需要倒计时功能，可以通过几个整型变量和布尔变量完成。另外需要飞碟发射功能，通过setting函数保存好飞碟的发射信息，每次倒计时完成后，通过emitDisks获取飞碟对象，并通过发射信息初始化飞碟，再给飞碟一个力就可以发射了。而飞碟的回收在Update里完成，一种是飞碟被击中（飞碟不在场景中）了，需要调用Judge获得分数。另一种是飞碟在场景中，但是掉在地上了，需要调用Judge丢失分数。

```

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


```
将这5个代码拖入主摄像机然后按照下面的数据进行设置，将该拖入的预制拖入：
![](http://m.qpic.cn/psb?/V139yGGO35yTkp/w7357ZIlfJwIUrie1GsrMBZTdv1qGH31l0wYzxi*LZ0!/b/dEEBAAAAAAAA&bo=EQJSAwAAAAADF3A!&rf=viewer_4&t=5)
![](http://m.qpic.cn/psb?/V139yGGO35yTkp/4FC4vJWOUjjEiLqS4GM.pp3Vah6LNLRsfFsKFwg4k*0!/b/dDMBAAAAAAAA&bo=FwJNAQAAAAADB3s!&rf=viewer_4&t=5)

	下面是效果图：
![](http://m.qpic.cn/psb?/V139yGGO35yTkp/RRelBL1wodN8CCMDH.ahF8fXgvrRvGtnVHqfnj8S7LM!/b/dAQBAAAAAAAA&bo=qwNgAQAAAAADN9s!&rf=viewer_4&t=5)

试玩视频：[打飞碟](http://v.youku.com/v_show/id_XMzU0NTc2MzM2NA==.html?spm=a2hzp.8244740.0.0)
