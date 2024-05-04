using Coditech.DataAccessLayer.DataEntity;
using Coditech.DataAccessLayer.Helper;
using Coditech.DataAccessLayer.Repository;
using Coditech.ExceptionManager;
using Coditech.Model;
using Coditech.Resources;
using Coditech.Utilities.Helper;

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;

using static Coditech.Utilities.Helper.CoditechHelperUtility;
namespace Coditech.DataAccessLayer
{
    public class ApplicationLicenseDetailsDAL
    {
        private readonly ICoditechRepository<ApplicationLicenseDetail> _ApplicationLicenseDetailsRepository;
        public ApplicationLicenseDetailsDAL()
        {
            _ApplicationLicenseDetailsRepository = new CoditechRepository<ApplicationLicenseDetail>();
        }

        public ApplicationLicenseDetailsListModel GetProductList(FilterCollection filters, NameValueCollection sorts, int pagingStart, int pagingLength)
        {
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            ApplicationLicenseDetailsListModel listModel = new ApplicationLicenseDetailsListModel();
            List<ApplicationLicenseDetail> applicationLicenseList = _ApplicationLicenseDetailsRepository.GetEntityList(pageListModel.SPWhereClause)?.ToList();
            listModel.ApplicationLicenseDetailsList = new List<ApplicationLicenseDetailsModel>();
            foreach (ApplicationLicenseDetail applicationLicenseDetail in applicationLicenseList)
            {
                listModel.ApplicationLicenseDetailsList.Add(new ApplicationLicenseDetailsModel()
                {
                    ApplicationLicenseId = applicationLicenseDetail.ApplicationLicenseId,
                    LicenseType = applicationLicenseDetail.LicenseType,
                    ClientName = applicationLicenseDetail.ClientName,
                    DomainName = applicationLicenseDetail.DomainName,
                    APIKey = applicationLicenseDetail.APIKey,
                    ValidFromDate = applicationLicenseDetail.ValidFromDate,
                    ValidUptoDate = applicationLicenseDetail.ValidUptoDate,
                    IsActive = applicationLicenseDetail.IsActive
                });
            }
            listModel.BindPageListModel(pageListModel);
            return listModel;
        }

        //Create ApplicationLicenseDetail.
        public ApplicationLicenseDetailsModel CreateApplicationLicenseDetail(ApplicationLicenseDetailsModel ApplicationLicenseDetailsModel)
        {
            if (IsNull(ApplicationLicenseDetailsModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            if (IsClientNameAlreadyExist(ApplicationLicenseDetailsModel.ClientName))
            {
                throw new CoditechException(ErrorCodes.AlreadyExist, string.Format(GeneralResources.ErrorCodeExists, "ApplicationLicenseDetail name"));
            }

            //Create new ApplicationLicenseDetail and return it.
            ApplicationLicenseDetail applicationLicenseDetail = _ApplicationLicenseDetailsRepository.Insert(ApplicationLicenseDetailsModel.FromModelToEntity<ApplicationLicenseDetail>());
            if (applicationLicenseDetail?.ApplicationLicenseId > 0)
            {
                ApplicationLicenseDetailsModel.ApplicationLicenseId = applicationLicenseDetail.ApplicationLicenseId;
            }
            else
            {
                ApplicationLicenseDetailsModel.HasError = true;
                ApplicationLicenseDetailsModel.ErrorMessage = GeneralResources.ErrorFailedToCreate;
            }
            return ApplicationLicenseDetailsModel;
        }

        //Get ApplicationLicenseDetail by ApplicationLicenseDetail id.
        public ApplicationLicenseDetailsModel GetApplicationLicenseDetail(int applicationLicenseId)
        {
            if (applicationLicenseId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "ApplicationLicenseId"));

            //Get the ApplicationLicenseDetail Details based on id.
            ApplicationLicenseDetail ApplicationLicenseDetailsData = _ApplicationLicenseDetailsRepository.Table.FirstOrDefault(x => x.ApplicationLicenseId == applicationLicenseId);
            ApplicationLicenseDetailsModel ApplicationLicenseDetailsModel = ApplicationLicenseDetailsData.FromEntityToModel<ApplicationLicenseDetailsModel>();
            return ApplicationLicenseDetailsModel;
        }

        //Update ApplicationLicenseDetail.
        public ApplicationLicenseDetailsModel UpdateApplicationLicenseDetail(ApplicationLicenseDetailsModel applicationLicenseDetailsModel)
        {
            if (IsNull(applicationLicenseDetailsModel))
                throw new CoditechException(ErrorCodes.InvalidData, GeneralResources.ModelNotNull);

            if (applicationLicenseDetailsModel.ApplicationLicenseId < 1)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "ApplicationLicenseId"));

