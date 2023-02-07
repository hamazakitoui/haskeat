using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> �ǂݍ��݃e�L�X�g����X�N���v�g </summary>
public class LoadingText : BaseMeshEffect
{
    int index = 0; // ���ݓ����������̔ԍ�
    // ��������p�ϐ�
    float wordDelta = 0;
    // ����������
    string loadingText = null;
    // �����̒��_��
    const int VERTEX_MAX = 6;
    // ��������Ԋu
    const float WORD_SPAN = 0.085f;
    Graphic wordGraphic = null; // �`��p�ϐ�
    // �����𓮂����ړ���
    [SerializeField] float wordVelocity = 20;
    // Update is called once per frame
    void Update()
    {
        Move(); // �A�j���[�V����
    }
    /// <summary> �S�̓��� </summary>
    void Move()
    {
        wordDelta += Time.deltaTime;
        if (wordDelta <= WORD_SPAN) return; // ��莞�Ԍo�߂���܂ŏ������Ȃ�
        // Graphic�N���X���擾���Ă��Ȃ��Ȃ�擾
        if (wordGraphic == null) wordGraphic = base.GetComponent<Graphic>();
        wordGraphic.SetVerticesDirty(); // �A�j���Đ�
        wordDelta = 0;
    }
    /// <summary> �e�L�X�g�̃|���S���̊e���_�ɃA�N�Z�X </summary>
    /// <param name="vh">�A�N�Z�X�p�N���X</param>
    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive()) return; // �A�N�e�B�u����Ȃ��Ȃ珈�����Ȃ�
        List<UIVertex> vertices = new List<UIVertex>(); // �e���_���X�g
        vh.GetUIVertexStream(vertices); // �A�N�Z�X�J�n
        WordMove(ref vertices); // �����𓮂���
        vh.Clear(); // �A�N�Z�X������������
        vh.AddUIVertexTriangleStream(vertices); // �|���S�����O�p�`�Ƃ��ĂƂ炦�e���_�ɃA�N�Z�X
    }
    /// <summary> �����𓮂��� </summary>
    /// <param name="vertices">�e���_���X�g</param>
    void WordMove(ref List<UIVertex> vertices)
    {
        Vector3 dir = Vector3.up; // �ړ�����
        dir.y *= wordVelocity; // �ړ��ʂ��v�Z
        // �Ή����镶���̊e���_�𓮂���
        // c += VERTEX�͒��_���v�Z��1�����P�ʂŌv�Z
        for(int c = 0; c < vertices.Count; c += VERTEX_MAX)
        {
            // �����������̏ꍇ
            if (c == index * VERTEX_MAX)
            {
                // �e���_�̐������v�Z
                for(int i = 0; i < VERTEX_MAX; i++)
                {
                    UIVertex vertex = vertices[c + i]; // ���������_���
                    vertex.position += dir; // �ړ��ʌv�Z
                    vertices[c + i] = vertex; // �v�Z�����l�𔽉f������
                }
                break;
            }
        }
        // ���������͂��ݒ肳��Ă��Ȃ���Ύ擾����
        if (loadingText == null) loadingText = GetComponent<Text>().text;
        index = index < loadingText.Length - 1 ? index + 1 : 0; // ���ɓ����������ԍ���ݒ�
    }
}
