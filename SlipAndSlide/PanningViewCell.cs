using System;
using System.Diagnostics;
using Xamarin;
using Xamarin.Forms;

namespace SlipAndSlide
{
    public class PanningViewCell : ViewCell
	{
        View mainCellView;
        BaseSwipeAction leftSideView;
        BaseSwipeAction rightSideView;

		public double MinimumSwipeAmount { get; set; } = 80;

        public bool HasLeftSwipe => leftSideView != null;

        public bool HasRightSwipe => rightSideView != null;

        public PanningViewCell(View mainView, BaseSwipeAction leftSideView, BaseSwipeAction rightSideView)
		{
            // get references to our bits
            mainCellView = mainView;
            this.leftSideView = leftSideView;
            this.rightSideView = rightSideView;

            // lay it out in a grid
			var grid = new Grid();
			grid.RowDefinitions.Add(new RowDefinition());
			grid.ColumnDefinitions.Add(new ColumnDefinition());

            // make swipe views invisible so they don't show through
            if (HasLeftSwipe)
            {
                leftSideView.IsVisible = false;
                grid.Children.Add(leftSideView, 0, 0);
            }

            if (HasRightSwipe)
            {
                rightSideView.IsVisible = false;
                grid.Children.Add(rightSideView, 0, 0);
            }

			grid.Children.Add(mainView, 0, 0);

			var panner = new PanGestureRecognizer();
            
			panner.PanUpdated += async(s,e) =>
			{

                if (e.StatusType == GestureStatus.Running)
                {
                    // if we don't have either swipe panel... we done here.
                    if (!HasLeftSwipe && !HasRightSwipe)
                        return;

                    // show the appropriate swipe menu based on direction
                    if (HasRightSwipe)
                    {
                        rightSideView.BackgroundColor = (e.TotalX < -MinimumSwipeAmount)
                                    ? rightSideView.ActiveBackgroundColor
                                    : rightSideView.InactiveBackgroundColor;

                        rightSideView.IsVisible = e.TotalX < 0 ? true : false;
                    }

                    if (HasLeftSwipe)
                    {
                        leftSideView.BackgroundColor = (e.TotalX > MinimumSwipeAmount) 
                                    ? leftSideView.ActiveBackgroundColor 
                                    : leftSideView.InactiveBackgroundColor;

                        leftSideView.IsVisible = e.TotalX > 0 ? true : false;
                    }

                    // slide the mainview
                    mainView.TranslationX = e.TotalX;
                }

                if (e.StatusType == GestureStatus.Completed)
                {
                    // haven't met left or right minimum swipe distances
                    if (mainView.TranslationX < MinimumSwipeAmount &&
                        mainView.TranslationX > -MinimumSwipeAmount)
                    {
                        // slide back to just show the mainview
						await mainView.TranslateTo(0, 0, 250, Easing.SinInOut);
						rightSideView.IsVisible = leftSideView.IsVisible = false;
					}
                    else
                    {
                        // show the full view
                        if (mainView.TranslationX < MinimumSwipeAmount)
                        {

                            if (rightSideView.AfterSwipeAction == BaseSwipeAction.AfterSwipe.ReturnToView)
                                await mainView.TranslateTo(0, 0, 250, Easing.SinInOut);
                            else
                                await mainView.TranslateTo(-mainView.Width, 0, 250, Easing.SinInOut);

                            if (rightSideView.ActionCommand != null)
                            {
                                rightSideView.ActionCommand.Execute(this.BindingContext);
                            }

                        }
                        else
                        {
                            if (leftSideView.AfterSwipeAction == BaseSwipeAction.AfterSwipe.ReturnToView)
                                await mainView.TranslateTo(0, 0, 250, Easing.SinInOut);
                            else
                                await mainView.TranslateTo(mainView.Width, 0, 250, Easing.SinInOut);

                            if (leftSideView.ActionCommand != null)
                            {
                                leftSideView.ActionCommand.Execute(this.BindingContext);

                            }

                        }
                    }
                }

                if (e.StatusType == GestureStatus.Canceled)
                {
                    await mainView.TranslateTo(0, 0, 250, Easing.SinInOut);
                    rightSideView.IsVisible = leftSideView.IsVisible = false;
                }
            };
			grid.GestureRecognizers.Add(panner);
			View = grid;
		}
	}
}
