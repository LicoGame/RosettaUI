using System;

namespace RosettaUI
{
    public class ButtonElement : ReadOnlyValueElement<string>
    {
        public event Action onClick;
        
        public ButtonElement(IGetter<string> readName = null, Action onClick = null, bool expand = false) : base(readName)
        {
            this.onClick += onClick;
            Style.FlexGrow = expand ? 1 : null;
            Style.MinHeight = 35;
        }

        public void OnClick() => onClick?.Invoke();
    }
}