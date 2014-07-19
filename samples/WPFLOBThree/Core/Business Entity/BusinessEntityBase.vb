Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Reflection
Imports System.Text
Imports System.Windows
Imports Core.StringFormatting
Imports Core.Validation

Namespace BusinessEntity

    'TODO need to work out serialization of business objects.  Will need to mark several members non-serializable and will also need to raise some events during the serialization reconstruction calls

    'threading references
    'http://www.informit.com/guides/printerfriendly.aspx?g=dotnet&seqNum=598&rl=1
    'http://msdn2.microsoft.com/en-us/library/1c9txz50.aspx
    ''' <summary> 
    ''' Implements the INotifyPropertyChanged interface and exposes a RaisePropertyChanged method for derived classes to raise the PropertyChange event. The event 
    ''' arguments created by this class are cached to prevent managed heap fragmentation. 
    ''' </summary> 
    <Serializable()> _
    Public MustInherit Class BusinessEntityBase
        Implements INotifyPropertyChanged
        Implements INotifyPropertyChanging
        Implements IDataErrorInfo
        Implements IBusinessEntityAudit

#Region " Declarations "

        Private Const STRING_END_LOADING_NEVER_CALLED As String = "EndLoading never called after a BeginLoading call was made.  No operations are permitted until EndLoading has been called."
        Private _bolHasBeenValidated As Boolean = False
        Private Shared _bolHaveSharedRulesBeenGenerated As Boolean = False
        Private _bolIsDirty As Boolean = False
        Private _bolIsLoading As Boolean = False
        Private _objInstanceValidationRulesManager As ValidationRulesManager
        Private Shared _objLockObject As New Object()
        Private _objValidationErrors As Dictionary(Of String, ValidationError)
        Private _strActiveRuleSet As String = String.Empty

#End Region

#Region " Public Events "

        '''' <summary> 
        '''' Raised when a public property of this object is set. 
        '''' </summary> 
        Public Event PropertyChanged As PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

        ''' <summary> 
        ''' Raised just before a public property of this object is set.  Used by LINQ for tracking changes.  Good reference: http://davidhayden.com/blog/dave/archive/2006/05/20/2949.aspx
        ''' </summary> 
        Public Event PropertyChanging As PropertyChangingEventHandler Implements System.ComponentModel.INotifyPropertyChanging.PropertyChanging

#End Region

