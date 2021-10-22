//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using System.IO;
//using System.Text.RegularExpressions;

//public class ResearchTreeEditor : EditorWindow
//{
//    //Data class for each node window
//    class ResearchNode
//    {
//        int m_id;
//        ResearchConfig m_config;
//        Rect m_rect;

//        public ResearchNode(int id, ResearchConfig config, Rect rect)
//        {
//            m_id = id;
//            m_config = config;
//            m_rect = rect;
//        }

//        public int GetId()
//        {
//            return m_id;
//        }

//        public ResearchConfig GetConfig()
//        {
//            return m_config;
//        }

//        public Rect GetRect()
//        {
//            return m_rect;
//        }

//        public Vector3 GetUpperPoint()
//        {
//            return new Vector3(m_rect.x + m_rect.width / 2, m_rect.y, 0);
//        }

//        public Vector3 GetBottomPoint()
//        {
//            return new Vector3(m_rect.x + m_rect.width / 2, m_rect.y + m_rect.height, 0);
//        }

//        public Vector3 GetLeftPoint()
//        {
//            return new Vector3(m_rect.x , m_rect.y + m_rect.height / 2, 0);
//        }

//        public Vector3 GetRightPoint()
//        {
//            return new Vector3(m_rect.x + m_rect.width, m_rect.y + m_rect.height / 2, 0);
//        }
//    }

//    const string RESEARCH_TREE_PATH = "Assets/Config/Researches/ResearchTree/";
//    const string RESEARCH_SECTION_PATH = "Config/Researches/ResearchSection/";
//    const string RESEARCH_PATH = "Config/Researches/Research/";

//    const float ELEMENT_HEIGHT = 100;
//    const float ELEMENT_WIDTH = 130;
//    const float SPACING_X = 30;
//    const float SPACING_Y = 30;
//    const float START_X = 20;
//    const float START_Y = 40;
//    const float RESEARCH_ICON_HEIGHT = 60;
//    const float RESEARCH_ICON_WIDTH = 60;

//    Texture2D backgroundTexture;
//    Color backgroundColor = new Color(0.72f, 0.72f, 0.72f);

//    ResearchTreeConfig m_config = null;
//    Dictionary<int, ResearchNode> m_researchNodeDict = new Dictionary<int, ResearchNode>(); 
//    Dictionary<int, ResearchSectionConfig> m_researchSection = new Dictionary<int, ResearchSectionConfig>(); 
//    int m_selectedId = -1;
//    int m_draggedEndId = -1;

//    Editor m_inspectorEditor;
//    Rect m_inspectorRect;
//    Vector2 m_inspectorScrollView;

//    bool m_isDragging;

//    [MenuItem("Cyberdew/Research Tree Editor")]
//    static void ShowEditor()
//    {
//        ResearchTreeEditor window = (ResearchTreeEditor)EditorWindow.GetWindow(typeof(ResearchTreeEditor), true, "Research Tree Editor");
//        window.Init();

//        FocusWindowIfItsOpen<ResearchTreeEditor>();
//        window.ShowPopup();
//    }

//    public void Init()
//    {
//        m_inspectorRect = new Rect(START_X + 7 * (ELEMENT_WIDTH + SPACING_X) + SPACING_X, START_Y, 400, 850 - START_Y);
//        m_inspectorScrollView = new Vector2(0, 0);
//        this.minSize = new Vector2(1600, 950);
//        this.maxSize = new Vector2(1700, 1000);

//        backgroundTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
//        backgroundTexture.SetPixel(0, 0, backgroundColor);
//        backgroundTexture.Apply();
//    }

//    void OnGUI()
//    {
//        //GUI.DrawTexture(new Rect(0, 0, maxSize.x, maxSize.y), backgroundTexture, ScaleMode.StretchToFill);
//        MouseEvent();

//        GUILayout.Space(10);
//        GUILayout.BeginHorizontal();
//        GUILayout.Space(START_X);

