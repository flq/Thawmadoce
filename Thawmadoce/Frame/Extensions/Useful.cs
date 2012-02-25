using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq.Expressions;
using Caliburn.Micro;

namespace Thawmadoce.Frame.Extensions
{
    public static class Useful
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var i in items)
                action(i);
        }

        /// <summary>
        /// If object implements T, the action gets executed, otherwise not. returns the same object for chaining
        /// </summary>
        public static object As<T>(this object @object, Action<T> action) where T : class
        {
            var thing = @object as T;
            if (thing != null)
                action(thing);
            return @object;
        }

        public static void Raise(this EventHandler @event, object sender)
        {
            if (@event != null)
                @event(sender, EventArgs.Empty);
        }

        public static void Raise<T>(this EventHandler<T> @event, object sender, T args) where T : EventArgs
        {
            if (@event != null)
                @event(sender, args);
        }

        public static void Raise(this PropertyChangedEventHandler @event, object sender, string property)
        {
            if (@event != null)
                @event(sender, new PropertyChangedEventArgs(property));
        }

        public static void Raise<T>(this PropertyChangedEventHandler @event, T sender, Expression<Func<T,object>> property)
        {
            if (@event != null)
                @event(sender, new PropertyChangedEventArgs(property.GetMemberInfo().Name));
        }

        public static IEnumerable<T> ToEnumerable<T>(this T item)
        {
            yield return item;
        }

        public static string FullOutput(this Exception x)
        {
            var sb = new StringBuilder();

            while (x != null)
            {
                sb.AppendLine(string.Format("{0}: {1}", x.GetType().Name, x.Message));
                sb.AppendLine("-");
                sb.AppendLine(x.StackTrace);
                sb.AppendLine("-------------");
                x = x.InnerException;
            }
            return sb.ToString();
        }
    }
}