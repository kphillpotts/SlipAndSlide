using System;
using System.Diagnostics;
using Xamarin;
using Xamarin.Forms;

namespace SlipAndSlide
{
	//public class PanningViewCell<MainView, LeftSwipe, RightSwipe> : ViewCell
		//where MainView : View, new()
		//where LeftSwipe : View, new()
        //where RightSwipe : View, new()
    public class PanningViewCell : ViewCell
	{
        //MainView mainView;
        //LeftSwipe leftView;
        //RightSwipe rightView;

        View mainCellView;
        BaseSwipeAction leftSideView;
        BaseSwipeAction rightSideView;

		public double MinimumSwipeAmount { get; set; } = 100;

        public bool HasLeftSwipe
        {
            get
            {
                return leftSideView != null;
            }
        }

        public bool HasRightSwipe
        {
            get
            {
                return rightSideView != null;
            }
        }

        public PanningViewCell(View mainView, BaseSwipeAction leftSideView, BaseSwipeAction rightSideView)
		{
            mainCellView = mainView;
            this.leftSideView = leftSideView;
            this.rightSideView = rightSideView;

            // put it all in a grid
			var grid = new Grid();
			grid.RowDefinitions.Add(new RowDefinition());
			grid.ColumnDefinitions.Add(new ColumnDefinition());

            //mainView = new MainView();
            //leftView = new LeftSwipe();
            //rightView = new RightSwipe();

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

            //grid.Children.Add(leftView, 0, 0);
            //grid.Children.Add(rightView, 0, 0);
			grid.Children.Add(mainView, 0, 0);

			var panner = new PanGestureRecognizer();
			panner.PanUpdated += async(s,e) =>
			{
                //if (e.StatusType == Ge)
                Debug.WriteLine($"PanGesture: {e.StatusType} - {e.TotalX}, {e.TotalY}");

                if (e.StatusType == GestureStatus.Running)
                {
                    // if we don't have either swipe panel... we done here.
                    if (!HasLeftSwipe && !HasRightSwipe)
                        return;


                    if (e.TotalX < 0) // moving towards the left, exposing the right view
                    {
                        if (HasRightSwipe)
                        {
                            // only show the appropriate swipe view
                            rightSideView.IsVisible = true;
                            leftSideView.IsVisible = false;
                        }

                    }


                    // show the appropriate swipe menu based on direction
                    if (HasRightSwipe)
                        rightSideView.IsVisible = e.TotalX < 0 ? true : false;

                    if (HasLeftSwipe)
                        leftSideView.IsVisible = e.TotalX > 0 ? true : false;

                    // slide the mainview
                    mainView.TranslationX = e.TotalX;
                }

                if (e.StatusType == GestureStatus.Completed)
                {
                    // haven't met left or right swipe distances
                    if (mainView.TranslationX < MinimumSwipeAmount &&
                        mainView.TranslationX > -MinimumSwipeAmount)
                    {
						await mainView.TranslateTo(0, 0, 250, Easing.SinInOut);
						rightSideView.IsVisible = leftSideView.IsVisible = false;
					}
                    else
                    {
                        if (mainView.TranslationX < MinimumSwipeAmount)
                        {
                            await mainView.TranslateTo(-mainView.Width, 0, 250, Easing.SinInOut);
                            //mainView.IsVisible = false;
                            rightSideView.ActionCommand.Execute(this.BindingContext);

                        }
                        else
                        {
                            await mainView.TranslateTo(mainView.Width, 0, 250, Easing.SinInOut);
                            //mainView.IsVisible = false;
                        }


					}

                }
                //System.Diagnostics.Debug.WriteLine(e.TotalX.ToString());
                //mainView.TranslationX = MinimumSwipeAmount;
                //mainView.TranslateTo(e.TotalX, 0);
				//if (e.TotalX < -MinimumSwipeAmount)
				//{
				//	// Its a swipe left
				//	hiddenView.IsVisible = true;
				//	mainView.IsVisible = false;
				//}
				//else if (e.TotalX > MinimumSwipeAmount)
				//{
				//	// Its a swipe right
				//	hiddenView.IsVisible = false;
				//	mainView.IsVisible = true;
				//}
			};
			grid.GestureRecognizers.Add(panner);
			View = grid;
		}
	}
}
