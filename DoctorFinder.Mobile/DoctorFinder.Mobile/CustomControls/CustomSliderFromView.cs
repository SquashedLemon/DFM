using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DoctorFinder.Mobile.CustomControls
{
    public class CustomSliderFromView : View
    {
        /// <summary>
        /// The ValueChanged event is fired when the Value property changes.
        /// </summary>
        public event EventHandler<ValueChangedEventArgs> ValueChanged;
        /// <summary>
        /// The MaximumChanged event is fired when the Maximum of the slider changes!
        /// </summary>
        public event EventHandler<ValueChangedEventArgs> MaximumChanged;
        /// <summary>
        /// The MinimumChanged event is fired when the Minimum of the slider changes!
        /// </summary>
        public event EventHandler<ValueChangedEventArgs> MinimumChanged;
#pragma warning disable CS0618 // 'BindableProperty.Create<TDeclarer, TPropertyType>(Expression<Func<TDeclarer, TPropertyType>>, TPropertyType, BindingMode, BindableProperty.ValidateValueDelegate<TPropertyType>, BindableProperty.BindingPropertyChangedDelegate<TPropertyType>, BindableProperty.BindingPropertyChangingDelegate<TPropertyType>, BindableProperty.CoerceValueDelegate<TPropertyType>, BindableProperty.CreateDefaultValueDelegate<TDeclarer, TPropertyType>)' is obsolete: 'Create<> (generic) is obsolete as of version 2.1.0 and is no longer supported.'
        /// <summary>
        /// Value of the Slider ranging between Maximum and Minimum. This is a bindable property.
        /// </summary>
        public static readonly BindableProperty ValueProperty = BindableProperty.Create<CustomSliderFromView, int>(p => p.Value, 40);
#pragma warning restore CS0618 // 'BindableProperty.Create<TDeclarer, TPropertyType>(Expression<Func<TDeclarer, TPropertyType>>, TPropertyType, BindingMode, BindableProperty.ValidateValueDelegate<TPropertyType>, BindableProperty.BindingPropertyChangedDelegate<TPropertyType>, BindableProperty.BindingPropertyChangingDelegate<TPropertyType>, BindableProperty.CoerceValueDelegate<TPropertyType>, BindableProperty.CreateDefaultValueDelegate<TDeclarer, TPropertyType>)' is obsolete: 'Create<> (generic) is obsolete as of version 2.1.0 and is no longer supported.'
#pragma warning disable CS0618 // 'BindableProperty.Create<TDeclarer, TPropertyType>(Expression<Func<TDeclarer, TPropertyType>>, TPropertyType, BindingMode, BindableProperty.ValidateValueDelegate<TPropertyType>, BindableProperty.BindingPropertyChangedDelegate<TPropertyType>, BindableProperty.BindingPropertyChangingDelegate<TPropertyType>, BindableProperty.CoerceValueDelegate<TPropertyType>, BindableProperty.CreateDefaultValueDelegate<TDeclarer, TPropertyType>)' is obsolete: 'Create<> (generic) is obsolete as of version 2.1.0 and is no longer supported.'
        /// <summary>
        /// Identifies the Maximum bindable property.
        /// </summary>
        public static readonly BindableProperty MaximumProperty = BindableProperty.Create<CustomSliderFromView, int>(p => p.Maximum, 5000);
#pragma warning restore CS0618 // 'BindableProperty.Create<TDeclarer, TPropertyType>(Expression<Func<TDeclarer, TPropertyType>>, TPropertyType, BindingMode, BindableProperty.ValidateValueDelegate<TPropertyType>, BindableProperty.BindingPropertyChangedDelegate<TPropertyType>, BindableProperty.BindingPropertyChangingDelegate<TPropertyType>, BindableProperty.CoerceValueDelegate<TPropertyType>, BindableProperty.CreateDefaultValueDelegate<TDeclarer, TPropertyType>)' is obsolete: 'Create<> (generic) is obsolete as of version 2.1.0 and is no longer supported.'
#pragma warning disable CS0618 // 'BindableProperty.Create<TDeclarer, TPropertyType>(Expression<Func<TDeclarer, TPropertyType>>, TPropertyType, BindingMode, BindableProperty.ValidateValueDelegate<TPropertyType>, BindableProperty.BindingPropertyChangedDelegate<TPropertyType>, BindableProperty.BindingPropertyChangingDelegate<TPropertyType>, BindableProperty.CoerceValueDelegate<TPropertyType>, BindableProperty.CreateDefaultValueDelegate<TDeclarer, TPropertyType>)' is obsolete: 'Create<> (generic) is obsolete as of version 2.1.0 and is no longer supported.'
        /// <summary>
        /// Identifies the Minimum bindable property.
        /// </summary>
        public static readonly BindableProperty MinimumProperty = BindableProperty.Create<CustomSliderFromView, int>(p => p.Minimum, 0);
#pragma warning restore CS0618 // 'BindableProperty.Create<TDeclarer, TPropertyType>(Expression<Func<TDeclarer, TPropertyType>>, TPropertyType, BindingMode, BindableProperty.ValidateValueDelegate<TPropertyType>, BindableProperty.BindingPropertyChangedDelegate<TPropertyType>, BindableProperty.BindingPropertyChangingDelegate<TPropertyType>, BindableProperty.CoerceValueDelegate<TPropertyType>, BindableProperty.CreateDefaultValueDelegate<TDeclarer, TPropertyType>)' is obsolete: 'Create<> (generic) is obsolete as of version 2.1.0 and is no longer supported.'

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
    }
}
