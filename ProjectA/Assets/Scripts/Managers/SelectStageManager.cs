using System.Collections.Generic;
using GameToBeNamed.Utils.Sound;
using ProjectA.Controllers;
using ProjectA.Singletons.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectA.Managers {
    
    public class SelectStageManager : MonoBehaviour {

        public GameSettings GameSettings;

        [Header("select level settings")]
        public Button AdvanceLevelBtn; 
        public Button BackLevelBtn;
        public Button PlayLevelBtn;
        
        [Header("select difficulty settings")]
        public Button AdvanceDifficultyBtn; 
        public Button BackDifficultyBtn;
        public TextMeshProUGUI DifficultyText;
        public TextMeshProUGUI DifficultyTextTooltip;
        [TextArea(2, 5)]
        public List<string> TooltipsTexts = new List<string>();

        [Header("level canvas settings")]
        public Image LevelImage;
        public TextMeshProUGUI LevelName;
        [TextArea(2, 5)]
        public List<string> LevelNameTexts = new List<string>();
        public List<Sprite> LevelImageSprites = new List<Sprite>();

        [Space(10)]
        [Header("Sounds settings")]
        public AudioClip MenuTheme;
        public AudioClip UIClick;
        
        public Button BackBtn;

        private int m_levelIndex = 0;
        private int m_difficultyIndex = 0;
        
        public void AdvanceDifficulty() {
            m_difficultyIndex += 1;
            
            if (m_difficultyIndex > 2) m_difficultyIndex = 0;
            
            AudioController.Instance.Play(UIClick, AudioController.SoundType.SoundEffect2D, GameSettings.GetSfxVolumeReduceScale());
            DifficultyText.text = SetTextDifficulty(m_difficultyIndex);
            DifficultyTextTooltip.text = TooltipsTexts[m_difficultyIndex];
        }
        
        public void BackDifficulty() {
            m_difficultyIndex -= 1;

            if (m_difficultyIndex < 0) m_difficultyIndex = 2;
            
            AudioController.Instance.Play(UIClick, AudioController.SoundType.SoundEffect2D, GameSettings.GetSfxVolumeReduceScale());
            DifficultyText.text = SetTextDifficulty(m_difficultyIndex);
            DifficultyTextTooltip.text = TooltipsTexts[m_difficultyIndex];
        }
        
        public void AdvanceLevel() {
            m_levelIndex += 1;
            
            if (m_levelIndex > 2) m_levelIndex = 0;

            AudioController.Instance.Play(UIClick, AudioController.SoundType.SoundEffect2D, GameSettings.GetSfxVolumeReduceScale());
            LevelImage.sprite = LevelImageSprites[m_levelIndex];
            LevelName.text = LevelNameTexts[m_levelIndex];

            ValidateStageUnlocked(m_levelIndex);
        }
        
        public void BackLevel() {
            m_levelIndex -= 1;

            if (m_levelIndex < 0) m_levelIndex = 2;
            
            AudioController.Instance.Play(UIClick, AudioController.SoundType.SoundEffect2D, GameSettings.GetSfxVolumeReduceScale());
            LevelImage.sprite = LevelImageSprites[m_levelIndex];
            LevelName.text = LevelNameTexts[m_levelIndex];

            ValidateStageUnlocked(m_levelIndex);
        }

        private string SetTextDifficulty(int difficultyIndex) {
            switch (difficultyIndex) {
                case 0:
                    GameSettings.GameDifficulty = GameDifficulty.EASY;
                    return "Easy";
                case 1:
                    GameSettings.GameDifficulty = GameDifficulty.NORMAL;
                    return "Normal";
                case 2:
                    GameSettings.GameDifficulty = GameDifficulty.HARD;
                    return "Hard";
                default:
                    GameSettings.GameDifficulty = GameDifficulty.EASY;
                    return "Easy";
            };
        }

        private void ValidateStageUnlocked(int stageIndex) {
           switch (stageIndex) {
                case 0:
                    LevelImage.color = Color.white;
                    PlayLevelBtn.interactable =  true;
                    break;
                case 1:
                    if(!GameSettings.HasUnlockedStage2) {
                        LevelImage.color = Color.black;
                        PlayLevelBtn.interactable =  false;
                    }
                    break;
                case 2:
                     if(!GameSettings.HasUnlockedStage3) {
                        LevelImage.color = Color.black;
                        PlayLevelBtn.interactable =  false;
                     }
                     break;
            }; 
        }

        private void Start() {
            TransitionModal.Instance.DoTransitionOut();
            
            BackBtn.onClick.AddListener(() => {
                AudioController.Instance.Play(UIClick, AudioController.SoundType.SoundEffect2D, GameSettings.GetSfxVolumeReduceScale());
                TransitionModal.Instance.DoTransitionIn(()=>SceneManager.LoadScene("Menu"));
            });
            
            PlayLevelBtn.onClick.AddListener(() => {
                AudioController.Instance.Play(UIClick, AudioController.SoundType.SoundEffect2D, GameSettings.GetSfxVolumeReduceScale());
                SceneManager.LoadScene("GameScene_" + (m_levelIndex + 1));
            });

            AudioController.Instance.Play(MenuTheme, AudioController.SoundType.Music, GameSettings.GetMusicVolumeReduceScale(), true);
        }

    }
}