            if (IsClientNameAlreadyExist(applicationLicenseDetailsModel.ClientName, applicationLicenseDetailsModel.ApplicationLicenseId))
                throw new CoditechException(ErrorCodes.AlreadyExist, string.Format(GeneralResources.ErrorCodeExists, "Country Code"));

            ApplicationLicenseDetail applicationLicenseDetailsData = _ApplicationLicenseDetailsRepository.Table.FirstOrDefault(x => x.ApplicationLicenseId == applicationLicenseDetailsModel.ApplicationLicenseId);

            if (applicationLicenseDetailsData.ClientName == applicationLicenseDetailsModel.ClientName 
                && applicationLicenseDetailsData.DomainName == applicationLicenseDetailsModel.DomainName
                && applicationLicenseDetailsData.IsActive == applicationLicenseDetailsModel.IsActive 
                && applicationLicenseDetailsData.ValidFromDate == applicationLicenseDetailsModel.ValidFromDate
                && applicationLicenseDetailsData.ValidUptoDate == applicationLicenseDetailsModel.ValidUptoDate)
            {
                return applicationLicenseDetailsModel;
            }

            applicationLicenseDetailsData.ClientName = applicationLicenseDetailsModel.ClientName;
            applicationLicenseDetailsData.DomainName = applicationLicenseDetailsModel.DomainName;
            applicationLicenseDetailsData.IsActive = applicationLicenseDetailsModel.IsActive;
            applicationLicenseDetailsData.ValidFromDate = applicationLicenseDetailsModel.ValidFromDate;
            applicationLicenseDetailsData.ValidUptoDate = applicationLicenseDetailsModel.ValidUptoDate;

            //Update ApplicationLicenseDetail
            bool isApplicationLicenseDetailUpdated = _ApplicationLicenseDetailsRepository.Update(applicationLicenseDetailsData);
            if (!isApplicationLicenseDetailUpdated)
            {
                applicationLicenseDetailsModel.HasError = true;
                applicationLicenseDetailsModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
            }
            return applicationLicenseDetailsModel;
        }

        //Delete ApplicationLicenseDetail.
        public bool DeleteApplicationLicenseDetail(ParameterModel parameterModel)
        {
            if (IsNull(parameterModel) || string.IsNullOrEmpty(parameterModel.Ids))
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "ApplicationLicenseDetailID"));

            CoditechViewRepository<View_ReturnBoolean> objStoredProc = new CoditechViewRepository<View_ReturnBoolean>();
            objStoredProc.SetParameter("ApplicationLicenseId", parameterModel.Ids, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("Status", null, ParameterDirection.Output, DbType.Int32);
            int status = 0;
            objStoredProc.ExecuteStoredProcedureList("Coditech_DeleteApplicationLicenseDetails @ApplicationLicenseId,  @Status OUT", 1, out status);

            return status == 1 ? true : false;
        }

        #region Private Method

        //Check if Product Master code is already present or not.
        private bool IsClientNameAlreadyExist(string productName, int applicationLicenseId = 0)
             => _ApplicationLicenseDetailsRepository.Table.Any(x => x.ClientName == productName && (x.ApplicationLicenseId != applicationLicenseId || applicationLicenseId == 0));

        #endregion
    }
}
