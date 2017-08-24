using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace SlipAndSlide
{
    public partial class SlipAndSlidePage : ContentPage
    {
        public SlipAndSlidePage()
        {
            InitializeComponent();

            var template = new DataTemplate(() =>
            {
                var rt = new RightContext();
                rt.ActionCommand = new Command((object obj) => {
                    items.Remove(obj.ToString());            
                });

                var lt = new LeftContext();
                lt.ActionCommand = new Command((object obj) => { ShowAlert(obj); });

                var x = new PanningViewCell(new TheMainDisplay(), lt, rt);

                return x;
            });

            myList.ItemTemplate = template;
            myList.ItemsSource = items;
            myList.ItemSelected += (sender, e) => { myList.SelectedItem = null; };
        }

        public async void ShowAlert(object obj)
        {
            await DisplayAlert("Scheduled", "Item has been scheudled", "OK");

        }

		ObservableCollection<string> items = new ObservableCollection<string>() {
			"One",
			"Two",
			"Three",
			"Four",
			"Five"
		};

	}
}
