﻿using System;
using System.Linq;
using System.Collections.ObjectModel;

using Cinch;
using MVVM.DataAccess;
using System.Collections.Generic;
using System.ComponentModel;



namespace MVVM.Models
{
    /// <summary>
    /// Respresents a UI Customer Model, which has all the
    /// good stuff like Validation/INotifyPropertyChanged/IEditableObject
    /// which are all ready to use within the base class.
    /// 
    /// This class also makes use of <see cref="Cinch.DataWrapper">
    /// Cinch.DataWrapper</see>s. Where the idea is that the ViewModel
    /// is able to control the mode for the data, and as such the View
    /// simply binds to a instance of a <see cref="Cinch.DataWrapper">
    /// Cinch.DataWrapper</see> for both its data and its editable state.
    /// Where the View can disable a control based on the 
    /// <see cref="Cinch.DataWrapper">Cinch.DataWrapper</see> editable state.
    /// </summary>
    public class CustomerModel : Cinch.EditableValidatingObject
    {
        #region Data
        //Any data item is declared as a Cinch.DataWrapper, to allow the ViewModel
        //to decide what state the data is in, and the View just renders 
        //the data state accordingly
        private Cinch.DataWrapper<Int32> customerId;
        private Cinch.DataWrapper<String> firstName;
        private Cinch.DataWrapper<String> lastName;
        private Cinch.DataWrapper<String> email;
        private Cinch.DataWrapper<String> homePhoneNumber;
        private Cinch.DataWrapper<String> mobilePhoneNumber;
        private Cinch.DataWrapper<String> address1;
        private Cinch.DataWrapper<String> address2;
        private Cinch.DataWrapper<String> address3;
        private IEnumerable<DataWrapperBase> cachedListOfDataWrappers;
        private Cinch.DispatcherNotifiedObservableCollection<OrderModel> orders =
            new Cinch.DispatcherNotifiedObservableCollection<OrderModel>();
        private Boolean hasOrders = false;

        //rules
        private static SimpleRule firstNameRule;
        private static SimpleRule lastNameRule;
        private static SimpleRule emailRule;
        private static SimpleRule homePhoneNumberRule;
        private static SimpleRule address1Rule;
        private static SimpleRule address2Rule;
        private static SimpleRule address3Rule;

        #endregion

        #region Ctor

        public CustomerModel()
        {
            #region Create DataWrappers

            CustomerId = new DataWrapper<Int32>(this, customerIdChangeArgs);
            FirstName = new DataWrapper<String>(this, firstNameChangeArgs);
            LastName = new DataWrapper<String>(this, lastNameChangeArgs);
            Email = new DataWrapper<String>(this, emailChangeArgs);
            HomePhoneNumber = new DataWrapper<String>(this, homePhoneNumberChangeArgs);
            MobilePhoneNumber = new DataWrapper<String>(this, mobilePhoneNumberChangeArgs);
            Address1 = new DataWrapper<String>(this, address1ChangeArgs);
            Address2 = new DataWrapper<String>(this, address2ChangeArgs);
            Address3 = new DataWrapper<String>(this, address3ChangeArgs);

            //fetch list of all DataWrappers, so they can be used again later without the
            //need for reflection
            cachedListOfDataWrappers =
                DataWrapperHelper.GetWrapperProperties<CustomerModel>(this);
            #endregion

            #region Create Validation Rules

            firstName.AddRule(firstNameRule);
            lastName.AddRule(lastNameRule);
            email.AddRule(emailRule);
            homePhoneNumber.AddRule(homePhoneNumberRule);
            address1.AddRule(address1Rule);
            address2.AddRule(address2Rule);
            address3.AddRule(address3Rule);
            #endregion
        }


        static CustomerModel()
        {

            firstNameRule = new SimpleRule("DataValue", "Firstname can not be empty",
                      (Object domainObject)=>
                      {
                          DataWrapper<String> obj = (DataWrapper<String>)domainObject;
                          return String.IsNullOrEmpty(obj.DataValue);
                      });
            lastNameRule = new SimpleRule("DataValue", "Lastname can not be empty",
                      (Object domainObject) =>
                      {
                          DataWrapper<String> obj = (DataWrapper<String>)domainObject;
                          return String.IsNullOrEmpty(obj.DataValue);
                      });
            emailRule = new SimpleRule("DataValue", "Email can not be empty",
                      (Object domainObject) =>
                      {
                          DataWrapper<String> obj = (DataWrapper<String>)domainObject;
                          return String.IsNullOrEmpty(obj.DataValue);
                      });
            homePhoneNumberRule = new SimpleRule("DataValue", "HomePhoneNumber can not be empty",
                      (Object domainObject) =>
                      {
                          DataWrapper<String> obj = (DataWrapper<String>)domainObject;
                          return String.IsNullOrEmpty(obj.DataValue);
                      });
            address1Rule = new SimpleRule("DataValue", "Address1 can not be empty",
                      (Object domainObject) =>
                      {
                          DataWrapper<String> obj = (DataWrapper<String>)domainObject;
                          return String.IsNullOrEmpty(obj.DataValue);
                      });
            address2Rule = new SimpleRule("DataValue", "Address2 can not be empty",
                      (Object domainObject) =>
                      {
                          DataWrapper<String> obj = (DataWrapper<String>)domainObject;
                          return String.IsNullOrEmpty(obj.DataValue);
                      });
            address3Rule = new SimpleRule("DataValue", "Address3 can not be empty",
                      (Object domainObject) =>
                      {
                          DataWrapper<String> obj = (DataWrapper<String>)domainObject;
                          return String.IsNullOrEmpty(obj.DataValue);
                      });

        }

