using System;
using UnityEngine;

namespace HologramShader
{
    public class HologramConfig
    {
        private static readonly int PropMainTexture = Shader.PropertyToID("_MainTex");
        private static readonly int PropNormalTexture = Shader.PropertyToID("_NormalTex");
        private static readonly int PropNoiseVertexTexture = Shader.PropertyToID("_NoiseVertexTex");
        private static readonly int PropNoiseColorTexture = Shader.PropertyToID("_NoiseColorTex");
        private static readonly int PropColor = Shader.PropertyToID("_Color");
        private static readonly int PropBias = Shader.PropertyToID("_Bias");
        private static readonly int PropClipAmount = Shader.PropertyToID("_ClipAmount");
        private static readonly int PropClipType = Shader.PropertyToID("_ClipType");
        private static readonly int PropScanningFrequency = Shader.PropertyToID("_ScanningFrequency");
        private static readonly int PropScanningSpeed = Shader.PropertyToID("_ScanningSpeed");
        private static readonly int PropHologramDirection = Shader.PropertyToID("_HologramDirection");
        private static readonly int PropHologramSignDirection = Shader.PropertyToID("_HologramSignDirection");
        private static readonly int PropEnableWobble = Shader.PropertyToID("_EnableWobble");
        private static readonly int PropWobbleFreq = Shader.PropertyToID("_WobbleFreq");
        private static readonly int PropWobbleSpeed = Shader.PropertyToID("_WobbleSpeed");
        private static readonly int PropWobbleAmount = Shader.PropertyToID("_WobbleAmount");
        private static readonly int PropNoiseVertex = Shader.PropertyToID("_NoiseVertex");
        private static readonly int PropNoiseNormal = Shader.PropertyToID("_NoiseNormal");
        private static readonly int PropNoiseColors = Shader.PropertyToID("_NoiseColors");

        public static void ChangeMainTexture (Material mat, Texture2D tex) => mat.SetTexture(PropMainTexture, tex);
        public static void ChangeNormalTexture (Material mat, Texture2D tex) => mat.SetTexture(PropNormalTexture, tex);
        public static void ChangeVertexTexture (Material mat, Texture2D tex) => mat.SetTexture(PropNoiseVertexTexture, tex);
        public static void ChangeNoiseTexture (Material mat, Texture2D tex) => mat.SetTexture(PropNoiseColorTexture, tex);
        public static void ChangeColorTint (Material mat, Color color) => mat.SetColor(PropColor, color);
        public static void ChangeBias (Material mat, float value) => mat.SetFloat(PropBias, value);
        public static void ChangeScanFreq (Material mat, float value) => mat.SetFloat(PropScanningFrequency, value);
        public static void ChangeSpeedFreq (Material mat, int value) => mat.SetFloat(PropScanningSpeed, value);
        public static void ChangeHoloDirection (Material mat, int value) => mat.SetInt(PropHologramDirection, value);
        public static void ChangeHoloSign (Material mat, int value) => mat.SetInt(PropHologramSignDirection, value);
        public static void ChangeEnableWobble (Material mat, float value) => mat.SetFloat(PropEnableWobble, value);
        public static void ChangeWobbleFreq (Material mat, float value) => mat.SetFloat(PropWobbleFreq, value);
        public static void ChangeWobbleSpeed (Material mat, float value) => mat.SetFloat(PropWobbleSpeed, value);
        public static void ChangeWobbleAmount (Material mat, float value) => mat.SetFloat(PropWobbleAmount, value);
        public static void ChangeNoiseVertex (Material mat, float value) => mat.SetFloat(PropNoiseVertex, value);
        public static void ChangeNoiseNormal (Material mat, float value) => mat.SetFloat(PropNoiseNormal, value);
        public static void ChangeNoiseColors (Material mat, float value) => mat.SetFloat(PropNoiseColors, value);

        public static void ChangeVariable (Material material, HologramMaterialEditor.Vars variable, Texture2D value)
        {
            switch (variable)
            {
                case HologramMaterialEditor.Vars.MainTex:
                    ChangeMainTexture(material, value);
                    break;
                case HologramMaterialEditor.Vars.NormalTex:
                    ChangeNormalTexture(material, value);
                    break;
                case HologramMaterialEditor.Vars.NoiseVertexTex:
                    ChangeVertexTexture(material, value);
                    break;
                case HologramMaterialEditor.Vars.NoiseColorTex:
                    ChangeNoiseTexture(material, value);
                    break;
            }
        }

        public static void ChangeVariable (Material material, HologramMaterialEditor.Vars variable, Color value)
        {
            switch (variable)
            {
                case HologramMaterialEditor.Vars.Color:
                    ChangeColorTint(material, value);
                    break;
            }
        }

        public static void ChangeVariable (Material material, HologramMaterialEditor.Vars variable, float value)
        {
            switch (variable)
            {
                case HologramMaterialEditor.Vars.Bias:
                    ChangeBias(material, value);
                    break;
                case HologramMaterialEditor.Vars.ScanFreq:
                    ChangeScanFreq(material, value);
                    break;
                case HologramMaterialEditor.Vars.WobbleEnable:
                    ChangeEnableWobble(material, value);
                    break;
                case HologramMaterialEditor.Vars.WobbleFreq:
                    ChangeWobbleFreq(material, value);
                    break;
                case HologramMaterialEditor.Vars.WobbleSpeed:
                    ChangeWobbleSpeed(material, value);
                    break;
                case HologramMaterialEditor.Vars.WobbleAmount:
                    ChangeWobbleAmount(material, value);
                    break;
                case HologramMaterialEditor.Vars.NoiseVertex:
                    ChangeNoiseVertex(material, value);
                    break;
                case HologramMaterialEditor.Vars.NoiseNormal:
                    ChangeNoiseNormal(material, value);
                    break;
                case HologramMaterialEditor.Vars.NoiseColors:
                    ChangeNoiseColors(material, value);
                    break;
                case HologramMaterialEditor.Vars.ScanSpeed:
                    ChangeSpeedFreq(material, (int)value);
                    break;
                case HologramMaterialEditor.Vars.HoloDirect:
                    ChangeHoloDirection(material, (int)value);
                    break;
                case HologramMaterialEditor.Vars.HoloSign:
                    ChangeHoloSign(material, (int)value);
                    break;
            }
        }

        public static int GetVariable (HologramMaterialEditor.Vars variable)
        {
            int[] vars = { PropMainTexture, PropNormalTexture, PropNoiseVertexTexture, PropNoiseColorTexture, PropColor, PropBias, PropClipAmount, PropClipType, PropScanningFrequency, PropScanningSpeed, PropHologramDirection, PropHologramSignDirection, PropEnableWobble, PropWobbleFreq, PropWobbleSpeed, PropWobbleAmount, PropNoiseVertex, PropNoiseNormal, PropNoiseColors };

            return vars[(int)variable];
        }
    }
}