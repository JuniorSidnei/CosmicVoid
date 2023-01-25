// using System;
// using System.Collections;
// using System.Collections.Generic;
// using DG.Tweening;
// using HaremCity.Controllers;
// using HaremCity.Events;
// using HaremCity.Managers;
// using ProjectA.Singletons.Managers;
// using TMPro;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;
//
// namespace HaremCity.Utils {
//     
//     public class WinModal : MonoBehaviour {
//
//         public struct StageResults {
//             public int EnemiesKilledResult;
//             public float LifePercentResult;
//             public int GoldCollectedResult;
//             public float StageTotalMinutesResult;
//             public float StageTotalSecondsResult;
//             public int StarsStage;
//         }
//         
//         [Header("texts")]
//         public TextMeshProUGUI TimeTxt;
//         public TextMeshProUGUI LifePercentTxt;
//         public TextMeshProUGUI GoldTxt;
//         public TextMeshProUGUI EnemiesKilledTxt;
//
//         [Header("buttons")]
//         public Button NextBtn;
//         public Button RetryBtn;
//         public Button StageSelectBtn;
//
//         [Header("bgs")]
//         public RectTransform BgStatistics;
//         public RectTransform BgSelection;
//         public GameObject StatisticsPanel;
//         public GameObject SelectionPanel;
//
//         [Header("stars")]
//         public List<Image> StarsStastitics;
//         public List<Image> StarsSelection;
//         public Sprite StarAchieved;
//
//         private bool m_isAnimating;
//         private Action m_onWinCallback;
//         
//         public void Awake() {
//             GameManager.Instance.Dispatcher.Subscribe<OnWinGame>(OnWinGame);
//             
//             RetryBtn.onClick.AddListener(()=> {
//                 Time.timeScale = 1;
//                 GameManager.Instance.IsGamePaused = false;
//                 HUDManager.Instance.DoTransition(() => {
//                     SceneManager.LoadScene("Stage01");
//                 });
//             });
//             
//             StageSelectBtn.onClick.AddListener(()=> {
//                 Time.timeScale = 1;
//                 HUDManager.Instance.DoTransition(() => {
//                     AudioController.Instance.StartBiomeEmitters(GameManager.Instance.LevelInfoData.Biome, true);
//                     m_onWinCallback?.Invoke();
//                 });
//             });
//             
//             NextBtn.onClick.AddListener(()=> {
//                 BgStatistics.DOAnchorPosY(-250, .5f).OnComplete(() => {
//                     BgSelection.DOAnchorPosY(0, .5f);
//                 });
//             });
//             
//             gameObject.SetActive(false);
//         }
//          
//         private void OnWinGame(OnWinGame ev) {
//             AudioController.Instance.VictoryEmitter.Play();
//             m_onWinCallback = ev.OnWinCallback;
//             gameObject.SetActive(true);
//
//             EnemiesKilledTxt.text = ev.StageResults.EnemiesKilledResult.ToString();
//             TimeTxt.text = ev.StageResults.StageTotalMinutesResult.ToString("00") + ":" + ev.StageResults.StageTotalSecondsResult.ToString("00");
//             GoldTxt.text = ev.StageResults.GoldCollectedResult.ToString();
//             LifePercentTxt.text = ev.StageResults.LifePercentResult.ToString("00") + "%";
//
//             for (var i = 0; i < ev.StageResults.StarsStage; i++) {
//                 StarsStastitics[i].sprite = StarAchieved;
//                 StarsSelection[i].sprite = StarAchieved;
//             }
//             
//             BgStatistics.DOAnchorPosY(0, .5f);
//         }
//     }
// }