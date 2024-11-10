//đăng nhập
function Login() {

    if (!navigator.onLine) {
        Swal.fire({
            title: 'Thông báo!',
            text: 'Kiểm tra kết nối internet!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }
    var phone = $("#txtUsernameLogin").val();
    var password = $("#txtPasswordLogin").val();

    if (phone == "" || password == "") {
      
        Swal.fire({
            title: 'Thông báo!',
            text: 'Vui lòng điền đầy đủ thông tin!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }
    $.ajax({
        url: '/Home/UserLogin',
        data: { email: phone, password: password },
        type: 'POST',
        success: function (response) {
            if (response == 1) {
                window.location.assign("/Home/Index");
            } else if (response == -1) {
                Swal.fire({
                    title: 'Thông báo!',
                    text: 'Tài khoản hoặc mật khẩu không đúng!',
                    icon: 'warning',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
            }
            else if (response == -2) {
                Swal.fire({
                    title: 'Thông báo!',
                    text: 'Tài khoản của bạn đã bị khóa!',
                    icon: 'warning',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
            } else {
                Swal.fire({
                    title: 'Có lỗi xảy ra!',
                    text: ' Không thể đăng nhập tài khoản!',
                    icon: 'error',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
            }
        },
    
    });
}

function logout() {
    if (!navigator.onLine) {
        Swal.fire({
            title: 'Thông báo!',
            text: 'Kiểm tra kết nối internet!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }
    $.ajax({
        url: '/Home/Logout',
        data: {},
        type: 'POST',
        success: function (response) {
            if (response == 1) {
                window.location = '/Home/Login'
            }
        },
        error: function (result) {
            Swal.fire({
                title: 'Có lỗi xảy ra!',
                text: ' Không thể đăng xuất tài khoản!',
                icon: 'error',
                customClass: {
                    confirmButton: 'btn btn-primary'
                },
                buttonsStyling: false
            });
        }
    });
}

function forgotPasswordModal() {
    $("#forgotPasswordModal").modal('show');
}

function ForgotPassword() {
    var email = $('#email-forgot-password').val();
    if (email == "") {
        Swal.fire({
            title: 'Thông báo!',
            text: 'Vui lòng nhập Email của bạn!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }

    $.ajax({
        url: '/Home/ForgotPassword',
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ email: email }),
        beforeSend: function () {
            $('#forgotPasswordModal').modal('hide');
            $("#modalLoad").modal("show");
        },
        success: function (res) {
            $("#modalLoad").modal("hide");

            if (res == 1) {
                Swal.fire({
                    title: 'Thành công!',
                    text: 'Vui lòng kiểm tra Email của bạn để xác nhận mật khẩu mới!',
                    icon: 'success',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
            }
            else if (res == -3) {
                Swal.fire({
                    title: 'Thông báo!',
                    text: 'Email không hợp lệ!',
                    icon: 'warning',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
            }
            else {
                Swal.fire({
                    title: 'Có lỗi xảy ra!',
                    text: ' Không thể gửi email!',
                    icon: 'error',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
            }
        }
    })
}

function LoginAdmin() {

    if (!navigator.onLine) {
        swal({
            title: "Kiểm tra kết nối internet!",
            text: "",
            icon: "warning"
        })
        return;
    }
    var phone = $("#txtUsernameLogin").val();
    var password = $("#txtPasswordLogin").val();
    if (phone == "" || password == "") {
        swal({
            title: "Vui lòng điền đầy đủ thông tin",
            text: "",
            icon: "warning"
        })
        return;
    }
    $.ajax({
        url: '/Home/UserLogin',
        data: { phone: phone, password: password },
        type: 'POST',
        success: function (response) {
            if (response.Status == SUCCESS) {
                const tabMapping = {
                    "tabHome": "/Home/Index",
                    "tabWasher": "/Customer/Index",
                    "tabShipper": "/Shipper/Index",
                    "tabProduct": "/Shop/Index",
                    "tabServiceCategory": "/ServiceCategory/Index",
                    "tabServiceProduct": "/ServicePrice/Index",
                    "tabDelivery": "/TransactionDelivery/Index",
                    "tabFood": "/TransactionFood/Index",
                    "tabPromocode": "/Promocode/Index",
                    "tabWalletShipper": "/WalletShipper/Index",
                    "tabWalletShop": "/WalletShop/Index",
                    "tabRegisterShipper": "/RegisterFormShipper/Index",
                    "tabRegisterShop": "/RegisterFormShop/Index",
                    "tabNotify": "/Notification/Index",
                    "tabStatisticCustomer": "/StatisticCustomer/Index",
                    "tabStatisticWasher": "/StatisticShiper/Index",
                    "tabStatisticShop": "/StatisticShop/Index",
                    "tabStatisticPayment": "/StatisticPayment/Index",
                    "tabRequestWithdrawShipper": "/WithdrawalRequest/Index",
                    "tabNews": "/News/Index",
                    "tabConfig": "/Config/Index",
                    "tabUser": "/User/Index"
                };
                for (const tab in tabMapping) {
                    if (response.Data.includes(tab)) {
                        window.location.assign(tabMapping[tab]);
                        return;
                    }
                }
                window.location.assign("/Home/Login");

            } else {
                swal({
                    title: response.Message,
                    text: "",
                    icon: "error"
                })
            }
        },
        error: function (result) {
            console.log(result.responseText);
        }
    });
}