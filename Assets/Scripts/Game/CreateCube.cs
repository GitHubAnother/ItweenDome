/*
作者名称:YHB

脚本作用:ItweenTest

建立时间:2016.8.5.12.28
*/

using UnityEngine;
using System.Collections;

public class CreateCube : MonoBehaviour
{
    #region 字段
    public GameObject cube;
    public GameObject ball;
    public int size = 9;
    public float DownTime = 0.6f;
    public float UpTime = 0.3f;
    public float UpDistance = 0.5f;

    private GameObject oldCube;//用来记录上一个cube
    private GameObject ballCube;//记录小球所在的那个cube
    private Vector3[] points = new Vector3[2];
    private int point;
    #endregion

    #region Unity内置函数
    void Start()
    {
        Create();
    }
    void Update()
    {
        RayMousePosition();
    }
    #endregion

    #region 射线检测鼠标的位置
    void RayMousePosition()
    {
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(r, out hit))
        {
            if (ballCube != null)//不等于空就是小球所在的下面的那个cube颜色变为红色，且不能抬起，射线检测没用了
            {
                iTween.MoveTo(ballCube, new Vector3(ballCube.transform.position.x, 0, ballCube.transform.position.z), 0.3f);
                iTween.ColorTo(ballCube, Color.red, 0.1f);
            }

            if (hit.transform.tag == "cube")
            {
                MoveUpColor(hit.transform.gameObject);

                if (Input.GetMouseButtonDown(0))//鼠标左键点击
                {
                    //这里是还原上一个ballCube的颜色
                    if (ballCube != null)//一定要判断，不然没有ballCube还原个毛呀
                    {
                        if ((ballCube.transform.position.x + ballCube.transform.position.z) % 2 == 0)
                        {
                            //变为黑色
                            iTween.ColorTo(ballCube, Color.black, DownTime);
                        }
                        else
                        {
                            //变为白色
                            iTween.ColorTo(ballCube, Color.white, DownTime);
                        }
                    }

                    //这一步是关键，得把小球的位置jilu下来，因为只要点击了一个cube，那么这个cube必然会成为ballCube
                    ballCube = hit.transform.gameObject;

                    point = 0;//现重置一下

                    //下面的0.5表示的是球在Y轴上的偏移值
                    points[0] = new Vector3(hit.transform.position.x, 0.5f, ball.transform.position.z);//现移动X轴向上的距离
                    points[1] = new Vector3(hit.transform.position.x, 0.5f, hit.transform.position.z);//在移动Z轴向上的距离

                    MoveToPoint();
                }
            }
        }
        else
        {
            //不判断上一个cube是否为空的话运行起来没问题，但是控制台会报一个空指针
            //因为一开始游戏的时候是没有上一个cube的，这样在下面使用oldCube的时候就会出现空指针，但是就运行效果来看正常的
            if (oldCube != null)
            {
                iTween.MoveTo(oldCube, new Vector3(oldCube.transform.position.x, 0, oldCube.transform.position.z), DownTime);

                //根据上一个cube的坐标位置回归颜色
                if ((oldCube.transform.position.x + oldCube.transform.position.z) % 2 == 0)
                {
                    //变为黑色
                    iTween.ColorTo(oldCube, Color.black, DownTime);
                }
                else
                {
                    //变为白色
                    iTween.ColorTo(oldCube, Color.white, DownTime);
                }
            }
        }
    }
    #endregion

    #region 生成cube
    void Create()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                GameObject go = Instantiate(cube, new Vector3(i, 0, j), Quaternion.identity) as GameObject;

                if ((i + j) % 2 == 0)
                {
                    iTween.ColorTo(go, Color.black, 0);//0表示直接变换过去
                }

                go.transform.parent = this.transform;
            }
        }
    }
    #endregion

    #region cube的向上移动和变色
    void MoveUpColor(GameObject go)
    {
        //下去
        if (oldCube != go && oldCube != null)//上一个cube不等于当前这个cube并且上一个cube不等于空
        {
            //下降上一个cube
            iTween.MoveTo(oldCube, new Vector3(oldCube.transform.position.x, 0, oldCube.transform.position.z), DownTime);

            //根据上一个cube的坐标位置回归颜色
            if ((oldCube.transform.position.x + oldCube.transform.position.z) % 2 == 0)
            {
                //变为黑色
                iTween.ColorTo(oldCube, Color.black, DownTime);
            }
            else
            {
                //变为白色
                iTween.ColorTo(oldCube, Color.white, DownTime);
            }
        }

        //上来
        if (go.transform.position.y == 0)//表示当前的cube在Y轴上没有移动
        {
            iTween.ColorTo(go, Color.blue, UpTime);
            iTween.MoveTo(go, new Vector3(go.transform.position.x, UpDistance, go.transform.position.z), UpTime);
            oldCube = go;//这样当前的cube就变为了下一个cube了
        }
    }
    #endregion

    #region 回调函数
    void MoveToPoint()
    {
        if (point < 2)
        {
            iTween.MoveTo(ball, iTween.Hash("position", points[point], "speed", 10f, "easetype", "linear",
                "oncomplete", "MoveToPoint",
                "oncompletetarget", this.gameObject));

            point++;
        }
    }
    #endregion
}
