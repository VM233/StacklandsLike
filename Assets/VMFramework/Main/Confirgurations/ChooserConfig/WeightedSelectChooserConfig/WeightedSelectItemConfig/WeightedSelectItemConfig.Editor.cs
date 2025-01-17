﻿#if UNITY_EDITOR
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.Configuration
{
    public partial class WeightedSelectItemConfig<T>
    {
        protected override void OnInspectorInit()
        {
            base.OnInspectorInit();

            ReflectionUtility.TryCreateInstance(ref value);
        }
    }
}
#endif