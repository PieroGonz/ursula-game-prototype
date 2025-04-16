// AnimationQuery.cs
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardSystem.Animation
{
    public class AnimationQuery : MonoBehaviour
    {
        private Queue<IAnimationAction> actions = new Queue<IAnimationAction>();
        private MonoBehaviour runner;
        private Action onComplete;
        private bool isRunning = false;

        public void AddToQuery(IAnimationAction action)
        {
            actions.Enqueue(action);
        }

        public void Start(MonoBehaviour runner, Action onComplete = null)
        {
            if (isRunning)
                return;

            this.runner = runner;
            this.onComplete = onComplete;
            isRunning = true;

            if (actions.Count > 0)
                runner.StartCoroutine(ProcessQueue());
            else
            {
                isRunning = false;
                onComplete?.Invoke();
            }
        }

        private IEnumerator ProcessQueue()
        {
            while (actions.Count > 0)
            {
                var action = actions.Dequeue();
                yield return runner.StartCoroutine(action.Execute());
            }

            isRunning = false;
            onComplete?.Invoke();
        }
    }

    public interface IAnimationAction
    {
        IEnumerator Execute();
    }
}