//        AddTreeDropdown();
//        AddCreateTreeButton();

//        if (m_config != null)
//        {
//            if (m_selectedId != -1 && m_researchNodeDict[m_selectedId].GetConfig().GetResearchType() == ResearchType.Section)
//            {
//                AddRemoveSectionButton();
//            }
            
//            if (m_selectedId != -1 && m_researchNodeDict[m_selectedId].GetConfig().GetResearchType() != ResearchType.Section)
//            {
//                AddRemoveResearchButton();
//            }

//            if (m_selectedId != -1 && UnityEngine.Event.current.type == EventType.MouseDown && UnityEngine.Event.current.button == 1)
//            {
//                OpenContextMenu(UnityEngine.Event.current.mousePosition);
//            }
//        }

//        GUILayout.EndHorizontal();

//        if (m_config != null)
//        {
//            BeginWindows();
//            m_researchNodeDict.Clear();
//            m_researchSection.Clear();
//            DrawTree();
//            DrawResearchLines();
//            DrawInspector();
//            EndWindows();
//        }
//    }

//    void MouseEvent()
//    {
//        if (Event.current.type == EventType.MouseDown)
//        {
//            //Debug.Log("MouseDown: " + Event.current.mousePosition.ToString());
//        }

//        if (Event.current.type == EventType.MouseDrag)
//        {
//            m_isDragging = true;
//            m_draggedEndId = GetControlFromPosition(Event.current.mousePosition);
//        }

//        if (Event.current.type == EventType.MouseUp)
//        {
//            m_isDragging = false;
//            m_draggedEndId = GetControlFromPosition(Event.current.mousePosition);

//            if (m_selectedId != -1 && m_draggedEndId != -1 && m_draggedEndId != m_selectedId)
//            {
//                ResearchConfig inConfig = m_researchNodeDict[m_draggedEndId].GetConfig();
//                inConfig.AddDeps(m_researchNodeDict[m_selectedId].GetConfig().GetID());
//                EditorUtility.SetDirty(inConfig);
//            }

//            m_draggedEndId = -1;
//        }
//    }

//    int GetControlFromPosition(Vector2 position)
//    {
//        foreach(ResearchNode node in m_researchNodeDict.Values)
//        {
//            if(node.GetRect().Contains(position))
//            {
//                return node.GetId();
//            }
//        }

//        return -1;
//    }

//    void AddTreeDropdown()
//    {
//        string selectedTreeTitle = m_config ? m_config.name : "Select Tree Here";
//        EditorGUI.BeginChangeCheck();
//        m_config = GUITools.PopupButtonLayout<ResearchTreeConfig>(m_config, new GUIContent(selectedTreeTitle), ResearchTreeConfigDatabase.GetItems(), x => x.name, GUICustomStyles.popupButtonCenterText, GUILayout.Width(130));
//        if (EditorGUI.EndChangeCheck())
//        {
//            m_selectedId = -1;
//        }
//    }

//    void AddCreateTreeButton()
//    {
//        if (GUILayout.Button("Create New Tree", GUILayout.Width(150)))
//        {
//            string fileName = Path.GetFileName(EditorUtility.SaveFilePanel("Create a new research tree", "Assets/Config/Researches/ResearchTree/", "New Research Tree", "asset"));
//            if (!string.IsNullOrEmpty(fileName))
//            {
//                ResearchTreeConfig newTreeConfig = ScriptableObject.CreateInstance<ResearchTreeConfig>();
//                AssetDatabase.CreateAsset(newTreeConfig, "Assets/Config/Researches/ResearchTree/" + fileName);
//                AssetDatabase.Refresh();

//                m_config = newTreeConfig;

//                m_selectedId = -1;
//            }
//        }
//    }

//    void AddRemoveSectionButton()
//    {
//        if (GUILayout.Button("Remove Section", GUILayout.Width(150)))
//        {
//            ResearchSectionConfig config = m_researchSection[m_selectedId];

