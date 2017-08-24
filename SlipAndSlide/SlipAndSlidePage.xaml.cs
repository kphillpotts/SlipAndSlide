using Xamarin.Forms;

namespace SlipAndSlide
{
    public partial class SlipAndSlidePage : ContentPage
    {
        public SlipAndSlidePage()
        {
            InitializeComponent();

			//myList.ItemTemplate =
                      //new DataTemplate(typeof(PanningViewCell<TheMainDisplay, LeftContext, RIghtContext>));

            var template = new DataTemplate(() =>
            {
                var rt = new RIghtContext();
                rt.ActionCommand = new Command((object obj) => {
                    System.Diagnostics.Debug.WriteLine("Hello");
                });

                var x = new PanningViewCell(new TheMainDisplay(), new LeftContext(), rt);

                return x;
            });

            myList.ItemTemplate = template;
            myList.ItemsSource = items;
            myList.ItemSelected += (sender, e) => { myList.SelectedItem = null; };
        }

		string[] items = {
			"Glenn",
			"Kym",
			"Rob",
			"Judy",
			"Sam"
		};

	}
}
