﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace RosettaUI
{
    public static partial class UI
    {
        public static Element List<T>(Expression<Func<IList<T>>> targetExpression, ListViewOption option)
            => List(targetExpression, null, option);
        public static Element List<T>(Expression<Func<IList<T>>> targetExpression, Func<IBinder<T>, int, Element> createItemElement = null, ListViewOption option = null)
            => List(ExpressionUtility.CreateLabelString(targetExpression), targetExpression, createItemElement, option);

        public static Element List<T>(LabelElement label, Expression<Func<IList<T>>> targetExpression, ListViewOption option)
            => List(label, targetExpression, null, option);
        public static Element List<T>(LabelElement label, Expression<Func<IList<T>>> targetExpression, Func<IBinder<T>, int, Element> createItemElement = null, ListViewOption option = null) 
            => List(label, ExpressionUtility.CreateBinder(targetExpression), createItemElement, option);

        public static Element List<T>(LabelElement label, Func<IList<T>> readValue, Action<IList<T>> writeValue, ListViewOption option)
            => List(label, readValue, writeValue, null, option);
        public static Element List<T>(LabelElement label, Func<IList<T>> readValue, Action<IList<T>> writeValue, Func<IBinder<T>, int, Element> createItemElement = null, ListViewOption option = null) 
            => List(label, Binder.Create(readValue, writeValue), createItemElement, option);

        public static Element ListReadOnly<T>(Expression<Func<IList<T>>> targetExpression, ListViewOption option)
            => ListReadOnly(targetExpression, null, option);
        public static Element ListReadOnly<T>(Expression<Func<IList<T>>> targetExpression, Func<IBinder<T>, int, Element> createItemElement = null, ListViewOption option = null)
        {
            var labelString = ExpressionUtility.CreateLabelString(targetExpression);
            var binder = CreateReadOnlyBinder(targetExpression);
            return List(labelString, binder, createItemElement, option);
        }


        public static Element ListReadOnly<T>(LabelElement label, Func<IList<T>> readValue, ListViewOption option)
            => ListReadOnly(label, readValue, null, option);
        public static Element ListReadOnly<T>(LabelElement label, Func<IList<T>> readValue, Func<IBinder<T>, int, Element> createItemElement = null, ListViewOption option = null)
        {
            var binder = Binder.Create(readValue, null);
            return List(label, binder, createItemElement, option);
        }
        

        public static Element List<T>(LabelElement label, IBinder listBinder, Func<IBinder<T>, int, Element> createItemElement = null, ListViewOption option = null)
        {
            var createItemElementIBinder = createItemElement == null
                ? (Func<IBinder, int, Element>)null
                : (ib, idx) => createItemElement(ib as IBinder<T>, idx);

            return List(label, listBinder, createItemElementIBinder, option);
        }
        
        public static Element List(LabelElement label, IBinder listBinder, Func<IBinder, int, Element> createItemElement = null, ListViewOption option = null)
        {
            option ??= ListViewOption.Default;

            var countField = ListCounterField(listBinder, option);

            return Fold(
                label,countField,
                new[]{
                    ListItemContainer(listBinder, createItemElement, option)
                }
            ).Open();
        }

        public static Element ListCounterField(IBinder listBinder, ListViewOption option)
        {
            var isReadOnly = ListBinder.IsReadOnly(listBinder);
            
            return Field(null,
                () => ListBinder.GetCount(listBinder),
                count => ListBinder.SetCount(listBinder, count)
            ).SetMinWidth(50f).SetInteractable(!isReadOnly && !option.fixedSize);
        }
        
        public static Element ListItemContainer(IBinder listBinder, Func<IBinder, int, Element> createItemElement = null, ListViewOption option = null)
        {
            return NullGuard(null, listBinder,
                () => new ListViewItemContainerElement(
                    listBinder,
                    createItemElement ?? ListItemDefault,
                    option)
            );
        }
        
        public static Element ListItemDefault(IBinder binder, int index) => Field($"Item {index}", binder);


#if false

        public static Element List_(LabelElement label, IBinder listBinder, Func<IBinder, int, Element> createItemElement = null)
        {
            var isReadOnly = ListBinder.IsReadOnly(listBinder);

            var countFieldWidth = 80f;
            var field = Field(null,
                () => ListBinder.GetCount(listBinder),
                isReadOnly ? (Action<int>) null : (count) => ListBinder.SetCount(listBinder, count)
            ).SetWidth(countFieldWidth);
     
            var buttons = isReadOnly
                ? null
                : Row(
                    Space(),
                    Button("＋", () => ListBinder.AddItemAtLast(listBinder)),
                    Button("－", () => ListBinder.RemoveItemAtLast(listBinder))
                );

            return Fold(
                barLeft: label,
                barRight: field,
                elements: new[]
                {
                    Box(Indent(
                        List(listBinder, createItemElement),
                        buttons
                        )
                    )
                }
            );
        }
        
        public static Element List(IBinder listBinder, Func<IBinder, int, Element> createItemElement = null)
        {
            return DynamicElementOnStatusChanged(
                readStatus: () => ListBinder.GetCount(listBinder),
                build: _ =>
                {
                    createItemElement ??= ((binder, idx) => Field("Element " + idx, binder));

                    var itemBinderToElement = createItemElement;

                    var isReadOnly = ListBinder.IsReadOnly(listBinder);
                    if (!isReadOnly)
                    {
                        itemBinderToElement = (binder,idx) => ListViewItemContainerElement.AddPopupMenu(createItemElement(binder, idx), binder, idx);
                    }

                    return Column(
                        ListBinder.CreateItemBinders(listBinder).Select(itemBinderToElement)
                    ).SetInteractable(!isReadOnly);
                });
        }

#endif
    }
}