using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;
using UnityEditor;

public class FileIoC : MonoBehaviour {

    [MenuItem("科明插件/文件替换")]
    public static	void Text () {
        //修改
        //CopyFolder(@"H:\wjText\1", @"H:\wjText\4-62\TemplateData");
        List<string> fold1 = new List<string>(Directory.GetDirectories(@"H:\HHX_webgl_201117"));
        fold1.ForEach(f=> {
           string s=@"H:\HHX_webgl_201117" + @"\" + Path.GetFileName(f);
            List<string> fold = new List<string>(Directory.GetDirectories(s));
            fold.ForEach(c=> {
                string destPath = s + @"\"+Path.GetFileName(c) + @"\TemplateData";
               // Debug.Log(destPath);
                CopyFolder(@"H:\wjText\1", destPath);
            });
        });


        //List<string> fold = new List<string>(Directory.GetDirectories(@"H:\wjText\ceshi"));
        //fold.ForEach(c=> {
        //   string destPath=@"H:\wjText\ceshi\"+Path.GetFileName(c)+ @"\TemplateData";
        //    //CopyFolder(@"H:\wjText\1", destPath);
        //});
    }
	

    /// <summary>
    /// 复制文件夹所有文件
    /// </summary>
    /// <param name="sourcePath">源目录</param>
    /// <param name="destPath">目的目录</param>
    public static void CopyFolder(string sourcePath, string destPath)
    {
        if (Directory.Exists(sourcePath))
        {
            if (!Directory.Exists(destPath))
            {
                //目标目录不存在则创建
                try
                {
                    Directory.CreateDirectory(destPath);
                }
                catch (Exception ex)
                {
                    throw new Exception("创建目标目录失败：" + ex.Message);
                }
            }
            //获得源文件下所有文件
            List<string> files = new List<string>(Directory.GetFiles(sourcePath));
            files.ForEach(c =>
            {
                Debug.Log(destPath+ Path.GetFileName(c));
                string destFile = Path.Combine(destPath, Path.GetFileName(c));
                File.Copy(c, destFile, true);//覆盖模式
            });
            //获得源文件下所有目录文件
           // List<string> folders = new List<string>(Directory.GetDirectories(sourcePath));
            //folders.ForEach(c =>
            //{
            //    string destDir = Path.Combine(destPath, Path.GetFileName(c));
            //    //采用递归的方法实现
            //    CopyFolder(c, destDir);
            //});

        }
    }
    public void cd()
    {
        //新增测试方法
    }
}

