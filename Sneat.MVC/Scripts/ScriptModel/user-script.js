function searchUser() {
    if (!navigator.onLine) {
        swal({
            title: "Check internet connection!",
            text: "",
            icon: "warning"
        })
        return;
    }
    var key = $("#txt-key-search").val().replace(/\s\s+/g, ' ');
    var teamID = $('#slTeam').val();

    $.ajax({
        url: '/Users/Search',
        data: {
            page: 1,
            search: key,
            teamID: teamID,
        },
        type: 'POST',
        success: function (response) {
            $("#list_user").html(response);
        },
        error: function (result) {
            console.log(result.responseText);
        }
    });
}

function saveCreateUser() {
    var name = $('#nameCreate').val();
    var email = $('#emailCreate').val();
    var phone = $('#input-add-phone').val();
    var password = $('#txt-pass').val();
    var passwordConfirm = $('#txt-pass-confirm').val();
    var avatar = $('#currentImage').val();
    var roles = $('#slRole').val();

    var firstName = $('#txtFirstName').val();
    var lastName = $('#txtLastName').val();
    var DOB = $('#flatpickr-date').val();
    var gender = $('#slGender').val();

    var identity = $('#txtIdentity').val();
    var identityReceivedDate = $('#flatpickr-date-identity').val();
    var identityReceivedPlace = $('#txtReceivePlace').val();
    var identityImages = $('#currentMultilImage').val();

    var bankBin = $('#slBank').val();
    var bankAccountName = $('#valAccountName').val();
    var bankAccountNo = $('#valAccountNo').val();
    var bankQRImage = $("#imgVietQR").attr('src');

    var districtHomeID = $('#slProvinceHome').val();
    var homeAddress = $('#txtHomeAddress').val();
    var districtOfficeID = $('#slProvinceOffice').val();
    var officeAddress = $('#txtOfficeAddress').val();

    if (name == "" ) {
      /*  swal({
            title: "Thông báo",
            text: "Vui lòng nhập tên người dùng!",
            icon: "warning"
        })*/
        Swal.fire({
            title: 'Thông báo!',
            text: ' Vui lòng nhập tên người dùng!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }

    if (phone == "") {
        Swal.fire({
            title: 'Thông báo!',
            text: ' Vui lòng nhập số điện thoại người dùng!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }

    if (email == "") {
        Swal.fire({
            title: 'Thông báo!',
            text: ' Vui lòng nhập Email người dùng!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }

    if (roles == "") {
      
        Swal.fire({
            title: 'Thông báo!',
            text: ' Vui lòng chọn ít nhất một vai trò!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }

    if (password == "") {
        swal({
            title: "Thông báo",
            text: "Vui lòng nhập mật khẩu người dùng!",
            icon: "warning"
        })
        return;
    }

    var phone_validate = new RegExp("^[0-9]{9,11}");
    if (!phone_validate.test(phone)) {
        Swal.fire({
            title: 'Thông báo!',
            text: 'Số điện thoại không đúng định dạng!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }

    var email_validate = /^\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b$/i;
    if (!email_validate.test(email)) {
        Swal.fire({
            title: 'Thông báo!',
            text: 'Email không đúng định dạng!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }

    if (password != passwordConfirm) {
        Swal.fire({
            title: 'Thông báo!',
            text: 'Xác nhận mật khẩu không đúng!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }

    $.ajax({
        url: "/Users/CreateUser",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            name: name,
            email: email,
            phone: phone,
            password: password,
            avatar: avatar,
            roleIds: roles,

            firstName: firstName,
            lastName: lastName,
            DOB: DOB,
            gender: gender,

            identity: identity,
            identityReceivedDate: identityReceivedDate,
            identityReceivedPlace: identityReceivedPlace,
            identityImages: identityImages,

            bankBin: bankBin,
            bankAccountName: bankAccountName,
            bankAccountNo: bankAccountNo,
            bankQRImage: bankQRImage,

            districtHomeID: districtHomeID,
            homeAddress: homeAddress,
            districtOfficeID: districtOfficeID,
            officeAddress: officeAddress
        }),
        beforeSend: function () {
            $("#modalLoad").modal("show");
        },
        success: function (response) {
            $("#modalLoad").modal("hide");
            if (response == -1) {
                Swal.fire({
                    title: 'Không thể tạo tài khoản',
                    text: 'Email này đã được sử dụng!',
                    icon: 'warning',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
            }
            else if (response == -2) {
                Swal.fire({
                    title: 'Không thể tạo tài khoản',
                    text: 'Số điện thoại đã được sử dụng!',
                    icon: 'warning',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
            }
            else if (response == 1) {
                Swal.fire({
                    title: 'Thành công!',
                    text: 'Tạo tài khoản thành công!',
                    icon: 'success',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
                setTimeout(function () {
                    window.location = "/Users/Index?";
                    searchUser();
                }, 2000);
            }
           
            else {
                Swal.fire({
                    title: 'Có lỗi xảy ra!',
                    text: ' Không thể tạo tài khoản!',
                    icon: 'error',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
            }
        }
    });
}

function saveUpdateUser(ID) {
    var name = $('#nameCreate').val();
    var email = $('#emailCreate').val();
    var phone = $('#input-add-phone').val();
    var avatar = $('#currentImage').val();
    var roles = $('#slRole').val();

    var firstName = $('#txtFirstName').val();
    var lastName = $('#txtLastName').val();
    var DOB = $('#flatpickr-date').val();
    var gender = $('#slGender').val();

    var identity = $('#txtIdentity').val();
    var identityReceivedDate = $('#flatpickr-date-identity').val();
    var identityReceivedPlace = $('#txtReceivePlace').val();
    var identityImages = $('#currentMultilImage').val();

    var bankBin = $('#slBank').val();
    var bankAccountName = $('#valAccountName').val();
    var bankAccountNo = $('#valAccountNo').val();
    var bankQRImage = $("#imgVietQR").attr('src');

    var districtHomeID = $('#slProvinceHome').val();
    var homeAddress = $('#txtHomeAddress').val();
    var districtOfficeID = $('#slProvinceOffice').val();
    var officeAddress = $('#txtOfficeAddress').val();

    if (name == "") {
        Swal.fire({
            title: 'Thông báo!',
            text: ' Vui lòng nhập tên người dùng!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }

    if (phone == "") {
        Swal.fire({
            title: 'Thông báo!',
            text: ' Vui lòng nhập số điện thoại người dùng!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }

    if (email == "") {
        Swal.fire({
            title: 'Thông báo!',
            text: ' Vui lòng nhập Email người dùng!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }

    if (roles == "") {

        Swal.fire({
            title: 'Thông báo!',
            text: ' Vui lòng chọn ít nhất một vai trò!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }

    var phone_validate = new RegExp("^[0-9]{9,11}");
    if (!phone_validate.test(phone)) {
        Swal.fire({
            title: 'Thông báo!',
            text: 'Số điện thoại không đúng định dạng!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }

    var email_validate = /^\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b$/i;
    if (!email_validate.test(email)) {
        Swal.fire({
            title: 'Thông báo!',
            text: 'Email không đúng định dạng!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }

    $.ajax({
        url: "/Users/UpdateUser",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            id: ID,
            name: name,
            phone: phone,
            email: email,
            avatar: avatar,
            roleIds: roles,

            firstName: firstName,
            lastName: lastName,
            DOB: DOB,
            gender: gender,

            identity: identity,
            identityReceivedDate: identityReceivedDate,
            identityReceivedPlace: identityReceivedPlace,
            identityImages: identityImages,

            bankBin: bankBin,
            bankAccountName: bankAccountName,
            bankAccountNo: bankAccountNo,
            bankQRImage: bankQRImage,

            districtHomeID: districtHomeID,
            homeAddress: homeAddress,
            districtOfficeID: districtOfficeID,
            officeAddress: officeAddress
        }),
        beforeSend: function () {
            $("#modalLoad").modal("show");
        },
        success: function (response) {
            $("#modalLoad").modal("hide");
            if (response == -1) {
                Swal.fire({
                    title: 'Không thể cập nhật tài khoản',
                    text: 'Email này đã được sử dụng!',
                    icon: 'warning',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
            }
            else if (response == -3) {
                Swal.fire({
                    title: 'Cập nhật tài khoản thất bại!',
                    text: 'Tài khoản hiện không tồn tại hoặc đã bị xóa!',
                    icon: 'warning',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
            }
            else if (response == -2) {
                Swal.fire({
                    title: 'Không thể cập nhật tài khoản',
                    text: 'Số điện thoại đã được sử dụng!',
                    icon: 'warning',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
            }
            else if (response == 1) {
                Swal.fire({
                    title: 'Thành công!',
                    text: 'Cập nhật tài khoản thành công!',
                    icon: 'success',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
                setTimeout(function () {
                    window.location = "/Users/Index?";
                    searchUser();
                }, 2000);
            }

            else {
                Swal.fire({
                    title: 'Có lỗi xảy ra!',
                    text: ' Không thể cập nhật tài khoản!',
                    icon: 'error',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
            }
        }
    });
}

function saveUpdateProfile(ID) {
    var name = $('#nameCreate').val();
    var email = $('#emailCreate').val();
    var phone = $('#input-add-phone').val();
    var avatar = $('#currentImage').val();

    if (name == "") {
        Swal.fire({
            title: 'Thông báo!',
            text: ' Vui lòng nhập tên người dùng!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }

    if (phone == "") {
        Swal.fire({
            title: 'Thông báo!',
            text: ' Vui lòng nhập số điện thoại người dùng!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }

    if (email == "") {
        Swal.fire({
            title: 'Thông báo!',
            text: ' Vui lòng nhập Email người dùng!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }

    var phone_validate = new RegExp("^[0-9]{9,11}");
    if (!phone_validate.test(phone)) {
        Swal.fire({
            title: 'Thông báo!',
            text: 'Số điện thoại không đúng định dạng!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }

    var email_validate = /^\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b$/i;
    if (!email_validate.test(email)) {
        Swal.fire({
            title: 'Thông báo!',
            text: 'Email không đúng định dạng!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }

    $.ajax({
        url: "/Users/UpdateUser",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            id: ID,
            name: name,
            phone: phone,
            email: email,
            avatar: avatar,
        }),
        beforeSend: function () {
            $("#modalLoad").modal("show");
        },
        success: function (response) {
            $("#modalLoad").modal("hide");
            if (response == -1) {
                Swal.fire({
                    title: 'Không thể cập nhật tài khoản',
                    text: 'Email này đã được sử dụng!',
                    icon: 'warning',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
            }
            else if (response == -3) {
                Swal.fire({
                    title: 'Cập nhật tài khoản thất bại!',
                    text: 'Tài khoản hiện không tồn tại hoặc đã bị xóa!',
                    icon: 'warning',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
            }
            else if (response == -2) {
                Swal.fire({
                    title: 'Không thể cập nhật tài khoản',
                    text: 'Số điện thoại đã được sử dụng!',
                    icon: 'warning',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
            }
            else if (response == 1) {
                $("#editProfile").modal("hide");
                Swal.fire({
                    title: 'Thành công!',
                    text: 'Cập nhật tài khoản thành công!',
                    icon: 'success',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
                setTimeout(function () {
                    window.location = "/Users/UserProfile?ID=" + ID;
                }, 2000);
            }

            else {
                Swal.fire({
                    title: 'Có lỗi xảy ra!',
                    text: ' Không thể cập nhật tài khoản!',
                    icon: 'error',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
            }
        }
    });
}

function deleteUser(id) {
    if (!navigator.onLine) {
        swal({
            title: "Kiểm tra kết nối internet!",
            text: "",
            icon: "warning"
        })
        return;
    }
    Swal.fire({
        title: 'Bạn chắc chắn muốn xóa chứ?',
        text: "Dữ liệu tài khoản này sẽ bị xóa!",
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'Vâng, tôi xác nhận!',
        customClass: {
            confirmButton: 'btn btn-primary me-3',
            cancelButton: 'btn btn-label-secondary'
        },
        buttonsStyling: false
    }).then(function (resultConfirm) {
        console.log(resultConfirm)
        if (resultConfirm.isConfirmed) {
            $.ajax({
                url: '/Users/DeleteUser',
                data: { id: id },
                type: "POST",
                success: function (result) {
                    if (result == 1) {
                        Swal.fire({
                            title: 'Thành công!',
                            text: 'Xóa tài khoản thành công!',
                            icon: 'success',
                            customClass: {
                                confirmButton: 'btn btn-primary'
                            },
                            buttonsStyling: false
                        });
                        setTimeout(function () {
                            window.location = "/Users/Index?";
                            searchUser();
                        }, 2000);
                    } else if (result == -3) {
                        Swal.fire({
                            title: 'Xóa tài khoản thất bại!',
                            text: 'Tài khoản hiện không tồn tại hoặc đã bị xóa!',
                            icon: 'warning',
                            customClass: {
                                confirmButton: 'btn btn-primary'
                            },
                            buttonsStyling: false
                        });
                        setTimeout(
                            function () {
                                window.location = "/Users/Index?";
                                searchUser();
                            }, 1000);

                    }
                    else {
                        Swal.fire({
                            title: 'Có lỗi xảy ra!',
                            text: ' Không thể xóa tài khoản!',
                            icon: 'error',
                            customClass: {
                                confirmButton: 'btn btn-primary'
                            },
                            buttonsStyling: false
                        });
                    }
                }
            });
        }
    });
   
}

function deactivateAccount(id) {
    const checkbox = document.getElementById('accountActivation');
    const checkboxLabel = document.querySelector('label[for="accountActivation"]');
    const warningMessage = document.getElementById('warningMessage');

    if (!checkbox.checked) {
        checkbox.style.outline = '2px solid red';
        checkboxLabel.style.color = 'red';
        warningMessage.style.display = 'block';
    } else {
        Swal.fire({
            title: 'Ngừng hoạt động tài khoản',
            text: "Bạn chắc chắn muốn ngừng hoạt động tài khoản này chứ?",
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: 'Vâng, tôi xác nhận!',
            customClass: {
                confirmButton: 'btn btn-primary me-3',
                cancelButton: 'btn btn-label-secondary'
            },
            buttonsStyling: false
        }).then(function (resultConfirm) {
            if (resultConfirm.isConfirmed) {
                $.ajax({
                    url: '/Users/DeactiveUser',
                    data: { id: id },
                    type: "POST",
                    success: function (result) {
                        if (result == 1) {
                            Swal.fire({
                                title: 'Thành công!',
                                text: 'Ngừng hoạt động tài khoản thành công!',
                                icon: 'success',
                                customClass: {
                                    confirmButton: 'btn btn-primary'
                                },
                                buttonsStyling: false
                            });
                            setTimeout(function () {
                                window.location = "/Users/Index?";
                                searchUser();
                            }, 2000);
                        } else if (result == -3) {
                            Swal.fire({
                                title: 'Ngừng hoạt động tài khoản thất bại!',
                                text: 'Tài khoản hiện không tồn tại hoặc đã bị xóa!',
                                icon: 'warning',
                                customClass: {
                                    confirmButton: 'btn btn-primary'
                                },
                                buttonsStyling: false
                            });
                            setTimeout(
                                function () {
                                    window.location = "/Users/Index?";
                                    searchUser();
                                }, 1000);

                        }
                        else {
                            Swal.fire({
                                title: 'Có lỗi xảy ra!',
                                text: ' Không thể ngừng hoạt động tài khoản!',
                                icon: 'error',
                                customClass: {
                                    confirmButton: 'btn btn-primary'
                                },
                                buttonsStyling: false
                            });
                        }
                    }
                });
            }
        });

        checkbox.style.outline = '';
        checkboxLabel.style.color = '';
        warningMessage.style.display = 'none';
    }
}

function activateAccount(id) {
   
    Swal.fire({
        title: 'Kích hoạt tài khoản',
        text: "Bạn chắc chắn muốn kích hoạt tài khoản này chứ?",
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'Vâng, tôi xác nhận!',
        customClass: {
            confirmButton: 'btn btn-primary me-3',
            cancelButton: 'btn btn-label-secondary'
        },
        buttonsStyling: false
    }).then(function (resultConfirm) {
        if (resultConfirm.isConfirmed) {
            $.ajax({
                url: '/Users/ActivateUser',
                data: { id: id },
                type: "POST",
                success: function (result) {
                    if (result == 1) {
                        Swal.fire({
                            title: 'Thành công!',
                            text: 'Kích hoạt tài khoản thành công!',
                            icon: 'success',
                            customClass: {
                                confirmButton: 'btn btn-primary'
                            },
                            buttonsStyling: false
                        });
                        setTimeout(function () {
                            window.location = "/Users/Index?";
                            searchUser();
                        }, 2000);
                    } else if (result == -3) {
                        Swal.fire({
                            title: 'Kích hoạt tài khoản thất bại!',
                            text: 'Tài khoản hiện không tồn tại hoặc đã bị xóa!',
                            icon: 'warning',
                            customClass: {
                                confirmButton: 'btn btn-primary'
                            },
                            buttonsStyling: false
                        });
                        setTimeout(
                            function () {
                                window.location = "/Users/Index?";
                                searchUser();
                            }, 1000);

                    }
                    else {
                        Swal.fire({
                            title: 'Có lỗi xảy ra!',
                            text: ' Không thể kích hoạt tài khoản!',
                            icon: 'error',
                            customClass: {
                                confirmButton: 'btn btn-primary'
                            },
                            buttonsStyling: false
                        });
                    }
                }
            });
        }
    });

}

function changePassword() {

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
    var currentPassword = $.trim($("#txtCurrentPassword").val());
    var newPassword = $.trim($("#txtNewPassword").val());
    var confirmPassword = $.trim($("#txtConfirmPassword").val());

    if (currentPassword == "" || newPassword == "" || confirmPassword == "") {
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
    if (newPassword != confirmPassword) {
        $("#txtConfirmPassword").val("");
        Swal.fire({
            title: 'Thông báo!',
            text: 'Xác nhận mật khẩu không đúng!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }
    $.ajax({
        url: '/Users/ChangePassword',
        data: {
            currentPass: currentPassword,
            newPass: newPassword
        },
        beforeSend: function () {
            $("#modalLoad").modal('show');
        },
        type: 'POST',
        success: function (response) {
            $("#modalLoad").modal('hide');
            $("#txtConfirmPassword").val("");
            $("#txtCurrentPassword").val("");
            $("#txtNewPassword").val("");
            if (response == 1) {
                Swal.fire({
                    title: 'Thành công!',
                    text: 'Đổi mật khẩu tài khoản thành công!',
                    icon: 'success',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
                setTimeout(function () {
                    return;
                }, 2000);
            }
            else if (response == -4) {
                Swal.fire({
                    title: 'Có lỗi xảy ra!',
                    text: ' Mật khẩu cũ không đúng!',
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
                    text: ' Không thể đổi mật khẩu tài khoản!',
                    icon: 'error',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
            }
        },
        error: function (response) {
            Swal.fire({
                title: 'Có lỗi xảy ra!',
                text: ' Không thể đổi mật khẩu tài khoản!',
                icon: 'error',
                customClass: {
                    confirmButton: 'btn btn-primary'
                },
                buttonsStyling: false
            });
        }
    });
}

function checkBankAccount() {
    var BankBin = $('#slBank').val();
    var AccountNo = $('#valAccountNo').val();

    if (BankBin == "") {
        Swal.fire({
            title: 'Thông báo!',
            text: 'Vui lòng chọn ngân hàng!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }
    if (AccountNo == "") {
        Swal.fire({
            title: 'Thông báo!',
            text: 'Vui lòng nhập số tài khoản!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }

    var data = JSON.stringify({
        "bin": BankBin,
        "accountNumber": AccountNo
    });

    var requestOptions = {
        method: 'POST',
        headers: {
            /*
             ShopShip acc:
                 Client-ID: 1b98c74b-49aa-4e5a-b022-471c91f4e1de
                 API-Key: 42be61eb-d839-4522-9213-6381268ae8e6
             Backup-1 acc:
                 Client-ID: e4057385-f918-4882-a4ca-da9a135bdde6
                 API-Key: 2889cc0b-89d3-4f61-8fe0-75365698e2c7
             Backup-2 acc:
                 Client-ID: 0bf74c69-dceb-47a0-97a6-7711b9c78e5e
                 API-Key: e125e141-041c-4011-931b-a9b431c6418d
             */
            'x-client-id': 'e4057385-f918-4882-a4ca-da9a135bdde6',
            'x-api-key': '2889cc0b-89d3-4f61-8fe0-75365698e2c7',
            'Content-Type': 'application/json'
        },
        body: data
    };

    $("#modalLoad").modal('show');

    fetch('https://api.vietqr.io/v2/lookup', requestOptions)
        .then(response => response.json())
        .then(data => {
            $("#modalLoad").modal('hide');
            if (data.code === "42") {
                //{"code":"42","desc":"Account number Invalid - Số tài khoản không hợp lệ","data":null}
                //Invalid
                Swal.fire({
                    title: 'Thông báo!',
                    text: 'Account number Invalid - Số tài khoản không hợp lệ!',
                    icon: 'warning',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
                $('#imgVietQR').attr('src', "");
                console.log(JSON.stringify(data));
            } else if (data.code === "00") {
                //{"code":"00","desc":"Success - Thành công","data":{"accountName":"NGUYEN VIET HOANG"}}
                // Success
                Swal.fire({
                    title: 'Thành công!',
                    text: `Tên tài khoản: '${data.data.accountName}'`,
                    icon: 'success',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
                $('#valAccountName').val(data.data.accountName);
                // Update image source
                const imgSrc = `https://img.vietqr.io/image/${BankBin}-${AccountNo}-L4OvGuX.jpg?accountName=${encodeURIComponent(data.data.accountName)}`;
                $('#imgVietQR').attr('src', imgSrc);
                console.log($("#imgVietQR").attr('src'))
            }
            else if (data.code === "44") {
                //{"code":"44","desc":"Free API Key. Limit exceed. Please contact us(https://casso.vn) to get production Api Key."}
                //Expire request search
                Swal.fire({
                    title: 'Thông báo - Limit exceed!',
                    text: "Bạn đã đến giới hạn 25 lượt kiểm tra trong một tuần (số lượt kiểm tra sẽ được reset sau vài ngày hoặc bạn có thể nâng cấp tài khoản VietQR)",
                    icon: "error",
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
            }
            else {
                console.log(JSON.stringify(data));
                // Handle the success case here
            }
        })
        .catch(error => {
            console.error('Error:', error);
            Swal.fire({
                title: 'Có lỗi xảy ra!',
                text: 'Error:', error,
                icon: 'error',
                customClass: {
                    confirmButton: 'btn btn-primary'
                },
                buttonsStyling: false
            });
        });
}