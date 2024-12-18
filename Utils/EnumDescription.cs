/*
 * 枚举描述解析器
 * 可以给枚举增加 Description 特性，添加对枚举的描述
 * 支持双向获取，即通过枚举获取描述，通过描述获取枚举
 */

using System.Collections.Generic;
using System.Reflection;
using System;

namespace FinchUtils.Utils;

// 特性
[AttributeUsage(AttributeTargets.Field)]
public class DescriptionAttribute : Attribute {
    public string Description;

    public DescriptionAttribute(string description) {
        Description = description;
    }
}

// 解析器
public static class DescriptionParser<T> where T : Enum {

    #region Wrapper

    private class EnumWrapper {

        /**
         * 显然，同一个枚举类型中，不能添加相同的描述
         */
        private readonly Dictionary<T, string> _enumToDescription;
        private readonly Dictionary<string, T> _descriptionToEnum;

        public EnumWrapper() {
            _enumToDescription = new Dictionary<T, string>();
            _descriptionToEnum = new Dictionary<string, T>();
            foreach (T enumValue in Enum.GetValues(typeof(T))) {
                string description = GetEnumDescription(enumValue);
                _enumToDescription.Add(enumValue, description);
                _descriptionToEnum.Add(description, enumValue);
            }
        }

        public string GetDescriptionByEnum(T t) {
            return _enumToDescription[t];
        }

        public bool GetEnumByDescription(string description, out T value) {
            return _descriptionToEnum.TryGetValue(description, out value);
        }

        private static string GetEnumDescription(Enum value) {
            string valueStr = value.ToString();
            FieldInfo info = value.GetType().GetField(valueStr);
            var attrs = info.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
            if (attrs == null || attrs.Length == 0) {
                return valueStr;
            }

            string description = attrs[0].Description;
            if (string.IsNullOrEmpty(description)) {
                // 描述为空也返回Enum.ToString
                return valueStr;
            }

            return description;
        }
    }

    #endregion

    private static readonly EnumWrapper Wrapper = new();

    public static string GetDescription(T t) {
        return Wrapper.GetDescriptionByEnum(t);
    }

    public static bool GetEnum(string description, out T value) {
        return Wrapper.GetEnumByDescription(description, out value);
    }
}

#region Quick Access

// 快捷方式
public static partial class EnumDescriptionExtension {
    /// <summary>
    /// 获得枚举描述
    /// 没有添加特性，或特性中的参数为空，则返回 someEnum.ToString()
    /// </summary>
    public static string GetDescription<T>(this T value) where T : Enum {
        return DescriptionParser<T>.GetDescription(value);
    }

    /// <summary>
    /// 通过描述获得对应枚举
    /// 获取不到返回false
    /// </summary>
    public static bool GetEnum<T>(this string description, out T value) where T : Enum {
        return DescriptionParser<T>.GetEnum(description, out value);
    }
}

#endregion