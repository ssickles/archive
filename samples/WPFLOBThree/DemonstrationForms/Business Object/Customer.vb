Imports Core.BusinessEntity
Imports Core.Validation
Imports Core.StringFormatting

Public Class Customer
    Inherits BusinessEntityBase

#Region " Declarations "

    Private _dblDeposit As Double
    Private _intYearJoined As Nullable(Of Integer)
    Private _strEmail As String = String.Empty
    Private _strFirstName As String = String.Empty
    Private _strLastName As String = String.Empty
    Private _strWorkPhone As String = String.Empty

#End Region

#Region " Properties "

    ''' <summary>
    ''' Gets or sets the deposit amount the customer paid.  Deposit is required.
    ''' </summary>
    ''' <returns>Deposit amount the custom paid the day they joined this program.</returns>
    ''' <remarks>This property demonstrates a required value greater than 0.  Since this is a Double, I have explictly passed in a double value by using the "#" sign.</remarks>
    <CompareValueValidator(ComparisionType.GreaterThan, 0.0#, True)> _
    Public Property Deposit() As Double
        Get
            Return _dblDeposit
        End Get
        Set(ByVal Value As Double)
            MyBase.SetPropertyValue("Deposit", _dblDeposit, Value)
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the email of the customer
    ''' </summary>
    ''' <returns>Email of the customer</returns>
    ''' <remarks>This property demonstrates not allowing a Null String, 
    ''' requiring an entry between 1 - 50 characters and 
    ''' validating against a regular expression.</remarks>
    <Audit(3)> _
    <CharacterCasingFormatting(CharacterCasing.LowerCase)> _
    <StringLengthValidator(1, 50)> _
    <RegularExpressionValidator(RegularExpressionPatternType.Email, True)> _
    Public Property Email() As String
        Get
            Return _strEmail
        End Get
        Set(ByVal Value As String)
            MyBase.SetPropertyValue("Email", _strEmail, Value)
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the first name of the customer
    ''' </summary>
    ''' <returns>First name of the customer</returns>
    ''' <remarks>This property demonstrates not allowing a Null String, requiring an entry between 1 and 25 characters</remarks>
    <Audit(1)> _
    <CharacterCasingFormatting(CharacterCasing.ProperName)> _
    <StringLengthValidator(1, 25)> _
    Public Property FirstName() As String
        Get
            Return _strFirstName
        End Get
        Set(ByVal Value As String)
            MyBase.SetPropertyValue("FirstName", _strFirstName, Value)
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the last name of the customer
    ''' </summary>
    ''' <returns>Last name of the customer</returns>
    ''' <remarks>This property demonstrates not allowing a Null String, requiring an entry between 1 and 25 characters</remarks>
    <Audit(2)> _
    <CharacterCasingFormatting(CharacterCasing.ProperName)> _
    <StringLengthValidator(1, 25)> _
    Public Property LastName() As String
        Get
            Return _strLastName
        End Get
        Set(ByVal Value As String)
            MyBase.SetPropertyValue("LastName", _strLastName, Value)
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the work phone number of the customer
    ''' </summary>
    ''' <returns>Work phone number of the customer</returns>
    ''' <remarks>This property demonstrates not allowing a Null String, not requiring an entry and limiting the length to 50
    ''' The Caption attribute is used here to report a different friendly name in the logging features
    ''' The PropertyFriendlyName is used to provide a custom friendly name for the work phone property.  This is here for demonstration purposes as Work Phone would be fine in an error message.
    ''' </remarks>
    <Audit(4)> _
    <CharacterCasingFormatting(CharacterCasing.OutlookPhoneProperName)> _
    <Caption("Work Phone Number")> _
    <StringLengthValidator(50, PropertyFriendlyName:="Work Phone Number")> _
    Public Property WorkPhone() As String
        Get
            Return _strWorkPhone
        End Get
        Set(ByVal Value As String)
            MyBase.SetPropertyValue("WorkPhone", _strWorkPhone, Value)
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the year the customer joined. Year is always equal to or greater than 2000 and less than or equal to the current year.
    ''' </summary>
    ''' <returns>Year the customer joined.</returns>
    ''' <remarks>
    ''' This property has its SHARED business rules added in code.  See the Sub OnAddSharedBusinessValidationRules below to see how SHARED business rules can be added in code as opposed to using attributes</remarks>
    Public Property YearJoined() As Nullable(Of Integer)
        Get
            Return _intYearJoined
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            MyBase.SetPropertyValue("YearJoined", _intYearJoined, Value)
        End Set
    End Property

#End Region

#Region " Methods "

    'The two below Sub both show how to add rules using code.
    '
    'The first one adds an instance rule and calculates the value that the property will be tested against
    '
    'The second add a SHARED rule using code rather than using an attribute.  I included this in case for some reason the developer didn't want to or was unable to add
    '  validation attributes to their business classes.  For example, if you are using the Visual Studio ORM designer, you won't be able to add attributes to the generated
    '  classes without risk loosing those changes the next time the ORM designer generates the classes.  By placing this type of code in a Partial class you can use the
    '  Visual Studio ORM designer and still have your classes validated using this system.
    Protected Overrides Sub AddInstanceBusinessValidationRules()
        'demonstrates how to add an instance rule based on a value determined at run-time.  In this case the current year.
        MyBase.AddInstanceRule(AddressOf ComparisionValidationRules.CompareValueRule, New CompareValueRuleEventArgs("YearJoined", ComparisionType.LessThanEqual, Now.Year, False))
    End Sub

    Protected Overrides Sub AddSharedBusinessValidationRules(ByVal mgrValidation As Core.Validation.ValidationRulesManager)
        'demonstrates how to add a SHARED rule in code rather that using an attribute
        'the following code does then same thing as this attribute placed on the YearJoined property
        '<CompareValueValidator(ComparisionType.GreaterThanEqual, 2000, False)>
        mgrValidation.AddRule(AddressOf ComparisionValidationRules.CompareValueRule, New CompareValueRuleEventArgs("YearJoined", ComparisionType.GreaterThanEqual, 2000, False))
    End Sub


#End Region

End Class
