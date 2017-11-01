using UnityEngine;
using System.Collections;
using UnityEditor;
//using UnityEditor.WindowsStandalone;
public class Laee : EditorWindow
{

}

[CustomEditor(typeof(Atr))]
//需要继承自editor，并且引入UnityEditor程序集
public class LearnInspector : Editor
{
    private Atr atr;
    private bool showWeapons;

    void OnEnable()
    {
        //获取当前自定义的Inspector对象
        atr = (Atr)target;
    }

    //执行该函数来自定义检视面板
    public override void OnInspectorGUI()
    {
        //不写默认是垂直布局
        EditorGUILayout.BeginVertical();

        //空格
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Base Info");
        atr.id = EditorGUILayout.IntField("Atr ID", atr.id);
        atr.Name = EditorGUILayout.TextField("Atr Name", atr.Name);

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Back Story");
        atr.BackStory = EditorGUILayout.TextArea(atr.BackStory, GUILayout.MinHeight(100));

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        atr.health = EditorGUILayout.Slider("Health", atr.health, 0, 100);

        if (atr.health < 20)
        {
            GUI.color = Color.red;
        }
        else if (atr.health > 80)
        {
            GUI.color = Color.green;
        }
        else
        {
            GUI.color = Color.grey;
        }


        Rect progressRect = GUILayoutUtility.GetRect(50, 50);

        EditorGUI.ProgressBar(progressRect, atr.health / 100.0f, "Health");

        GUI.color = Color.white;

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();


        atr.damage = EditorGUILayout.Slider("Damage", atr.damage, 0, 20);

        if (atr.damage < 10)
        {
            EditorGUILayout.HelpBox("伤害过低", MessageType.Error);
        }
        else if (atr.damage > 15)
        {
            EditorGUILayout.HelpBox("伤害过高", MessageType.Warning);
        }
        else
        {
            EditorGUILayout.HelpBox("伤害适中", MessageType.Info);
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Shoe");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Name", GUILayout.MaxWidth(50));
        atr.shoeName = EditorGUILayout.TextField(atr.shoeName);
        EditorGUILayout.LabelField("Size", GUILayout.MaxWidth(50));
        atr.shoeSize = EditorGUILayout.IntField(atr.shoeSize); EditorGUILayout.LabelField("Type", GUILayout.MaxWidth(50));
        atr.shoeType = EditorGUILayout.TextField(atr.shoeType);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }
}

//绘制字段用到的方法

//EditorGUILayout.LabelField（）标签字段
//EditorGUILayout.IntField（） 整数字段
//EditorGUILayout.FloatField（） 浮点数字段
//EditorGUILayout.TextField（） 文本字段
//EditorGUILayout.Vector2Field（） 二维向量字段
//EditorGUILayout.Vector3Field（） 三维向量字段
//EditorGUILayout.Vector4Field（） 四维向量字段
