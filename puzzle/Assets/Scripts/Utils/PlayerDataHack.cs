using App;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
    public class PlayerDataHack : MonoBehaviour
    {
        public void ClearAllData()
        {
            PlayerData.ClearAllData();
            SceneManager.LoadScene("Preloader");
        }

        public void UnlockProgress()
        {
            var levelsConfig = AppController.Instance.LevelsConfig;
            var progressData = AppController.Instance.GetProgressData();
            progressData.CompletedLevelIds.Clear();
            foreach (var cfg in levelsConfig.Levels)
            {
                progressData.CompletedLevelIds.Add(cfg.LevelId);
            }
            AppController.Instance.SaveProgressData(progressData);
            SceneManager.LoadScene("Menu");
        }
    }
}