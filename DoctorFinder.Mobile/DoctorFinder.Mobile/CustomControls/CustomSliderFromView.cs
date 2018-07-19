using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DoctorFinder.Mobile.CustomControls
{
    public class CustomSliderFromView : View
    {
        public event EventHandler StoppedDragging;
        public event EventHandler<ValueChangedEventArgs> ValueChanged;
        public event EventHandler<ValueChangedEventArgs> MaximumChanged;
        public event EventHandler<ValueChangedEventArgs> MinimumChanged;

        public static readonly BindableProperty ValueProperty =
            BindableProperty.Create(nameof(Value), typeof(int), typeof(CustomSliderFromView), 40);
        public static readonly BindableProperty MaximumProperty =
            BindableProperty.Create(nameof(Maximum), typeof(int), typeof(CustomSliderFromView), 100);
        public static readonly BindableProperty MinimumProperty =
            BindableProperty.Create(nameof(Minimum), typeof(int), typeof(CustomSliderFromView), 0);

        public int Value
        {
            get
            {
                return (int)GetValue(ValueProperty);
            }

            set
            {
                ValueChanged?.Invoke(this, new ValueChangedEventArgs((int)GetValue(ValueProperty), value));
                this.SetValue(ValueProperty, value);
            }
        }
        /// <summary>
        /// Gets or sets the maximum selectable value for the Slider. This is a bindable property.
        /// </summary>
        public int Maximum
        {
            get
            {
                return (int)GetValue(MaximumProperty);
            }

            set
            {
                MaximumChanged?.Invoke(this, new ValueChangedEventArgs((int)GetValue(MaximumProperty), value));
                this.SetValue(MaximumProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the minimum selectable value for the Slider. This is a bindable property.
        /// </summary>
        public int Minimum
        {
            get
            {
                return (int)GetValue(MinimumProperty);
            }

            set
            {
                MinimumChanged?.Invoke(this, new ValueChangedEventArgs((int)GetValue(MinimumProperty), value));
                this.SetValue(MinimumProperty, value);
            }
        }

        public void OnStoppedDragging()
        {
            StoppedDragging?.Invoke(this, EventArgs.Empty);
        }
    }
}
