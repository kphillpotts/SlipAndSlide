using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace SlipAndSlide
{
    public class BaseSwipeAction : ContentView
    {
        public enum AfterSwipe
        {
            DoNothing = 0,
            ReturnToView = 1,
        }

        public static readonly BindableProperty ActionCommandProperty = 
            BindableProperty.Create("ActionCommand", typeof(ICommand), typeof(BaseSwipeAction), null);

		public ICommand ActionCommand
		{
			get { return (ICommand)GetValue(ActionCommandProperty); }
			set { SetValue(ActionCommandProperty, value); }
		}

        public static readonly BindableProperty AfterSwipeActionProperty =
            BindableProperty.Create("AfterSwipeAction", typeof(AfterSwipe), typeof(BaseSwipeAction), AfterSwipe.ReturnToView);

        public AfterSwipe AfterSwipeAction
        {
            get { return (AfterSwipe)GetValue(AfterSwipeActionProperty); }
            set { SetValue(AfterSwipeActionProperty, value); }
        }

        public static readonly BindableProperty InactiveBackgroundColorProperty = 
            BindableProperty.Create("InactiveBackgroundColor", typeof(Color), typeof(BaseSwipeAction), Color.Silver);

        public Color InactiveBackgroundColor
        {
            get {return (Color) GetValue(InactiveBackgroundColorProperty);}
            set { SetValue(InactiveBackgroundColorProperty, value); }
        }


		public static readonly BindableProperty ActiveBackgroundColorProperty = 
            BindableProperty.Create("ActiveBackgroundColor", typeof(Color), typeof(BaseSwipeAction), Color.Green);

		public Color ActiveBackgroundColor
		{
			get { return (Color)GetValue(ActiveBackgroundColorProperty); }
			set { SetValue(ActiveBackgroundColorProperty, value); }
		}



	}
}
