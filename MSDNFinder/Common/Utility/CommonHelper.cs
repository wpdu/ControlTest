using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace WinSFA.Common.Utility
{
    public static class CommonHelper
    {
        public async static void InvokeOnUI(Action action)
        {
            await Window.Current.Content.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, delegate
            {
                if (action != null)
                {
                    action();
                }
            });
        }

        public static void DelayInvokeOnUI(Action action, int ms)
        {
            Task.Run(async () =>
            {
                await Task.Delay(ms);
                await Window.Current.Content.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, delegate
                {
                    if (action != null)
                    {
                        action();
                    }
                });
            });
        }

        public static string GetPropertyName<T>(Expression<Func<T, object>> expr)
        {
            var rtn = "";
            if (expr.Body is UnaryExpression)
            {
                rtn = ((MemberExpression)((UnaryExpression)expr.Body).Operand).Member.Name;
            }
            else if (expr.Body is MemberExpression)
            {
                rtn = ((MemberExpression)expr.Body).Member.Name;
            }
            else if (expr.Body is ParameterExpression)
            {
                rtn = ((ParameterExpression)expr.Body).Type.Name;
            }
            return rtn;
        }
    }
}