using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace GettingStarted_Ink
{
    internal class CostumPenButton : Button
    {

        private double buttonSize = 35.0;
        private double buttonMargin = 15.0;
        private double borderThickness = 3.0;

        private InkDrawingAttributes attribs;
        public InkCanvas Canvas { get; set; }

        private Color color;

        public Color Color
        {
            get { return color; }
            set
            {
                color = value;
                this.Background = new SolidColorBrush(color);
            }
        }

        private bool isActive;

        public bool IsActive
        {
            get { return isActive; }
            set
            {
                isActive = value;

                if (isActive)
                {
                    UIElementCollection parentChildren = (this.Parent as StackPanel).Children;
                    foreach (var penButton in parentChildren)
                    {
                        if (penButton is CostumPenButton)
                        {
                            if ((penButton as CostumPenButton) != this)
                            {
                                (penButton as CostumPenButton).IsActive = false;
                            };
                        }
                    }

                    this.BorderBrush = new SolidColorBrush(Colors.Black);
                }
                else
                {
                    this.BorderBrush = new SolidColorBrush(Color);
                }
            }
        }


        public CostumPenButton()
        {
            this.Width = buttonSize;
            this.Height = buttonSize;
            this.CornerRadius = new Windows.UI.Xaml.CornerRadius(buttonSize);
            this.Margin = new Thickness(buttonMargin);
            this.BorderThickness = new Thickness(borderThickness);
            this.IsActive = false;


            attribs = new InkDrawingAttributes();
            attribs.FitToCurve = true;
            attribs.IgnorePressure = false;
        }

        public void Activate()
        {
            IsActive = true;
            attribs.Color = Color;
            Canvas.InkPresenter.UpdateDefaultDrawingAttributes(attribs);
        }
    }
}
