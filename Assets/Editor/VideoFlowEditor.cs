using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class VideoFlowEditor : EditorWindow
{
    //Data class for each node window
    class Node
    {
        int m_id;
        VideoNode m_data;
        Rect m_rect;

        public Node(int id, VideoNode data, Rect rect)
        {
            m_id = id;
            m_data = data;
            m_rect = rect;
        }

        public int GetId()
        {
            return m_id;
        }

        public Rect GetRect()
        {
            return m_rect;
        }

        public Vector3 GetUpperPoint()
        {
            return new Vector3(m_rect.x + m_rect.width / 2, m_rect.y, 0);
        }

        public Vector3 GetBottomPoint()
        {
            return new Vector3(m_rect.x + m_rect.width / 2, m_rect.y + m_rect.height, 0);
        }

        public Vector3 GetLeftPoint()
        {
            return new Vector3(m_rect.x, m_rect.y + m_rect.height / 2, 0);
        }

        public Vector3 GetRightPoint()
        {
            return new Vector3(m_rect.x + m_rect.width, m_rect.y + m_rect.height / 2, 0);
        }
    }

    VideoNode m_startNode = null;

    Rect window1;
    Rect window2;

    const float SPACING_X = 30;
    const float SPACING_Y = 30;
    const float START_X = 20;

    const float ELEMENT_HEIGHT = 100;
    const float ELEMENT_WIDTH = 100;


    [MenuItem("Tool/Video Flow Editor")]
    static void ShowEditor()
    {
        VideoFlowEditor editor = EditorWindow.GetWindow<VideoFlowEditor>();
        editor.Init();

        
    }

    public void Init()
    {
        this.maximized = true;

        float startY = this.maxSize.y / 2;

        VideoNode startNode = AssetDatabase.LoadAssetAtPath<VideoNode>("Assets/Data/Video1.asset");
        Queue<VideoNode> list = new Queue<VideoNode>();
        list.Enqueue(startNode);

        int count = 0; 

        while(list.Count > 0)
        {
            VideoNode node = list.Dequeue();
            window1 = new Rect(10, 10, 100, 100);
        }


        
    }

    void OnGUI()
    {
        DrawNodeCurve(window1, window2); // Here the curve is drawn under the windows

        BeginWindows();




        window1 = GUI.Window(1, window1, DrawNodeWindow, "Window 1");   // Updates the Rect's when these are dragged
        window2 = GUI.Window(2, window2, DrawNodeWindow, "Window 2");
        EndWindows();
    }

    void DrawNodeWindow(int id)
    {
        GUI.DragWindow();
    }

    void DrawNodeCurve(Rect start, Rect end)
    {
        Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
        Vector3 startTan = startPos + Vector3.right * 50;
        Vector3 endTan = endPos + Vector3.left * 50;
        Color shadowCol = new Color(0, 0, 0, 0.06f);
        for (int i = 0; i < 3; i++) // Draw a shadow
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
    }
}
