using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace GettingStarted_Ink
{
    internal class CostumPenButton : Button
    {
        private const double defaultPenThickness = 2.0;
        private const double buttonSize = 35.0;
        private const double buttonMargin = 15.0;
        private const double defaultBorderThickness = 3.0;

        private InkDrawingAttributes attribs;
        public InkCanvas Canvas { get; set; }

        private Color color;

        public Color Color
        {
            get { return color; }
            set
            {
                color = value;
                attribs.Color = Color;
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

        private bool hasIcon;

        public bool HasIcon
        {
            get { return hasIcon; }
            set { 
                hasIcon = value;

                if (hasIcon)
                {
                    this.CornerRadius = new CornerRadius(0);
                    this.Margin = new Thickness(0);
                }
                else
                {
                    this.CornerRadius = new  CornerRadius(buttonSize);
                    this.Margin = new Thickness(buttonMargin);
                }
            }
        }
        public double PenThickness {
            get {
                return PenThickness;
            }
            set {
                attribs.Size = new Size(value, value);
            } 
        }

        public CostumPenButton()
        {
            this.BorderThickness = new Thickness(defaultBorderThickness);

            this.Width = buttonSize;
            this.Height = buttonSize;

            this.IsActive = false;
            this.HasIcon = false;


            attribs = new InkDrawingAttributes();
            attribs.FitToCurve = true;
            attribs.IgnorePressure = false;
            attribs.PenTip = PenTipShape.Circle;
            
            PenThickness = defaultPenThickness;
        }

        public void Activate()
        {
            IsActive = true;
            Canvas.InkPresenter.UpdateDefaultDrawingAttributes(attribs);
        }
    }
}
