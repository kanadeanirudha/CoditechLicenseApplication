using Coditech.BusinessLogicLayer;
using Coditech.Model.Model;
using Coditech.Resources;
using Coditech.Utilities.Constant;
using Coditech.ViewModel;

using System.Web.Mvc;

namespace Coditech.Controllers
{
    [Authorize]
    public class ApplicationLicenseDetailsController : BaseController
    {
        readonly ApplicationLicenseDetailsBA _applicationLicenseDetailBA = null;
        private const string createEdit = "~/Views/ApplicationLicenseDetails/CreateEdit.cshtml";
        public ApplicationLicenseDetailsController()
        {
            _applicationLicenseDetailBA = new ApplicationLicenseDetailsBA();
        }

        public ActionResult List(DataTableModel dataTableModel)
        {
            if (IsLoginSessionExpired())
                return RedirectToAction<UserController>(x => x.Login());

            dataTableModel = dataTableModel ?? new DataTableModel();
            ApplicationLicenseDetailsListViewModel list = _applicationLicenseDetailBA.GetProductList(dataTableModel);
            return View($"~/Views/ApplicationLicenseDetails/List.cshtml", list);
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (IsLoginSessionExpired())
                return RedirectToAction<UserController>(x => x.Login());

            return View(createEdit, new ApplicationLicenseDetailsViewModel());
        }

        [HttpPost]
        public virtual ActionResult Create(ApplicationLicenseDetailsViewModel applicationLicenseDetailViewModel)
        {
            if (IsLoginSessionExpired())
                return RedirectToAction<UserController>(x => x.Login());

            string errorMessage = string.Empty;
            if (ModelState.IsValid)
            {
                applicationLicenseDetailViewModel = _applicationLicenseDetailBA.CreateApplicationLicenseDetails(applicationLicenseDetailViewModel);
                if (!applicationLicenseDetailViewModel.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage(GeneralResources.RecordCreationSuccessMessage));
                    return RedirectToAction<ApplicationLicenseDetailsController>(x => x.List(null));
                }
                errorMessage = applicationLicenseDetailViewModel.ErrorMessage;
            }
            SetNotificationMessage(GetErrorNotificationMessage(errorMessage));
            return View(createEdit, applicationLicenseDetailViewModel);
        }

        [HttpGet]
        public virtual ActionResult Edit(int applicationLicenseId)
        {
            if (IsLoginSessionExpired())
                return RedirectToAction<UserController>(x => x.Login());

            ApplicationLicenseDetailsViewModel applicationLicenseDetailViewModel = _applicationLicenseDetailBA.GetApplicationLicenseDetails(applicationLicenseId);
            return ActionView(createEdit, applicationLicenseDetailViewModel);
        }


        //Post:Edit Product Master.
        [HttpPost]
        public virtual ActionResult Edit(ApplicationLicenseDetailsViewModel applicationLicenseDetailViewModel)
        {
            if (IsLoginSessionExpired())
                return RedirectToAction<UserController>(x => x.Login());

            string errorMessage = string.Empty;
            if (ModelState.IsValid)
            {
                bool status = _applicationLicenseDetailBA.UpdateApplicationLicenseDetails(applicationLicenseDetailViewModel).HasError;
                SetNotificationMessage(status
                ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));

                if (!status)
                    return RedirectToAction<ApplicationLicenseDetailsController>(x => x.List(new DataTableModel() { SortByColumn = SortKeys.ModifiedDate, SortBy = CoditechConstant.DESCKey }));
            }
            return View(createEdit, applicationLicenseDetailViewModel);
        }

        //Delete Product Master.
        public virtual ActionResult Delete(string applicationLicenseId)
        {
            if (IsLoginSessionExpired())
                return RedirectToAction<UserController>(x => x.Login());

            string message = string.Empty;
            bool status = false;
            if (!string.IsNullOrEmpty(applicationLicenseId))
            {
                status = _applicationLicenseDetailBA.DeleteApplicationLicenseDetails(applicationLicenseId, out message);
                SetNotificationMessage(!status
                ? GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.DeleteMessage));
                return RedirectToAction<ApplicationLicenseDetailsController>(x => x.List(null));
            }

            SetNotificationMessage(GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage));
            return RedirectToAction<ApplicationLicenseDetailsController>(x => x.List(null));
        }
    }
}