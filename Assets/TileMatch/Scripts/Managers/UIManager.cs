using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TileMatch.Scripts.Core.NotifySystem;

namespace TileMatch.Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        [Header("Header")]
        [SerializeField] private RectTransform progress;
        [SerializeField] private TextMeshProUGUI levelHeaderText;
        [SerializeField] private TextMeshProUGUI currentLevelText;
        [SerializeField] private TextMeshProUGUI nextLevelText;

        [Header("Footer")]
        [SerializeField] private Button drawButton;
        [SerializeField] private Button reverseButton;
        [SerializeField] private Button randomizeButton;
        
        [Header("GameScreen")]
        [SerializeField] private CanvasGroup gameScreen;
        [SerializeField] private GameObject slotContainer;
        
        [Header("LoseScreen")]
        [SerializeField] private CanvasGroup loseScreen;
        [SerializeField] private Button playButton;
        
        [Header("WinScreen")]
        [SerializeField] private CanvasGroup winScreen;
        [SerializeField] private Button continueButton;
        
        private void OnClickDrawButton()
        {
            NotificationCenter.PostNotification(NotificationTag.OnDrawAction);
        }
        
        private void OnClickReverseButton()
        {
            NotificationCenter.PostNotification(NotificationTag.OnReverseAction);
        }
        
        private void OnClickRandomizeButton()
        {
            NotificationCenter.PostNotification(NotificationTag.OnRandomizeBoardAction);
        }

        #region ScreenManagement

        private void SetSlotContainerVisibility(bool status)
        {
            slotContainer.SetActive(status);
        }
        
        private void SetScreenVisibility(CanvasGroup screen, bool status)
        {
            if(screen.gameObject.activeSelf == status) return;
            if (status) { screen.gameObject.SetActive(true); }

            screen.DOFade(status ? 1F : 0F, .25F).OnComplete(() =>
            {
                screen.interactable = status;
                screen.blocksRaycasts = status;
                if (!status) { screen.gameObject.SetActive(false); }
            });
        }

        private void OnGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Start:
                    SetScreenVisibility(loseScreen, false);
                    SetScreenVisibility(winScreen, false);
                    SetScreenVisibility(gameScreen, true);
                    break;
                case GameState.Failed:
                    SetScreenVisibility(winScreen, false);
                    SetScreenVisibility(gameScreen, false);
                    SetScreenVisibility(loseScreen, true);
                    break;
                case GameState.Completed:
                    SetScreenVisibility(gameScreen, false);
                    SetScreenVisibility(loseScreen, false);
                    SetScreenVisibility(winScreen, true);
                    HideSlotContainer();
                    break;
                
            }
        }

        private void SetScreenVisibilities(int _)
        {
            SetScreenVisibility(loseScreen, false);
            SetScreenVisibility(winScreen, false);
            SetScreenVisibility(gameScreen, true);
            SetSlotContainerVisibility(true);
        }
        
        #region LoseScreen

        public void OnPressHoldToSeeButton()
        {
            loseScreen.alpha = 0F;
        }
        
        public void OnReleaseHoldToSeeButton()
        {
            loseScreen.alpha = 1F;
        }

        private void OnClickPlayButton()
        {
            NotificationCenter.PostNotification(NotificationTag.OnRequestReloadLevel);
        }

        #endregion
        
        #region WinScreen

        private void OnClickContinueButton()
        {
            NotificationCenter.PostNotification(NotificationTag.OnRequestLoadNextLevel);
        }

        private void HideSlotContainer()
        {
            SetSlotContainerVisibility(false);
        }

        #endregion

        #endregion

        private void UpdateProgress(float value)
        {
            DOTween.Kill(progress);
            var progressValue = Mathf.Lerp(0F, 535F, value);
            progress.DOSizeDelta(new Vector2(progressValue, 32.5F), .2F);
        }

        private void UpdateLevelTexts(int currentLevel)
        {
            levelHeaderText.text = $"LEVEL {currentLevel}";
            currentLevelText.text = currentLevel.ToString();
            nextLevelText.text = $"{currentLevel + 1}";
        }
        
        private void OnEnable()
        {
            playButton.onClick.AddListener(OnClickPlayButton);
            drawButton.onClick.AddListener(OnClickDrawButton);
            reverseButton.onClick.AddListener(OnClickReverseButton);
            continueButton.onClick.AddListener(OnClickContinueButton);
            randomizeButton.onClick.AddListener(OnClickRandomizeButton);
            
            NotificationCenter.AddObserver<int>(NotificationTag.OnLevelLoaded, UpdateLevelTexts);
            NotificationCenter.AddObserver<int>(NotificationTag.OnLevelLoaded, SetScreenVisibilities);
            NotificationCenter.AddObserver<float>(NotificationTag.OnLevelProgressChanged, UpdateProgress);
            NotificationCenter.AddObserver<GameState>(NotificationTag.OnGameStateChanged, OnGameStateChanged);
        }

        private void OnDisable()
        {
            playButton.onClick.RemoveListener(OnClickPlayButton);
            drawButton.onClick.RemoveListener(OnClickDrawButton);
            reverseButton.onClick.RemoveListener(OnClickReverseButton);
            continueButton.onClick.RemoveListener(OnClickContinueButton);
            randomizeButton.onClick.RemoveListener(OnClickRandomizeButton);
            
            NotificationCenter.RemoveObserver<int>(NotificationTag.OnLevelLoaded, UpdateLevelTexts);
            NotificationCenter.RemoveObserver<int>(NotificationTag.OnLevelLoaded, SetScreenVisibilities);
            NotificationCenter.RemoveObserver<float>(NotificationTag.OnLevelProgressChanged, UpdateProgress);
            NotificationCenter.RemoveObserver<GameState>(NotificationTag.OnGameStateChanged, OnGameStateChanged);
        }
    }
}