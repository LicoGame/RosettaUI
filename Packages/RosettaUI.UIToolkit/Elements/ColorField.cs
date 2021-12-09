using UnityEngine.UIElements;

namespace RosettaUI.UIToolkit
{
    public class ColorField : UnityInternalAccess.ColorField
    {
        public new class UxmlFactory : UxmlFactory<ColorField, UxmlTraits> { }
        
        public ColorField() : base(null) { }
        
        public ColorField(string label) : base(label)
        {
        }
    }
}