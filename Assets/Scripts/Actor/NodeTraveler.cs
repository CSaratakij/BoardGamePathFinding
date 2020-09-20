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
        float moveSpeed = 10.0f;

        [SerializeField]
        int startNodeID = 0;

        [SerializeField]
        float waitTimeWhenReachPerNode = 0.5f;

        public bool IsMove => isMove;

        bool isMove = false;
        bool isBeginMovingTowardNode = false;

        int currentPathIndice = 1;
        int[] currentPath;

        float currentWaitTime = 0.0f;

        Vector3 targetPosition;
        Vector3 targetDirection;

        CharacterController characterController;
        BoardManager boardManager;

        void Awake()
        {
            Initialize();
        }

        void Update()
        {
            MoveHandler();
        }

        void Initialize()
        {
            boardManager = BoardManager.Instance;

            if (!boardManager) {
                Debug.LogError("Can't find BoardManager instance.");
                return;
            }

            var startNode = boardManager.GetNode(startNodeID);
            transform.position = (startNode) ? (startNode.transform.position) : transform.position;

            characterController = GetComponent<CharacterController>();
        }

        void MoveHandler()
        {
            if (!isMove) {
                return;
            }

            if (currentPath == null) {
                Debug.LogError("Attemping to move without settting the path first...");
                StartMove(false);
                return;
            }

            bool shouldPauseMoving = (currentWaitTime > Time.time);

            if (shouldPauseMoving) {
                return;
            }

            if (!isBeginMovingTowardNode)
            {
                var nodeID = currentPath[currentPathIndice];
                var node = boardManager.GetNode(nodeID);

                targetPosition = node.transform.position;
                targetDirection = (targetPosition - transform.position);

                if (targetDirection.magnitude > 1.0f) {
                    targetDirection = targetDirection.normalized;
                }

                isBeginMovingTowardNode = true;
            }

            characterController.SimpleMove(targetDirection * moveSpeed);

            var product = Vector3.Dot(targetDirection, (transform.position - targetPosition).normalized);
            var isGettingCloserToTarget = (product > 0.0f);

            if (isGettingCloserToTarget)
            {
                currentPathIndice += 1;

                bool isReachDestination = (currentPathIndice >= currentPath.Length);

                if (isReachDestination)
                {
                    startNodeID = currentPath[currentPath.Length - 1];

                    StartMove(false);
                    ClearPath();

                    OnReachDestination?.Invoke();
                }
                else
                {
                    currentWaitTime = (Time.time + waitTimeWhenReachPerNode);
                    isBeginMovingTowardNode = false;
                }
            }
        }

        public Dictionary<int, List<List<int>>> FindPossiblePath(int totalMove)
        {
            var path = boardManager.GetPath(startNodeID, totalMove);
            return path;
        }

        public void SetPath(int[] path)
        {
            currentPath = path;

            string result = "";

            foreach (var id in path)
            {
                result += id + ", ";
            }
        }

        public void ClearPath()
        {
            isBeginMovingTowardNode = false;
            currentPath = null;
            currentPathIndice = 1;
        }

        public void StartMove(bool value)
        {
            isMove = value;
        }
    }
}