//            if (ValidateRemoveSection(config))
//            {
//                m_selectedId = -1;

//                List<ResearchConfig> listForRemove = config.ClearSection();

//                for (int i = listForRemove.Count - 1; i >= 0; i--)
//                {
//                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(listForRemove[i]));
//                }

//                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(config.GetSectionResearch()));

//                m_config.GetSections().Remove(config);
//                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(config));
//            }
//        }
//    }

//    void AddRemoveResearchButton()
//    {
//        if (GUILayout.Button("Remove Research", GUILayout.Width(150)))
//        {
//            ResearchConfig config = m_researchNodeDict[m_selectedId].GetConfig();

//            if (ValidateRemoveResearch(config))
//            {
//                foreach (ResearchSectionConfig section in m_config.GetSections())
//                {
//                    m_selectedId = -1;
//                    section.RemoveResearch(config);
//                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(config));
//                }
//            }
//        }
//    }

//    //Should remove dependencies manually to avoid complications
//    bool ValidateRemoveResearch(ResearchConfig config)
//    {
//        foreach (ResearchNode compareNode in m_researchNodeDict.Values)
//        {
//            if (compareNode.GetConfig().GetDependencies() != null)
//            {
//                List<ulong> deps = new List<ulong>(compareNode.GetConfig().GetDependencies());
//                if (deps.Contains(config.GetID()))
//                {
//                    EditorUtility.DisplayDialog("Error", "Remove dependencies first!", "Ok");
//                    return false;
//                }
//            }
//        }

//        return true;
//    }

//    bool ValidateRemoveSection(ResearchSectionConfig config)
//    {
//        if(m_config.GetSections().IndexOf(config) != m_config.GetSections().Count - 1)
//        {
//            EditorUtility.DisplayDialog("Error", "Only can remove last section", "Ok");
//            return false;
//        }

//        return true;
//    }

//    void DrawTree()
//    {
//        float currX = START_X;
//        float currY = START_Y;
//        int sectionId = 0;
//        int elementId = 100;

//        if(m_config.GetSections() == null)
//        {
//            return;
//        }

//        foreach (ResearchSectionConfig researchSection in m_config.GetSections())
//        {
//            Rect rect = new Rect(currX, currY, ELEMENT_WIDTH, ELEMENT_HEIGHT);
//            GUI.Window(sectionId, rect, DrawNodeWindow, researchSection.name);
//            m_researchNodeDict[sectionId] = new ResearchNode(sectionId,researchSection.GetSectionResearch(), rect);
//            m_researchSection[sectionId] = researchSection;
//            sectionId++;

//            //Create add research button or existed research
//            for(int i = 1; i < 7; i++)
//            {
//                ResearchConfig config = researchSection.GetResearchAtPosition(i);

//                float x = START_X + i * (ELEMENT_WIDTH + SPACING_X);
//                if (config == null)
//                {
//                    if (GUI.Button(new Rect(x, currY, ELEMENT_WIDTH, ELEMENT_HEIGHT), AssetDatabase.LoadAssetAtPath<Texture>("Assets/UI/Textures/attributes_icon_plus.png"), GUICustomStyles.AddResearchSection))
//                    {
//                        if (!Directory.Exists(Application.dataPath + "/" + RESEARCH_PATH + m_config.name))
//                        {
//                            Directory.CreateDirectory(Application.dataPath + "/" + RESEARCH_PATH + m_config.name);
//                        }

//                        int index = (m_config.GetSections().IndexOf(researchSection) + 1) * 100 + i;
//                        ResearchConfig newResearch = ScriptableObject.CreateInstance<ResearchConfig>();
//                        AssetDatabase.CreateAsset(newResearch, "Assets/" + RESEARCH_PATH + m_config.name + "/" + m_config.name + "Reseach" + index.ToString() + ".asset");
//                        newResearch.SetIcon(AssetDatabase.LoadAssetAtPath<Sprite>("Assets/UI/Textures/icon_happiness_happy_01.png"));
//                        researchSection.AddResearch(newResearch, i - 1);

