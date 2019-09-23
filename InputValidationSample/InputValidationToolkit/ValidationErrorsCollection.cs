using System;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Specialized;

namespace InputValidationSample.InputValidationToolkit
{
    /// <summary>
    /// Without this class, the pain of the terrible story of .NET Language projections is real. Either my Google/Bing skills
    /// are fairly poor, or there is no concrete way for a .NET developer to go from System.ComponentModel.ObservableVector -> Windows.Foundation.Collections.IObservableVector
    /// without implementing this class themselves. I don't think i'm the only one feeling this pain:
    /// https://social.msdn.microsoft.com/Forums/en-US/53b3a0e1-05aa-47cf-a2c9-32799471b66e/ivectorchangedeventargs-why-u-no-provide-affected-object.
    /// 
    /// We'll probably need to provide an actual implementation class for this, or actually fix the .NET Projects in WinUI3.0.
    /// My vote is for the later, but you know, that like requires actual work across multiple teams.
    /// <summary/>
    public class InputValidationErrorsCollection :
        System.Collections.ObjectModel.ObservableCollection<InputValidationError>,
        IObservableVector<InputValidationError>
    {
        public InputValidationErrorsCollection()
        {
            CollectionChanged += InputValidationErrorsCollection_CollectionChanged;
        }

        private void InputValidationErrorsCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            VectorChanged?.Invoke(this, new VectorChangedEventArgs(e));
        }

        public event VectorChangedEventHandler<InputValidationError> VectorChanged;
    }

    /// <summary>
    /// This class doesn't even make sense because the differences between NotifyCollectionChangedEventArgs and IVectorChangedEventArgs are
    /// annoying enough to make this non-trivial. But like, why???
    /// <summary/>
    class VectorChangedEventArgs : IVectorChangedEventArgs
    {
        NotifyCollectionChangedEventArgs _impl;
        public VectorChangedEventArgs(NotifyCollectionChangedEventArgs args)
        {
            _impl = args;
        }

        public CollectionChange CollectionChange
        {
            get
            {
                switch (_impl.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        return CollectionChange.ItemInserted;
                    case NotifyCollectionChangedAction.Move:
                    case NotifyCollectionChangedAction.Replace:
                        return CollectionChange.ItemChanged;
                    case NotifyCollectionChangedAction.Remove:
                        return CollectionChange.ItemRemoved;
                    case NotifyCollectionChangedAction.Reset:
                        // Wow, a 1-1 match!
                        return CollectionChange.Reset;
                    default:
                        throw new System.Exception("Invalid value for NotifyCollectionChangedAction");
                }
            }

        }

        public uint Index
        {
            get
            {
                switch (_impl.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        return (uint)_impl.NewStartingIndex;
                    case NotifyCollectionChangedAction.Move:
                    case NotifyCollectionChangedAction.Replace:
                    case NotifyCollectionChangedAction.Remove:
                        return (uint)_impl.OldStartingIndex;
                    case NotifyCollectionChangedAction.Reset:
                    default:
                        // Because why not?
                        return 0;
                }
            }
        }
    }
}
