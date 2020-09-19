using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
    using BoardGame.Tool;
#endif

namespace BoardGame
{
    public class BoardManager : MonoBehaviour
    {
        public static BoardManager Instance = null;

        [Header("Graph Setting")]
        [SerializeField]
        List<GameObject> nodes;

        [SerializeField]
        List<string> edgeSetup;

        [Header("Visual Setting")]
        [Range(0.1f, 1.0f)]
        [SerializeField]
        float lineWidth = 0.2f;

        [SerializeField]
        Color lineColor = Color.red;

        [SerializeField]
        Material lineMaterial;

        public List<GameObject> Nodes => nodes;
        public List<string> EdgeSetup => edgeSetup;

        Graph graph;
        Board board;

#if UNITY_EDITOR

    static BoardManager TempInstance = null;

    Graph tempGraph;
    bool shouldDrawEdges = false;

    void OnDrawGizmos()
    {
        if (!shouldDrawEdges)
            return;

        if (tempGraph == null)
        {
            UpdateGraph();
            return;
        }

        Gizmos.color = Color.red;
        string labelFormat = "N:[{0}]";

        for (int i = 0; i < nodes.Count; ++i)
        {
            if (nodes[i] == null)
            {
                Debug.LogError("Node at index : " + i + " is null");
                continue;
            }

            Handles.Label(nodes[i].transform.position, string.Format(labelFormat, i));
        }

        foreach (var key in tempGraph.Edges.Keys)
        {
            Vector3 from = nodes[key].transform.position;

            foreach (var id in tempGraph.Edges[key])
            {
                Vector3 to = nodes[id].transform.position;
                Vector3 direction = (to - from).normalized;

                Gizmos.DrawLine(from, to);
                GizmosUtility.DrawArrow(from, direction);
            }
        }
    }

    public void AddNode(GameObject obj)
    {
        if (nodes == null)
            nodes = new List<GameObject>();

        if (nodes.Contains(obj))
            return;

        nodes.Add(obj);
        UpdateGraph();

        Undo.RegisterCompleteObjectUndo(gameObject, ("Add node : " + obj.name));
    }

    public void UpdateGraph()
    {
        shouldDrawEdges = false;
        InitializeGraph(ref tempGraph, edgeSetup);
        shouldDrawEdges = true;
    }

    public void ClearNodes()
    {
        nodes.Clear();
        tempGraph.Clear();

    }

    public void ClearEdges()
    {
        edgeSetup.Clear();
    }

    public void ClearAll()
    {
        ClearNodes();
        ClearEdges();
    }

    public void SetNewEdgeSetup(ICollection<string> collection)
    {
        edgeSetup = (List<string>)collection;
    }

    static void TryInitTempInstace()
    {
        if (TempInstance != null)
            return;

        TempInstance = FindObjectOfType<BoardManager>();
    }

    [MenuItem("BoardManager/Refresh", false, 1)]
    static void UpdateGraphMenuItem()
    {
        TryInitTempInstace();

        if (TempInstance == null)
        {
            Alert("Cannnot find the 'BoardManager' in this scene...");
            return;
        }

        TempInstance.UpdateGraph();
        Alert("'BoardManager' has been updated...");
    }

    [MenuItem("BoardManager/Add selected gameobject to node list")]
    static void AddSelectedGameObjectToNodeList()
    {
        TryInitTempInstace();

        if (TempInstance == null)
        {
            Alert("Cannnot find the 'BoardManager' in this scene...");
            return;
        }

        foreach (var obj in Selection.transforms)
        {
            TempInstance.AddNode(obj.gameObject);
        }
    }