//                        AssetDatabase.Refresh();
//                    }
//                }
//                else
//                {
//                    Rect rectElement = new Rect(x, currY, ELEMENT_WIDTH, ELEMENT_HEIGHT);
//                    GUI.Window(elementId, rectElement, DrawNodeWindow, config.name);
//                    m_researchNodeDict[elementId] = new ResearchNode(elementId, config, rectElement);
//                    elementId++;
//                }
//            }

//            currY += SPACING_Y + ELEMENT_HEIGHT;
//        }

//        //Add section button
//        if (m_config.GetSections().Count < 7)
//        {
//            if (GUI.Button(new Rect(currX, currY, ELEMENT_WIDTH, ELEMENT_HEIGHT), AssetDatabase.LoadAssetAtPath<Texture>("Assets/UI/Textures/attributes_icon_plus.png"), GUICustomStyles.AddResearchSection))
//            {
//                if (m_config.GetSections() == null || m_config.GetSections().Count == 0)
//                {
//                    if (!Directory.Exists(Application.dataPath + "/" + RESEARCH_SECTION_PATH + m_config.name))
//                    {
//                        Directory.CreateDirectory(Application.dataPath + "/" + RESEARCH_SECTION_PATH + m_config.name);
//                    }

//                    if (!Directory.Exists(Application.dataPath + "/" + RESEARCH_PATH + m_config.name + "Section"))
//                    {
//                        Directory.CreateDirectory(Application.dataPath + "/" + RESEARCH_PATH + m_config.name + "Section");
//                    }
//                }

//                ResearchSectionConfig newSectionConfig = ScriptableObject.CreateInstance<ResearchSectionConfig>();
//                AssetDatabase.CreateAsset(newSectionConfig, "Assets/" + RESEARCH_SECTION_PATH + m_config.name + "/" + m_config.name + "Section" + (m_config.GetSections().Count + 1).ToString() + ".asset");

//                ResearchConfig newSectionResearch = ScriptableObject.CreateInstance<ResearchConfig>();
//                AssetDatabase.CreateAsset(newSectionResearch, "Assets/" + RESEARCH_PATH + m_config.name + "Section" + "/" + m_config.name + "SectionReseach" + (m_config.GetSections().Count + 1).ToString() + ".asset");
//                newSectionResearch.SetIcon(AssetDatabase.LoadAssetAtPath<Sprite>("Assets/UI/Textures/icon_research_sectional_0" + (m_config.GetSections().Count + 1).ToString() + ".png"));
//                newSectionResearch.SetSection();
//                newSectionConfig.AddSectionResearch(newSectionResearch);

//                AssetDatabase.Refresh();

//                m_config.AddSection(newSectionConfig);
//            }
//        }
//    }

//    void DrawNodeWindow(int id)
//    {
//        if (Event.current.GetTypeForControl(id) == EventType.MouseDown && id != "Inspector".GetHashCode())
//        {
//            m_selectedId = id;
//        }

//        ResearchNode node;
//        if(m_researchNodeDict.TryGetValue(id, out node))
//        {
//            Rect rect = new Rect(ELEMENT_WIDTH / 2 - RESEARCH_ICON_WIDTH / 2, ELEMENT_HEIGHT / 2 - RESEARCH_ICON_HEIGHT / 2 + 5,   RESEARCH_ICON_WIDTH, RESEARCH_ICON_HEIGHT);
//            GUI.Label(rect, node.GetConfig()?.GetIcon()?.texture);
//        }

//        if(m_isDragging == true && m_selectedId == id)
//        {
//            Rect rect = new Rect(90, 70, 40, 30);
//            GUI.Label(rect, AssetDatabase.LoadAssetAtPath<Texture>("Assets/UI/Textures/bridge_link.png"));
//        }        
        