        #endregion

        #region Public Properties

        public String FullName
        {
            get { return FirstName.DataValue + " " + LastName.DataValue; }
        }

        /// <summary>
        /// CustomerId
        /// </summary>
        static PropertyChangedEventArgs customerIdChangeArgs =
            ObservableHelper.CreateArgs<CustomerModel>(x => x.CustomerId);

        public Cinch.DataWrapper<Int32> CustomerId
        {
            get { return customerId; }
            private set
            {
                customerId = value;
                NotifyPropertyChanged(customerIdChangeArgs);
            }
        }

        /// <summary>
        /// FirstName
        /// </summary>
        static PropertyChangedEventArgs firstNameChangeArgs =
            ObservableHelper.CreateArgs<CustomerModel>(x => x.FirstName);

        public Cinch.DataWrapper<String> FirstName
        {
            get { return firstName; }
            private set
            {
                firstName = value;
                NotifyPropertyChanged(firstNameChangeArgs);
            }
        }

        /// <summary>
        /// LastName
        /// </summary>
        static PropertyChangedEventArgs lastNameChangeArgs =
            ObservableHelper.CreateArgs<CustomerModel>(x => x.LastName);

        public Cinch.DataWrapper<String> LastName
        {
            get { return lastName; }
            private set
            {
                lastName = value;
                NotifyPropertyChanged(lastNameChangeArgs);
            }
        }

        /// <summary>
        /// Email
        /// </summary>
        static PropertyChangedEventArgs emailChangeArgs =
            ObservableHelper.CreateArgs<CustomerModel>(x => x.Email);

        public Cinch.DataWrapper<String> Email
        {
            get { return email; }
            private set
            {
                email = value;
                NotifyPropertyChanged(emailChangeArgs);
            }
        }

        /// <summary>
        /// HomePhoneNumber
        /// </summary>
        static PropertyChangedEventArgs homePhoneNumberChangeArgs =
            ObservableHelper.CreateArgs<CustomerModel>(x => x.HomePhoneNumber);

        public Cinch.DataWrapper<String> HomePhoneNumber
        {
            get { return homePhoneNumber; }
            private set
            {
                homePhoneNumber = value;
                NotifyPropertyChanged(homePhoneNumberChangeArgs);
            }
        }

        /// <summary>
        /// MobilePhoneNumber
        /// </summary>
        static PropertyChangedEventArgs mobilePhoneNumberChangeArgs =
            ObservableHelper.CreateArgs<CustomerModel>(x => x.MobilePhoneNumber);

        public Cinch.DataWrapper<String> MobilePhoneNumber
        {
            get { return mobilePhoneNumber; }
            private set
            {
                mobilePhoneNumber = value;
                NotifyPropertyChanged(mobilePhoneNumberChangeArgs);
            }
        }

        /// <summary>
        /// Address1
        /// </summary>
        static PropertyChangedEventArgs address1ChangeArgs =
            ObservableHelper.CreateArgs<CustomerModel>(x => x.Address1);

        public Cinch.DataWrapper<String> Address1
        {
            get { return address1; }
            private set
            {
                address1 = value;
                NotifyPropertyChanged(address1ChangeArgs);
            }
        }

        /// <summary>
        /// Address2
        /// </summary>
        static PropertyChangedEventArgs address2ChangeArgs =
            ObservableHelper.CreateArgs<CustomerModel>(x => x.Address2);

        public Cinch.DataWrapper<String> Address2
        {
            get { return address2; }
            private set
            {
                address2 = value;
                NotifyPropertyChanged(address2ChangeArgs);
            }
        }

        /// <summary>
        /// Address3
        /// </summary>
        static PropertyChangedEventArgs address3ChangeArgs =
            ObservableHelper.CreateArgs<CustomerModel>(x => x.Address3);

        public Cinch.DataWrapper<String> Address3
        {
            get { return address3; }
            private set
            {
                address3 = value;
                NotifyPropertyChanged(address3ChangeArgs);
            }
        }

