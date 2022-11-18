using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] public GameObject[] objectTypes;
    [SerializeField] public Collectable[] map;
    public class Collectable
    {
        private Transform transform;
        private int ID;

        public Collectable(int _id, Transform _transform)
        {
            transform = _transform;
            ID = _id;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public int GETID()
        {
            return ID;
        }
    }
}
