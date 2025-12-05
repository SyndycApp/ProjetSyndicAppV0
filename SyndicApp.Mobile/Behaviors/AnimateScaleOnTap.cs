using Microsoft.Maui.Controls;

namespace SyndicApp.Mobile.Behaviors
{
    public class AnimateScaleOnTap : Behavior<View>
    {
        public double ScaleTo { get; set; } = 1.05;
        public uint Duration { get; set; } = 120;

        protected override void OnAttachedTo(View view)
        {
            base.OnAttachedTo(view);

            view.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () =>
                {
                    await view.ScaleTo(ScaleTo, Duration, Easing.CubicOut);
                    await view.ScaleTo(1, Duration, Easing.CubicIn);
                })
            });
        }
    }
}
