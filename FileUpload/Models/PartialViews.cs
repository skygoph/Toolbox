using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileUpload.Models
{
    public class ViewDataDownloadedFiles
    {
        public string name { get; set; }
        public long size { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string delete_url { get; set; }
    }

    public class ViewDataDownloadedFilesFinal
    {
        public string name { get; set; }
        public string size { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string desc { get; set; }
        public string filename { get; set; }
        public string uploaddate { get; set; }
        public string prevUrl { get; set; }
    }

    public class ViewDataDownloadFinal
    {
        public virtual ICollection<ViewDataDownloadedFiles> ViewDataDownloadFiles { get; set; }
        //public UploadDownloadCategory UploadDownloadCategory { get; set; }
        public int CategoryID { get; set; }
        public int intMstGroupType { get; set; }
        public int intMstDept { get; set; }
    }

    public class UploadDownloadCategory
    {
        public int CategoryID { get; set; }
    }

    public class ViewDataUserList
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string UserGroupName { get; set; }
        public bool isAdmin { get; set; }
    }

    public class ViewSendTo
    {
        public string CategoryName { get; set; }
    }

    public class ViewSendToID
    {
        public string ColValue { get; set; }
        public string ColDisplay { get; set; }
    }

    public class ViewMemo
    {
        public Memo memo { get; set; }
        public int intTrnMemoDetail { get; set; }
        public int intTrnMemo { get; set; }
        public string BranchCode { get; set; }
        public int intGroupNameID { get; set; }
        public int intMstDepartment { get; set; }
        public int intMstPosition { get; set; }
        public string intMstEmpPersonal { get; set; }
        public HttpPostedFileBase file { get; set; }
    }

    public class ViewNoti
    {
        public Notification noti { get; set; }
        public string BranchCode { get; set; }
        public int intMstDepartment { get; set; }
        public int intMstPosition { get; set; }
        public string intMstEmpPersonal { get; set; }
        public HttpPostedFileBase file { get; set; }
    }

    public class ViewPolicy
    {
        public Policy poli { get; set; }
        public int intGroupNameID { get; set; }
        public string BranchCode { get; set; }
        public int intMstDepartment { get; set; }
        public int intMstPosition { get; set; }
        public string intMstEmpPersonal { get; set; }
        public HttpPostedFileBase file { get; set; }
    }

    public class ViewEmpNotice
    {
        public EmployeeNotice notice { get; set; }
        public string BranchCode { get; set; }
        public int intMstDepartment { get; set; }
        public int intMstPosition { get; set; }
        public string intMstEmpPersonal { get; set; }
        public HttpPostedFileBase file { get; set; }
    }

    public class ComTrailReport
    {
        public string FileName { get; set; }
    }

    public class ViewNasi
    {
        public NasiAnnouncement noti { get; set; }
        public string BranchCode { get; set; }
        public int intMstDepartment { get; set; }
        public int intMstPosition { get; set; }
        public string intMstEmpPersonal { get; set; }
        public HttpPostedFileBase file { get; set; }
    }
}
