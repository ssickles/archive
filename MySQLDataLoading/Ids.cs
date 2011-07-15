using System;

using Mindscape.LightSpeed;
using Mindscape.LightSpeed.Validation;
using Mindscape.LightSpeed.Linq;

namespace MySQLDataLoading
{
  [Serializable]
  [System.CodeDom.Compiler.GeneratedCode("LightSpeedModelGenerator", "1.0.0.0")]
  [Table("identities")]
  public partial class Identity : Entity<int>
  {
    #region Fields
  
    [Column("first_name")]
    [ValidatePresence]
    [ValidateLength(0, 50)]
    private string _firstName;
    [Column("last_name")]
    [ValidatePresence]
    [ValidateLength(0, 50)]
    private string _lastName;
    [Column("country_code")]
    [ValidateLength(0, 2)]
    private string _countryCode;
    private byte _active;
    [Column("bio_enabled")]
    private byte _bioEnabled;
    [Column("identity_code")]
    [ValidatePresence]
    [ValidateLength(0, 5)]
    private string _identityCode;
    [Column("t24_id")]
    [ValidateLength(0, 64)]
    private string _t24Id;

    #endregion
    
    #region Field attribute and view names
    
    public const string FirstNameField = "FirstName";
    public const string LastNameField = "LastName";
    public const string CountryCodeField = "CountryCode";
    public const string ActiveField = "Active";
    public const string BioEnabledField = "BioEnabled";
    public const string IdentityCodeField = "IdentityCode";
    public const string T24IdField = "T24Id";


    #endregion
    
    #region Properties


    public string FirstName
    {
      get { return Get(ref _firstName); }
      set { Set(ref _firstName, value, "FirstName"); }
    }

    public string LastName
    {
      get { return Get(ref _lastName); }
      set { Set(ref _lastName, value, "LastName"); }
    }

    public string CountryCode
    {
      get { return Get(ref _countryCode); }
      set { Set(ref _countryCode, value, "CountryCode"); }
    }

    public byte Active
    {
      get { return Get(ref _active); }
      set { Set(ref _active, value, "Active"); }
    }

    public byte BioEnabled
    {
      get { return Get(ref _bioEnabled); }
      set { Set(ref _bioEnabled, value, "BioEnabled"); }
    }

    public string IdentityCode
    {
      get { return Get(ref _identityCode); }
      set { Set(ref _identityCode, value, "IdentityCode"); }
    }

    public string T24Id
    {
      get { return Get(ref _t24Id); }
      set { Set(ref _t24Id, value, "T24Id"); }
    }

    #endregion
  }

  [Serializable]
  [System.CodeDom.Compiler.GeneratedCode("LightSpeedModelGenerator", "1.0.0.0")]
  [Table("logins")]
  public partial class Login : Entity<int>
  {
    #region Fields
  
    [ValidatePresence]
    [ValidateLength(0, 45)]
    private string _login;
    [ValidateLength(0, 128)]
    private string _password;
    [Column("application_code")]
    [ValidatePresence]
    [ValidateLength(0, 3)]
    private string _applicationCode;
    [Column("identity_id")]
    private uint _identityId;
    [Column("systemlogin_id")]
    [ValidateLength(0, 45)]
    private string _systemloginId;
    [Column("systemlogin_password")]
    [ValidateLength(0, 128)]
    private string _systemloginPassword;
    [Column("role_code")]
    [ValidatePresence]
    [ValidateLength(0, 8)]
    private string _roleCode;
    [Column("use_generated_pass")]
    private uint _useGeneratedPass;
    [Column("orig_password")]
    [ValidateLength(0, 128)]
    private string _origPassword;
    [Column("first_logon")]
    private uint _firstLogon;

    #endregion
    
    #region Field attribute and view names
    
    public const string LoginField = "Username";
    public const string PasswordField = "Password";
    public const string ApplicationCodeField = "ApplicationCode";
    public const string IdentityIdField = "IdentityId";
    public const string SystemloginIdField = "SystemloginId";
    public const string SystemloginPasswordField = "SystemloginPassword";
    public const string RoleCodeField = "RoleCode";
    public const string UseGeneratedPassField = "UseGeneratedPass";
    public const string OrigPasswordField = "OrigPassword";
    public const string FirstLogonField = "FirstLogon";


    #endregion
    
    #region Properties


    public string Username
    {
      get { return Get(ref _login); }
      set { Set(ref _login, value, "Username"); }
    }

    public string Password
    {
      get { return Get(ref _password); }
      set { Set(ref _password, value, "Password"); }
    }

    public string ApplicationCode
    {
      get { return Get(ref _applicationCode); }
      set { Set(ref _applicationCode, value, "ApplicationCode"); }
    }

    public uint IdentityId
    {
      get { return Get(ref _identityId); }
      set { Set(ref _identityId, value, "IdentityId"); }
    }

    public string SystemloginId
    {
      get { return Get(ref _systemloginId); }
      set { Set(ref _systemloginId, value, "SystemloginId"); }
    }

    public string SystemloginPassword
    {
      get { return Get(ref _systemloginPassword); }
      set { Set(ref _systemloginPassword, value, "SystemloginPassword"); }
    }

    public string RoleCode
    {
      get { return Get(ref _roleCode); }
      set { Set(ref _roleCode, value, "RoleCode"); }
    }

    public uint UseGeneratedPass
    {
      get { return Get(ref _useGeneratedPass); }
      set { Set(ref _useGeneratedPass, value, "UseGeneratedPass"); }
    }

    public string OrigPassword
    {
      get { return Get(ref _origPassword); }
      set { Set(ref _origPassword, value, "OrigPassword"); }
    }

    public uint FirstLogon
    {
      get { return Get(ref _firstLogon); }
      set { Set(ref _firstLogon, value, "FirstLogon"); }
    }

    #endregion
  }


  [System.CodeDom.Compiler.GeneratedCode("LightSpeedModelGenerator", "1.0.0.0")]
  public partial class IdsUnitOfWork : Mindscape.LightSpeed.UnitOfWork
  {
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public IdsUnitOfWork()
    {
    }
    

    public System.Linq.IQueryable<Identity> Identities
    {
      get { return this.Query<Identity>(); }
    }
    
    public System.Linq.IQueryable<Login> Logins
    {
      get { return this.Query<Login>(); }
    }
    
  }

}
