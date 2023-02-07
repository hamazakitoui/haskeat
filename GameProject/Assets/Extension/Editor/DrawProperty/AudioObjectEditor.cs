using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
/// <summary> ���y�I�u�W�F�N�g�G�f�B�^ </summary>
[CustomPropertyDrawer(typeof(AudioObject))]
public class AudioObjectEditor : PropertyDrawer
{
    // �v���p�e�B��
    private readonly string nameName = "audioName";
    // �p�X��
    private readonly string bgmPath = "BGM/", sePath = "SE/";
    private readonly string audioPath = "Assets/Extension/Resources/Audio/";
    // �g���q�z��
    private readonly string[] fileKindList = { ".mp3" };
    /// <summary> ���y�t�@�C������ </summary>
    /// <param name="name">�t�@�C����</param>
    /// <returns>�������ꂽ���y�t�@�C��</returns>
    private AudioClip GetAudioObject(string name)
    {
        // �V�[��������Ȃ�Null��Ԃ�
        if (string.IsNullOrEmpty(name)) return null;
        // �t�@�C������
        for(int a = 0; a < fileKindList.Length; a++)
        {
            // BGM�t�H���_���猟��
            AudioClip clip = AssetDatabase.LoadAssetAtPath
                (audioPath + bgmPath + name + fileKindList[a], typeof(AudioClip)) as AudioClip;
            // ������Ȃ����SE�t�H���_���猟��
            if (clip == null) clip = AssetDatabase.LoadAssetAtPath
                    (audioPath + sePath + name + fileKindList[a], typeof(AudioClip)) as AudioClip;
            if (clip != null) return clip; // ���������Ό������ꂽ�t�@�C��������
        }
        // ��������Ȃ���Όx��
        Debug.Log($"{name}�͎g���܂���.Resources�t�H���_�ɂ��̃t�@�C����ǉ����Ă�������");
        return null;
    }
    /// <summary> �`�� </summary>
    /// <param name="position">�`����W</param> <param name="property">�`��v���p�e�B</param>
    /// <param name="label">�\�����x��</param>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty nameProp = property.FindPropertyRelative(nameName); // ���y���v���p�e�B�擾
        AudioClip audioObj = GetAudioObject(nameProp.stringValue); // ���y�I�u�W�F�N�g
        // Inspector��̃I�u�W�F�N�g�t�B�[���h
        Object clip = EditorGUI.ObjectField(position, label, audioObj, typeof(AudioClip), false);
        if (clip == null) nameProp.stringValue = ""; // �t�B�[���h�������Ȃ疼�O�v���p�e�B�̒��g����ɂ���
        else
        {
            // �I�u�W�F�N�g���Ɩ��O�v���p�e�B�̒��g���Ⴄ�Ȃ�
            if (clip.name != nameProp.stringValue)
            {
                AudioClip obj = GetAudioObject(clip.name); // �I�u�W�F�N�g����
                // ������������Ȃ�������x��
                if (obj == null) Debug.LogWarning
                        ($"{clip.name}�͎g���܂���.Resources�t�H���_�Ƀt�@�C���ǉ����Ă�������");
                else nameProp.stringValue = clip.name; // ���O�v���p�e�B�̒��g���I�u�W�F�N�g���ɂ���
            }
        }
    }
}
#endif
