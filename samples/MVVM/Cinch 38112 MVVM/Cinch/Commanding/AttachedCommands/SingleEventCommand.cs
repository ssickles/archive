﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Reflection;
using System.Windows.Media;

namespace Cinch
{
    #region SCommandArgs Class
    /// <summary>
    /// Allows a CommandParameter to be associated with a SingleEventCommand
    /// </summary>
    public class SCommandArgs
    {
        #region Data
        public object Sender { get; set; }
        public object EventArgs { get; set; }
        public object CommandParameter { get; set; }
        #endregion

        #region Ctor
        public SCommandArgs()
        {
        }

        public SCommandArgs(object sender, object eventArgs, object commandParameter)
        {
            Sender = sender;
            EventArgs = eventArgs;
            CommandParameter = commandParameter;
        }
        #endregion
    }
    #endregion




    #region SingleEventCommand Class
    /// <summary>
    /// This class allows a single command to event mappings.  
    /// It is used to wire up View events to a
    /// ViewModel ICommand implementation.  
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// 
    /// <ListBox ...     
    /// Cinch:SingleEventCommand.RoutedEventName="SelectionChanged"     
    /// Cinch:SingleEventCommand.TheCommandToRun="{Binding Path=BoxEditCommand}"     
    /// Cinch:SingleEventCommand.CommandParameter="{Binding ElementName=ListBoxVehicle, Path=SelectedItem}">
    /// </ListBox>
    /// 
    /// ]]>
    /// </example>
    public static class SingleEventCommand
    {
        #region TheCommandToRun

        /// <summary>
        /// TheCommandToRun : The actual ICommand to run
        /// </summary>
        public static readonly DependencyProperty TheCommandToRunProperty =
            DependencyProperty.RegisterAttached("TheCommandToRun",
                typeof(ICommand),
                typeof(SingleEventCommand),
                new FrameworkPropertyMetadata((ICommand)null));

        /// <summary>
        /// Gets the TheCommandToRun property.  
        /// </summary>
        public static ICommand GetTheCommandToRun(DependencyObject d)
        {
            return (ICommand)d.GetValue(TheCommandToRunProperty);
        }

        /// <summary>
        /// Sets the TheCommandToRun property.  
        /// </summary>
        public static void SetTheCommandToRun(DependencyObject d, ICommand value)
        {
            d.SetValue(TheCommandToRunProperty, value);
        }
        #endregion

        #region RoutedEventName

        /// <summary>
        /// RoutedEventName : The event that should actually execute the
        /// ICommand
        /// </summary>
        public static readonly DependencyProperty RoutedEventNameProperty =
            DependencyProperty.RegisterAttached("RoutedEventName", typeof(String),
            typeof(SingleEventCommand),
                new FrameworkPropertyMetadata((String)String.Empty,
                    new PropertyChangedCallback(OnRoutedEventNameChanged)));

        /// <summary>
        /// Gets the RoutedEventName property.  
        /// </summary>
        public static String GetRoutedEventName(DependencyObject d)
        {
            return (String)d.GetValue(RoutedEventNameProperty);
        }

        /// <summary>
        /// Sets the RoutedEventName property.  
        /// </summary>
        public static void SetRoutedEventName(DependencyObject d, String value)
        {
            d.SetValue(RoutedEventNameProperty, value);
        }

        /// <summary>
        /// Hooks up a Dynamically created EventHandler (by using the 
        /// <see cref="EventHooker">EventHooker</see> class) that when
        /// run will run the associated ICommand
        /// </summary>
        private static void OnRoutedEventNameChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            String routedEvent = (String)e.NewValue;

            if (d == null || String.IsNullOrEmpty(routedEvent))
                return;


            //If the RoutedEvent string is not null, create a new
            //dynamically created EventHandler that when run will execute
            //the actual bound ICommand instance (usually in the ViewModel)
            EventHooker eventHooker = new EventHooker();
            eventHooker.ObjectWithAttachedCommand = d;

            EventInfo eventInfo = d.GetType().GetEvent(routedEvent,
                BindingFlags.Public | BindingFlags.Instance);

            //Hook up Dynamically created event handler
            if (eventInfo != null)
            {
                eventInfo.RemoveEventHandler(d,
                    eventHooker.GetNewEventHandlerToRunCommand(eventInfo));

                eventInfo.AddEventHandler(d,
                    eventHooker.GetNewEventHandlerToRunCommand(eventInfo));
            }

        }
        #endregion

        #region CommandParameter
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter", typeof(object),
            typeof(SingleEventCommand), new UIPropertyMetadata(null));

        /// <summary>        
        /// Gets the CommandParameter property.          
        /// </summary>        
        public static object GetCommandParameter(DependencyObject obj)
        {
            return (object)obj.GetValue(CommandParameterProperty);
        }


        /// <summary>        
        /// Sets the CommandParameter property.          
        /// </summary>        
        public static void SetCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(CommandParameterProperty, value);
        }
        #endregion

    }
    #endregion

    #region EventHooker Class
    /// <summary>
    /// Contains the event that is hooked into the source RoutedEvent
    /// that was specified to run the ICommand
    /// </summary>
    sealed class EventHooker
    {
        #region Public Methods/Properties
        /// <summary>
        /// The DependencyObject, that holds a binding to the actual
        /// ICommand to execute
        /// </summary>
        public DependencyObject ObjectWithAttachedCommand { get; set; }

        /// <summary>
        /// Creates a Dynamic EventHandler that will be run the ICommand
        /// when the user specified RoutedEvent fires
        /// </summary>
        /// <param name="eventInfo">The specified RoutedEvent EventInfo</param>
        /// <returns>An Delegate that points to a new EventHandler
        /// that will be run the ICommand</returns>
        public Delegate GetNewEventHandlerToRunCommand(EventInfo eventInfo)
        {
            Delegate del = null;

            if (eventInfo == null)
                throw new ArgumentNullException("eventInfo");

            if (eventInfo.EventHandlerType == null)
                throw new ArgumentException("EventHandlerType is null");

            if (del == null)
                del = Delegate.CreateDelegate(eventInfo.EventHandlerType, this,
                      GetType().GetMethod("OnEventRaised",
                        BindingFlags.NonPublic |
                        BindingFlags.Instance));

            return del;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Runs the ICommand when the requested RoutedEvent fires
        /// </summary>
        private void OnEventRaised(object sender, EventArgs e)
        {
            ICommand command = (ICommand)(sender as DependencyObject).
                GetValue(SingleEventCommand.TheCommandToRunProperty);

            object commandParameter = (sender as DependencyObject).
                GetValue(SingleEventCommand.CommandParameterProperty);

            SCommandArgs commandArgs = new SCommandArgs(sender, e, commandParameter);
            if (command != null)
                command.Execute(commandArgs);

        }
        #endregion
    }
    #endregion

}
