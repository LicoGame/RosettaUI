﻿namespace RosettaUI
{
    /// <summary>
    /// 値を持ち外部と同期するFieldElement
    /// ラベル付きのReadOnlyValueElement
    /// </summary>
    public abstract class ReadOnlyFieldElement<T> : ReadOnlyValueElement<T>
    {
        public LabelElement Label { get; }

        public ReadOnlyFieldElement(LabelElement label, IGetter<T> getter) : base(getter)
        {
            if (label != null)
            {
                Label = label;
                Label.SetParent(this);
                Label.isPrefix = true;
            }
        }

        protected override void UpdateInternal()
        {
            Label?.Update();
            base.UpdateInternal();
        }
    }
}