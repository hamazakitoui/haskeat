using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Inspector�p���y�t�@�C������N���X </summary>
[System.Serializable]
public class AudioObject
{
    [SerializeField] private string audioName; // ���y��
    /// <summary> string�^�ϊ��֐� </summary>
    /// <param name="audio">�ϊ��I�u�W�F�N�g</param>
    public static implicit operator string(AudioObject audio) { return audio.audioName; }
    /// <summary> string�^�ϊ��R���X�g���N�^ </summary>
    /// <param name="name">�ϊ����y�t�@�C��</param>
    public static implicit operator AudioObject(string name)
    {
        return new AudioObject() { audioName = name };
    }
}
