using Coditech.DataAccessLayer;
using Coditech.ExceptionManager;
using Coditech.Model;
using Coditech.Model.Model;
using Coditech.Resources;
using Coditech.Utilities.Constant;
using Coditech.Utilities.Helper;
using Coditech.ViewModel;

using System;
using System.Collections.Specialized;
using System.Linq;

using static Coditech.Utilities.Helper.CoditechHelperUtility;
namespace Coditech.BusinessLogicLayer
{
    public class ApplicationLicenseDetailsBA : BaseBusinessLogic
    {
        ApplicationLicenseDetailsDAL _applicationLicenseDetailDAL = null;
        public ApplicationLicenseDetailsBA()
        {
            _applicationLicenseDetailDAL = new ApplicationLicenseDetailsDAL();
        }

        public ApplicationLicenseDetailsListViewModel GetProductList(DataTableModel dataTableModel)
        {
            FilterCollection filters = null;
            if (!string.IsNullOrEmpty(dataTableModel.SearchBy))
            {
                filters = new FilterCollection();
                filters.Add("ClientName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("DomainName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("LicenseType", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("IsActive", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
            }

            NameValueCollection sortlist = SortingData(dataTableModel.SortByColumn, dataTableModel.SortBy);
            ApplicationLicenseDetailsListModel ApplicationLicenseDetailsList = _applicationLicenseDetailDAL.GetProductList(filters, sortlist, dataTableModel.PageIndex, dataTableModel.PageSize);
            ApplicationLicenseDetailsListViewModel listViewModel = new ApplicationLicenseDetailsListViewModel { ApplicationLicenseDetailsList = ApplicationLicenseDetailsList?.ApplicationLicenseDetailsList?.ToViewModel<ApplicationLicenseDetailsViewModel>().ToList() };

            SetListPagingData(listViewModel.PageListViewModel, ApplicationLicenseDetailsList, dataTableModel, listViewModel.ApplicationLicenseDetailsList.Count);

            return listViewModel;
        }

        //Create ApplicationLicenseDetails.
        public ApplicationLicenseDetailsViewModel CreateApplicationLicenseDetails(ApplicationLicenseDetailsViewModel applicationLicenseDetailViewModel)
        {
            try
            {
                applicationLicenseDetailViewModel.CreatedBy = LoginUserId();
                applicationLicenseDetailViewModel.APIKey = Guid.NewGuid().ToString(); ;
                ApplicationLicenseDetailsModel applicationLicenseDetailModel = _applicationLicenseDetailDAL.CreateApplicationLicenseDetail(applicationLicenseDetailViewModel.ToModel<ApplicationLicenseDetailsModel>());
                return IsNotNull(applicationLicenseDetailModel) ? applicationLicenseDetailModel.ToViewModel<ApplicationLicenseDetailsViewModel>() : new ApplicationLicenseDetailsViewModel();
            }
            catch (CoditechException ex)
            {
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (ApplicationLicenseDetailsViewModel)GetViewModelWithErrorMessage(applicationLicenseDetailViewModel, ex.ErrorMessage);
                    default:
                        return (ApplicationLicenseDetailsViewModel)GetViewModelWithErrorMessage(applicationLicenseDetailViewModel, GeneralResources.ErrorFailedToCreate);
                }
            }
            catch (Exception ex)
            {
                CoditechFileLogging.LogMessage(ex.Message, CoditechComponents.Components.ApplicationLicenseDetails.ToString());
                return (ApplicationLicenseDetailsViewModel)GetViewModelWithErrorMessage(applicationLicenseDetailViewModel, GeneralResources.ErrorFailedToCreate);
            }
        }

        //Get ApplicationLicenseDetails by ApplicationLicenseDetails id.
        public ApplicationLicenseDetailsViewModel GetApplicationLicenseDetails(int applicationLicenseId)
            => _applicationLicenseDetailDAL.GetApplicationLicenseDetail(applicationLicenseId).ToViewModel<ApplicationLicenseDetailsViewModel>();

        //Update ApplicationLicenseDetails.
        public ApplicationLicenseDetailsViewModel UpdateApplicationLicenseDetails(ApplicationLicenseDetailsViewModel applicationLicenseDetailViewModel)
        {
            try
            {
                applicationLicenseDetailViewModel.ModifiedBy = LoginUserId();
                ApplicationLicenseDetailsModel applicationLicenseDetailModel = _applicationLicenseDetailDAL.UpdateApplicationLicenseDetail(applicationLicenseDetailViewModel.ToModel<ApplicationLicenseDetailsModel>());
                return IsNotNull(applicationLicenseDetailModel) ? applicationLicenseDetailModel.ToViewModel<ApplicationLicenseDetailsViewModel>() : (ApplicationLicenseDetailsViewModel)GetViewModelWithErrorMessage(new ApplicationLicenseDetailsListViewModel(), GeneralResources.UpdateErrorMessage);
            }
            catch (Exception ex)
            {
                CoditechFileLogging.LogMessage(ex.Message, CoditechComponents.Components.ApplicationLicenseDetails.ToString());
                return (ApplicationLicenseDetailsViewModel)GetViewModelWithErrorMessage(applicationLicenseDetailViewModel, GeneralResources.UpdateErrorMessage);
            }
        }

        //Delete ApplicationLicenseDetails.
        public bool DeleteApplicationLicenseDetails(string applicationLicenseId, out string errorMessage)
        {
            errorMessage = GeneralResources.ErrorFailedToDelete;
            try
            {
                return _applicationLicenseDetailDAL.DeleteApplicationLicenseDetail(new ParameterModel() { Ids = applicationLicenseId });
            }
            catch (CoditechException ex)
            {
                switch (ex.ErrorCode)
                {
                    default:
                        errorMessage = GeneralResources.ErrorFailedToDelete;
                        return false;
                }
            }
            catch (Exception ex)
            {
                CoditechFileLogging.LogMessage(ex.Message, CoditechComponents.Components.ApplicationLicenseDetails.ToString());
                errorMessage = GeneralResources.ErrorFailedToDelete;
                return false;
            }
        }

        public ActiveApplicationLicenseModel IsApplicationLicenseActive(string apiKeyWithDomainName)
        {
            ActiveApplicationLicenseModel model = new ActiveApplicationLicenseModel();
            if (string.IsNullOrEmpty(apiKeyWithDomainName))
            {
                model.ErrorMessage = "parameter is null or empty";
            }
            string[] apiKeyWithDomainNameArray = CoditechHelperUtility.Base64Decode(apiKeyWithDomainName)?.Split('|');
            model = _applicationLicenseDetailDAL.IsApplicationLicenseActive(apiKeyWithDomainNameArray[0], apiKeyWithDomainNameArray[1]);
            return model;
        }


    }
}
