using System;

namespace Settings
{
    public sealed class FullScreen : Setting<FullScreen, bool>
    {
        protected override bool DefaultValue => true;

        protected override Type SaveType => typeof(bool);
    }
}
