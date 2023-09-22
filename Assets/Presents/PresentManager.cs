using Feudal.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Feudal.Presents
{
    public class PresentManager
    {
        public Dictionary<Type, IPresent> dict = new Dictionary<Type, IPresent>();

        public Session session { get; set; }

        public PresentManager()
        {
            var presents = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => type.BaseType != null
                    && type.BaseType.IsGenericType
                    && type.BaseType.GetGenericTypeDefinition() == typeof(Present<>))
                .ToArray();

            dict = presents.ToDictionary(type => type.BaseType.GetGenericArguments()[0], type => Activator.CreateInstance(type) as IPresent);
        }

        public void RefreshUIView(UIView uiview, bool isRecursion = false)
        {
            if (dict.TryGetValue(uiview.GetType(), out IPresent present))
            {
                present.session = session;
                present.RefreshUIView(uiview);
            }

            if(isRecursion)
            {
                foreach (var view in IteratorChildren(uiview.transform))
                {
                    RefreshUIView(view, isRecursion);
                }
            }
        }

        private IEnumerable<UIView> IteratorChildren(Transform transform)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                if (!child.gameObject.activeInHierarchy)
                {
                    continue;
                }

                Debug.Log($"IteratorChildren {child} {i}");
                var childUIView = child.GetComponent<UIView>();
                if (childUIView != null)
                {
                    yield return childUIView;
                }
                else
                {
                    foreach (var nextUIView in IteratorChildren(child))
                    {
                        yield return nextUIView;
                    }
                }
            }
        }
    }
}