//        if(m_isDragging == true && m_draggedEndId == id)
//        {
//            Rect rect = new Rect(90, 70, 40, 30);
//            GUI.Label(rect, AssetDatabase.LoadAssetAtPath<Texture>("Assets/UI/Textures/bridge_link.png"));
//        }
//    }

//    void DrawResearchLines()
//    {
//        foreach(ResearchNode node in m_researchNodeDict.Values)
//        {
//            ResearchConfig config = node.GetConfig();

//            if(config?.GetDependencies() != null)
//            {
//                List<ulong> deps = new List<ulong>(config.GetDependencies());
//                foreach (ResearchNode compareNode in m_researchNodeDict.Values)
//                {
//                    if (node.GetId() != compareNode.GetId() && deps.Contains(compareNode.GetConfig().GetID()))
//                    {
//                        if (node.GetRect().y == compareNode.GetRect().y && node.GetRect().x > compareNode.GetRect().x)
//                        {
//                            DrawLineWithShadow(node.GetLeftPoint(), compareNode.GetRightPoint());
//                        }
//                        else if (node.GetRect().y == compareNode.GetRect().y && node.GetRect().x < compareNode.GetRect().x)
//                        {
//                            DrawLineWithShadow(node.GetRightPoint(), compareNode.GetLeftPoint());
//                        }
//                        else if (node.GetRect().y > compareNode.GetRect().y)
//                        {
//                            DrawLineWithShadow(node.GetUpperPoint(), compareNode.GetBottomPoint());
//                        }
//                    }
//                }
//            }         
//        }
//    }

//    void DrawInspector()
//    {
//        if(m_selectedId != -1)
//        {
//            m_inspectorRect = GUI.Window("Inspector".GetHashCode(), m_inspectorRect, DrawInspector, "Inspector");
//        }
//    }

//    void DrawInspector(int id)
//    {
//        m_inspectorScrollView = GUILayout.BeginScrollView(m_inspectorScrollView);
//        m_inspectorEditor = Editor.CreateEditor(m_researchNodeDict[m_selectedId].GetConfig());
//        m_inspectorEditor?.OnInspectorGUI();
//        m_inspectorEditor.serializedObject.ApplyModifiedProperties();
//        GUILayout.EndScrollView();
//    }

//    void DrawLineWithShadow(Vector3 startPos, Vector3 endPos)
//    {
//        Color shadowCol = new Color(0, 0, 0, 0.06f);
//        for (int i = 0; i < 3; i++) // Draw a shadow
//            Handles.DrawBezier(startPos, endPos, startPos, endPos, shadowCol, null, (i + 1) * 5);
//        Handles.DrawBezier(startPos, endPos, startPos, endPos, Color.black, null, 1);
//    }

//    /// <summary>
//    /// Context menu when right clicking on the graph canvas
//    /// </summary>
//    /// <param name="position"></param>
//    void OpenContextMenu(Vector2 position)
//    {
//        GenericMenu menu = new GenericMenu();
//        menu.AddItem(new GUIContent("Show in project"), false, OnShowProject, m_researchNodeDict[m_selectedId].GetConfig().GetResearchType());
//        menu.ShowAsContext();
//    }

//    void OnShowProject(object type)
//    {
//        ResearchType researchType = (ResearchType)type;

//        if(researchType == ResearchType.Section)
//        {
//            ResearchSectionConfig config = m_researchSection[m_selectedId];
//            Selection.activeObject = config;
//            EditorGUIUtility.PingObject(config);
//        }
//        else
//        {
//            ResearchConfig config = m_researchNodeDict[m_selectedId].GetConfig();
//            Selection.activeObject = config;
//            EditorGUIUtility.PingObject(config);
//        }
//    }

//    void OnInspectorUpdate()
//    {
//        this.Repaint();
//    }
//}