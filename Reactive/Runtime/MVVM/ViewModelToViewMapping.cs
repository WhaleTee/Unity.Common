using System;

namespace WhaleTee.Reactive.Runtime.MVVM
{
    [Serializable]
    public class ViewModelToViewMapping
    {
        public string ViewModelTypeFullName;
        public View PrefabView;
    }
}