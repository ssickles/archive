﻿// ****************************************************************************
// <copyright file="ViewModelBase.cs" company="GalaSoft Laurent Bugnion">
// Copyright © GalaSoft Laurent Bugnion 2009
// </copyright>
// ****************************************************************************
// <author>Laurent Bugnion</author>
// <email>laurent@galasoft.ch</email>
// <date>22.4.2009</date>
// <project>GalaSoft.MvvmLight</project>
// <web>http://www.galasoft.ch</web>
// <license>
// See license.txt in this project or http://www.galasoft.ch/license_MIT.txt
// </license>
// <LastBaseLevel>BL0006</LastBaseLevel>
// ****************************************************************************

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;

////using GalaSoft.Utilities.Attributes;

namespace GalaSoft.MvvmLight
{
    /// <summary>
    /// A base class for the ViewModel classes in the MVVM pattern.
    /// </summary>
    //// [ClassInfo(typeof(ViewModelBase),
    ////  VersionString = "2.0.0.0",
    ////  DateString = "200909281609",
    ////  Description = "A base class for the ViewModel classes in the MVVM pattern.",
    ////  UrlContacts = "http://www.galasoft.ch/contact_en.html",
    ////  Email = "laurent@galasoft.ch")]
    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        private static bool? _isInDesignMode;

        /// <summary>
        /// Initializes a new instance of the ViewModelBase class.
        /// </summary>
        protected ViewModelBase()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ViewModelBase class.
        /// </summary>
        /// <param name="messenger">An instance of a <see cref="Messenger" /> used to broadcast
        /// messages to other objects. If null, this class will attempt
        /// to broadcast using the Messenger's default instance.</param>
        protected ViewModelBase(Messenger messenger)
        {
            MessengerInstance = messenger;
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets a value indicating whether the control is in design mode (running in Blend
        /// or Visual Studio).
        /// </summary>
        [SuppressMessage(
            "Microsoft.Security", 
            "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands",
            Justification = "The security risk here is neglectible.")]
        public static bool IsInDesignModeStatic
        {
            get
            {
                if (!_isInDesignMode.HasValue)
                {
#if SILVERLIGHT
                    _isInDesignMode = DesignerProperties.IsInDesignTool;
#else
                    var prop = DesignerProperties.IsInDesignModeProperty;
                    _isInDesignMode
                        = (bool)DependencyPropertyDescriptor
                        .FromProperty(prop, typeof(FrameworkElement))
                        .Metadata.DefaultValue;

                    // Just to be sure
                    if (!_isInDesignMode.Value
                        && Process.GetCurrentProcess().ProcessName.StartsWith("devenv", StringComparison.Ordinal))
                    {
                        _isInDesignMode = true;
                    }
#endif
                }

                return _isInDesignMode.Value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the control is in design mode (running under Blend
        /// or Visual Studio).
        /// </summary>
        [SuppressMessage(
            "Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "Non static member needed for data binding")]
        public bool IsInDesignMode
        {
            get
            {
                return IsInDesignModeStatic;
            }
        }

        /// <summary>
        /// Gets or sets an instance of a <see cref="Messenger" /> used to broadcast
        /// messages to other objects. If null, this class will attempt
        /// to broadcast using the Messenger's default instance.
        /// </summary>
        protected Messenger MessengerInstance
        {
            get;
            set;
        }

        /// <summary>
        /// Cleans up all resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Broadcasts a PropertyChangedMessage using either the instance of
        /// the Messenger that was passed to this class (if available) 
        /// or the Messenger's default instance.
        /// </summary>
        /// <typeparam name="T">The type of the property that changed.</typeparam>
        /// <param name="oldValue">The value of the property before it changed.</param>
        /// <param name="newValue">The value of the property after it changed.</param>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void Broadcast<T>(T oldValue, T newValue, string propertyName)
        {
            var message = new PropertyChangedMessage<T>(this, oldValue, newValue, propertyName);

            if (MessengerInstance != null)
            {
                MessengerInstance.Send(message);
                
                // OLD
                MessengerInstance.Broadcast(message);
            }
            else
            {
                Messenger.Default.Send(message);

                // OLD
                Messenger.Default.Broadcast(message);
            }
        }

        /// <summary>
        /// Cleans up resources.
        /// </summary>
        /// <param name="disposing">If true, all resources are cleaned. If false,
        /// only unmanaged resources are cleaned.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Messenger.Default.Unregister(this);
            }
        }

        /// <summary>
        /// Raises the PropertyChanged event if needed, and broadcasts a
        /// PropertyChangedMessage using the Messenger instance (or the
        /// static default instance if no Messenger instance is available).
        /// </summary>
        /// <typeparam name="T">The type of the property that changed.</typeparam>
        /// <param name="propertyName">The name of the property that changed.</param>
        /// <param name="oldValue">The property's value before the change occurred.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="broadcast">If true, a PropertyChangedMessage will
        /// be broadcasted. If false, only the event will be raised.</param>
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate",
            Justification = "This cannot be an event")]
        protected virtual void RaisePropertyChanged<T>(string propertyName, T oldValue, T newValue, bool broadcast)
        {
            RaisePropertyChanged(propertyName);

            if (broadcast)
            {
                Broadcast(oldValue, newValue, propertyName);
            }
        }

        /// <summary>
        /// Raises the PropertyChanged event if needed, and broadcasts a
        /// PropertyChangedMessage using the Messenger instance (or the
        /// static default instance if no Messenger instance is available).
        /// This method is obsolete and should be replaced by a call to
        /// RaisePropertyChanged(string, T, T, bool). Calling
        /// this obsolete method will use default(T) as the OldValue 
        /// of the PropertyChangedMessage.
        /// </summary>
        /// <typeparam name="T">The type of the property that changed.</typeparam>
        /// <param name="propertyName">The name of the property that changed.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="broadcast">If true, a PropertyChangedMessage will
        /// be broadcasted. If false, only the event will be raised.</param>
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate",
            Justification = "This cannot be an event")]
        [Obsolete("Use the method RaisePropertyChanged<T>(string, T, T, bool) instead.")]
        protected virtual void RaisePropertyChanged<T>(string propertyName, T newValue, bool broadcast)
        {
            RaisePropertyChanged(propertyName);

            if (broadcast)
            {
                Broadcast(default(T), newValue, propertyName);
            }
        }

        /// <summary>
        /// Raises the PropertyChanged event if needed.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate",
            Justification = "This cannot be an event")]
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}