using System;
using System.Diagnostics;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;

using Windows.Foundation;

// TODO: animate layout (position)?

namespace WwiseTool.Controls {
    public class DynamicContainer : ContentControl {
        Storyboard board;

        public DynamicContainer() {
            // Animation setup:
            SetupAnimation();
        }

        void SetupAnimation() {
            // TODO: move animation/board out to Resources?
            DoubleAnimation anim = new DoubleAnimation() {
                From = 0, To = 1, Duration = TimeSpan.FromSeconds(0.40),
                EasingFunction = new QuarticEase() { EasingMode = EasingMode.EaseOut },
                EnableDependentAnimation = true
            };
            Storyboard.SetTarget(anim, this);
            Storyboard.SetTargetProperty(anim, "DynamicT"); // [0-1]
            Storyboard.SetTargetName(anim, Name);

            board = new() { Duration = anim.Duration };
            board.Children.Add(anim);
            board.Completed += (o, e) => { IsAnimating = false; };
        }

        bool IsAnimating = false;
        void StartAnimation() {
            IsAnimating = true;
            board.Begin();
            DynamicT = 0; // Storyboards start on the next frame. Set T to 0 during initial measuring!
        }

        static readonly Size InitialSize = new(0, 0);
        Size PreviousSize;
        Size NewSize;

        public static bool GLOBAL_IsDebugEnabled = false;
        public static bool GLOBAL_EnableDynamicSizing = true;

        // Called when the UI wants to calculate the size of this control.
        // Overriding this lets us give back any size value, regardless of content.
        protected override Size MeasureOverride(Size availableSize) {
            NewSize = base.MeasureOverride(availableSize); // Store requested size, will be used as t:1
            
            if (!GLOBAL_EnableDynamicSizing) return NewSize; // TEMP: DEMO: toggle for comparison

            // If we aren't animating yet, assume that a new size was requested:
            if (!IsAnimating) {
                // Store the previous size to animate against:
                PreviousSize = new(ActualSize.X, ActualSize.Y);  // Store current size (before animation), will be used as t:0

                // Don't animate initial size change
                // TODO: this is a bit odd with lists, as they load in their contents dynamically at runtime.
                // The size can animate while the list is loading in.
                if (PreviousSize == InitialSize) return NewSize;

                // If the requested size is different from what we had previously, start the animation.
                // TODO: don't use 'From', just animate 'To' for DynamicT, if possible!
                // TODO: is this check necessary?
                if (PreviousSize != NewSize) StartAnimation();

                if (GLOBAL_IsDebugEnabled) Debug.WriteLine($"MeasureOverride() N: t: {DynamicT}\nPrevSize: [{PreviousSize}]\nNewSize: [{NewSize}]\n");
            }
            // Otherwise, skip storing the previous value and just calculate the in-between animated value:

            // Calculate size:
            // 'DynamicT' is driven by the animation 'board' [0-1]
            double width  = lerp(PreviousSize.Width,  NewSize.Width,  DynamicT);
            double height = lerp(PreviousSize.Height, NewSize.Height, DynamicT);
            Size size = new Size(width, height); // In-between size for the given 't'

            return size; // Report back the animated size to use for layout
        }

        // ('t' is clamped so that it can only be between inclusive 0 and 1)
        double lerp(double a, double b, double t) {
            t = Math.Min(1, Math.Max(0, t));
            return a + (b - a) * t;
        }

        // This has to be a propdp for DoubleAnimation/Storyboard to work with it:
        public double DynamicT {
            get { return (double)GetValue(DynamicTProperty); }
            set { SetValue(DynamicTProperty, value); }
        }
        public static readonly DependencyProperty DynamicTProperty =
            DependencyProperty.Register("DynamicT", typeof(double), typeof(DynamicContainer), new PropertyMetadata(1.0, DynamicTChanged));

        static void DynamicTChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            // TODO: Perf!
            DynamicContainer target = (DynamicContainer)d;
            // double value = (double)e.NewValue;

            // NOTE: In WPF, it was possible to attach FrameworkPropertyMetadataOptions.AffectsMeasure to the propdp
            // to automatically invalidate measure when the property changes.
            // This is no longer there in WinUI, so let's invalidate it ourselves every time 't' changes, in order for
            // the new size to be calculated in-between the animation frames:
            target.InvalidateMeasure();
        }
    }
}
