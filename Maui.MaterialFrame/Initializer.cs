﻿namespace Sharpnado.MaterialFrame
{
    public static class Initializer
    {
        public static void Initialize(bool loggerEnable, bool debugLogEnable)
        {
            InternalLogger.EnableDebug = debugLogEnable;
            InternalLogger.EnableLogging = loggerEnable;
        }
    }
}
