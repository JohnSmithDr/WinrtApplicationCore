using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace Windows.UI.Xaml
{
    public static class VisualTreeExtensions
    {
        /// <summary>
        /// Get the child elements.
        /// </summary>
        public static IEnumerable<DependencyObject> GetChildren(this DependencyObject source)
        {
            var popup = source as Popup;

            if (popup != null)
            {
                if (popup.Child != null)
                {
                    yield return popup.Child;
                    yield break;
                }
            }

            var count = VisualTreeHelper.GetChildrenCount(source);

            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(source, i);
                yield return child;
            }
        }

        /// <summary>
        /// Get the descendants.
        /// </summary>
        public static IEnumerable<DependencyObject> GetDescendants(this DependencyObject source)
        {
            if (source == null)
            {
                yield break;
            }

            var queue = new Queue<DependencyObject>();

            var popup = source as Popup;

            if (popup != null)
            {
                if (popup.Child != null)
                {
                    queue.Enqueue(popup.Child);
                    yield return popup.Child;
                }
            }
            else
            {
                var count = VisualTreeHelper.GetChildrenCount(source);

                for (int i = 0; i < count; i++)
                {
                    var child = VisualTreeHelper.GetChild(source, i);
                    queue.Enqueue(child);
                    yield return child;
                }
            }

            while (queue.Count > 0)
            {
                var parent = queue.Dequeue();

                popup = parent as Popup;

                if (popup != null)
                {
                    if (popup.Child != null)
                    {
                        queue.Enqueue(popup.Child);
                        yield return popup.Child;
                    }
                }
                else
                {
                    var count = VisualTreeHelper.GetChildrenCount(parent);

                    for (int i = 0; i < count; i++)
                    {
                        var child = VisualTreeHelper.GetChild(parent, i);
                        yield return child;
                        queue.Enqueue(child);
                    }
                }
            }
        }

        /// <summary>
        /// Get the descendants of the given type.
        /// </summary>
        public static IEnumerable<T> GetDescendants<T>(this DependencyObject start) where T : DependencyObject
        {
            return start.GetDescendants().OfType<T>();
        }

        /// <summary>
        /// Gets the ancestors, starting with parent and going towards the visual tree root.
        /// </summary>
        public static IEnumerable<DependencyObject> GetAncestors(this DependencyObject source)
        {
            var parent = VisualTreeHelper.GetParent(source);

            while (parent != null)
            {
                yield return parent;
                parent = VisualTreeHelper.GetParent(parent);
            }
        }

        /// <summary>
        /// Gets the ancestors of a given type, starting with parent and going towards the visual tree root.
        /// </summary>
        public static IEnumerable<T> GetAncestors<T>(this DependencyObject source) where T : DependencyObject
        {
            return source.GetAncestors().OfType<T>();
        }

        /// <summary>
        /// Gets the siblings, including the start element.
        /// </summary>
        public static IEnumerable<DependencyObject> GetSiblings(this DependencyObject source)
        {
            var parent = VisualTreeHelper.GetParent(source);

            if (parent == null)
            {
                yield return source;
            }
            else
            {
                var count = VisualTreeHelper.GetChildrenCount(parent);

                for (int i = 0; i < count; i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i);
                    yield return child;
                }
            }
        }
    }
}