#Region " Properties "

        ''' <summary>
        ''' Used by this base class internally as a method of getting the Instance Validation RulesManager.  This property uses lazy object creation.
        ''' </summary>
        ''' <returns>ValidationRulesManager</returns>
        <Browsable(False)> _
        Private ReadOnly Property InstanceValidationRulesManager() As ValidationRulesManager
            Get

                If _bolIsLoading = True Then
                    Throw New InvalidOperationException(STRING_END_LOADING_NEVER_CALLED)
                End If

                'lazy object creation
                If _objInstanceValidationRulesManager Is Nothing Then
                    _objInstanceValidationRulesManager = New ValidationRulesManager
                End If

                Return _objInstanceValidationRulesManager
            End Get
        End Property

        ''' <summary>
        ''' Use this property to have a specific rule set checked in addition to all rules that are not assigned a specific rule set.  For example, if you set this property to, Insert; when the rules are checked, all general rules will be checked and the Insert rules will also be checked.
        ''' </summary>
        ''' <value>String</value>
        ''' <returns>String</returns>
        Public Property ActiveRuleSet() As String
            Get
                Return _strActiveRuleSet
            End Get
            Set(ByVal Value As String)
                _strActiveRuleSet = Value
            End Set
        End Property

        ''' <summary>
        ''' Gets a multi-line error message indicating what is wrong with this object.  Every error in the Broken Rules collection is returned.
        ''' </summary>
        ''' <returns>String</returns>
        <Browsable(False)> _
        Public ReadOnly Property [Error]() As String Implements System.ComponentModel.IDataErrorInfo.Error
            Get

                If _bolIsLoading = True Then
                    Throw New InvalidOperationException(STRING_END_LOADING_NEVER_CALLED)
                End If

                Dim sb As New StringBuilder(4096)

                For Each obj As ValidationError In Me.GetAllBrokenRules

                    If obj.BrokenRule.RuleEventArgs.CustomMessage.Length > 0 Then
                        sb.Append(obj.BrokenRule.RuleEventArgs.CustomMessage)
                        sb.Append(" : ")
                        sb.AppendLine(obj.BrokenRule.RuleEventArgs.BrokenRuleDescription)

                    Else
                        sb.AppendLine(obj.BrokenRule.RuleEventArgs.BrokenRuleDescription)
                    End If

                    sb.AppendLine()
                Next

                'this removes the last append line characters
                If sb.Length > 4 Then
                    sb.Length = sb.Length - 4
                End If

                Return sb.ToString
            End Get
        End Property

        ''' <summary>
        ''' Has this object been validated.  This value is automatically assigned by this base class.  After the IsValid property has returned a True value, this value is then set to True.
        ''' </summary>
        ''' <returns>Boolean</returns>
        <Browsable(False)> _
        Public ReadOnly Property HasBeenValidated() As Boolean
            Get

                If _bolIsLoading = True Then
                    Throw New InvalidOperationException(STRING_END_LOADING_NEVER_CALLED)
                End If

                Return _bolHasBeenValidated
            End Get
        End Property

        ''' <summary>
        ''' Is this object dirty.  Have changes been made since the object was loaded or a new object was constructed.  This is automatically kept track of by this base class.
        ''' </summary>
        ''' <returns>Boolean</returns>
        <Browsable(False)> _
        Public ReadOnly Property IsDirty() As Boolean
            Get

                If _bolIsLoading = True Then
                    Throw New InvalidOperationException(STRING_END_LOADING_NEVER_CALLED)
                End If

                Return _bolIsDirty
            End Get
        End Property

        ''' <summary>
        ''' Gets a multi-line error message indicating what is wrong with this property.  Every error for this property in the Broken Rules collection is returned.
        ''' </summary>
        ''' <returns>String</returns>
        <Browsable(False)> _
        Default Public ReadOnly Property Item(ByVal columnName As String) As String Implements System.ComponentModel.IDataErrorInfo.Item
            Get

                Dim sb As New StringBuilder(1024)

                For Each obj As ValidationError In Me.GetBrokenRulesForProperty(columnName)

                    If obj.BrokenRule.RuleEventArgs.CustomMessage.Length > 0 Then
                        sb.Append(obj.BrokenRule.RuleEventArgs.CustomMessage)
                        sb.Append(" : ")
                        sb.AppendLine(obj.BrokenRule.RuleEventArgs.BrokenRuleDescription)

                    Else
                        sb.AppendLine(obj.BrokenRule.RuleEventArgs.BrokenRuleDescription)
                    End If

                    sb.AppendLine()
                Next

                'this removes the last append line characters
                If sb.Length > 4 Then
                    sb.Length = sb.Length - 4
                End If

                Return sb.ToString
            End Get
        End Property

        ''' <summary>
        ''' A dictionary object of all broken rules (validation errors)
        ''' </summary>
        ''' <returns>Dictionary(Of String, ValidationError)</returns>
        <Browsable(False)> _
        Public ReadOnly Property ValidatationErrors() As Dictionary(Of String, ValidationError)
            Get

                If _bolIsLoading = True Then
                    Throw New InvalidOperationException(STRING_END_LOADING_NEVER_CALLED)
                End If

                'lazy object creation
                If _objValidationErrors Is Nothing Then
                    _objValidationErrors = New Dictionary(Of String, ValidationError)
                End If

                Return _objValidationErrors
            End Get
        End Property

#End Region

#Region " Constructors "

        Shared Sub New()

        End Sub

        Protected Sub New()
            AddInstanceBusinessValidationRules()

            If _bolHaveSharedRulesBeenGenerated = False Then
                'AddSharedBusinessRules is only called once.  It is here because this Private Sub AddSharedBusinessRules calls Overrideable non-shared methods 
                AddSharedBusinessRules()
            End If

        End Sub

#End Region

#Region " Rules Methods "

        ''' <summary>
        ''' Runs the CheckAllRules methods and returns a Boolean indicating if the object is valid (does it pass all Shared and Instance rules).
        ''' </summary>
        ''' <returns>Boolean</returns>
        Public Function IsValid() As Boolean

            Dim bolIsValid As Boolean = False

            If _bolIsLoading = True Then
                Throw New InvalidOperationException(STRING_END_LOADING_NEVER_CALLED)
            End If

            CheckAllRules()

            If Me.ValidatationErrors.Count = 0 Then
                bolIsValid = True

            Else
                bolIsValid = False
            End If

            _bolHasBeenValidated = bolIsValid
            OnPropertyChanged("HasBeenValidated")
            Return bolIsValid

        End Function

        ''' <summary>
        ''' This code gets called on the first time this object type is constructed
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub AddSharedBusinessRules()

            If Not Validation.SharedValidationRules.RulesExistFor(Me.GetType) Then
                SyncLock _objLockObject

                    If _bolHaveSharedRulesBeenGenerated = True Then
                        Exit Sub
                    End If

                    _bolHaveSharedRulesBeenGenerated = True

                    Dim mgrValidation As ValidationRulesManager = Validation.SharedValidationRules.GetManager(Me.GetType)
                    Dim mgrCharacterCasing As CharacterCasingRulesManager = SharedCharacterCasingRules.GetManager(Me.GetType)

                    For Each prop As PropertyInfo In Me.GetType.GetProperties

                        For Each atr As BaseValidatorAttribute In prop.GetCustomAttributes(GetType(BaseValidatorAttribute), False)
                            mgrValidation.AddRule(atr.Create(prop.Name), prop.Name)
                        Next

                        For Each atr As CharacterCasingFormattingAttribute In prop.GetCustomAttributes(GetType(CharacterCasingFormattingAttribute), False)
                            mgrCharacterCasing.AddRule(prop.Name, atr.CharacterCasing)
                        Next

                    Next

                    AddSharedBusinessValidationRules(mgrValidation)
                    AddSharedCharacterCasingFormattingRules(mgrCharacterCasing)

                End SyncLock
            End If

        End Sub

        ''' <summary>
        ''' Override this method in your business class to be notified when you need to set up SHARED business rules.  This method is only used by the sub-class and not consumers of the sub-class.
        ''' To add shared rules to business objects to derving class properties, override this method in deriving classes and add the rules to the ValidationRulesManager.
        ''' This method will only be called once; the first time the deriving class is created
        ''' </summary>
        Protected Overridable Sub AddSharedBusinessValidationRules(ByVal mgrValidation As ValidationRulesManager)

        End Sub

        ''' <summary>
        ''' Override this method in your business class to be notified when you need to set up SHARED character casing rules.  This method is only used by the sub-class and not consumers of the sub-class.
        ''' To add shared character case formatting to deriving class properties, override this method in deriving classes and add the rules to the CharacterCasingRulesManager.
        ''' This method will only be called once; the first time the deriving class is created.
        ''' </summary>
        Protected Overridable Sub AddSharedCharacterCasingFormattingRules(ByVal mgrCharacterCasing As CharacterCasingRulesManager)

        End Sub

        ''' <summary>
        ''' This is used by sub-classes and classes that consume the sub-class business entity and need to add an instance rule to the list of rules to be enforced.
        ''' </summary>
        Public Sub AddInstanceRule(ByVal handler As RuleHandler, ByVal e As RuleEventArgs)
            Me.InstanceValidationRulesManager.GetRulesForProperty(e.PropertyName).List.Add(New Validator(handler, e, True))

        End Sub

        ''' <summary>
        ''' Override this method in your business class to be notified when you need to set up business rules.  This method is only used by the sub-class and not consumers of the sub-class.
        ''' Use the instance method, AddInstanceRule to in deriving classes to add instance rules to the object.
        ''' </summary>
        Protected Overridable Sub AddInstanceBusinessValidationRules()

        End Sub

        ''' <summary>
        ''' Validates the entity against all shared and instance rules.
        ''' </summary>
        Public Sub CheckAllRules()

            Dim bolRaiseErrorPropertyChanged As Boolean = False
            Dim mgr As ValidationRulesManager = Validation.SharedValidationRules.GetManager(Me.GetType)

            For Each vrl As ValidationRulesList In mgr.RulesDictionary.Values

                For Each obj As IValidationRuleMethod In vrl.List

                    If obj.RuleEventArgs.RuleSet.Length = 0 OrElse (Me.ActiveRuleSet.Length > 0 AndAlso String.Compare(obj.RuleEventArgs.RuleSet, Me.ActiveRuleSet, StringComparison.CurrentCultureIgnoreCase) = 0) Then

                        'remove broken rule if it exists, if not does nothing
                        If Me.ValidatationErrors.Remove(obj.RuleName) Then
                            bolRaiseErrorPropertyChanged = True
                        End If

                        If obj.Invoke(Me) = False Then
                            bolRaiseErrorPropertyChanged = True
                            Me.ValidatationErrors.Add(obj.RuleName, New ValidationError(obj))
                            OnPropertyChanged(obj.RuleEventArgs.PropertyName)
                        End If

                    End If

                Next

            Next

            If _objInstanceValidationRulesManager IsNot Nothing Then

                For Each vrl As ValidationRulesList In Me.InstanceValidationRulesManager.RulesDictionary.Values

                    For Each obj As IValidationRuleMethod In vrl.List

                        If obj.RuleEventArgs.RuleSet.Length = 0 OrElse (Me.ActiveRuleSet.Length > 0 AndAlso String.Compare(obj.RuleEventArgs.RuleSet, Me.ActiveRuleSet, StringComparison.CurrentCultureIgnoreCase) = 0) Then

                            'remove broken rule if it exists, if not does nothing
                            If Me.ValidatationErrors.Remove(obj.RuleName) Then
                                bolRaiseErrorPropertyChanged = True
                            End If

                            If obj.Invoke(Me) = False Then
                                bolRaiseErrorPropertyChanged = True
                                Me.ValidatationErrors.Add(obj.RuleName, New ValidationError(obj))
                                OnPropertyChanged(obj.RuleEventArgs.PropertyName)
                            End If

                        End If

                    Next

                Next

            End If

            If bolRaiseErrorPropertyChanged Then
                OnPropertyChanged("Error")
            End If

        End Sub

        ''' <summary>
        ''' Validates the property against all shared and instance rules for the property.
        ''' </summary>
        Public Sub CheckRulesForProperty(ByVal strPropertyName As String)

            Dim bolRaiseErrorPropertyChanged As Boolean = False
            Dim mgr As ValidationRulesManager = Validation.SharedValidationRules.GetManager(Me.GetType)

            For Each obj As IValidationRuleMethod In mgr.GetRulesForProperty(strPropertyName).List

                If obj.RuleEventArgs.RuleSet.Length = 0 OrElse (Me.ActiveRuleSet.Length > 0 AndAlso String.Compare(obj.RuleEventArgs.RuleSet, Me.ActiveRuleSet, StringComparison.CurrentCultureIgnoreCase) = 0) Then

                    'remove broken rule if it exists, if not does nothing
                    If Me.ValidatationErrors.Remove(obj.RuleName) Then
                        bolRaiseErrorPropertyChanged = True
                    End If

                    If obj.Invoke(Me) = False Then
                        bolRaiseErrorPropertyChanged = True
                        Me.ValidatationErrors.Add(obj.RuleName, New ValidationError(obj))
                    End If

                End If

            Next

            If _objInstanceValidationRulesManager IsNot Nothing Then

                For Each obj As IValidationRuleMethod In Me.InstanceValidationRulesManager.GetRulesForProperty(strPropertyName).List

                    If obj.RuleEventArgs.RuleSet.Length = 0 OrElse (Me.ActiveRuleSet.Length > 0 AndAlso String.Compare(obj.RuleEventArgs.RuleSet, Me.ActiveRuleSet, StringComparison.CurrentCultureIgnoreCase) = 0) Then

                        'remove broken rule if it exists, if not does nothing
                        If Me.ValidatationErrors.Remove(obj.RuleName) Then
                            bolRaiseErrorPropertyChanged = True
                        End If

                        If obj.Invoke(Me) = False Then
                            bolRaiseErrorPropertyChanged = True
                            Me.ValidatationErrors.Add(obj.RuleName, New ValidationError(obj))
                        End If

                    End If

                Next

            End If

            If bolRaiseErrorPropertyChanged Then
                OnPropertyChanged("Error")
            End If

        End Sub

        ''' <summary>
        ''' A List of ValidationError objects for the object. 
        ''' </summary>
        Public Function GetAllBrokenRules() As List(Of ValidationError)

            Dim objValidationErrors As New List(Of ValidationError)

            For Each obj As Generic.KeyValuePair(Of String, ValidationError) In Me.ValidatationErrors
                objValidationErrors.Add(obj.Value)
            Next

            Return objValidationErrors

        End Function

        ''' <summary>
        ''' A List of ValidationError objects for the property. 
        ''' </summary>
        Public Function GetBrokenRulesForProperty(ByVal strPropertyName As String) As List(Of ValidationError)

            Dim objValidationErrors As New List(Of ValidationError)

            For Each obj As Generic.KeyValuePair(Of String, ValidationError) In Me.ValidatationErrors

                If String.Compare(obj.Value.PropertyName, strPropertyName, StringComparison.OrdinalIgnoreCase) = 0 Then
                    objValidationErrors.Add(obj.Value)
                End If

            Next

            Return objValidationErrors

        End Function

