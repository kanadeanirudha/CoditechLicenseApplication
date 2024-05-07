using Coditech.DataAccessLayer.DataEntity;
using Coditech.DataAccessLayer.Helper;
using Coditech.DataAccessLayer.Repository;
using Coditech.ExceptionManager;
using Coditech.Model;
using Coditech.Resources;
using Coditech.Utilities.Helper;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

using static Coditech.Utilities.Helper.CoditechHelperUtility;
namespace Coditech.DataAccessLayer
{
    public class ApplicationLicenseDetailsDAL
    {
        private readonly ICoditechRepository<ApplicationLicenseDetail> _applicationLicenseDetailsRepository;
        public ApplicationLicenseDetailsDAL()
        {
            _applicationLicenseDetailsRepository = new CoditechRepository<ApplicationLicenseDetail>();
        }

        public ApplicationLicenseDetailsListModel GetProductList(FilterCollection filters, NameValueCollection sorts, int pagingStart, int pagingLength)
        {
            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            ApplicationLicenseDetailsListModel listModel = new ApplicationLicenseDetailsListModel();
            List<ApplicationLicenseDetail> applicationLicenseList = _applicationLicenseDetailsRepository.GetEntityList(pageListModel.SPWhereClause)?.ToList();
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
        public ApplicationLicenseDetailsModel CreateApplicationLicenseDetail(ApplicationLicenseDetailsModel applicationLicenseDetailsModel)
        {
            if (IsNull(applicationLicenseDetailsModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            if (IsClientNameAlreadyExist(applicationLicenseDetailsModel.ClientName))
            {
                throw new CoditechException(ErrorCodes.AlreadyExist, string.Format(GeneralResources.ErrorCodeExists, "ApplicationLicenseDetail name"));
            }

            //Create new ApplicationLicenseDetail and return it.
            ApplicationLicenseDetail applicationLicenseDetail = _applicationLicenseDetailsRepository.Insert(applicationLicenseDetailsModel.FromModelToEntity<ApplicationLicenseDetail>());
            if (applicationLicenseDetail?.ApplicationLicenseId > 0)
            {
                applicationLicenseDetailsModel.ApplicationLicenseId = applicationLicenseDetail.ApplicationLicenseId;
            }
            else
            {
                applicationLicenseDetailsModel.HasError = true;
                applicationLicenseDetailsModel.ErrorMessage = GeneralResources.ErrorFailedToCreate;
            }
            return applicationLicenseDetailsModel;
        }

        //Get ApplicationLicenseDetail by ApplicationLicenseDetail id.
        public ApplicationLicenseDetailsModel GetApplicationLicenseDetail(int applicationLicenseId)
        {
            if (applicationLicenseId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "ApplicationLicenseId"));

            //Get the ApplicationLicenseDetail Details based on id.
            ApplicationLicenseDetail ApplicationLicenseDetailsData = _applicationLicenseDetailsRepository.Table.FirstOrDefault(x => x.ApplicationLicenseId == applicationLicenseId);
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

            ApplicationLicenseDetail applicationLicenseDetailsData = _applicationLicenseDetailsRepository.Table.FirstOrDefault(x => x.ApplicationLicenseId == applicationLicenseDetailsModel.ApplicationLicenseId);

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
            bool isApplicationLicenseDetailUpdated = _applicationLicenseDetailsRepository.Update(applicationLicenseDetailsData);
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
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "ApplicationLicenseId"));

            CoditechViewRepository<View_ReturnBoolean> objStoredProc = new CoditechViewRepository<View_ReturnBoolean>();
            objStoredProc.SetParameter("ApplicationLicenseId", parameterModel.Ids, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("Status", null, ParameterDirection.Output, DbType.Int32);
            int status = 0;
            objStoredProc.ExecuteStoredProcedureList("Coditech_DeleteApplicationLicenseDetails @ApplicationLicenseId,  @Status OUT", 1, out status);

            return status == 1 ? true : false;
        }

        public ActiveApplicationLicenseModel IsApplicationLicenseActive(string domainName, string apiKey)
        {
            ActiveApplicationLicenseModel model = new ActiveApplicationLicenseModel();
            ApplicationLicenseDetail applicationLicenseDetail = _applicationLicenseDetailsRepository.Table.Where(x => x.APIKey == apiKey && x.DomainName == domainName)?.FirstOrDefault();
            DateTime todayDate = DateTime.Now.Date;
            if (IsNull(applicationLicenseDetail))
            {
                model.ErrorMessage = "Invalid license details found.";
            }
            else if (!applicationLicenseDetail.IsActive)
            {
                model.ErrorMessage = "Application license is not Active.";
            }
            else if (applicationLicenseDetail.ValidFromDate > todayDate)
            {
                model.ErrorMessage = $"Application license will active from {applicationLicenseDetail.ValidFromDate.ToShortDateString()}.";
            }
            else if (applicationLicenseDetail.ValidUptoDate < todayDate)
            {
                model.ErrorMessage = "Application license date is expired.";
            }
            else
            {
                model.IsActive = true;
            }
            return model;
        }

        #region Private Method

        //Check if Product Master code is already present or not.
        private bool IsClientNameAlreadyExist(string productName, int applicationLicenseId = 0)
             => _applicationLicenseDetailsRepository.Table.Any(x => x.ClientName == productName && (x.ApplicationLicenseId != applicationLicenseId || applicationLicenseId == 0));

        #endregion
    }
}
