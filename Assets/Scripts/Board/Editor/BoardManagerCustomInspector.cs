using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BoardGame.Tool
{
    [CustomEditor(typeof(BoardManager))]
    public class BoardManagerCustomInspector : Editor
    {
        BoardManager script;

        static BoardManagerCustomInspector()
        {
            Undo.undoRedoPerformed += BoardManager.OnUndoRedoCallback;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            DrawCustomInspector();
        }

        void DrawCustomInspector()
        {
            if (script == null)
            {
                script = (BoardManager)target;
            }

            EditorGUILayout.Space(10);
            DropAreaGUI();

            EditorGUILayout.Space(5);
            ButtonUpdateGraph();
            EditorGUILayout.Space(10);
        }

        void ButtonUpdateGraph()
        {
            if (GUILayout.Button("Refresh", GUILayout.Height(30)))
            {
                script.UpdateGraph();
            }
        }

        public void DropAreaGUI()
        {
            Event evt = Event.current;
            Rect dropArea = GUILayoutUtility.GetRect(0.0f, 60.0f, GUILayout.ExpandWidth(true));

            GUI.Box(dropArea, "Add Nodes");

            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!dropArea.Contains(evt.mousePosition))
                        return;

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();

                        foreach (Object obj in DragAndDrop.objectReferences)
                        {
                            GameObject gameObject = (GameObject)obj;
                            script.AddNode(gameObject);
                        }
                    }

                    break;
            }
        }
    }
}
