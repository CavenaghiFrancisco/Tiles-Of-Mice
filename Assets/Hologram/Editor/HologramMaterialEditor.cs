using UnityEditor;
using UnityEngine;

public class HologramMaterialEditor : ShaderGUI
{
    private Material material;

    private bool showTextures = true;
    private bool showSettings = true;
    private bool showWooble = true;
    private bool showNoise = true;

    private static readonly int PropMainTexture = Shader.PropertyToID("_MainTex");
    private static readonly int PropNormalTexture = Shader.PropertyToID("_NormalTex");
    private static readonly int PropNoiseVertexTexture = Shader.PropertyToID("_NoiseVertexTex");
    private static readonly int PropNoiseColorTexture = Shader.PropertyToID("_NoiseColorTex");
    private static readonly int PropColor = Shader.PropertyToID("_Color");
    private static readonly int PropBias = Shader.PropertyToID("_Bias");
    private static readonly int PropClipAmount = Shader.PropertyToID("_ClipAmount");
    private static readonly int PropClipType= Shader.PropertyToID("_ClipType");
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

    public enum Vars
    {
        MainTex,
        NormalTex,
        NoiseVertexTex,
        NoiseColorTex,
        Color,
        Bias,
        ClipAmount,
        ClipType,
        ScanFreq,
        ScanSpeed,
        HoloDirect,
        HoloSign,
        WobbleEnable,
        WobbleFreq,
        WobbleSpeed,
        WobbleAmount,
        NoiseVertex,
        NoiseNormal,
        NoiseColors
    }

    enum HoloDirection
    {
        Vertical,
        Horizontal,
        Front, 
        All
    }

    enum ClipType
    {
        VerticalVertex,
        HorizontalVertex,
        VerticalUV, 
        HorizontalUV
    }

    enum HoloSign
    {
        Front,
        None,
        Back
    }

    public override void OnGUI (MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        HoloDirection asd = (HoloDirection)properties[(int)Vars.HoloDirect].floatValue;
        material = (Material)(materialEditor.target);

        if (material.shader.name != "SauCa/Hologram")
        {
            base.OnGUI(materialEditor, properties);
            return;
        }

        DrawTextures(properties);
        EditorGUILayout.Space();
        DrawSettings(properties);
        EditorGUILayout.Space();
        DrawWobble(properties);
        EditorGUILayout.Space();
        DrawNoise(properties);

        materialEditor.serializedObject.ApplyModifiedProperties();
    }

    private void DrawTextures (MaterialProperty[] properties)
    {
        showTextures = EditorGUILayout.BeginFoldoutHeaderGroup(showTextures, "Textures");
        if (showTextures)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            material.SetTexture(PropMainTexture, TextureField("Main", properties[(int)Vars.MainTex].textureValue));
            material.SetTexture(PropNormalTexture, TextureField("Normal", properties[(int)Vars.NormalTex].textureValue));
            material.SetTexture(PropNoiseVertexTexture, TextureField("Noise Vertex", properties[(int)Vars.NoiseVertexTex].textureValue));
            material.SetTexture(PropNoiseColorTexture, TextureField("Noise Color", properties[(int)Vars.NoiseColorTex].textureValue));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndFoldoutHeaderGroup();
    }

    private void DrawSettings (MaterialProperty[] properties)
    {
        material.SetColor(PropColor, EditorGUILayout.ColorField("Tint Color", properties[(int)Vars.Color].colorValue));
        SliderField(PropBias, "Intensity", properties[(int)Vars.Bias]);
        FloatField(PropClipAmount, "Clip Amount", properties[(int)Vars.ClipAmount]);
        material.SetFloat(PropClipType, (float)((ClipType)(EditorGUILayout.EnumPopup("ClipType", (ClipType)properties[(int)Vars.ClipType].floatValue))));

        EditorGUILayout.Space();

        showSettings = EditorGUILayout.BeginFoldoutHeaderGroup(showSettings, "Settings");
        if (showSettings)
        {
            FloatField(PropScanningFrequency, "Scanning Frequency", properties[(int)Vars.ScanFreq]);
            FloatField(PropScanningSpeed, "Scanning Speed", properties[(int)Vars.ScanSpeed]);
            material.SetFloat(PropHologramDirection, (float)((HoloDirection)(EditorGUILayout.EnumPopup("Hologram Direction", (HoloDirection)properties[(int)Vars.HoloDirect].floatValue))));
            material.SetFloat(PropHologramSignDirection, (float)((HoloSign)(EditorGUILayout.EnumPopup("Hologram Sign Direction", (HoloSign)properties[(int)Vars.HoloSign].floatValue))));
        }

        EditorGUILayout.EndFoldoutHeaderGroup();
    }

    private void DrawWobble (MaterialProperty[] properties)
    {
        showWooble = EditorGUILayout.BeginFoldoutHeaderGroup(showWooble, "Wobble");
        if (showWooble)
        {
            material.SetFloat(PropEnableWobble, EditorGUILayout.Toggle("Wobble Enabled", properties[(int)Vars.WobbleEnable].floatValue > 0.5f) ? 1 : 0);
            if (properties[(int)Vars.WobbleEnable].floatValue > 0.5f)
            {
                SliderField(PropWobbleFreq, "Wobble Frequency", properties[(int)Vars.WobbleFreq]);
                SliderField(PropWobbleSpeed, "Wobble Speed", properties[(int)Vars.WobbleSpeed]);
                SliderField(PropWobbleAmount, "Wobble Amount", properties[(int)Vars.WobbleAmount]);
            }
        }

        EditorGUILayout.EndFoldoutHeaderGroup();
    }

    private void DrawNoise (MaterialProperty[] properties)
    {
        showNoise = EditorGUILayout.BeginFoldoutHeaderGroup(showNoise, "Noise");
        if (showNoise)
        {
            SliderField(PropNoiseVertex, "Noise Vertex", properties[(int)Vars.NoiseVertex]);
            SliderField(PropNoiseNormal, "Noise Normal", properties[(int)Vars.NoiseNormal]);
            SliderField(PropNoiseColors, "Noise Colors", properties[(int)Vars.NoiseColors]);
        }

        EditorGUILayout.EndFoldoutHeaderGroup();
    }

    private Texture TextureField (string name, Texture texture)
    {
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical();
        var style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.UpperCenter;
        style.fixedWidth = 70;
        GUILayout.Label(name, style);
        Texture result = (Texture)EditorGUILayout.ObjectField(texture, typeof(Texture), false, GUILayout.Width(70), GUILayout.Height(70));
        GUILayout.EndVertical();
        return result;
    }

    private void FloatField (int idProp, string text, MaterialProperty property) => material.SetFloat(idProp, EditorGUILayout.FloatField(text, property.floatValue));

    private void SliderField (int idProp, string text, MaterialProperty property) => material.SetFloat(idProp, EditorGUILayout.Slider(text, property.floatValue, property.rangeLimits.x, property.rangeLimits.y));
}