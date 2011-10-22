using System;
using System.Collections.Generic;
using System.Text;

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

        public static IEnumerable<T> Pipeline<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var i in items)
            {
                action(i);
                yield return i;
            }
        }

        public static void IfNotNull<T>(this T item, Action<T> actionOnItem) where T : class
        {
            if (item != null)
                actionOnItem(item);
        }

        public static O IfNotNull<T, O>(this T item, Func<T, O> passThroughOnItem) where T : class
        {
            return item != null ? passThroughOnItem(item) : default(O);
        }

        public static void Raise<T>(this EventHandler<T> @event, object sender, T args) where T : EventArgs
        {
            if (@event != null)
                @event(sender, args);
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