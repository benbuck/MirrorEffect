using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using PaintDotNet;
using PaintDotNet.PropertySystem;
using PaintDotNet.Effects;

[assembly: AssemblyTitle("Mirror plugin for Paint.NET")]
[assembly: AssemblyDescription("Mirror along diagonal from bottom-left to top-right")]
[assembly: AssemblyConfiguration("mirror diagonal")]
[assembly: AssemblyCompany("Benbuck Nason")]
[assembly: AssemblyProduct("Mirror")]
[assembly: AssemblyCopyright("Copyright ©2020 by Benbuck Nason")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: AssemblyVersion("1.0.*")]

namespace MirrorEffect
{
    public class PluginSupportInfo : IPluginSupportInfo
    {
        public string Author
        {
            get
            {
                return base.GetType().Assembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
            }
        }

        public string Copyright
        {
            get
            {
                return base.GetType().Assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;
            }
        }

        public string DisplayName
        {
            get
            {
                return base.GetType().Assembly.GetCustomAttribute<AssemblyProductAttribute>().Product;
            }
        }

        public Version Version
        {
            get
            {
                return base.GetType().Assembly.GetName().Version;
            }
        }

        public Uri WebsiteUri
        {
            get
            {
                return new Uri("https://www.getpaint.net/redirect/plugins.html");
            }
        }
    }

    [PluginSupportInfo(typeof(PluginSupportInfo), DisplayName = "Mirror diagonal")]
    public class MirrorEffectPlugin : PropertyBasedEffect
    {
        public static string StaticName
        {
            get
            {
                return "Mirror diagonal";
            }
        }

        public static Image StaticIcon
        {
            get
            {
                return null;
            }
        }

        public static string SubmenuName
        {
            get
            {
                return null;
            }
        }

        public MirrorEffectPlugin()
            : base(StaticName, StaticIcon, SubmenuName, new EffectOptions() { Flags = EffectFlags.None, RenderingSchedule = EffectRenderingSchedule.None })
        {
        }

        protected override PropertyCollection OnCreatePropertyCollection()
        {
            return PropertyCollection.CreateEmpty();
        }

        protected override void OnSetRenderInfo(PropertyBasedEffectConfigToken token, RenderArgs dstArgs, RenderArgs srcArgs)
        {
            base.OnSetRenderInfo(token, dstArgs, srcArgs);
        }

        protected override unsafe void OnRender(Rectangle[] rois, int startIndex, int length)
        {
            if (length == 0) return;
            for (int i = startIndex; i < startIndex + length; ++i)
            {
                Render(DstArgs.Surface, SrcArgs.Surface, rois[i]);
            }
        }
        void Render(Surface dst, Surface src, Rectangle rect)
        {
            for (int y = rect.Top; y < rect.Bottom; ++y)
            {
                float yNorm = (float)y / (float)rect.Height;
                for (int x = rect.Left; x < rect.Right; ++x)
                {
                    float xNorm = (float)x / (float)rect.Width;
                    ColorBgra newColor;
                    if ((xNorm + yNorm) >= 1.0)
                    {
                        newColor = src[x, y];
                    }
                    else
                    {
                        int srcX = (int)((1.0 - yNorm) * (rect.Width - 1));
                        int srcY = (int)((1.0 - xNorm) * (rect.Height - 1));
                        newColor = src[srcX, srcY];
                    }
                    dst[x, y] = newColor;
                }
            }
        }
    }
}
