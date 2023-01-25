// using System;
// using System.Collections;
// using System.Collections.Generic;
// using DG.Tweening;
// using HaremCity.Actions;
// using HaremCity.Controllers;
// using HaremCity.Events;
// using HaremCity.Managers;
// using TMPro;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;
//
// namespace HaremCity.Utils {
//     
//     public class PauseModal : MonoBehaviour {
//         
//         [Header("buttons")]
//         public Button ResumeBtn;
//         public Button RetryBtn;
//         public Button StageSelectBtn;
//         public Button MenuBtn;
//
//         [Header("bg")]
//         public RectTransform BgModal;
//         public TextMeshProUGUI TittleTxt;
//         
//         private bool m_isAnimating;
//         
//         public void Awake() {
//             GameManager.Instance.Dispatcher.Subscribe<OnPauseGame>(OnPauseGame);
//             
//             ResumeBtn.onClick.AddListener(()=> {
//                 Time.timeScale = 1;
//                 GameManager.Instance.IsGamePaused = false;
//                 AnimatePause(1, -250f, 0f, true);
//             });
//             
//             StageSelectBtn.onClick.AddListener(()=> {
//                 Time.timeScale = 1;
//                 DialogueController.Instance.HasPosStageDialogue = false;
//                 HUDManager.Instance.DoTransition(() => {
//                     AudioController.Instance.StartBiomeEmitters(GameManager.Instance.LevelInfoData.Biome, true);
//                     AudioController.Instance.StopBossTheme();
//                     SceneManager.LoadScene("HUB");
//                 });
//             });
//             
//             MenuBtn.onClick.AddListener(()=> {
//                 Time.timeScale = 1; 
//                 DialogueController.Instance.HasPosStageDialogue = false;
//                 HUDManager.Instance.DoTransition(() => {
//                     AudioController.Instance.StartBiomeEmitters(GameManager.Instance.LevelInfoData.Biome, true);
//                     AudioController.Instance.StopBossTheme();
//                     SceneManager.LoadScene("MainMenu");
//                 });
//             });
//             
//             RetryBtn.onClick.AddListener(()=> {
//                 Time.timeScale = 1; 
//                 GameManager.Instance.IsGamePaused = false;
//                 GameManager.Instance.IsGameEnded = false;
//                 HUDManager.Instance.DoTransition(() => {
//                     AudioController.Instance.StartBiomeEmitters(GameManager.Instance.LevelInfoData.Biome, true);
//                     AudioController.Instance.StopBossTheme();
//                     SceneManager.LoadScene("Stage01");
//                 });
//             });
//             
//             gameObject.SetActive(false);
//         }
//
//         private void OnPauseGame(OnPauseGame ev) {
//             if (!GameManager.Instance.IsPlayerDead) {
//                 if (GameManager.Instance.IsGameEnded || GameManager.Instance.PlayerData.IsCastingWeaponSkill ||
//                     GameManager.Instance.PlayerData.IsCastingSkill) return;    
//             }
//             
//             if (GameManager.Instance.PlayerData.IsCastingSkill) {
//                 if (GameManager.Instance.PlayerData.IsWeaponSkillPreview) {
//                     GameManager.Instance.PlayerPosition.transform.GetChild(0).GetComponent<PlayerWeaponSkillAction>().CancelSkillPreview();
//                 }
//                 else {
//                     GameManager.Instance.PlayerPosition.transform.GetChild(0).GetComponent<PlayerSkillAction>().CancelPreviewSkill();
//                 }
//             }
//             
//             gameObject.SetActive(true);
//             
//             RetryBtn.gameObject.SetActive(ev.IsEndGame);
//             ResumeBtn.gameObject.SetActive(!ev.IsEndGame);
//             TittleTxt.text = "Pause";
//
//             if (ev.IsEndGame) {
//                 TittleTxt.text = "Lose";
//                 AudioController.Instance.DefeatEmitter.Play();
//             }
//             
//             switch (ev.IsPaused) {
//                 case true:
//                     AudioController.Instance.PlayPauseSnapshot();
//                     AnimatePause(0, 0, 0.85f);
//                     break;
//                 case false:
//                     Time.timeScale = 1;
//                     AudioController.Instance.StopPauseSnapshot();
//                     AnimatePause(1, -250f, 0f, true);
//                     break;
//             }
//         }
//
//         private void AnimatePause(float timeScale, float positionY, float modalFadeValue, bool deactivateModal = false) {
//             if (m_isAnimating) return;
//
//             m_isAnimating = true;
//             GetComponent<Image>().DOFade(modalFadeValue, 0.25f);
//             BgModal.DOAnchorPosY(positionY, 0.30f).OnComplete(() => {
//                 Time.timeScale = timeScale;
//                 m_isAnimating = false;
//
//                 if (deactivateModal) {
//                     gameObject.SetActive(false);
//                 }
//             });
//         }
//     }
// }