using Gameplay;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class RestoreData
    {
        [Serializable]
        public struct LetterPart
        {
            public Vector3 Rotation;
            public Vector3 Position;
        }

        [SerializeField] public List<LetterPart> LetterParts = new List<LetterPart>();
        [SerializeField] public Letter LetterPrefab;
    }
}
