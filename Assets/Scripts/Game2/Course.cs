/*
作者名称:YHB

脚本作用:实现准星跟随鼠标

建立时间:2016.8.5.14.56
*/

using UnityEngine;
using System.Collections;

public class Course : MonoBehaviour
{
    #region 字段
    public GameObject target;
    public GameObject ball;
    public GameObject ballParent;

    //public Vector3[] paths;//Test用的

    private Vector3[] paths = new Vector3[3];//Test用的
    #endregion

    #region Unity内置函数
    void Awake()
    {

    }
    void Start()
    {

    }
    void Update()
    {
        RayMousePosition();
    }
    #endregion

    #region 射线检测
    void RayMousePosition()
    {
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(r, out hit))
        {
            if (hit.transform.tag == "p")
            {
                //这个只能在fixedupdate lateupdate update 或循环里面使用
                iTween.MoveUpdate(target, new Vector3(hit.point.x, 0.1f, hit.point.z), 0.3f);

                //点击
                if (Input.GetMouseButtonDown(0))
                {
                    GameObject go = Instantiate(ball, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

                    go.transform.parent = ballParent.transform;

                    //设置抛物线的3个点
                    paths[0] = new Vector3(0, 1, 0);//起点

                    //因为起点为000，所以加跟不加都一样
                    paths[1] = new Vector3(paths[2].x / 2f, 9f, paths[2].z / 2f);//抛物线中间的最高点
                    paths[2] = hit.point;//终点

                    //hash里面的表示 按照path路径来移动（路径是向量数组），movetopath表示按照路径来移动后面对应的是bool值
                    //orienttopath表示是否一开始就启用路径移动的方式，time是运动从头到尾的时间，easetype是运动的类型，这里是线性
                    iTween.MoveTo(go, iTween.Hash("path", paths, "movetopath", true, "orienttopath", true, "time", 1, "easetype", iTween.EaseType.linear));

                    //销毁ball
                    GameObject.Destroy(go, 3f);
                }
            }
        }
    }
    #endregion






    #region 画线---Test
    /* void OnDrawGizmos()
     {
         iTween.DrawLine(paths, Color.red);//画红色的直线
         iTween.DrawPath(paths, Color.blue);//话蓝色的抛物线
     }*/
    #endregion
}
