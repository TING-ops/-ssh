using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DrawManager : MonoBehaviour
{

    public LineRenderer Line;

    /// <summary>
    /// 恢复速度
    /// </summary>
    [SerializeField]
    [Range(10,50f)]
    private float recorverySpeed = 10;

    /// <summary>
    /// 最小检测距离，距离越小精度越好
    /// </summary>
    [SerializeField]
    [Range(0.01f, 0.3f)]
    private float minDetectedDistance = 0.2f;

    [SerializeField]
    [Range(0,0.5f)]
    private float lineWidth = 0.2f;

    private Transform lineFather;
    private LineRenderer currentLine;
    private Queue<LineRenderer> lineQueue = new Queue<LineRenderer>();
    void Awake()
    {
        lineFather = GameObject.Find("Lines").transform;
    }

    void Start()
    {
     
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateLine();
        }
        else if (Input.GetMouseButton(0) && currentLine)
        {
            ControlLine();
        }

        if (Input.GetMouseButton(1))
        {
            ClearLine();
        }
    }

    private void CreateLine()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5));
        currentLine = Instantiate(Line,lineFather);
        currentLine.name = "Line";
        lineQueue.Enqueue(currentLine);
        SetLinePosition(pos);
        currentLine.startWidth = lineWidth;
        currentLine.endWidth = lineWidth;
    }

    private Vector3 lastPos;
    private void ControlLine()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,5));
        if(Vector3.Distance(lastPos,pos) >= minDetectedDistance)
        {
            SetLinePosition(pos);
            lastPos = pos;
        }

    }
    private void ClearLine()
    {
        currentLine = null;
        StartCoroutine(BackwardRecorvery());
    }

    private LineRenderer lineTemp = null;
    private List<Vector3> positionList = new List<Vector3>();
    IEnumerator ForwardRecorvery()
    {
        if (lineTemp == null && lineQueue.Count > 0)
        {

            lineTemp = lineQueue.Dequeue();
            int count = lineTemp.positionCount;

            for (int i = 0; i < lineTemp.positionCount; i++)
            {
                positionList.Add(lineTemp.GetPosition(i));
            }

            float timer = 0;
            while (lineTemp.positionCount > 0)
            {
                if (timer < 0.2f)
                {
                    timer += Time.fixedDeltaTime * recorverySpeed;
                }
                else
                {
                    if (count > 0)
                    {
                        positionList.RemoveAt(0);
                        lineTemp.positionCount = positionList.Count;
                        for (int i = 0; i < positionList.Count; i++)
                        {
                            lineTemp.SetPosition(i, positionList[i]);
                        }
                    }
                    timer = 0;
                }
                yield return null;
            }

            lineTemp = null;
            StartCoroutine(ForwardRecorvery());

        }

    }
    IEnumerator BackwardRecorvery()
    {
        if (lineTemp == null && lineQueue.Count > 0)
        {

            lineTemp = lineQueue.Dequeue();
            int count = lineTemp.positionCount;

            float timer = 0;
            while (lineTemp.positionCount > 0)
            {
                if (timer < 0.2f)
                {
                    timer += Time.fixedDeltaTime * recorverySpeed;
                }
                else
                {
                    if (count > 0)
                    {
                        lineTemp.positionCount = --count;
                    }
                    else
                    {
                        lineTemp.positionCount = 0;
                    }
                    timer = 0;
                }
                yield return null;
            }

            lineTemp.startWidth = 0;
            lineTemp.endWidth = 0;
            lineTemp = null;
            StartCoroutine(BackwardRecorvery());

        }

    }
    private void SetLinePosition(Vector3 pos)
    {
        int positionCount = currentLine.positionCount;
        currentLine.positionCount += 1;
        currentLine.SetPosition(positionCount, pos);
    }

}