    [MenuItem("BoardManager/Add two ways edge %e")]
    static void AddEdgeFromSelectedGameObject()
    {
        if (Selection.transforms.Length <= 0)
            return;

        if (Selection.transforms.Length < 2)
        {
            Alert("Need to select atleast 2 node", 0.5f);
            return;
        }

        TryInitTempInstace();

        if (TempInstance == null)
        {
            Alert("Can't find BoardManager in this scene..");
            return;
        }

        ICollection<string> edgeCollection = TempInstance.EdgeSetup;

        var edges = NodeUtility.ParseEdge(edgeCollection);
        var nodeID = new List<int>();

        foreach (var obj in Selection.transforms)
        {
            int id = TempInstance.Nodes.IndexOf(obj.gameObject);

            if (id == -1)
            {
                nodeID.Clear();
                Alert("Cannot wield node\nSome selected game object might not be in the 'Nodes' collection", 2.0f);
                return;
            }

            nodeID.Add(id);
        }

        for (int i = 0; i < nodeID.Count; ++i)
        {
            int key = nodeID[i];

            for (int j = 0; j < nodeID.Count; ++j)
            {
                if (!edges.ContainsKey(key))
                {
                    edges.Add(key, new List<int>());
                }

                int value = nodeID[j];

                if (key == value)
                    continue;

                if (edges[key].Contains(value))
                    continue;

                edges[key].Add(value);
            }
        }

        ICollection<string> newEdgeSetup = NodeUtility.ToStringCollection(edges);

        TempInstance.SetNewEdgeSetup(newEdgeSetup);
        TempInstance.UpdateGraph();

        Undo.RegisterCompleteObjectUndo(TempInstance.gameObject, "Wield edges...");
    }

    [MenuItem("BoardManager/Remove edges from selected gameobjects %#e")]
    static void RemoveSelectedEdges()
    {
        if (Selection.transforms.Length <= 0)
            return;

        if (Selection.transforms.Length < 2)
        {
            Alert("Need to select atleast 2 node", 0.5f);
            return;
        }

        TryInitTempInstace();

        if (TempInstance == null)
        {
            Alert("Can't find 'BoardManager' in this scene..");
            return;
        }

        ICollection<string> edgeCollection = TempInstance.EdgeSetup;

        var edges = NodeUtility.ParseEdge(edgeCollection);
        var nodeID = new List<int>();

        foreach (var obj in Selection.transforms)
        {
            int id = TempInstance.Nodes.IndexOf(obj.gameObject);

            if (id == -1)
            {
                nodeID.Clear();
                Alert("Cannot remove nodes\nSome selected gameobjects might not be in the 'Nodes' collection", 2.0f);
                return;
            }

            nodeID.Add(id);
        }

        for (int i = 0; i < nodeID.Count; ++i)
        {
            int key = nodeID[i];

            for (int j = 0; j < nodeID.Count; ++j)
            {
                if (!edges.ContainsKey(key))
                {
                    continue;
                }

                int value = nodeID[j];

                edges[key].Remove(value);

                if (edges[key].Count <= 0)
                {
                    edges.Remove(key);
                }
            }
        }

        ICollection<string> newEdgeSetup = NodeUtility.ToStringCollection(edges);

        TempInstance.SetNewEdgeSetup(newEdgeSetup);
        TempInstance.UpdateGraph();

        Undo.RegisterCompleteObjectUndo(TempInstance.gameObject, "Remove edges...");
    }

    [MenuItem("BoardManager/Reset/Clear nodes", false, 2)]
    static void ClearAllNode()
    {
        TryInitTempInstace();

        if (TempInstance == null)
        {
            Alert("Cannnot find the 'BoardManager' in this scene...");
            return;
        }

        bool isConfirm = EditorUtility.DisplayDialog("Warning", "Are you sure to clear all the 'Nodes'?\nThis Operation cannot be undo", "OK", "Cancel");

        if (!isConfirm)
            return;

        TempInstance.ClearNodes();
        Undo.RegisterCompleteObjectUndo(TempInstance.gameObject, "Clear 'Nodes'");

        EditorUtility.DisplayDialog("Success", "'Nodes' has been cleared...", "OK");
    }

