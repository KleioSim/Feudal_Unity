using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Feudal.Scenes.Main;
using System.Collections.Generic;
using System.Reflection;
using Feudal.Presents;

namespace Feudal.Scenes.Initial
{
    public class InitialScene : MonoBehaviour
    {
        public UnityEvent OnSwitchScene;

        void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene("MainScene", new LoadSceneParameters(LoadSceneMode.Single));
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode model)
        {
            OnSwitchScene.Invoke();

            var session = new Session();

            var presentMgr = new PresentManager();
            presentMgr.session = session;

            var mainScene = SceneManager.GetActiveScene().GetRootGameObjects()
                .Select(obj => obj.GetComponent<MainScene>())
                .Single(x => x != null);

            UIView.OnEnableAction = (view) =>
            {
                presentMgr.RefreshMonoBehaviour(view);
            };

            UIView.ExecUICmd = (obj) =>
            {
                session.ExecUICmd(obj);

                presentMgr.RefreshMonoBehaviour(mainScene);
            };

            presentMgr.RefreshMonoBehaviour(mainScene);
        }
    }
}