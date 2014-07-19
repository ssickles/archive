JulMar Model-View-ViewModel library
--------------------------------------------------------
This library is provided "as-is" with no warranty. It was developed by JulMar Technology, Inc. and is  
distributed from www.julmar.com. You are free to utilize the source code (in whole, or in part) or 
provided assembly however you desire, including creating derivative works without any compensation or requirements back
to JulMar Technology, Inc..  Any questions or comments can be directed to "mark@julmar.com"  This libary was intended to be used 
as a learning and teaching aid, and I hope it has some value to each person who looks at it.

Mark Smith 5/2009 mark@julmar.com

Releases:

1.0: 
Initial revision - all basic functionality is present.

1.01: rolled in a bunch of fixes from internal library -- 6/09
Fixed a bug in the event commander which was causing multiple invocations in some cases.
Added support for ShowDialog return results.
Added HelpProvider behavior to support invoking Windows Help
Removed automatic mediator registration from ViewModel - unnecessary perf hit when not using service mediator - you must call RegisterWithMessageMediator() deliberately now.
Added SystemInfo class to retrieve Windows version - can detect difference between W2K8 SP2 and Windows 7 (Environment.OSVersion does not appear to do so).
Added LinearGradientMarkupExtension for easy gradients
Added ENTER/ESC/Fx/CTRL support to NumericTextBoxBehavior.
Added DelegatingCommand<T>
Added ObservableDictionary<K,V>

1.02: additional support 7/09
Added ScrollingPreviewService
Added CommandParameter and Command to EventCommander event arguments
Added Past/Drop conversion to NumericTextBoxBehavior
Split out EditingViewModel into implementation class to make it easier to provide other forms of edit support.
Added PropertyObserver<T> from Josh Smith

1.03: Added new Behaviors assembly  7/30
Added dependency to System.Windows.Interactivity.dll
Ported over existing attached behaviors to Blend style behaviors [BREAKING CHANGE]
Added WatermarkedTextBoxBehavior
Added InvokeCommand action to bind to VM ICommands
Added CommandEventTrigger to support EventCommander from Blend

1.04: 9/15
Added Designer.InDesignMode static property to test design surface
Added test for null conditions in VisualTreeHelper extensions
Moved ValidatingViewModel into proper namespace
Added clipboard paste support to the WatermarkedTextBoxBehavior
Changed MTObservableCollection<T> to support true multi-threaded access based on discussion on WPFDisciples list.

1.05 11/1
Ported to VS2010 Beta 2
Removed ViewModel.OnPropertiesChanged - merged into ViewModel.OnPropertyChanged [BREAKING CHANGE]
Added generic support to MessageMediator
Added BindingTrigger to trigger of ViewModel binding values in Blend
Added CloseWindowBehavior to auto-close a dialog or modaless form without code behind
Added SelectTextOnFocusBehavior to select all text in a TextBox when it receives focus
Added ViewportSynchronizerBehavior to track the ViewPort of a ScrollViewer and bind it to ViewModel properties.
Added Dispatcher transition for RaiseClose and RaiseActivate from VM for cross-thread invocation
Added MultiConverter to aggregate value converters together.
Added ScrollingPreviewBehavior and ScrollBarPreviewBehaviors (replaced ScrollingPreviewService)
Added DeferredScrollBehavior