using System.Text;
using UnityEngine;

namespace Cobilas.Unity.Test.Graphics.IGU {
    internal sealed class TextEditorStatus : TextEditor {
        private string textMod;

        public char CharMask { get; set; }
        public int MaxLength { get; set; }
        public int TextLength => text.Length;
        public int SelectionSize => Mathf.Abs(selectIndex - cursorIndex);
        public bool IsMaxLength => TextLength > MaxLength && MaxLength > 0;
        public int CursorPosition => cursorIndex > selectIndex ? selectIndex : cursorIndex;
        public string TextMod {
            get {
                if (isPasswordField)
                    return textMod;
                return text;
            }
            set {
                if (isPasswordField) textMod = value;
                else text = value;
                // text = value;
            }
        }

        public TextEditorStatus() : base() {
            MaxLength = 0;
            CharMask = char.MinValue;
            base.text = textMod = string.Empty;
        }

        public new bool Paste() {
            if (isPasswordField) {
                string paste = GUIUtility.systemCopyBuffer;
                StringBuilder builder = new StringBuilder(textMod);
                textMod = builder.Remove(CursorPosition, SelectionSize)
                    .Insert(CursorPosition, paste).ToString();
                ReplaceSelection(string.Empty.PadLeft(paste.Length, CharMask));
                bool res = !string.IsNullOrEmpty(paste);
                return res;
            }
            return base.Paste();
        }
    }
}