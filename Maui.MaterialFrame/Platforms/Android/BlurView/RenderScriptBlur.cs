﻿//------------------------------------------------------------------------------
//
// https://github.com/Dimezis/BlurView
// Latest commit a955a76 on 4 Nov 2019
//
// Copyright 2016 Dmitry Saviuk
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
//------------------------------------------------------------------------------
// Adapted to csharp by Jean-Marie Alfonsi
//------------------------------------------------------------------------------
// <auto-generated/>

using Android.Content;
using Android.Graphics;
using Android.Renderscripts;
using Element = Android.Renderscripts.Element;

namespace Sharpnado.MaterialFrame.Droid.BlurView
{
    public class RenderScriptBlur
    {
        private readonly RenderScript _renderScript;

        private readonly ScriptIntrinsicBlur _blurScript;

        private Allocation _outAllocation;

        private int _lastBitmapWidth = -1;

        private int _lastBitmapHeight = -1;

        public RenderScriptBlur(Context context)
        {
            _renderScript = RenderScript.Create(context);
            _blurScript = ScriptIntrinsicBlur.Create(_renderScript, Element.U8_4(_renderScript));
        }

        /**
        * @param bitmap     bitmap to blur
        * @param blurRadius blur radius (1..25)
        * @return blurred bitmap
        */
        public Bitmap Blur(Bitmap bitmap, float blurRadius)
        {
            // Allocation will use the same backing array of pixels as bitmap if created with USAGE_SHARED flag
            Allocation inAllocation = Allocation.CreateFromBitmap(_renderScript, bitmap);

            if (!CanReuseAllocation(bitmap))
            {
                _outAllocation?.Destroy();

                _outAllocation = Allocation.CreateTyped(_renderScript, inAllocation.Type);
                _lastBitmapWidth = bitmap.Width;
                _lastBitmapHeight = bitmap.Height;
            }

            _blurScript.SetRadius(blurRadius);
            _blurScript.SetInput(inAllocation);

            // do not use inAllocation in forEach. it will cause visual artifacts on blurred Bitmap
            _blurScript.ForEach(_outAllocation);
            _outAllocation.CopyTo(bitmap);

            inAllocation.Destroy();
            return bitmap;
        }

        public void Destroy()
        {
            _blurScript.Destroy();
            _renderScript.Destroy();
            _outAllocation?.Destroy();
        }

        public bool CanModifyBitmap()
        {
            return true;
        }

        public Bitmap.Config GetSupportedBitmapConfig()
        {
            return Bitmap.Config.Argb8888;
        }

        private bool CanReuseAllocation(Bitmap bitmap)
        {
            return bitmap.Height == _lastBitmapHeight && bitmap.Width == _lastBitmapWidth;
        }
    }
}