    [MenuItem("BoardManager/Reset/Clear edges", false, 2)]
    static void ClearAllEdge()
    {
        TryInitTempInstace();

        if (TempInstance == null)
        {
            Alert("Cannnot find the 'BoardManager' in this scene...");
            return;
        }

        bool isConfirm = EditorUtility.DisplayDialog("Warning", "Are you sure to clear all the 'Edges'?\nThis Operation cannot be undo", "OK", "Cancel");

        if (!isConfirm)
            return;

        TempInstance.ClearEdges();
        Undo.RegisterCompleteObjectUndo(TempInstance.gameObject, "Clear 'Edges'");

        EditorUtility.DisplayDialog("Success", "'Edges' has been cleared...", "OK");
        UpdateGraphMenuItem();
    }

    [MenuItem("BoardManager/Reset/Clear all nodes and edges", false, 2)]
    static void ClearAllNodeAndEdges()
    {
        TryInitTempInstace();

        if (TempInstance == null)
        {
            Alert("Cannnot find the 'BoardManager' in this scene...");
            return;
        }

        bool isConfirm = EditorUtility.DisplayDialog("Warning", "Are you sure to clear the 'BoardManager'?\nThis Operation cannot be undo", "OK", "Cancel");

        if (!isConfirm)
            return;

        TempInstance.ClearAll();
        Undo.RegisterCompleteObjectUndo(TempInstance, "Clear 'BoardManager'");

        EditorUtility.DisplayDialog("Success", "'BoardManager' has been cleared...", "OK");
    }

    static void Alert(string message, float fadeoutWait = 0.2f)
    {
        foreach (SceneView scene in SceneView.sceneViews)
        {
            scene.ShowNotification(new GUIContent(message), fadeoutWait);
        }
    }

    public static void OnUndoRedoCallback()
    {
        TryInitTempInstace();
        TempInstance?.UpdateGraph();
    }

#endif
        void Awake()
        {
            Initialize();
        }

        void Initialize()
        {
            if (Instance == null)
                Instance = this;

            InitializeGraph(ref graph, edgeSetup);
            board = new Board(graph);

            InitVisualNodeConnection();
        }

        void InitializeGraph(ref Graph graph, ICollection<string> edgeSetup)
        {
            Node[] graphNodes = new Node[nodes.Count];

            for (int i = 0; i < nodes.Count; ++i)
            {
                graphNodes[i] = new Node(i);
            }

            graph = new Graph(graphNodes);
            var edges = NodeUtility.ParseEdge(edgeSetup);

            foreach (var key in edges.Keys)
            {
                foreach (var value in edges[key])
                {
                    graph.AddEdge(key, value);
                }
            }
        }

        void InitVisualNodeConnection()
        {
            var edges = NodeUtility.ParseEdge(edgeSetup);
            var traverseKey = new List<int>();

            foreach (var key in edges.Keys)
            {
                var totalNode = edges[key].Count;
                var lineRenderer = nodes[key].gameObject.AddComponent<LineRenderer>();

                lineRenderer.material = lineMaterial;
                lineRenderer.widthMultiplier = lineWidth;
                lineRenderer.positionCount = (totalNode * 2);
                lineRenderer.SetColors(lineColor, lineColor);

                var originNode = nodes[key];
                var offset = 0;

                for (int i = 0; i < totalNode; ++i)
                {
                    var targetNodeID = edges[key][i];
                    var targetNode = nodes[targetNodeID];

                    lineRenderer.SetPosition(offset, originNode.transform.position);
                    lineRenderer.SetPosition(offset + 1, targetNode.transform.position);

                    offset += 2;
                }
            }
        }

        public GameObject GetNode(int id)
        {
            if (id >= nodes.Count)
                return null;

            return nodes[id];
        }

        public Dictionary<int, List<List<int>>> GetPath(int startID, int totalMove)
        {
            Node root = new Node(startID);
            return board.GetPossiblePath(root, totalMove);
        }
    }
}
