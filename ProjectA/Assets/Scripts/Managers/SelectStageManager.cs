using System.Collections.Generic;
using ProjectA.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectA.Managers {
    
    public class SelectStageManager : MonoBehaviour {

        public GameDifficulty GameDifficulty;

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

        public Button BackBtn;

        private int m_levelIndex = 0;
        private int m_difficultyIndex = 0;

        public void AdvanceDifficulty() {
            m_difficultyIndex += 1;
            
            if (m_difficultyIndex > 2) m_difficultyIndex = 0;
            
            DifficultyText.text = SetTextDifficulty(m_difficultyIndex);
            DifficultyTextTooltip.text = TooltipsTexts[m_difficultyIndex];
        }
        
        public void BackDifficulty() {
            m_difficultyIndex -= 1;

            if (m_difficultyIndex < 0) m_difficultyIndex = 2;
            
            DifficultyText.text = SetTextDifficulty(m_difficultyIndex);
            DifficultyTextTooltip.text = TooltipsTexts[m_difficultyIndex];
        }
        
        public void AdvanceLevel() {
            m_levelIndex += 1;
            
            if (m_levelIndex > 3) m_levelIndex = 0;

            LevelImage.sprite = LevelImageSprites[m_levelIndex];
            LevelName.text = LevelNameTexts[m_levelIndex];
        }
        
        public void BackLevel() {
            m_levelIndex -= 1;

            if (m_levelIndex < 0) m_levelIndex = 3;
            
            LevelImage.sprite = LevelImageSprites[m_levelIndex];
            LevelName.text = LevelNameTexts[m_levelIndex];
        }

        private string SetTextDifficulty(int difficultyIndex) {
            return difficultyIndex switch {
                0 => "Easy",
                1 => "Normal",
                2 => "Hard",
                _ => "Easy"
            };
        }

        private void Start() {
            TransitionModal.Instance.DoTransitionOut();
            
            AdvanceLevelBtn.interactable = PlayerPrefs.GetInt("tutorial_finished") == 1;
            BackLevelBtn.interactable = PlayerPrefs.GetInt("tutorial_finished") == 1;
            
            BackBtn.onClick.AddListener(() => {
                TransitionModal.Instance.DoTransitionIn(()=>SceneManager.LoadScene("Menu"));
            });
        }

    }
}
