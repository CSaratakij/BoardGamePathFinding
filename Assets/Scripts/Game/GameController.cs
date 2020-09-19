using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    public enum GameState
    {
        Roll,
        PickDestination,
        Move
    }

    public class GameController : MonoBehaviour
    {
        public Action<GameState> OnChangeState;

        [SerializeField]
        NodeTraveler[] nodeTravelers;

        GameState State => gameState;
        GameState gameState;

        void Awake()
        {
            Initialize();
        }

        void Initialize()
        {
            SubscribeEvent();
        }

        void SubscribeEvent()
        {
            foreach (var traveler in nodeTravelers)
            {
                traveler.OnReachDestination += OnReachDestination;
            }
        }

        void OnReachDestination()
        {
            Debug.Log("Traveler reach its destination...");
            //TODO : go back to roll state here...
        }
    }
}

