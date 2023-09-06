﻿#if UNITY_5_3_OR_NEWER
#define NOESIS
#else
#endif


namespace Feudal.Scenes.Main
{
    partial class EstateWorkViewModel
    {
        private static EstateWorkViewModel @default;
        public static EstateWorkViewModel Default
        {
            get
            {
                if(@default == null)
                {
                    @default = new EstateWorkViewModel();
                    @default.OutputType = "OUT_PUT";
                    @default.OutputValue = +10;
                }
                return @default;
            }
        }
    }
}