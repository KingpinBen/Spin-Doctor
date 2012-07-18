using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLibrary.Graphics.UI
{
    /// <summary>
    /// 3 styles of scaling:
    /// AutoScale - 
    ///     Keeps the whole image onscreen, scaling it to be the largest it can be without
    ///     the pixels being offscreen.
    /// MaxWidth -
    ///     Scales the image so the width is the same as the viewport width.
    /// MaxHeight -
    ///     Scales the image so the height is the same as the viewport height. 
    ///     
    /// Autoscale is default. Change before loading.
    /// </summary>
    public enum ScaleType
    {
        AutoScale,
        MaxWidth,
        MaxHeight
    }
}