#End Region

#Region " Property Set and Change Notification Methods "

        ''' <summary> 
        ''' Derived classes can override this method to execute logic after the property is set. The base implementation does nothing. 
        ''' </summary> 
        ''' <param name="strPropertyName"> 
        ''' The property which was changed. 
        ''' </param> 
        Protected Overridable Sub AfterPropertyChanged(ByVal strPropertyName As String)

        End Sub

        ''' <summary> 
        ''' Derived classes can override this method to execute logic before the property is set. The base implementation does nothing. 
        ''' </summary> 
        ''' <param name="strPropertyName"> 
        ''' The property which was changed. 
        ''' </param> 
        Protected Overridable Sub BeforePropertyChanged(ByVal strPropertyName As String)

        End Sub

        ''' <summary> 
        ''' Raises the PropertyChanged event, and invokes the AfterPropertyChanged method
        ''' </summary> 
        ''' <param name="strPropertyName"> 
        ''' The property which was changed. 
        ''' </param> 
        Protected Sub OnPropertyChanged(ByVal strPropertyName As String)

            If Me.PropertyChangedEvent IsNot Nothing Then

                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(strPropertyName))
            End If

            Me.AfterPropertyChanged(strPropertyName)

        End Sub

        ''' <summary> 
        ''' Raises the PropertyChanging event, and invokes the BeforePropertyChanged method
        ''' </summary> 
        ''' <param name="strPropertyName"> 
        ''' The property which was changed. 
        ''' </param> 
        Protected Sub OnPropertyChanging(ByVal strPropertyName As String)
            Me.BeforePropertyChanged(strPropertyName)

            If Me.PropertyChangingEvent IsNot Nothing Then

                RaiseEvent PropertyChanging(Me, New PropertyChangingEventArgs(strPropertyName))
            End If

        End Sub

        ''' <summary>
        ''' Called by in business entity sub-classes in their property setters to set the value of the property.
        ''' If the business object is not in a loading state, this method performs validation on the property
        ''' <example>Example:
        ''' <code>
        '''   Set(ByVal Value As String)
        '''       MyBase.SetPropertyValue("SL_DatabaseConnection", _strSL_DatabaseConnection, Value)
        '''   End Set
        ''' </code>
        ''' </example>
        ''' </summary>
        ''' <param name="strPropertyName">Property Name</param>
        ''' <param name="strCurrentValue">Current property value</param>
        ''' <param name="strNewValue">New property value</param>
        ''' <remarks></remarks>
        Protected Sub SetPropertyValue(ByVal strPropertyName As String, ByRef strCurrentValue As String, ByVal strNewValue As String)

            If strCurrentValue Is Nothing Then

                If strNewValue Is Nothing Then
                    Exit Sub
                End If

            ElseIf strNewValue IsNot Nothing AndAlso strCurrentValue.Equals(strNewValue) Then
                Exit Sub
            End If

            If Not _bolIsLoading Then
                _bolHasBeenValidated = False
                _bolIsDirty = True

                OnPropertyChanging(strPropertyName)

                'only apply character casing rules after the object is loaded.
                Dim enumCharacterCasing As CharacterCasing = SharedCharacterCasingRules.GetManager(Me.GetType).GetRulesForProperty(strPropertyName)

                If enumCharacterCasing <> CharacterCasing.None Then
                    strCurrentValue = FormatText.ApplyCharacterCasing(strNewValue, enumCharacterCasing)

                Else
                    strCurrentValue = strNewValue
                End If

                CheckRulesForProperty(strPropertyName)
                OnPropertyChanged(strPropertyName)
                OnPropertyChanged("IsDirty")
                OnPropertyChanged("HasBeenValidated")

            Else
                'since we are loading, just set the value
                strCurrentValue = strNewValue
            End If

        End Sub

        ''' <summary>
        ''' Called by business entity sub-classes in their property setters to set the value of the property.
        ''' If the business object is not in a loading state, this method performs validation on the property
        ''' <example>Example:
        ''' <code>
        '''   Set(ByVal Value As String)
        '''       MyBase.SetPropertyValue("Birthday", _datBirthDay, Value)
        '''   End Set
        ''' </code>
        ''' </example>
        ''' </summary>
        ''' <typeparam name="T">Property Type</typeparam>
        ''' <param name="strPropertyName">Property Name</param>
        ''' <param name="objCurrentValue">variable that holds the current value of the property</param>
        ''' <param name="objNewValue">Is the Value parameter from the Setter Set.</param>
        ''' <remarks></remarks>
        Protected Sub SetPropertyValue(Of T)(ByVal strPropertyName As String, ByRef objCurrentValue As T, ByVal objNewValue As T)

            If objCurrentValue Is Nothing Then

                If objNewValue Is Nothing Then
                    Exit Sub
                End If

            ElseIf objNewValue IsNot Nothing AndAlso objCurrentValue.Equals(objNewValue) Then
                Exit Sub
            End If

            If Not _bolIsLoading Then
                _bolHasBeenValidated = False
                _bolIsDirty = True
                OnPropertyChanging(strPropertyName)
                objCurrentValue = objNewValue
                CheckRulesForProperty(strPropertyName)
                OnPropertyChanged(strPropertyName)
                OnPropertyChanged("IsDirty")
                OnPropertyChanged("HasBeenValidated")

            Else
                'if we are loading the object then just assign the value
                objCurrentValue = objNewValue
            End If

        End Sub

#End Region

#Region " Object Loading, Complete Loading & Persisted Methods "

        ''' <summary>
        ''' Called when the business object is being loaded from a database.  This saves time and processing; by passing property setter logic during loading.  After the business object has been loaded the EndLoading MUST be called.
        ''' </summary>
        Public Sub BeginLoading()
            _bolIsLoading = True

        End Sub

        ''' <summary>
        ''' After a business object has been loaded and the BeginLoading method was called, developers must call this method, EndLoading.  This method marks the entity IsDirty = False, HasBeenValidated = False and raises these property changed events.
        ''' </summary>
        Public Sub EndLoading()
            _bolIsLoading = False
            _bolHasBeenValidated = False
            _bolIsDirty = False
            OnPropertyChanged("IsDirty")
            OnPropertyChanged("HasBeenValidated")

        End Sub

        ''' <summary>
        ''' This method should be called by the business layer after a Valid business object has been persisted to a database, web service, etc.  Calling this method, marks this business object as Not Dirty.  
        ''' </summary>
        Public Sub ObjectPersisted()
            _bolIsDirty = False
            OnPropertyChanged("IsDirty")

        End Sub

