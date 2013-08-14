using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace CustomControls
{
    public sealed partial class CustomColorPicker : UserControl
    {
        public CustomColorPicker()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// The property store the List of colors.
        /// </summary>
        public List<ColorPickerBrush> Colors { get; set; }

        /// <summary>
        /// The dependency Property
        /// This will be used to exposed the selected color
        /// from the ListBox
        /// </summary>
        public Brush SelectedColor
        {
            get { return (Brush)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }
        // Using a DependencyProperty as the backing store for SelectedColor. 
        //This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedColorProperty =
         DependencyProperty.Register("SelectedColor", typeof(Brush),
         typeof(CustomColorPicker), new PropertyMetadata(0));

        /// <summary>
        /// The Event which will be subscribed by the consumer
        /// </summary>
        public event EventHandler ColorSelectedEvent;

        /// <summary>
        /// The below implementation will do the below things:
        /// 1. Get the Colors class Type
        /// 2. Read Runtime properties from the Colots type
        /// 3. Read each property id convert it into Color structure
        /// 4. Put the colors in the Colors List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Colors = new List<ColorPickerBrush>();
            //1
            Type type = typeof(Windows.UI.Colors);
            //2
            PropertyInfo[] Properties =
             type.GetRuntimeProperties().ToArray<PropertyInfo>();
            //3
            foreach (var item in Properties)
            {
                string s = item.Name;
                Color c = (Color)item.GetValue(null, null);
                //4 
                Colors.Add(new ColorPickerBrush()
                {
                    ColorName = item.Name,
                    ColorBrush = new SolidColorBrush(c)
                });
            }
            this.DataContext = this;
        }

        private void lstColorPalette_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ColorPickerBrush objColor = (ColorPickerBrush)lstColorPalette.SelectedItem;
            if (ColorSelectedEvent != null)
            {
                SelectedColor = objColor.ColorBrush;
                ColorSelectedEvent(this, EventArgs.Empty);
            }
        }
    }
}
