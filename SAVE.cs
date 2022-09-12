using System.Collections.Generic;
using UnityEngine;

namespace SAVE
{
    public class GameController : MonoBehaviour
    {
        #region 資料區
        public float totalTime { get; private set; }
        public static int clickNumber { get; private set; }
        int dead;
        public static int power;
        public SaveDate saveDate;
        SaveDate loadDate;
        ObjectController objectController;
        public static List<Weapon> weapons = new List<Weapon>();
        #endregion
        #region 讀存取
        /// <summary>
        /// 載入
        /// </summary>
        /// <param name="name">檔名</param>
        public void Loading(string name)
        {
            if (name == "") PlayerPrefs.DeleteKey("");
            var a = PlayerPrefs.GetString(name, "");
            if (a != "")
            {
                totalTime = PlayerPrefs.GetFloat("time" + name, 0);
                loadDate = JsonUtility.FromJson<SaveDate>(a);
                clickNumber = loadDate.clickNumber;
                dead = loadDate.dead;
                power = loadDate.power;
                saveDate.crystal = loadDate.crystal;
                saveDate.CrystalLevel = loadDate.CrystalLevel;
                saveDate.coin = loadDate.coin;
                saveDate.cookie = loadDate.cookie;
                saveDate.note = loadDate.note;
                saveDate.monsterNumber = loadDate.monsterNumber;
                saveDate.weapon = loadDate.weapon;
                weapons.Clear();
                foreach (var item in saveDate.weapon)
                {
                    weapons.Add(Weapon.weapons[item]);
                }
                objectController.Show(loadDate.showObject);
                Player.transform.position = loadDate.position;
                Camera.transform.position = loadDate.position;
                saveDate.position = loadDate.position;
            }
            else
            {
                Player.transform.position = new Vector2(-40.7f, 2);
                clickNumber = 0;
                totalTime = 0;
                power = 5;
                saveDate.weapon = new List<int>();
                weapons.Clear();
                saveDate.CrystalLevel = new List<int> { 0 };
                saveDate.crystal = 0;
                saveDate.coin = 0;
                saveDate.cookie = 0;
            }
        }
        /// <summary>
        /// 存檔
        /// </summary>
        /// <param name="name">檔名</param>
        public void Saving(string name)
        {
            saveDate.coin = GetComponent<BackBag>().coin;
            saveDate.cookie = GetComponent<BackBag>().cookie;
            saveDate.CrystalLevel = GetComponent<BackBag>().CrystalLevel;
            saveDate.crystal = GetComponent<BackBag>().crystal;
            saveDate.clickNumber = clickNumber;
            saveDate.power = power;
            saveDate.dead = dead;
            saveDate.weapon.Clear();
            foreach (var item in weapons)
            { saveDate.weapon.Add(item.No); }
            saveDate.showObject = objectController.Write();
            if (saveDate.position == Vector2.zero) saveDate.position = new Vector2(-40.7f, 2);
            PlayerPrefs.SetString(name, JsonUtility.ToJson(saveDate));
            PlayerPrefs.SetString("choose", name);
            PlayerPrefs.SetFloat("time" + name, totalTime);
            PlayerPrefs.Save();
            print(JsonUtility.ToJson(saveDate));
        }
        #endregion
        ///遊戲主程式
    }
    public class SaveDate : Item
    {
        public Vector2 position;
        public int clickNumber;
        public string note;
        public int dead;
        public int power;
        public int monsterNumber;
        public int showObject;
    }
    public class Item
    {
        public int coin = 0;
        public int cookie = 0;
        public List<int> weapon;//0裝備
        public int crystal;//小精靈編號
        public List<int> CrystalLevel;//小精靈等級
    }
    public class BackBag : MonoBehaviour
    {
        #region 資料區
        public int coin;
        public int cookie;
        public int crystal;
        public List<int> CrystalLevel;
        private void Awake()
        {
            coin = GetComponent<GameController>().saveDate.coin;
            cookie = GetComponent<GameController>().saveDate.cookie;
            crystal = GetComponent<GameController>().saveDate.crystal;
            CrystalLevel = GetComponent<GameController>().saveDate.CrystalLevel;
        }
        #endregion
        ///其他道具欄方法
    }
    public class ObjectController : MonoBehaviour
    {
        public GameObject g1;
        public GameObject g2;
        public GameObject g3;
        public GameObject g4;
        public GameObject g5;
        public GameObject g6;
        public GameObject g7;
        public GameObject g8;
        public GameObject g9;
        public GameObject g10;
        public GameObject g11;
        public GameObject g12;
        public GameObject g13;
        public GameObject g14;
        public GameObject g15;
        /// <summary>
        /// 物件生成
        /// </summary>
        /// <param name="a">生成碼</param>
        public void Show(int a)
        {
            _Bool(g1, a % 2);
            _Bool(g2, a / 2 % 2);
            _Bool(g3, a / 4 % 2);
            _Bool(g4, a / 8 % 2);
            _Bool(g5, a / 16 % 2);
            _Bool(g6, a / 32 % 2);
            _Bool(g7, a / 64 % 2);
            _Bool(g8, a / 128 % 2);
            _Bool(g9, a / 256 % 2);
            _Bool(g10, a / 512 % 2);
            _Bool(g11, a / 1024 % 2);
            _Bool(g12, a / 2048 % 2);
            _Bool(g13, a / 4096 % 2);
            _Bool(g14, a / 8192 % 2);
            _Bool(g15, a / 16384 % 2);
        }
        /// <summary>
        /// 紀錄物件
        /// </summary>
        /// <returns>生成碼</returns>
        public int Write()
        {
            return _Int(g1) +
                2 * _Int(g2) +
                4 * _Int(g3) + 
                8 * _Int(g4) +
                16 * _Int(g5) + 
                32 * _Int(g6) + 
                64 * _Int(g7) + 
                128 * _Int(g8) + 
                256 * _Int(g9) + 
                512 * _Int(g10) + 
                1024 * _Int(g11) + 
                2048 * _Int(g12) + 
                4096 * _Int(g13) + 
                8192 * _Int(g14) + 
                16384 * _Int(g15);
        }
        /// <summary>
        /// 紀錄
        /// </summary>
        /// <param name="a">物件</param>
        /// <returns>0：不在，1：在</returns>
        int _Int(GameObject a)
        {
            bool b;
            if (a != null)
            {
                if (a.tag == "chest") b = a.GetComponent<Chest>().opened;
                else b = a;
                if (b) return 1;
                else return 0;
            }
            else return 0;
        }
        /// <summary>
        /// 讀取
        /// </summary>
        /// <param name="a">物件</param>
        /// <param name="b">>0：不在，1：在</param>
        void _Bool(GameObject a, int b)
        {
            if (a != null)
            {
                if (a.tag == "chest")
                {
                    if (b == 1)
                        a.GetComponent<Chest>().opened = true;
                    if (b == 0)
                        a.GetComponent<Chest>().opened = false;
                }
                else
                {
                    if (b == 0)
                        Destroy(a);
                }
            }
        }
    }
}
