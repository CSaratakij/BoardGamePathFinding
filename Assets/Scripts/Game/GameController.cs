using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BoardGame
{
    public enum GameState
    {
        Roll,
        PickDestination,
        WaitMove
    }

    public class GameController : MonoBehaviour
    {
        public Action<GameState> OnChangeState;

        [SerializeField]
        NodeTraveler[] actors;

        public GameState State => gameState;
        public NodeTraveler CurrentActor => actors[currentActorIndex];

        int currentSeed = 0;
        int currentActorIndex = 0;

        GameState gameState;

        void Awake()
        {
            Initialize();
        }

        void Initialize()
        {
            currentSeed = Random.Range(0, 100);
            Random.InitState(currentSeed);

            SubscribeEvent();
        }

        void SubscribeEvent()
        {
            foreach (var actor in actors)
            {
                actor.OnReachDestination += OnReachDestination;
            }
        }

        void OnReachDestination()
        {
            ChangeToNextActorTurn();
        }

        void ChangeToNextActorTurn()
        {
            CurrentActor.StartMove(false);
            CurrentActor.ClearPath();

            currentActorIndex = (currentActorIndex + 1) >= actors.Length ? 0 : (currentActorIndex + 1);
            ChangeState(GameState.Roll);
        }

        public void ChangeState(GameState state)
        {
            gameState = state;
            OnChangeState?.Invoke(gameState);
        }

        public int RollDice()
        {
            int result = Random.Range(1, 7);

            currentSeed = Random.Range(0, 100);
            Random.InitState(currentSeed);

            return result;
        }
    }
}

