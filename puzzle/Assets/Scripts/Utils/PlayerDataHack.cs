using App;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Utils {
    public class PlayerDataHack : MonoBehaviour
    {
        public void Remove()
        {
            PlayerData.ClearRestoreData();
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