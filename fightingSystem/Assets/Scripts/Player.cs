using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region 公有
    /// <summary>
    /// 移動速度
    /// </summary>
    [Header("移動速度")]
    public float speed;
    /// <summary>
    /// 取得範圍內的敵人
    /// </summary>
    [Header("取得範圍內的敵人")]
    public List<GameObject> _enemys;

    /// <summary>
    /// 範圍內的敵人數量
    /// </summary>
    [Header("範圍內的敵人數量")]
    public int _enemysCount;

    /// <summary>
    /// 怪物出招
    /// </summary>
    [Header("怪物出招")]
    public List<GameObject> _enemysSkill;
    #endregion
    #region 私域
    /// <summary>
    /// 是否正在移動
    /// </summary>
    bool isrun;
    /// <summary>
    /// 縱向移動
    /// </summary>
    float _v;
    /// <summary>
    /// 橫向移動
    /// </summary>
    float _h;

    /// <summary>
    /// 自身剛體
    /// </summary>
    Rigidbody2D _rd2d;
    /// <summary>
    /// Aim剛體
    /// </summary>
    Rigidbody2D _rd2dAim;
    /// <summary>
    /// 攝影機
    /// </summary>
    Camera _camera;

    /// <summary>
    /// 連線物件
    /// </summary>
    LineRenderer _lineRenderer;
    /// <summary>
    /// 起始點(角色)
    /// </summary>
    Transform _startLintPoint;
    /// <summary>
    /// 終點(目前目標)
    /// </summary>
    Transform _endLintPoint;

    Image _timing;

    Text _enemySkill;

    Text _playerSkill;

    GameObject _enemy;

    float _seconds;
    #endregion
    void Start()
    {
        _rd2d = GetComponent<Rigidbody2D>();
        _rd2dAim = GameObject.Find("Aim").gameObject.GetComponent<Rigidbody2D>();
        _camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        isrun = false;

        _timing = GameObject.Find("Timing").GetComponent<Image>();
        _enemySkill = GameObject.Find("對手").GetComponent<Text>();
        _playerSkill = GameObject.Find("Skill").GetComponent<Text>();
        _lineRenderer = GetComponent<LineRenderer>();
    }
    void FixedUpdate()
    {
        if (isrun)
        {
            _rd2d.velocity = new Vector2(_h * speed, _v * speed);
            _camera.transform.position = new Vector3(transform.position.x, transform.position.y, _camera.transform.position.z);
        }
        _startLintPoint = transform;



        Aim();
    }

    void Update()
    {

        _v = Input.GetAxis("Vertical");
        _h = Input.GetAxis("Horizontal");
        if (_v != 0 || _h != 0) isrun = true;

        skillKeyDow();

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemys"))
        {
            if (!_enemys.Contains(collision.gameObject))
                _enemys.Add(collision.gameObject);
        }
        ListIndexReset();
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (_enemys.Contains(collision.gameObject))
            _enemys.Remove(collision.gameObject);
        if (_enemysSkill.Contains(collision.gameObject))
            _enemysSkill.Remove(collision.gameObject);
        ListIndexReset();
    }

    /// <summary>
    /// 隨時更新範圍內的敵人數量
    /// </summary>
    void ListIndexReset()
    {
        _enemysCount = _enemys.Count;
    }
    /// <summary>
    /// 目標
    /// </summary>
    void Aim()
    {
        _lineRenderer.SetPosition(0, _startLintPoint.position);
        if (_enemysSkill.Count > 0)
        {
            _endLintPoint = _enemysSkill[0].transform;
            _lineRenderer.SetPosition(1, _endLintPoint.position);
            _rd2dAim.position = new Vector2(
                Mathf.Lerp(_rd2dAim.position.x, _endLintPoint.position.x, speed * Time.fixedDeltaTime),
                Mathf.Lerp(_rd2dAim.position.y, _endLintPoint.position.y, speed * Time.fixedDeltaTime));

            _enemy = _enemysSkill[0];
            timeOut(_enemy);
        }



        if (_enemysSkill.Count == 0)
        {
            _enemy = null;
            _rd2dAim.position = new Vector2(
                Mathf.Lerp(_rd2dAim.position.x, _startLintPoint.position.x, speed * Time.fixedDeltaTime),
                Mathf.Lerp(_rd2dAim.position.y, _startLintPoint.position.y, speed * Time.fixedDeltaTime));
            _lineRenderer.SetPosition(1, _startLintPoint.position);
        }
    }
    /// <summary>
    /// 新增被攻擊列表
    /// </summary>
    /// <param name="_go">攻擊的人</param>
    public void AddSkill(GameObject _go)
    {
        if (!_enemysSkill.Contains(_go))
            _enemysSkill.Add(_go);
    }
    /// <summary>
    /// 刪除攻擊角色
    /// </summary>
    /// <param name="_go">被刪除人</param>
    public void RemoveSkill(GameObject _go)
    {
        if (_enemysSkill.Contains(_go))
            _enemysSkill.Remove(_go);
    }

    /// <summary>
    /// 選擇招式
    /// </summary>
    public void skillKeyDow()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            changeEnemy(_enemy);
            _playerSkill.text = "攻擊";
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            changeEnemy(_enemy);
            _playerSkill.text = "防禦";
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            changeEnemy(_enemy);
            _playerSkill.text = "破防";
        }
    }
    /// <summary>
    /// 重置計時條
    /// </summary>
    void timingFillAmountReset()
    {
        _timing.fillAmount = 1;
    }
    /// <summary>
    /// 超時
    /// </summary>
    void timeOut(GameObject _go)
    {
        enemy _enemyScript;
        if (_go != null)
        {
            _seconds += Time.fixedDeltaTime;
            if (_seconds > 1f)
            {
                _timing.fillAmount -= 0.02f;
                if (_timing.fillAmount == 0)
                {
                    _enemyScript = _go.GetComponent<enemy>();
                    _enemyScript.SkillNow();
                    changeEnemy(_go);
                    timingFillAmountReset();
                }
            }
        }
    }
    /// <summary>
    /// 更換對象
    /// </summary>
    void changeEnemy(GameObject go)
    {
        if (_enemysSkill.Count > 0)
        {
            if (_enemys.Contains(go))
            {
                _enemysSkill.Add(go);
                timingFillAmountReset();
                _enemysSkill.RemoveAt(0);
            }
        }
    }
    /// <summary>
    /// 現在出招
    /// </summary>
    void skillNo(GameObject go)
    {
        enemy _enemyScript;
        if (go != null)
        {
            _enemyScript = go.GetComponent<enemy>();
            _enemyScript.SkillNow();
        }
    }
}
