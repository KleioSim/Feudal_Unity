using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Feudal.MessageBuses
{
    public interface Message
    {

    }

    public interface IMessageBus
    {
        void Register(object obj);
        IPost PostMessage(Message message);

        public interface IPost
        {
            T WaitAck<T>();
        }
    }

    public class MessageProcessAttribute : Attribute
    {
    }

    public class MessageBus : IMessageBus
    {
        private Dictionary<Type, List<(object target, MethodInfo method)>> dict = new Dictionary<Type, List<(object target, MethodInfo method)>>();

        public IMessageBus.IPost PostMessage(Message message)
        {
            var postRequest = new PostRequest();

            if (dict.TryGetValue(message.GetType(), out List<(object target, MethodInfo method)> processers))
            {
                foreach (var processer in processers)
                {
                    var ret = processer.method.Invoke(processer.target, new object[] { message });
                    if (processer.method.ReturnType != typeof(void))
                    {
                        postRequest.Add(ret);
                    }
                }
            }

            return postRequest;
        }

        public void Register(object target)
        {
            var methods = target.GetType().GetMethods(BindingFlags.Instance
                | BindingFlags.DeclaredOnly
                | BindingFlags.Public
                | BindingFlags.NonPublic);

            foreach (var method in methods)
            {
                var messageProcessAttrib = method.GetCustomAttribute<MessageProcessAttribute>();
                if (messageProcessAttrib == null)
                {
                    continue;
                }

                var parmeters = method.GetParameters();
                if (parmeters.Length != 1 || !typeof(Message).IsAssignableFrom(parmeters[0].ParameterType))
                {
                    throw new Exception();
                }

                if(!dict.ContainsKey(parmeters[0].ParameterType))
                {
                    dict.Add(parmeters[0].ParameterType, new List<(object target, MethodInfo method)>());
                }

                dict[parmeters[0].ParameterType].Add((target, method));
            }
        }
    }

    internal class PostRequest : IMessageBus.IPost
    {
        private List<object> messageAcks = new List<object>();

        public T WaitAck<T>()
        {
            return (T)messageAcks.FirstOrDefault(x => x is T);
        }

        internal void Add(object obj)
        {
            messageAcks.Add(obj);
        }
    }
}
