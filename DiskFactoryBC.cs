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
