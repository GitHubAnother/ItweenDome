/*
作者名称:YHB

脚本作用:用来切换场景

建立时间:2016.8.5.14.25
*/

using UnityEngine;
using System.Collections;

public class LoadScene : MonoBehaviour
{
    public void LoadScenes()
    {
        if (Application.loadedLevel == 0)
        {
            Application.LoadLevel(1);
        }
        else
        {
            Application.LoadLevel(0);
        }
    }
}
