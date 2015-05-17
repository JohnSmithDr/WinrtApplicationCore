using System;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace JohnSmithDr.Application.Behaviors
{
    public class EnumVisualStateBehavior : DependencyObject, IBehavior
    {
        #region IBehavior

        public DependencyObject AssociatedObject { get; private set; }

        public void Attach(DependencyObject associatedObject)
        {
            var control = associatedObject as Control;

            if (control == null)
                throw new ArgumentException(
                    "EnumVisualStateBehavior can only be attached to Control");

            AssociatedObject = associatedObject;
        }

        public void Detach()
        {
            AssociatedObject = null;
        }

        #endregion

        #region public bool UseTransitions

        public bool UseTransitions
        {
            get { return (bool)GetValue(UseTransitionsProperty); }
            set { SetValue(UseTransitionsProperty, value); }
        }

        public static readonly DependencyProperty UseTransitionsProperty =
            DependencyProperty.Register(
                "UseTransitions",
                typeof(bool),
                typeof(EnumVisualStateBehavior),
                new PropertyMetadata(true));

        #endregion

        #region public object State

        public object State
        {
            get { return (object)GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }

        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register(
                "State",
                typeof(object),
                typeof(EnumVisualStateBehavior),
                new PropertyMetadata(null, OnStatePropertyChanged));

        private static void OnStatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var source = d as EnumVisualStateBehavior;
            source.OnStateChanged((object)e.OldValue, (object)e.NewValue);
        }

        private void OnStateChanged(object oldValue, object newValue)
        {
            if (this.AssociatedObject != null && newValue != null)
            {
                VisualStateManager.GoToState(this.AssociatedObject as Control, newValue.ToString(), true);
            }
        }

        #endregion
    }
}
