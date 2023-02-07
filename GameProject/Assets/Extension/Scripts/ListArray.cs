using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 多次元配列Inspector表示用構造体 </summary>
namespace UnityEngine.ListArray
{
    /// <summary> 2次元配列Inspector表示用クラス Int型 </summary>
    [Serializable]
    public class ListArrayInt
    {
        // 収納配列
        [SerializeField] private List<int> storage = new List<int>();
        /// <summary> 要素 </summary> <param name="i">要素番号</param> <returns></returns>
        public int this[int i]
        {
            get { return this.storage[i]; }
            set { this.storage[i] = value; }
        }
        /// <summary> 要素数 </summary>
        public int Length { get { return storage.Count; } }
    }
    /// <summary> 2次元配列Inspector表示用クラス Float型 </summary>
    [Serializable]
    public class ListArrayFloat
    {
        // 収納配列
        [SerializeField] private List<float> storage = new List<float>();
        /// <summary> 要素 </summary> <param name="i">要素番号</param> <returns></returns>
        public float this[int i]
        {
            get { return this.storage[i]; }
            set { this.storage[i] = value; }
        }
        /// <summary> 要素数 </summary>
        public int Length { get { return storage.Count; } }
    }
    /// <summary> 2次元配列Inspector表示用クラス String型 </summary>
    [Serializable]
    public class ListArrayString
    {
        // 収納配列
        [SerializeField] private List<string> storage = new List<string>();
        /// <summary> 要素 </summary> <param name="i">要素番号</param> <returns></returns>
        public string this[int i]
        {
            get { return this.storage[i]; }
            set { this.storage[i] = value; }
        }
        /// <summary> 要素数 </summary>
        public int Length { get { return storage.Count; } }
    }
    /// <summary> 2次元配列Inspector表示用クラス Vector2型 </summary>
    [Serializable]
    public class ListArrayVector2
    {
        // 収納配列
        [SerializeField] private List<Vector2> storage = new List<Vector2>();
        /// <summary> 要素 </summary> <param name="i">要素番号</param> <returns></returns>
        public Vector2 this[int i]
        {
            get { return this.storage[i]; }
            set { this.storage[i] = value; }
        }
        /// <summary> 要素数 </summary>
        public int Length { get { return storage.Count; } }
    }
    /// <summary> 2次元配列Inspector表示用クラス Vector3型 </summary>
    [Serializable]
    public class ListArrayVector3
    {
        // 収納配列
        [SerializeField] private List<Vector3> storage = new List<Vector3>();
        /// <summary> 要素 </summary> <param name="i">要素番号</param> <returns></returns>
        public Vector3 this[int i]
        {
            get { return this.storage[i]; }
            set { this.storage[i] = value; }
        }
        /// <summary> 要素数 </summary>
        public int Length { get { return storage.Count; } }
    }
    /// <summary> 2次元配列Inspector表示用クラス GameObject型 </summary>
    [Serializable]
    public class ListArrayGameObject
    {
        // 収納配列
        [SerializeField] private List<GameObject> storage = new List<GameObject>();
        /// <summary> 要素 </summary> <param name="i">要素番号</param> <returns></returns>
        public GameObject this[int i]
        {
            get { return this.storage[i]; }
            set { this.storage[i] = value; }
        }
        /// <summary> 要素数 </summary>
        public int Length { get { return storage.Count; } }
    }
    /// <summary> 2次元配列Inspector表示用クラス Sprite型 </summary>
    [Serializable]
    public class ListArraySprite
    {
        // 収納配列
        [SerializeField] private List<Sprite> storage = new List<Sprite>();
        /// <summary> 要素 </summary> <param name="i">要素番号</param> <returns></returns>
        public Sprite this[int i]
        {
            get { return this.storage[i]; }
            set { this.storage[i] = value; }
        }
        /// <summary> 要素数 </summary>
        public int Length { get { return storage.Count; } }
    }
    /// <summary> 2次元配列Inspector表示用クラス Transform型 </summary>
    [Serializable]
    public class ListArrayTransform
    {
        // 収納配列
        [SerializeField] private List<Transform> storage = new List<Transform>();
        /// <summary> 要素 </summary> <param name="i">要素番号</param> <returns></returns>
        public Transform this[int i]
        {
            get { return this.storage[i]; }
            set { this.storage[i] = value; }
        }
        /// <summary> 要素数 </summary>
        public int Length { get { return storage.Count; } }
    }
}
