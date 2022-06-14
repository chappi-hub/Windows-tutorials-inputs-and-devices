using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace GettingStarted_Ink
{
    internal class MyButton : Button
    {
        private const int squareButtonSize = 35;
        private const double buttonMargin = 15.0;

        public MyButton()
        {
            this.Width = squareButtonSize;
            this.Height = squareButtonSize;
            this.Margin = new Thickness(buttonMargin);
            this.Background = new SolidColorBrush(Colors.White);
        }
    }
}