#End Region

#Region " Audit Methods "

        ''' <summary>
        ''' Used to generate a Dictionary(Of String, String) for each property decorated with the AuditAttribute.  Dictionary is property name, property value.
        ''' </summary>
        ''' <param name="strDefaultValue">Default message if no class propeties are decorated</param>
        ''' <param name="objIDictionary">Pass an IDictionary object that needs to be populated.  Typically this would be the Data property of an Exception object.</param>
        ''' <returns>IDictionary</returns>
        Public Function ToAuditIDictionary(ByVal strDefaultValue As String, ByVal objIDictionary As IDictionary) As IDictionary Implements IBusinessEntityAudit.ToAuditIDictionary
            Return ClassToStringToDictionaryHelper.AuditToIDictionary(Me, strDefaultValue, objIDictionary)

        End Function

        ''' <summary>
        ''' Returns a string of each property decorated with the AuditAttribute.  The string displays the property name, property friendly name and property value.  This function is typically used when a developer needs to make an audit log entry.  It provides a very simple method to generate these messages.
        ''' </summary>
        ''' <param name="strDefaultValue">If no properties have been decorated with the AuditAttribute, then this message is displayed.</param>
        ''' <param name="strDelimiter">What delimiter do you want between each property.  Defaults to comma.  Could use vbcrlf, etc.</param>
        Public Function ToAuditString(ByVal strDefaultValue As String, Optional ByVal strDelimiter As String = ", ") As String Implements IBusinessEntityAudit.ToAuditString
            Return ClassToStringToDictionaryHelper.AuditToString(Me, strDefaultValue, strDelimiter)

        End Function

        ''' <summary>
        ''' Used to generate an Dictionary(Of String, String) for each property in the class.
        ''' </summary>
        ''' <param name="strDefaultValue">Default message if no class propeties are decorated</param>
        ''' <param name="objIDictionary">Pass an IDictionary object that needs to be populated.  Typicaly this would be the Data property of an Exception object.</param>
        ''' <param name="bolSortByPropertyName">Normally sorts the output by property name.  To leave in ordinal order, set to False</param>
        ''' <returns>IDictionary</returns>
        Public Function ToClassIDictionary(ByVal strDefaultValue As String, ByVal objIDictionary As IDictionary, Optional ByVal bolSortByPropertyName As Boolean = True) As IDictionary Implements IBusinessEntityAudit.ToClassIDictionary
            Return ClassToStringToDictionaryHelper.ClassToIDictionary(Me, strDefaultValue, objIDictionary, bolSortByPropertyName)

        End Function

        ''' <summary>
        ''' This function returns a string with each property and value in the class.  The string displays the property name, property friendly name and property value.
        ''' </summary>
        ''' <param name="strDelimiter">What delimiter do you want between each property.  Defaults to comma.  Could use vbcrlf, etc.</param>
        ''' <param name="bolSortByProperytName">Normally sorts the output by property name.  To leave in ordinal order, set to False</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ToClassString(Optional ByVal strDelimiter As String = ", ", Optional ByVal bolSortByProperytName As Boolean = True) As String Implements IBusinessEntityAudit.ToClassString
            Return ClassToStringToDictionaryHelper.ClassToString(Me, strDelimiter, bolSortByProperytName)

        End Function

#End Region

    End Class

End Namespace
