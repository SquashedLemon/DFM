using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using DoctorFinder.Mobile.CustomControls;
using DoctorFinder.Mobile.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomSliderFromView), typeof(CustomSliderRenderer))]
namespace DoctorFinder.Mobile.Droid.Renderer
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class CustomSliderRenderer : ViewRenderer<CustomSliderFromView, VerticalSlider>
    {
        VerticalSlider UIVerticalSlider;

        protected override void OnElementChanged(ElementChangedEventArgs<CustomSliderFromView> _E)
        {
            base.OnElementChanged(_E);

            if (this.Control != null && Element != null)
            {
                System.Diagnostics.Debug.WriteLine(">  Control Found: " + Element.Value + "   " + Control.Progress);
            }

            if (this.Control == null)
            {
                // Instantiate the native control and assign it to the Control property with
                // the SetNativeControl method
                UIVerticalSlider = new VerticalSlider(this.Context);
                this.SetNativeControl(UIVerticalSlider);
                this.Control.ProgressChanged += ElementOnPropertyChanged;
            }

            if (_E.OldElement != null)
            {
                // Unsubscribe from event handlers and cleanup any resources
                System.Diagnostics.Debug.WriteLine(">  OldElement: " + Element.Value);
                _E.OldElement.PropertyChanged -= ElementOnPropertyChanged;
            }

            if (_E.NewElement != null)
            {
                // Configure the control and subscribe to event handlers
                Control.Progress = _E.NewElement.Value;
                Control.ProgressTintList = ColorStateList.ValueOf(Xamarin.Forms.Color.FromHex("#ff232f").ToAndroid());
                Control.ProgressTintMode = PorterDuff.Mode.SrcIn;
                Control.SecondaryProgressTintList = ColorStateList.ValueOf(Xamarin.Forms.Color.FromHex("#ff6069").ToAndroid());
                Control.ProgressTintMode = PorterDuff.Mode.SrcIn;
                Control.SetThumb(new ColorDrawable(Android.Graphics.Color.Red));

                System.Diagnostics.Debug.WriteLine(">  New Element: " + Element.Value);
                _E.NewElement.PropertyChanged += ElementOnPropertyChanged;

                var slider = (CustomSliderFromView)_E.NewElement;
                Control.Max = (int)(slider.Maximum - slider.Minimum);
                Control.Progress = (int)(slider.Value - slider.Minimum);
                Control.StopTrackingTouch += Control_StopTrackingTouch;
            }
        }

        void Control_StopTrackingTouch(object sender, SeekBar.StopTrackingTouchEventArgs e)
        {
            var slider = (CustomSliderFromView)Element;
            slider.Value = Control.Progress + slider.Minimum;
            slider.OnStoppedDragging();
            Element.Value = Control.Progress;
        }

        void ElementOnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        void ElementOnPropertyChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(">  Event subscribe fire!: " + Element.Value + "   " + Control.Progress);
            Element.Value = Control.Progress;
        }
    }
#pragma warning restore CS0618 // Type or member is obsolete

    public delegate void VerticalSeekBarStartTrackingTouchEventHandler(object sender, SeekBar.StartTrackingTouchEventArgs args);
    public delegate void VerticalSeekBarStopTrackingTouchEventHandler(object sender, SeekBar.StopTrackingTouchEventArgs args);

    public class VerticalSlider : SeekBar
    {
        #region ctor

        protected VerticalSlider(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        { }

        public VerticalSlider(Context context)
            : base(context)
        { }

        public VerticalSlider(Context context, IAttributeSet attrs)
            : base(context, attrs)
        { }

        public VerticalSlider(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        { }

        #endregion

        #region fields

        private int _min;

        #endregion

        #region properties

#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public int Min
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        {
            get { return _min; }
            set
            {
                if (Min > Progress)
                    Progress = Min;
                _min = value;
                OnSizeChanged(Width, Height, 0, 0);
            }
        }

        public override int Progress
        {
            get
            {
                return base.Progress <= Min ? Min : base.Progress;
            }
            set
            {
                if (value <= Min)
                    base.Progress = Min;
                else if (value >= Max)
                    base.Progress = Max;
                else
                    base.Progress = value;

                OnSizeChanged(Width, Height, 0, 0);
            }
        }

        #endregion

        #region events

        public new event VerticalSeekBarStartTrackingTouchEventHandler StartTrackingTouch;
        public new event VerticalSeekBarStopTrackingTouchEventHandler StopTrackingTouch;

        #endregion

        public override void Draw(Android.Graphics.Canvas canvas)
        {
            canvas.Rotate(-90);
            canvas.Translate(-Height, 0);
            base.OnDraw(canvas);
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(heightMeasureSpec, widthMeasureSpec);
            SetMeasuredDimension(MeasuredHeight, MeasuredWidth);
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(h, w, oldh, oldw);
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (!Enabled)
                return false;

            switch (e.Action)
            {
                case MotionEventActions.Down:
                    StartTrackingTouch?.Invoke(this, new StartTrackingTouchEventArgs(this));
                    Selected = true;
                    Pressed = true;
                    Progress = Max - (int)(Max * e.GetY() / Height);
                    System.Diagnostics.Debug.WriteLine(">  Down: " + Progress);
                    break;
                case MotionEventActions.Move:
                    Progress = Max - (int)(Max * e.GetY() / Height);
                    System.Diagnostics.Debug.WriteLine(">  Move: " + Progress);
                    break;
                case MotionEventActions.Up:
                    StopTrackingTouch?.Invoke(this, new StopTrackingTouchEventArgs(this));
                    Selected = false;
                    Pressed = false;
                    Progress = Max - (int)(Max * e.GetY() / Height);
                    System.Diagnostics.Debug.WriteLine(">  Up: " + Progress);
                    break;
                case MotionEventActions.Cancel:
                    Selected = false;
                    Pressed = false;
                    System.Diagnostics.Debug.WriteLine(">  Cancel: " + Progress);
                    break;
            }

            return true;
        }
    }
}