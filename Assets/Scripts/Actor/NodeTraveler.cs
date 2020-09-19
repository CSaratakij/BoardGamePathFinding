using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    public class NodeTraveler : MonoBehaviour
    {
        public Action OnReachDestination;

        [SerializeField]
        int startNodeID = 0;

        BoardManager boardManager;

        void Awake()
        {
            Initialize();
        }

        //Test
        void Start()
        {
            OnReachDestination?.Invoke();
        }

        //Test
        /* void Start() */
        /* { */
        /*     FindNextPossiblePath(3); */
        /* } */

        void Initialize()
        {
            boardManager = BoardManager.Instance;

            if (!boardManager) {
                Debug.LogError("Can't find BoardManager instance.");
                return;
            }

            var startNode = boardManager.GetNode(startNodeID);
            transform.position = (startNode) ? (startNode.transform.position) : transform.position;
        }

        public Dictionary<int, List<List<int>>> FindNextPossiblePath(int totalMove)
        {
            var path = boardManager.GetPath(startNodeID, totalMove);
            return path;

            /*
            //TODO: move this to the move routine.
            foreach (var id in path.Keys)
            {
                int totalPossiblePath = path[id].Count;

                for (int i = 0;  i < totalPossiblePath; ++i)
                {
                    int[] currentPath = path[id][i].ToArray();

                    // ------------------------------
                    //Print path here (can change this to show a path).
                    //If there are more than one possible path to reach those destination, just random picks.
                    // ------------------------------
                    //Don't forget to make a ui that tell player which path to select.
                    //Don't forget to make a dice.
                    //Don't forget to make a move routine.
                    // ------------------------------
                    string strPath = "";
                    foreach (var nodeID in currentPath)
                    {
                        strPath += $"{nodeID},";
                    }

                    Debug.Log($"Destination {id}");
                    Debug.Log($"Path : {strPath}");
                }
            }
            */
        }
    }
}
