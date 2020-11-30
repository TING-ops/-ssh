using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static int num = 0;
    public static int numMax;
    public static string scenesName;
    /// <summary>
    /// 场景目录
    /// </summary>
    private static readonly string scenePath = "Screen";
    /// <summary>
    /// 文件目录
    /// </summary>
    static string pathX = @"H:\demo\";
    // Use this for initialization
    void Start()
    {
    }
    void Update()
    {

    }
    [MenuItem("科明插件/一键导出WebGl")]
    public static void SettingPlayer()
    {
        RefreshAllScene(true);
       // Debug.Log(numMax);
        string[] scenesNamseList = scenesName.Split('\\');
        string[] sceneNamesList_Right = scenesNamseList[scenesNamseList.Length - 1].Split('/');
        string sceneNamesLis_Right_Left = sceneNamesList_Right[sceneNamesList_Right.Length - 1].Replace(".unity", "");
        PlayerSettings.productName = sceneNamesLis_Right_Left;
     //   Debug.Log(sceneNamesLis_Right_Left.Split(' ')[0]);
        string path = pathX + sceneNamesLis_Right_Left.Split(' ')[0];                         //2-7 公差带图 取2-7作为目录
        //string path = pathX + (sceneNamesLis_Right_Left) + ".exe";                                                           //所有场景都在一个文件夹
        BuildPipeline.BuildPlayer(GetBuildScenes(), path, BuildTarget.WebGL, BuildOptions.None);
        if (num < numMax - 1)
        {
            num++;
            SettingPlayer();
        }
    }
    [MenuItem("科明插件/一键删除")]
    public static void RefreshAllScene()
    {
        // 设置场景 *.unity 路径
        string pathUnity = Path.Combine(Application.dataPath, scenePath);
        // 遍历获取目录下所有 .unity 文件
        string[] files = Directory.GetFiles(pathUnity, "*.unity", SearchOption.AllDirectories);//***H:/2020年项目/20200204机工出版社/BaiBan_xiaoguo-PC/Assets\Screen/0211\图9-9 行星轮系.unity***/
        for (int i = 0; i < files.Length; i++)
        {
            string[] ones = files[i].Split('/');               //  ***0211\图9-23 双波谐波齿轮传动.unity***  //
            string[] twos = ones[ones.Length - 1].Split('\\'); //  ***图9-23 双波谐波齿轮传动.unity  ***     //
            files[i] = pathX + twos[twos.Length - 1].Replace(".unity", "");//  ***图9-23 双波谐波齿轮传动  ***     //
        }
        for (int i = 0; i < files.Length; i++)
        {
            File.Delete(files[i] + @"\" + "player_win_x86.pdb");
            File.Delete(files[i] + @"\" + "player_win_x86_s.pdb");
        }
        print("删除完毕");
    }
    static string[] GetBuildScenes()
    {
        List<string> names = new List<string>();
        foreach (EditorBuildSettingsScene item in EditorBuildSettings.scenes)
        {
            if (item == null)
                continue;
            if (item.enabled)
            {
                names.Add(item.path);
            }
        }
        return names.ToArray();
    }


    // 添加菜单选项
    //[MenuItem("Tool/RefreshScene")]
    public static void RefreshAllScene(bool value)
    {
        // 设置场景 *.unity 路径
        string pathUnity = Path.Combine(Application.dataPath, scenePath);
        // 遍历获取目录下所有 .unity 文件
        string[] files = Directory.GetFiles(pathUnity, "*.unity", SearchOption.AllDirectories);
        // 定义 场景数组
        EditorBuildSettingsScene[] scenes = new EditorBuildSettingsScene[1];
        //将即将发布的场景赋值
        string scenePathFiles = files[num];
        // 通过scene路径初始化
        scenes[0] = new EditorBuildSettingsScene(scenePathFiles, value);
        numMax = files.Length;
        // 设置 scene 数组
        EditorBuildSettings.scenes = scenes;
        //导出路径      *****路径+场景名
        scenesName = files[num].ToString();
    }
}
