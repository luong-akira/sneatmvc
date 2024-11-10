using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sneat.MVC.Common
{
    public class SystemParam
    {
        #region Datetime param
        public const string CONVERT_DATETIME = "dd/MM/yyyy";
        public const string CONVERT_DATETIME_DATE_PICKER_BASIC = "yyyy-MM-dd";
        public const string CONVERT_DATETIME_HAVE_HOUR = "yyyy-MM-dd HH:mm";
        public const string CONVERT_DATETIME_HOUR = "HH:mm";
        public const string CONVERT_DATETIME_EXCEL = "dd_MM_yyyy";
        #endregion

        #region Common status param
        public const int ACTIVE = 1;
        public const string STATUS_ACTIVE_STR = "Đang hoạt động";
        public const int IN_ACTIVE = 0;
        public const string STATUS_IN_ACTIVE_STR = "Ngừng hoạt động";
        public const int IS_DELETED = 1;
        public const int IS_NOT_DELETED = 0;
        #endregion

        #region Paging
        public const int PAGE_DEFAULT = 1;
        public const int MAX_ROW_IN_LIST_WEB = 12;
        #endregion

        #region Returned data status
        public const int SUCCESS = 1;
        public const int FAILED = -1;
        public const int ERROR = 0;
        public const int RETURN_TRUE = 1;
        public const int RETURN_FALSE = 0;
        #endregion

        #region Https status 
        public const int SUCCESS_CODE = 200;
        public const string MESSAGE_SUCCESS = "Thành công";
        public const int SERVER_ERROR_CODE = 500;
        public const string SERVER_ERROR = "Hệ thống đang bảo trì";
        public const int TOKEN_INVALID_CODE = 401;
        public const string MESSAGE_TOKEN_INVALID = "Đăng nhập để thực hiện chức năng này";
        public const int PERMISSION_INVALID = 402;
        public const string MESSAGE_PERMISSION_INVALID = "Bạn không có quyền thực hiện chức năng này";
        public const int TOKEN_ERROR = 403;
        public const string MESSAGE_TOKEN_ERROR = "Tài khoản của bạn đã đăng nhập ở nơi khác";
        #endregion

        #region Config System
        public const string SESSION_LOGIN = "LoginSneat";
        // Email
        public const string EMAIL_CONFIG = "viethoang2001gun@gmail.com";
        public const string PASS_CONFIG = "wjvs nvnr qigk tpjo";
        public const string HOST_DEFAUL = "smtp.gmail.com";
        public const string EMAIL_TITLE = "[HỆ THỐNG SNEAT]";
        // Default Setting
        public const string DEFAULT_SYSTEM_IMAGE = "https://res.cloudinary.com/dduv8pom4/image/upload/v1721178529/favicon_epkkts.png";
        #endregion

        #region Authentication
        public const int INVALID_EMAIL_OR_PASSWORD_ERR = -1;
        public const string INVALID_EMAIL_OR_PASSWORD_ERR_STR = "Sai tên đăng nhập hoặc mật khẩu!";
        public const int ACCOUNT_HAD_BEEN_BLOCKED_ERR = -2;
        public const string ACCOUNT_HAD_BEEN_BLOCKED_ERR_STR = "Tài khoản của bạn đã bị khóa!";
        public const int INVALID_EMAIL_ERR = -3;
        public const int INVALID_PASSWORD_ERR = -4;
        public const string INVALID_PASSWORD_ERR_STR = "Mật khẩu không phù hợp!";
        #endregion

        #region User
        public const int EMAIL_USED_ERR = -1;
        public const string EMAIL_USED_ERR_STR = "Email đã tồn tại!";
        public const int PHONE_USED_ERR = -2;
        public const string PHONE_USED_ERR_STR = "Số điện thoại đã tồn tại!";
        public const int ACCOUNT_NOT_FOUND_ERR = -3;
        public const string ACCOUNT_NOT_FOUND_ERR_STR = "Không tìm thấy tài khoản!";
        #endregion

        #region Role - Permission
        public const int EXISTED_ROLE_NAME_ERR = -1;
        public const string EXISTED_ROLE_NAME_ERR_STR = "Tên vai trò đã tồn tại!";
        public const int ROLE_NOT_FOUND_ERR = -2;
        public const string ROLE_NOT_FOUND_ERR_STR = "Vai trò không tồn tại hoặc đã bị xóa!";
        #endregion

        #region Project - Team
        public const int EXISTED_TEAM_NAME_ERR = -1;
        public const string EXISTED_TEAM_NAME_ERR_STR = "Tên đội nhóm đã tồn tại!";
        public const int TEAM_NOT_FOUND_ERR = -2;
        public const string TEAM_NOT_FOUND_ERR_STR = "Đội nhóm không tồn tại hoặc đã bị xóa!";
        public const int EXISTED_PROJECT_NAME_ERR = -3;
        public const string EXISTED_PROJECT_NAME_ERR_STR = "Tên dự án đã tồn tại!";
        public const int PROJECT_NOT_FOUND_ERR = -4;
        public const string PROJECT_NOT_FOUND_ERR_STR = "Dự án không tồn tại!";
        #endregion

        #region Work Package
        // Priority
        public const int PRIORITY_TYPE_HOT_FIX = 1;
        public const string PRIORITY_TYPE_HOT_FIX_STR = "Hot fix";
        public const int PRIORITY_TYPE_IMMEDIATE = 2;
        public const string PRIORITY_TYPE_IMMEDIATE_STR = "Ngay lập tức";
        public const int PRIORITY_TYPE_HIGH = 3;
        public const string PRIORITY_TYPE_HIGH_STR = "Cao";
        public const int PRIORITY_TYPE_NORMAL = 4;
        public const string PRIORITY_TYPE_NORMAL_STR = "Bình thường";
        public const int PRIORITY_TYPE_LOW = 5;
        public const string PRIORITY_TYPE_LOW_STR = "Thấp";

        // Status
        public const string TASK_STATUS_NEW = "Mới";
        public const string TASK_STATUS_INPROGRESS = "Đang thực hiện";
        public const string TASK_STATUS_IN_TESTING = "Đang kiểm thử";
        public const string TASK_STATUS_CLOSED = "Dóng";
        public const string TASK_STATUS_DEVELOPED = "Đã phát triển";
        public const string TASK_STATUS_TESTED = "Đã kiểm thử";
        public const string TASK_STATUS_REJECTED = "Từ chối";
        public const string TASK_STATUS_TEST_FAILED = "Kiểm thử thất bại";

        // Task Type
        public const string TASK_TYPE_USER_STORY = "User Story";
        public const string TASK_TYPE_USER_TASK = "Nhiệm vụ";
        public const string TASK_TYPE_USER_BUG = "Lỗi";
        public const string TASK_TYPE_USER_FEATURE = "Tính năng";

        // Error
        public const int TASK_NOT_FOUND_ERR = -1;
        public const string TASK_NOT_FOUND_ERR_STR = "Nhiệm vụ này không tồn tạ hoặc đã bị xóa!";
        #endregion

        #region Document
        public const string DOCUMENT_NOT_FOUND_ERR_STR = "Tài liệu này không tồn tại!";
        #endregion

        #region Sprint
        public const string SPRINT_NOT_FOUND_ERR_STR = "Sprint này không tồn tại hoặc đã bị xóa!";
        #endregion

        #region Time Log
        public const string TIME_LOG_NOT_FOUND_ERR_STR = "Time Log này không tồn tại hoặc đã bị xóa!";
        #endregion

        #region Viet QR
        public const string VIET_QR_API_ROOT_V2 = "https://api.vietqr.io/v2/";
        public const string VIET_QR_API_LIST_BANK_V2 = "banks";
        public const string VIET_QR_API_GENERATE_QR_V2 = "generate";
        #endregion

        public const string SUCCESS_MESSAGE = "Thành công";
    }
}