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

        private double buttonSize = 25.0;
        private double buttonMargin = 10.0;
        private double borderThickness = 2.0;
        private InkDrawingAttributes attribs;
        public InkCanvas Canvas { get; set; }

        private Color color;

        public Color Color
        {
            get { return color; }
            set { 
                color = value;
                this.Background = new SolidColorBrush(color);
            }
        }

        private bool hasStroke = false;

        public bool HasStroke
        {
            get { return hasStroke; }
            set { 
                hasStroke = value;
                if (hasStroke)
                {
                    this.BorderThickness = new Thickness(borderThickness);
                    this.BorderBrush = new SolidColorBrush(Colors.Black);
                }
                else
                {
                    this.BorderThickness = new Thickness();
                }
            }
        }


        public CostumPenButton()
        {
            this.Width = buttonSize;
            this.Height = buttonSize;
            this.CornerRadius = new Windows.UI.Xaml.CornerRadius(buttonSize);
            this.Margin = new Thickness(buttonMargin);

            attribs = new InkDrawingAttributes();
            attribs.FitToCurve = true;
            attribs.IgnorePressure = false;
        }

        public void Activate()
        {
            attribs.Color = Color;
            Canvas.InkPresenter.UpdateDefaultDrawingAttributes(attribs);
        }
    }
}
