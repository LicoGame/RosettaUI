﻿using System;

namespace RosettaUI
{
    public class LabelElement : ReadOnlyValueElement<string>
    {
        public bool isPrefix;
        
        public LabelElement(IGetter<string> label) : base(label) { }

        public LabelElement(LabelElement other) : base(other.getter) { }


        public static implicit operator LabelElement(string label) => new(ConstGetter.Create(label));
        public static implicit operator LabelElement(Func<string> readLabel) => new(Getter.Create(readLabel));

        public static implicit operator string(LabelElement label) => label.Value;
    }
}