using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BoardGame
{
    public class UIController : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField]
        Transform inGamePanel;

        [SerializeField]
        Button btnRoll;

        [SerializeField]
        TextMeshProUGUI lblGameState;

        [SerializeField]
        TextMeshProUGUI lblDice;

        [Header("Arrow Button")]
        [SerializeField]
        int totalArrow = 1;

        [SerializeField]
        Vector3 arrowOffset;

        [SerializeField]
        GameObject imgArrowPrefab;

        [Header("Dependencies")]
        [SerializeField]
        GameController gameController;

        [SerializeField]
        BoardManager boardManager;

        Camera camera;
        GameObject[] imgArrows;
        Dictionary<int, List<List<int>>> currentPath;

        void Awake()
        {
            Initialize();
            UpdateUI(gameController.State);
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape)) {
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }

        void Initialize()
        {
            camera = Camera.main;
            imgArrows = new GameObject[totalArrow];

            for (int i = 0; i < totalArrow; ++i)
            {
                imgArrows[i] = Instantiate(imgArrowPrefab, inGamePanel.transform);
                imgArrows[i].gameObject.SetActive(false);
            }

            SubscribeEvent();
        }

        void SubscribeEvent()
        {
            gameController.OnChangeState += OnGameChangeState;

            btnRoll.onClick.AddListener(() => {
                if (GameState.Roll != gameController.State) {
                    return;
                }

                var totalMove = gameController.RollDice();
                lblDice.text = $"Roll ( {totalMove} )";

                currentPath = gameController.CurrentActor.FindPossiblePath(totalMove);
                gameController.ChangeState(GameState.PickDestination);
            });
        }

        void ShowPossiblePath(IEnumerable<int> destinations)
        {
            foreach (var id in destinations)
            {
                ShowArrow(id);
            }
        }

        void ShowArrow(int destination)
        {
            foreach (var arrow in imgArrows)
            {
                if (arrow.activeSelf)
                    continue;

                var button = arrow.GetComponent<Button>();

                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => {
                    int id = destination;
                    OnPickDestination(id);
                });

                var targetPosition = boardManager.GetNode(destination).transform.position;

                targetPosition += arrowOffset;
                targetPosition = camera.WorldToScreenPoint(targetPosition);

                arrow.transform.position = targetPosition;
                arrow.gameObject.SetActive(true);

                break;
            }
        }

        void HideAllArrow()
        {
            foreach (var arrow in imgArrows)
            {
                arrow.SetActive(false);
            }
        }

        void OnPickDestination(int destination)
        {
            var totalPath = currentPath[destination].Count;
            var pickPathID = Random.Range(0, totalPath);

            var selectedPath = currentPath[destination][pickPathID].ToArray();

            gameController.CurrentActor.SetPath(selectedPath);
            gameController.CurrentActor.StartMove(true);

            gameController.ChangeState(GameState.WaitMove);
        }

        void OnGameChangeState(GameState state)
        {
            if (GameState.PickDestination == state) {
                ShowPossiblePath(currentPath.Keys);
            }

            UpdateUI(state);
        }

        void UpdateUI(GameState state)
        {
            btnRoll.enabled = (GameState.Roll) == state;
            lblGameState.text = $"Game State : {state.ToString()}";

            if (GameState.Roll == state) {
                lblDice.text = string.Empty;
            }

            if (GameState.PickDestination != state) {
                HideAllArrow();
            }
        }
    }
}

