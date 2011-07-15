
/**
 * ApplicationServiceCallbackHandler.java
 *
 * This file was auto-generated from WSDL
 * by the Apache Axis2 version: 1.5.4  Built on : Dec 19, 2010 (08:18:42 CET)
 */

    package test.ws;

    /**
     *  ApplicationServiceCallbackHandler Callback class, Users can extend this class and implement
     *  their own receiveResult and receiveError methods.
     */
    public abstract class ApplicationServiceCallbackHandler{



    protected Object clientData;

    /**
    * User can pass in any object that needs to be accessed once the NonBlocking
    * Web service call is finished and appropriate method of this CallBack is called.
    * @param clientData Object mechanism by which the user can pass in user data
    * that will be avilable at the time this callback is called.
    */
    public ApplicationServiceCallbackHandler(Object clientData){
        this.clientData = clientData;
    }

    /**
    * Please use this constructor if you don't want to set any clientData
    */
    public ApplicationServiceCallbackHandler(){
        this.clientData = null;
    }

    /**
     * Get the client data
     */

     public Object getClientData() {
        return clientData;
     }

        
           /**
            * auto generated Axis2 call back method for checkTransactionAccess method
            * override this method for handling normal response from checkTransactionAccess operation
            */
           public void receiveResultcheckTransactionAccess(
                    test.ws.ApplicationServiceStub.CheckTransactionAccessResponse result
                        ) {
           }

          /**
           * auto generated Axis2 Error handler
           * override this method for handling error response from checkTransactionAccess operation
           */
            public void receiveErrorcheckTransactionAccess(java.lang.Exception e) {
            }
                
           /**
            * auto generated Axis2 call back method for verifyIdentity method
            * override this method for handling normal response from verifyIdentity operation
            */
           public void receiveResultverifyIdentity(
                    test.ws.ApplicationServiceStub.VerifyIdentityResponse result
                        ) {
           }

          /**
           * auto generated Axis2 Error handler
           * override this method for handling error response from verifyIdentity operation
           */
            public void receiveErrorverifyIdentity(java.lang.Exception e) {
            }
                
           /**
            * auto generated Axis2 call back method for getAuthenticationUnitsForTransactionGroupIdentity method
            * override this method for handling normal response from getAuthenticationUnitsForTransactionGroupIdentity operation
            */
           public void receiveResultgetAuthenticationUnitsForTransactionGroupIdentity(
                    test.ws.ApplicationServiceStub.GetAuthenticationUnitsForTransactionGroupIdentityResponse result
                        ) {
           }

          /**
           * auto generated Axis2 Error handler
           * override this method for handling error response from getAuthenticationUnitsForTransactionGroupIdentity operation
           */
            public void receiveErrorgetAuthenticationUnitsForTransactionGroupIdentity(java.lang.Exception e) {
            }
                
           /**
            * auto generated Axis2 call back method for identifyEmployee method
            * override this method for handling normal response from identifyEmployee operation
            */
           public void receiveResultidentifyEmployee(
                    test.ws.ApplicationServiceStub.IdentifyEmployeeResponse result
                        ) {
           }

          /**
           * auto generated Axis2 Error handler
           * override this method for handling error response from identifyEmployee operation
           */
            public void receiveErroridentifyEmployee(java.lang.Exception e) {
            }
                
           /**
            * auto generated Axis2 call back method for getApplicationTypes method
            * override this method for handling normal response from getApplicationTypes operation
            */
           public void receiveResultgetApplicationTypes(
                    test.ws.ApplicationServiceStub.GetApplicationTypesResponse result
                        ) {
           }

          /**
           * auto generated Axis2 Error handler
           * override this method for handling error response from getApplicationTypes operation
           */
            public void receiveErrorgetApplicationTypes(java.lang.Exception e) {
            }
                
           /**
            * auto generated Axis2 call back method for getTransactions method
            * override this method for handling normal response from getTransactions operation
            */
           public void receiveResultgetTransactions(
                    test.ws.ApplicationServiceStub.GetTransactionsResponse result
                        ) {
           }

          /**
           * auto generated Axis2 Error handler
           * override this method for handling error response from getTransactions operation
           */
            public void receiveErrorgetTransactions(java.lang.Exception e) {
            }
                
           /**
            * auto generated Axis2 call back method for getIdentitiesByAccountId method
            * override this method for handling normal response from getIdentitiesByAccountId operation
            */
           public void receiveResultgetIdentitiesByAccountId(
                    test.ws.ApplicationServiceStub.GetIdentitiesByAccountIdResponse result
                        ) {
           }

          /**
           * auto generated Axis2 Error handler
           * override this method for handling error response from getIdentitiesByAccountId operation
           */
            public void receiveErrorgetIdentitiesByAccountId(java.lang.Exception e) {
            }
                
           /**
            * auto generated Axis2 call back method for getIdentityByName method
            * override this method for handling normal response from getIdentityByName operation
            */
           public void receiveResultgetIdentityByName(
                    test.ws.ApplicationServiceStub.GetIdentityByNameResponse result
                        ) {
           }

          /**
           * auto generated Axis2 Error handler
           * override this method for handling error response from getIdentityByName operation
           */
            public void receiveErrorgetIdentityByName(java.lang.Exception e) {
            }
                
           /**
            * auto generated Axis2 call back method for getAuthenticationUnits method
            * override this method for handling normal response from getAuthenticationUnits operation
            */
           public void receiveResultgetAuthenticationUnits(
                    test.ws.ApplicationServiceStub.GetAuthenticationUnitsResponse result
                        ) {
           }

          /**
           * auto generated Axis2 Error handler
           * override this method for handling error response from getAuthenticationUnits operation
           */
            public void receiveErrorgetAuthenticationUnits(java.lang.Exception e) {
            }
                
           /**
            * auto generated Axis2 call back method for getLoginsForEmployeeApplication method
            * override this method for handling normal response from getLoginsForEmployeeApplication operation
            */
           public void receiveResultgetLoginsForEmployeeApplication(
                    test.ws.ApplicationServiceStub.GetLoginsForEmployeeApplicationResponse result
                        ) {
           }

          /**
           * auto generated Axis2 Error handler
           * override this method for handling error response from getLoginsForEmployeeApplication operation
           */
            public void receiveErrorgetLoginsForEmployeeApplication(java.lang.Exception e) {
            }
                
           /**
            * auto generated Axis2 call back method for getLanguages method
            * override this method for handling normal response from getLanguages operation
            */
           public void receiveResultgetLanguages(
                    test.ws.ApplicationServiceStub.GetLanguagesResponse result
                        ) {
           }

          /**
           * auto generated Axis2 Error handler
           * override this method for handling error response from getLanguages operation
           */
            public void receiveErrorgetLanguages(java.lang.Exception e) {
            }
                
           /**
            * auto generated Axis2 call back method for getIdentityBySourceId method
            * override this method for handling normal response from getIdentityBySourceId operation
            */
           public void receiveResultgetIdentityBySourceId(
                    test.ws.ApplicationServiceStub.GetIdentityBySourceIdResponse result
                        ) {
           }

          /**
           * auto generated Axis2 Error handler
           * override this method for handling error response from getIdentityBySourceId operation
           */
            public void receiveErrorgetIdentityBySourceId(java.lang.Exception e) {
            }
                
           /**
            * auto generated Axis2 call back method for getApplications method
            * override this method for handling normal response from getApplications operation
            */
           public void receiveResultgetApplications(
                    test.ws.ApplicationServiceStub.GetApplicationsResponse result
                        ) {
           }

          /**
           * auto generated Axis2 Error handler
           * override this method for handling error response from getApplications operation
           */
            public void receiveErrorgetApplications(java.lang.Exception e) {
            }
                
           /**
            * auto generated Axis2 call back method for getAuthenticationTemplates method
            * override this method for handling normal response from getAuthenticationTemplates operation
            */
           public void receiveResultgetAuthenticationTemplates(
                    test.ws.ApplicationServiceStub.GetAuthenticationTemplatesResponse result
                        ) {
           }

          /**
           * auto generated Axis2 Error handler
           * override this method for handling error response from getAuthenticationTemplates operation
           */
            public void receiveErrorgetAuthenticationTemplates(java.lang.Exception e) {
            }
                
           /**
            * auto generated Axis2 call back method for getValidApplications method
            * override this method for handling normal response from getValidApplications operation
            */
           public void receiveResultgetValidApplications(
                    test.ws.ApplicationServiceStub.GetValidApplicationsResponse result
                        ) {
           }

          /**
           * auto generated Axis2 Error handler
           * override this method for handling error response from getValidApplications operation
           */
            public void receiveErrorgetValidApplications(java.lang.Exception e) {
            }
                
           /**
            * auto generated Axis2 call back method for getSettings method
            * override this method for handling normal response from getSettings operation
            */
           public void receiveResultgetSettings(
                    test.ws.ApplicationServiceStub.GetSettingsResponse result
                        ) {
           }

          /**
           * auto generated Axis2 Error handler
           * override this method for handling error response from getSettings operation
           */
            public void receiveErrorgetSettings(java.lang.Exception e) {
            }
                
           /**
            * auto generated Axis2 call back method for getIdentityByT24Id method
            * override this method for handling normal response from getIdentityByT24Id operation
            */
           public void receiveResultgetIdentityByT24Id(
                    test.ws.ApplicationServiceStub.GetIdentityByT24IdResponse result
                        ) {
           }

          /**
           * auto generated Axis2 Error handler
           * override this method for handling error response from getIdentityByT24Id operation
           */
            public void receiveErrorgetIdentityByT24Id(java.lang.Exception e) {
            }
                
           /**
            * auto generated Axis2 call back method for getIdentityByUid method
            * override this method for handling normal response from getIdentityByUid operation
            */
           public void receiveResultgetIdentityByUid(
                    test.ws.ApplicationServiceStub.GetIdentityByUidResponse result
                        ) {
           }

          /**
           * auto generated Axis2 Error handler
           * override this method for handling error response from getIdentityByUid operation
           */
            public void receiveErrorgetIdentityByUid(java.lang.Exception e) {
            }
                
           /**
            * auto generated Axis2 call back method for login method
            * override this method for handling normal response from login operation
            */
           public void receiveResultlogin(
                    test.ws.ApplicationServiceStub.LoginResponse result
                        ) {
           }

          /**
           * auto generated Axis2 Error handler
           * override this method for handling error response from login operation
           */
            public void receiveErrorlogin(java.lang.Exception e) {
            }
                
           /**
            * auto generated Axis2 call back method for getEnrolledAuthenticationTypes method
            * override this method for handling normal response from getEnrolledAuthenticationTypes operation
            */
           public void receiveResultgetEnrolledAuthenticationTypes(
                    test.ws.ApplicationServiceStub.GetEnrolledAuthenticationTypesResponse result
                        ) {
           }

          /**
           * auto generated Axis2 Error handler
           * override this method for handling error response from getEnrolledAuthenticationTypes operation
           */
            public void receiveErrorgetEnrolledAuthenticationTypes(java.lang.Exception e) {
            }
                
           /**
            * auto generated Axis2 call back method for setLanguage method
            * override this method for handling normal response from setLanguage operation
            */
           public void receiveResultsetLanguage(
                    test.ws.ApplicationServiceStub.SetLanguageResponse result
                        ) {
           }

          /**
           * auto generated Axis2 Error handler
           * override this method for handling error response from setLanguage operation
           */
            public void receiveErrorsetLanguage(java.lang.Exception e) {
            }
                
           /**
            * auto generated Axis2 call back method for getEnrollments method
            * override this method for handling normal response from getEnrollments operation
            */
           public void receiveResultgetEnrollments(
                    test.ws.ApplicationServiceStub.GetEnrollmentsResponse result
                        ) {
           }

          /**
           * auto generated Axis2 Error handler
           * override this method for handling error response from getEnrollments operation
           */
            public void receiveErrorgetEnrollments(java.lang.Exception e) {
            }
                
           /**
            * auto generated Axis2 call back method for getIdentityImageAsJpeg method
            * override this method for handling normal response from getIdentityImageAsJpeg operation
            */
           public void receiveResultgetIdentityImageAsJpeg(
                    test.ws.ApplicationServiceStub.GetIdentityImageAsJpegResponse result
                        ) {
           }

          /**
           * auto generated Axis2 Error handler
           * override this method for handling error response from getIdentityImageAsJpeg operation
           */
            public void receiveErrorgetIdentityImageAsJpeg(java.lang.Exception e) {
            }
                
           /**
            * auto generated Axis2 call back method for getNumberOfActiveIdentitiesWithAccessToTransaction method
            * override this method for handling normal response from getNumberOfActiveIdentitiesWithAccessToTransaction operation
            */
           public void receiveResultgetNumberOfActiveIdentitiesWithAccessToTransaction(
                    test.ws.ApplicationServiceStub.GetNumberOfActiveIdentitiesWithAccessToTransactionResponse result
                        ) {
           }

          /**
           * auto generated Axis2 Error handler
           * override this method for handling error response from getNumberOfActiveIdentitiesWithAccessToTransaction operation
           */
            public void receiveErrorgetNumberOfActiveIdentitiesWithAccessToTransaction(java.lang.Exception e) {
            }
                
           /**
            * auto generated Axis2 call back method for getAuthenticationTypes method
            * override this method for handling normal response from getAuthenticationTypes operation
            */
           public void receiveResultgetAuthenticationTypes(
                    test.ws.ApplicationServiceStub.GetAuthenticationTypesResponse result
                        ) {
           }

          /**
           * auto generated Axis2 Error handler
           * override this method for handling error response from getAuthenticationTypes operation
           */
            public void receiveErrorgetAuthenticationTypes(java.lang.Exception e) {
            }
                


    }
    