using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary> �V�[���ǂݍ��݃}�l�[�W���[ </summary>
public class FadeSceneManager : MonoBehaviour
{
    private float fadeAlpha = 0; // �t�F�[�h���̃A���t�@�l
    private bool isFade = false; // �V�[�����[�h�t���O // �f�t�H���g�ǂݍ��ݎ���
    // �J�ڌW���A�t�F�[�h�ŏ��A���t�@�l�A�t�F�[�h�ő�A���t�@�l
    private readonly float fadeIntervalMul = 0.5f, fadeMinAlpha = 0, fadeMaxAlpha = 1;
    // �I�u�W�F�N�g��
    private readonly string backName = "Back", loadName = "LoadText", iconName = "LoadIcon";
    // �t�F�[�h�pUI�w�i
    private Image BackGround;
    // �ǂݍ��݃e�L�X�g�A�ǂݍ��݃A�C�R��
    private GameObject LoadObj, Icon;
    [SerializeField] private float loadTime = 1f;
    [SerializeField] private bool DebugMode = false; // �f�o�b�O���[�h
    [SerializeField] private Color fadeColor = Color.black; // �t�F�[�h�p�w�i�F
    // �ǂݍ��ݎ��\���pUI
    [SerializeField] private GameObject FadeCanvas;
    // �f�o�b�O�V�[�����X�g
    [SerializeField] private List<SceneObject> DebugScene;
    private static FadeSceneManager instance; // �C���X�^���X�ۑ��p�ϐ�
    /// <summary> �V���O���g���C���X�^���X </summary>
    public static FadeSceneManager Instance
    {
        get
        {
            // �C���X�^���X���Ȃ��Ȃ�
            if (instance == null)
            {
                instance = FindObjectOfType<FadeSceneManager>();
                // �C���X�^���X��������Ȃ��Ȃ�G���[����
                if (instance == null) Debug.LogError($"{typeof(FadeSceneManager)}��������܂���!");
            }
            return instance;
        }
    }
    void Awake()
    {
        if (this != Instance)
        {
            Destroy(FadeCanvas); // �t�F�[�h�p��UI���폜
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        if (this != Instance) return; // �O�̂���
        DontDestroyOnLoad(this.FadeCanvas); // �Ö�������ɋ쏜����Ȃ��悤����
        // �w�i�擾
        {
            BackGround = this.FadeCanvas.transform.Find(backName).GetComponent<Image>();
            BackGround.color = fadeColor;
        }
        // �e�L�X�g�擾
        {
            Transform load = this.FadeCanvas.transform.Find(loadName); // �e�L�X�g����
            // �e�L�X�g�������
            if (load != null)
            {
                LoadObj =load.gameObject;
                LoadObj.SetActive(false);
            }
        }
        // �A�C�R���擾
        {
            Transform icon = this.FadeCanvas.transform.Find(iconName); // �A�C�R������
            // �A�C�R���������
            if (icon != null)
            {
                Icon = icon.gameObject;
                Icon.SetActive(false);
            }
        }
        this.FadeCanvas.SetActive(false); // �t�F�[�h�p��UI���\��
    }
    void OnGUI()
    {
        if (!this.DebugMode) return; // �f�o�b�O���[�h����Ȃ��Ȃ珈�����Ȃ�
        // �f�o�b�O�V�[�����Ȃ��ꍇ
        if (DebugScene.Count <= 0)
        {
            GUI.Box(NoSceneBox, new GUIContent("Fade Debug")); // �g�\��
            GUI.Label(NoSceneLabel, new GUIContent("�f�o�b�O�p�V�[������")); // ���x���\��
            return;
        }
        GUI.Box(SceneBox, new GUIContent("Fade Debug")); // �g�\��
        // ���݂̃V�[���\��
        GUI.Label(SceneLabel, new GUIContent($"���݂̃V�[�� : {SceneManager.GetActiveScene().name}"));
        for(int s = 0; s < DebugScene.Count; s++)
        {
            // �{�^���������ꂽ��V�[����ǂݍ���
            if (GUI.Button(ButtonRect(s), new GUIContent("Load")))
                LoadScene(DebugScene[s]);
            GUI.Label(SceneButtonLabel(s), DebugScene[s]); // �V�[�����\��
        }
    }
    #region �f�o�b�O�p�֐�
    /// <summary> �{�^���p�g </summary>
    /// <param name="count">�{�^���ԍ�</param>
    /// <returns></returns>
    Rect ButtonRect(int count)
    {
        return new Rect(20, 80 + count * 25, 100, 25);
    }
    /// <summary> �{�^���p�V�[�����x�� </summary>
    /// <param name="count">�V�[���ԍ�</param>
    /// <returns></returns>
    Rect SceneButtonLabel(int count)
    {
        return new Rect(130, 80 + count * 25, 1000, 25);
    }
    /// <summary> �f�o�b�O�p�V�[���g </summary>
    Rect SceneBox { get { return new Rect(10, 25, 350, 30 * DebugScene.Count + 50); } }
    /// <summary> �f�o�b�O�p�V�[�����x�� </summary>
    Rect SceneLabel { get { return new Rect(20, 50, 300, 25); } }
    /// <summary> �f�o�b�O�p�V�[���������ꍇ </summary>
    Rect NoSceneBox { get { return new Rect(10, 25, 200, 50); } }
    /// <summary> �f�o�b�O�p�V�[���������ꍇ </summary>
    Rect NoSceneLabel { get { return new Rect(20, 50, 200, 25); } }
    #endregion
    /// <summary> �V�[���ǂݍ��� </summary>
    /// <param name="scene">�ǂݍ��ރV�[��</param>
    public void LoadScene(string scene)
    {
        LoadScene(scene, loadTime); // �ʏ펞�Ԃœǂݍ���
    }
    /// <summary> �V�[���ǂݍ��� </summary>
    /// <param name="scene">�ǂݍ��ރV�[��</param> <param name="interval">�ǂݍ��ݎ���</param>
    /// <param name="wait">�ǂݍ��ݑҋ@����</param>
    public void LoadScene(string scene, float interval, float wait = 1.0f)
    {
        if (isFade) return; // ���ɓǂݍ���ł���Ȃ珈�����Ȃ�
        StartCoroutine(LoadSceneFading(scene, interval, wait)); // �V�[���J�ڊJ�n
    }
    /// <summary> �V�[���ǂݍ��� �ǂݍ��݉�ʃ��[�v </summary>
    /// <param name="scene">�ǂݍ��݃V�[��</param>
    public void CloseScene(string scene)
    {
        CloseScene(scene, loadTime); // �ʏ펞�Ԃœǂݍ���
    }
    /// <summary> �V�[���ǂݍ��� �ǂݍ��݉�ʃ��[�v </summary>
    /// <param name="scene">�ǂݍ��݃V�[��</param> <param name="interval">�ǂݍ��ݎ���</param>
    /// <param name="wait">�ǂݍ��ݑҋ@����</param>
    public void CloseScene(string scene, float interval, float wait = 1.0f)
    {
        if (isFade) return; // ���ɓǂݍ���ł���Ȃ珈�����Ȃ�
        StartCoroutine(CloseSceneFadig(scene, interval, wait)); // �V�[���J�ڊJ�n
    }
    /// <summary> �ǂݍ��݉�ʔ�\�� </summary>
    public void OpenScene()
    {
        OpenScene(loadTime); // �ʏ펞�Ԃœǂݍ��݉�ʂ��\���ɂ���
    }
    /// <summary> �ǂݍ��݉�ʔ�\�� </summary>
    /// <param name="interval">��\���܂ł̎���</param>
    public void OpenScene(float interval)
    {
        if (!isFade) return; // �ǂݍ���ł��Ȃ��Ȃ珈�����Ȃ�
        StartCoroutine(OpenSceneFading(interval)); // �ǂݍ��݉�ʔ�\���J�n
    }
    /// <summary> �V�[���J�ڃR���[�`�� </summary>
    /// <param name="scene">�J�ڃV�[��</param> <param name="interval">�ǂݍ��ݎ���</param>
    /// <param name="wait">�ǂݍ��ݑҋ@����</param>
    /// <returns></returns>
    private IEnumerator LoadSceneFading(string scene, float interval, float wait)
    {
        isFade = true; // �ǂݍ��݊J�n
        FadeCanvas.SetActive(true); // �t�F�[�h�pUI�\��
        BackGround.transform.SetAsLastSibling(); // �t�F�[�h�pCanvas���Ō�ɕ\��
        float time = 0; // �J�ڗp�ϐ�
        AudioManager.Instance.StopBGM(); // BGM��~
        // �ǂݍ��݉�ʂ��������o��
        // �ǂݍ��ݎ��Ԃ̈�芄���̎��Ԃŕ\��
        while (time <= interval * fadeIntervalMul)
        {
            // �A���t�@�l�Ɍ��݂̌o�ߎ��Ԃ̑J�ڎ��Ԃ̊������A���t�@�l�̍ŏ��l����ő�l�̊ԂɎ��߂�
            fadeAlpha = Mathf.Lerp(fadeMinAlpha, fadeMaxAlpha, time / (interval * fadeIntervalMul));
            Color color = BackGround.color; // �t�F�[�h�p�w�iUI�̐F
            color.a = fadeAlpha; // �A���t�@�l�ύX
            BackGround.color = color; // �w�i���̂̐F�ɔ��f
            time += Time.deltaTime;
            yield return 0;
        }
        // �e�L�X�g�֌W
        if (LoadObj != null)
        {
            LoadObj.SetActive(true); // �ǂݍ��݃e�L�X�g�\��
            LoadObj.transform.SetAsLastSibling(); // �ǂݍ��݃e�L�X�g���Ō�ɕ\��
        }
        // �A�C�R���֌W
        if (Icon != null)
        {
            Icon.SetActive(true); // �A�C�R���\��
            Icon.transform.SetAsLastSibling(); // �ǂݍ��݃A�C�R�����Ō�ɕ\��
        }
        // �ǂݍ��ݎ��Ԃ̈�芄���̎��Ԉꎞ��~
        yield return new WaitForSeconds(wait);
        SceneManager.LoadScene(scene); // �V�[���؂�ւ�
        time = 0; // �o�ߎ��ԃ��Z�b�g
        if (LoadObj != null) LoadObj.SetActive(false); // �Ǎ��e�L�X�g��\��
        if (Icon != null) Icon.SetActive(false); // �A�C�R����\��
        // �ǂݍ��݉�ʂ�������������
        // �ǂݍ��ݎ��Ԃ̈�芄���̎��ԂŔ�\��
        while (time <= interval * fadeIntervalMul)
        {
            // �A���t�@�l�Ɍ��݂̌o�ߎ��Ԃ̑J�ڎ��Ԃ̊������A���t�@�l�̍ŏ��l����ő�l�̊ԂɎ��߂�
            fadeAlpha = Mathf.Lerp(fadeMaxAlpha, fadeMinAlpha, time / (interval * fadeIntervalMul));
            Color color = BackGround.color; // �t�F�[�h�p�w�iUI�̐F
            color.a = fadeAlpha; // �A���t�@�l�ύX
            BackGround.color = color; // �w�i���̂̐F�ɔ��f
            time += Time.deltaTime;
            yield return 0;
        }
        FadeCanvas.SetActive(false); // �t�F�[�h�pUI��\��
        isFade = false; // �ǂݍ��ݏI��
    }
    /// <summary> �V�[���J�ړǂݍ��݂܂ł̃R���[�`�� </summary>
    /// <param name="scene">�ǂݍ��݃V�[��</param> <param name="interval">�ǂݍ��ݎ���</param>
    /// <param name="wait">�ǂݍ��ݑҋ@����</param>
    /// <returns></returns>
    private IEnumerator CloseSceneFadig(string scene, float interval, float wait)
    {
        isFade = true; // �ǂݍ��݊J�n
        FadeCanvas.SetActive(true); // �t�F�[�h�pUI�\��
        BackGround.transform.SetAsLastSibling(); // �t�F�[�h�pCanvas���Ō�ɕ\��
        float time = 0; // �J�ڗp�ϐ�
        AudioManager.Instance.StopBGM(); // BGM��~
        // �ǂݍ��݉�ʂ��������o��
        while (time <= interval)
        {
            // �A���t�@�l�Ɍ��݂̌o�ߎ��Ԃ̑J�ڎ��Ԃ̊������A���t�@�l�̍ŏ��l����ő�l�̊ԂɎ��߂�
            fadeAlpha = Mathf.Lerp(fadeMinAlpha, fadeMaxAlpha, time / (interval * fadeIntervalMul));
            Color color = BackGround.color; // �t�F�[�h�p�w�iUI�̐F
            color.a = fadeAlpha; // �A���t�@�l�ύX
            BackGround.color = color; // �w�i���̂̐F�ɔ��f
            time += Time.deltaTime;
            yield return 0;
        }
        // �e�L�X�g�֌W
        if (LoadObj != null)
        {
            LoadObj.SetActive(true); // �ǂݍ��݃e�L�X�g�\��
            LoadObj.transform.SetAsLastSibling(); // �ǂݍ��݃e�L�X�g���Ō�ɕ\��
        }
        // �A�C�R���֌W
        if (Icon != null)
        {
            Icon.SetActive(true); // �A�C�R���\��
            Icon.transform.SetAsLastSibling(); // �ǂݍ��݃A�C�R�����Ō�ɕ\��
        }
        // �ǂݍ��ݎ��Ԃ̈�芄���̎��Ԉꎞ��~
        yield return new WaitForSeconds(wait);
        SceneManager.LoadScene(scene); // �V�[���؂�ւ�
    }
    /// <summary> �ǂݍ��݉�ʔ�\���R���[�`�� </summary>
    /// <param name="interval">��\���܂ł̎���</param>
    /// <returns></returns>
    private IEnumerator OpenSceneFading(float interval)
    {
        float time = 0; // �o�ߎ���
        if (LoadObj != null) LoadObj.SetActive(false); // �Ǎ��e�L�X�g��\��
        if (Icon != null) Icon.SetActive(false); // �A�C�R����\��
        // �ǂݍ��݉�ʂ�������������
        while (time <= interval * fadeIntervalMul)
        {
            // �A���t�@�l�Ɍ��݂̌o�ߎ��Ԃ̑J�ڎ��Ԃ̊������A���t�@�l�̍ŏ��l����ő�l�̊ԂɎ��߂�
            fadeAlpha = Mathf.Lerp(fadeMaxAlpha, fadeMinAlpha, time / (interval * fadeIntervalMul));
            Color color = BackGround.color; // �t�F�[�h�p�w�iUI�̐F
            color.a = fadeAlpha; // �A���t�@�l�ύX
            BackGround.color = color; // �w�i���̂̐F�ɔ��f
            time += Time.deltaTime;
            yield return 0;
        }
        FadeCanvas.SetActive(false); // �t�F�[�h�pUI��\��
        isFade = false; // �ǂݍ��ݏI��
    }
}