        /// <summary>
        /// Orders
        /// </summary>
        static PropertyChangedEventArgs ordersChangeArgs =
            ObservableHelper.CreateArgs<CustomerModel>(x => x.Orders);

        public DispatcherNotifiedObservableCollection<OrderModel> Orders
        {
            get { return orders; }
            set
            {
                orders = value;
                NotifyPropertyChanged(ordersChangeArgs);
                HasOrders = orders.Count > 0;
            }
        }

        /// <summary>
        /// HasOrders
        /// </summary>
        static PropertyChangedEventArgs hasOrdersChangeArgs =
            ObservableHelper.CreateArgs<CustomerModel>(x => x.HasOrders);

        public Boolean HasOrders
        {
            get { return hasOrders; }
            set
            {
                hasOrders = value;
                NotifyPropertyChanged(hasOrdersChangeArgs);
            }
        }

        /// <summary>
        /// Returns cached collection of DataWrapperBase
        /// </summary>
        public IEnumerable<DataWrapperBase> CachedListOfDataWrappers
        {
            get { return cachedListOfDataWrappers; }
        }
        #endregion

        #region Static Methods
        /// <summary>
        /// Allows Service layer objects to be translated into
        /// UI objects
        /// </summary>
        /// <param name="cust">Service layer object</param>
        /// <returns>UI layer object</returns>
        public static CustomerModel CustomerToCustomerModel(Customer cust)
        {
            CustomerModel customerModel = new CustomerModel();
            customerModel.CustomerId.DataValue = cust.CustomerId;
            customerModel.FirstName.DataValue = String.IsNullOrEmpty(cust.FirstName) ? String.Empty : cust.FirstName.Trim();
            customerModel.LastName.DataValue = String.IsNullOrEmpty(cust.LastName) ? String.Empty : cust.LastName.Trim();
            customerModel.Email.DataValue = String.IsNullOrEmpty(cust.Email) ? String.Empty : cust.Email.Trim();
            customerModel.HomePhoneNumber.DataValue = String.IsNullOrEmpty(cust.HomePhoneNumber) ? String.Empty : cust.HomePhoneNumber.Trim();
            customerModel.MobilePhoneNumber.DataValue = String.IsNullOrEmpty(cust.MobilePhoneNumber) ? String.Empty : cust.MobilePhoneNumber.Trim();
            customerModel.Address1.DataValue = String.IsNullOrEmpty(cust.Address1) ? String.Empty : cust.Address1.Trim();
            customerModel.Address2.DataValue = String.IsNullOrEmpty(cust.Address2) ? String.Empty : cust.Address2.Trim();
            customerModel.Address3.DataValue = String.IsNullOrEmpty(cust.Address3) ? String.Empty : cust.Address3.Trim();
            //convert to UI type objects
            customerModel.Orders = new Cinch.DispatcherNotifiedObservableCollection<OrderModel>(cust.Orders.ToList().ConvertAll(
                    new Converter<Order, OrderModel>(OrderModel.OrderToOrderModel)));
            return customerModel;

        }
        #endregion

        #region Overrides

        /// <summary>
        /// Is the Model Valid
        /// </summary>
        public override bool IsValid
        {
            get
            {
                //return base.IsValid and use DataWrapperHelper, if you are
                //using DataWrappers
                return base.IsValid &&
                    DataWrapperHelper.AllValid(cachedListOfDataWrappers);
            }
        }

        #endregion

        #region EditableValidatingObject overrides

        /// <summary>
        /// Override hook which allows us to also put any child 
        /// EditableValidatingObject objects into the BeginEdit state
        /// </summary>
        protected override void OnBeginEdit()
        {
            base.OnBeginEdit();
            //Now walk the list of properties in the CustomerModel
            //and call BeginEdit() on all Cinch.DataWrapper<T>s.
            //we can use the Cinch.DataWrapperHelper class for this
            DataWrapperHelper.SetBeginEdit(cachedListOfDataWrappers);
        }

        /// <summary>
        /// Override hook which allows us to also put any child 
        /// EditableValidatingObject objects into the EndEdit state
        /// </summary>
        protected override void OnEndEdit()
        {
            base.OnEndEdit();
            //Now walk the list of properties in the CustomerModel
            //and call CancelEdit() on all Cinch.DataWrapper<T>s.
            //we can use the Cinch.DataWrapperHelper class for this
            DataWrapperHelper.SetEndEdit(cachedListOfDataWrappers);
        }

        /// <summary>
        /// Override hook which allows us to also put any child 
        /// EditableValidatingObject objects into the CancelEdit state
        /// </summary>
        protected override void OnCancelEdit()
        {
            base.OnCancelEdit();
            //Now walk the list of properties in the CustomerModel
            //and call CancelEdit() on all Cinch.DataWrapper<T>s.
            //we can use the Cinch.DataWrapperHelper class for this
            DataWrapperHelper.SetCancelEdit(cachedListOfDataWrappers);

        }
        #endregion
    }
}
