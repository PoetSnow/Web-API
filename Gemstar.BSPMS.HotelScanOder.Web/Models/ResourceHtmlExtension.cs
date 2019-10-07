using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc.Html;

namespace System.Web.Mvc.Html
{
        /// <summary>
        /// html helper的label标签语言资源扩展方法
        /// </summary>
        public static class HtmlHelperLabelExtensions
        {
            /// <summary>
            /// 从当前资源文件中加载指定名称的当前语言的对应字符串
            /// </summary>
            /// <param name="name">要加载的资源名称</param>
            /// <returns>资源名称对应的当前语言的字符串，如果不存在则返回名称本身</returns>
            private static string GetResourceString(string name)
            {
                var resourceManager = GlobalResource.ResourceManager;
                var cultureInfo = GlobalResource.Culture;
                var data = name;
                try
                {
                    data = resourceManager.GetString(name);
                }
                catch
                {
                    data = null;
                }
                if (string.IsNullOrEmpty(data))
                {
                    data = name;
                }
                return data;
            }
            /// <summary>
            /// 返回一个 HTML label 元素以及由指定表达式表示的属性的属性名称。
            /// </summary>
            /// <typeparam name="TModel">模型的类型。</typeparam>
            /// <typeparam name="TValue">值的类型。</typeparam>
            /// <param name="html">此方法扩展的 HTML 帮助器实例。</param>
            /// <param name="expression">一个表达式，用于标识要显示的属性。</param>
            /// <param name="labelText">要显示的标签文本。</param>
            /// <param name="htmlAttributes">一个对象，其中包含要为该元素设置的 HTML 特性。</param>
            /// <returns>一个 HTML label 元素以及由表达式表示的属性的属性名称。</returns>
            public static MvcHtmlString ResourceLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText, IDictionary<string, object> htmlAttributes)
            {
                labelText = GetResourceString(labelText);
                return html.LabelFor(expression, labelText, htmlAttributes);
            }
            
            /// <summary>
            /// 返回一个 HTML label 元素以及由指定表达式表示的属性的属性名称。
            /// </summary>
            /// <typeparam name="TModel">模型的类型。</typeparam>
            /// <typeparam name="TValue">值。</typeparam>
            /// <param name="html">此方法扩展的 HTML 帮助器实例。</param>
            /// <param name="expression">一个表达式，用于标识要显示的属性。</param>
            /// <param name="labelText">标签文本。</param>
            /// <param name="htmlAttributes">一个对象，其中包含要为该元素设置的 HTML 特性。</param>
            /// <returns>一个 HTML label 元素以及由表达式表示的属性的属性名称。</returns>
            public static MvcHtmlString ResourceLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText, object htmlAttributes)
            {
                labelText = GetResourceString(labelText);
                return html.LabelFor(expression, labelText, htmlAttributes);
            }
            /// <summary>
            /// 返回一个 HTML label 元素以及由指定表达式表示的属性的属性名称。
            /// </summary>
            /// <typeparam name="TModel">模型的类型。</typeparam>
            /// <typeparam name="TValue">值的类型。</typeparam>
            /// <param name="html">此方法扩展的 HTML 帮助器实例。</param>
            /// <param name="expression">一个表达式，用于标识要显示的属性。</param>
            /// <param name="htmlAttributes">一个对象，其中包含要为该元素设置的 HTML 特性。</param>
            /// <returns>一个 HTML label 元素以及由表达式表示的属性的属性名称。</returns>
            public static MvcHtmlString ResourceLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes)
            {
                var labelText = GetDisplayName(expression);
                return html.LabelFor(expression, labelText, htmlAttributes);
            }
            /// <summary>
            /// 返回一个 HTML label 元素以及由指定表达式表示的属性的属性名称。
            /// </summary>
            /// <typeparam name="TModel">模型的类型。</typeparam>
            /// <typeparam name="TValue">值。</typeparam>
            /// <param name="html">此方法扩展的 HTML 帮助器实例。</param>
            /// <param name="expression">一个表达式，用于标识要显示的属性。</param>
            /// <param name="htmlAttributes">一个对象，其中包含要为该元素设置的 HTML 特性。</param>
            /// <returns>一个 HTML label 元素以及由表达式表示的属性的属性名称。</returns>
            public static MvcHtmlString ResourceLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
            {
                var labelText = GetDisplayName(expression);
                return html.LabelFor(expression, labelText, htmlAttributes);
            }
            /// <summary>
            /// 使用标签文本，返回一个 HTML label 元素以及由指定表达式表示的属性的属性名称。
            /// </summary>
            /// <typeparam name="TModel">模型的类型。</typeparam>
            /// <typeparam name="TValue">值的类型。</typeparam>
            /// <param name="html">此方法扩展的 HTML 帮助器实例。</param>
            /// <param name="expression">一个表达式，用于标识要显示的属性。</param>
            /// <param name="labelText">要显示的标签文本。</param>
            /// <returns>一个 HTML label 元素以及由表达式表示的属性的属性名称。</returns>
            public static MvcHtmlString ResourceLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText)
            {
                labelText = GetResourceString(labelText);
                return html.LabelFor(expression, labelText);
            }
            /// <summary>
            /// 返回一个 HTML label 元素以及由指定表达式表示的属性的属性名称。
            /// </summary>
            /// <typeparam name="TModel">模型的类型。</typeparam>
            /// <typeparam name="TValue">值的类型。</typeparam>
            /// <param name="html">此方法扩展的 HTML 帮助器实例。</param>
            /// <param name="expression">一个表达式，用于标识要显示的属性。</param>
            /// <returns>一个 HTML label 元素以及由表达式表示的属性的属性名称。</returns>
            public static MvcHtmlString ResourceLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
            {
                var labelText = GetDisplayName(expression);
                return html.LabelFor(expression, labelText);
            }
            private static string GetDisplayName<TModel, TValue>(Expression<Func<TModel, TValue>> expression)
            {
                MemberExpression memberExpression = expression.Body as MemberExpression;
                if (memberExpression == null)
                {
                    memberExpression = ((UnaryExpression)expression.Body).Operand as MemberExpression;
                }
                if (memberExpression != null)
                {
                    var info = memberExpression.Member;
                    var displayAttribute = info.CustomAttributes.FirstOrDefault(w => w.AttributeType.FullName == "System.ComponentModel.DataAnnotations.DisplayAttribute");
                    if (displayAttribute != null)
                    {
                        var name = displayAttribute.NamedArguments.FirstOrDefault(w => w.MemberName == "Name");
                        if (name != null)
                        {
                            var display = name.TypedValue.Value?.ToString();
                            return GetResourceString(display);
                        }
                    }
                }
                return "";
            }
        }
  
}