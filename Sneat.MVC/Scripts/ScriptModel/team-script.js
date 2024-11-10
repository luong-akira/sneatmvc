'use strict';

$(function () {
    // Modal id
    const appModal = document.getElementById('createApp'),
        appUpdateModal = document.getElementById('updateApp');

    appModal.addEventListener('show.bs.modal', function (event) {
        const wizardCreateApp = document.querySelector('#wizard-create-app');
        if (typeof wizardCreateApp !== undefined && wizardCreateApp !== null) {
            // Wizard next prev button
            const wizardCreateAppNextList = [].slice.call(wizardCreateApp.querySelectorAll('.btn-next'));
            const wizardCreateAppPrevList = [].slice.call(wizardCreateApp.querySelectorAll('.btn-prev'));
            const wizardCreateAppBtnSubmit = wizardCreateApp.querySelector('.btn-submit');

            const createAppStepper = new Stepper(wizardCreateApp, {
                linear: false
            });

            if (wizardCreateAppNextList) {
                wizardCreateAppNextList.forEach(wizardCreateAppNext => {
                    wizardCreateAppNext.addEventListener('click', event => {
                        createAppStepper.next();
                        //initCleave();
                    });
                });
            }
            if (wizardCreateAppPrevList) {
                wizardCreateAppPrevList.forEach(wizardCreateAppPrev => {
                    wizardCreateAppPrev.addEventListener('click', event => {
                        createAppStepper.previous();
                        //initCleave();
                    });
                });
            }

            if (wizardCreateAppBtnSubmit) {
                wizardCreateAppBtnSubmit.addEventListener('click', event => {
                    //alert('Submitted..!!');
                });
            }
        }
    });

    appUpdateModal.addEventListener('show.bs.modal', function (event) {
        const wizardCreateApp = document.querySelector('#wizard-update-app');
        if (typeof wizardCreateApp !== undefined && wizardCreateApp !== null) {
            // Wizard next prev button
            const wizardCreateAppNextList = [].slice.call(wizardCreateApp.querySelectorAll('.btn-next'));
            const wizardCreateAppPrevList = [].slice.call(wizardCreateApp.querySelectorAll('.btn-prev'));
            const wizardCreateAppBtnSubmit = wizardCreateApp.querySelector('.btn-submit');

            const createAppStepper = new Stepper(wizardCreateApp, {
                linear: false
            });

            if (wizardCreateAppNextList) {
                wizardCreateAppNextList.forEach(wizardCreateAppNext => {
                    wizardCreateAppNext.addEventListener('click', event => {
                        createAppStepper.next();
                        //initCleave();
                    });
                });
            }
            if (wizardCreateAppPrevList) {
                wizardCreateAppPrevList.forEach(wizardCreateAppPrev => {
                    wizardCreateAppPrev.addEventListener('click', event => {
                        createAppStepper.previous();
                        //initCleave();
                    });
                });
            }

            if (wizardCreateAppBtnSubmit) {
                wizardCreateAppBtnSubmit.addEventListener('click', event => {
                    //alert('Submitted..!!');
                });
            }
        }
    });
});

function searchTeam() {
    if (!navigator.onLine) {
        Swal.fire({
            title: 'Có lỗi xảy ra!',
            text: ' Kiểm tra kết nối internet!',
            icon: 'error',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }
    var key = $("#txt-key-search").val().replace(/\s\s+/g, ' ');

    $.ajax({
        url: '/Teams/Search',
        data: {
            page: 1,
            search: key
        },
        type: 'POST',
        success: function (response) {
            $("#list_team").html(response);
        },
        error: function (result) {
            console.log(result.responseText);
        }
    });
}

function saveCreateTeam() {
    var name = $('#nameCreate').val();
    var desc = $('#txtDescriptionCreate').val();
    var techstack = $('#slField').val();

    var selectedUserItems = [];
    $('input[name="selectedItemsUser"]:checked').each(function () {
        selectedUserItems.push(parseInt($(this).val()));
    });

    if (name == "") {
        Swal.fire({
            title: 'Thông báo!',
            text: ' Vui lòng nhập tên đội nhóm!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }

    if (techstack == "") {
        Swal.fire({
            title: 'Thông báo!',
            text: ' Vui lòng chọn ít nhất một lĩnh vực!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }

    if (techstack.length > 3) {
        Swal.fire({
            title: 'Thông báo!',
            text: "Chỉ được chọn tối đa 3 lĩnh vực!",
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }

    $.ajax({
        url: "/Teams/CreateTeam",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            name: name,
            techStack: techstack,
            Description: desc,
            UserIds: selectedUserItems,
        }),
        beforeSend: function () {
            $("#modalLoad").modal("show");
        },
        success: function (response) {
            $("#createApp").modal("hide");
            $("#modalLoad").modal("hide");
            if (response == -1) {
                Swal.fire({
                    title: 'Không thể tạo đội nhóm',
                    text: 'Tên đội nhóm này đã được sử dụng!',
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
                    text: 'Tạo đội nhóm thành công!',
                    icon: 'success',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
                setTimeout(function () {
                    window.location = "/Teams/Index?";
                    searchTeam();
                }, 2000);
            }

            else {
                Swal.fire({
                    title: 'Có lỗi xảy ra!',
                    text: ' Không thể tạo đội nhóm!',
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

function updateTeamModal(id) {
    $.ajax({
        url: '/Teams/DetailTeam',
        type: 'GET',
        data: { id: id },
        success: function (res) {
            $("#nameUpdate").val(res.Name);
            $("#txtDescriptionEdit").val(res.Description);
            $("#teamID").val(id);

            $("input[name='selectedItemsUser']").prop('checked', false);

            // Fetch pre-selected users
            var preSelectedUsers = res.UserIds;
            preSelectedUsers.forEach(function (userId) {
                $("input[name='selectedItemsUser'][value='" + userId + "']").prop('checked', true);
            });


            //$("#slFieldUpdate").val(res.TechStack);
           /* var optionProducts = data.map(function (item) {
                return {
                    label: item.Name,
                    value: item.ID
                };
            });*/

            var preSelectedTechStackValue = res.TechStack;
            var virtualSelectInstance = document.querySelector('#slFieldUpdate');
            if (virtualSelectInstance) {
                //virtualSelectInstance.setOptions(optionProducts);
                virtualSelectInstance.setValue(preSelectedTechStackValue);
            }

            // Open the modal
            $('#updateApp').modal('show');
        }
    })
}

function saveUpdateTeam() {
    var name = $('#nameUpdate').val();
    var desc = $('#txtDescriptionEdit').val();
    var techstack = $('#slFieldUpdate').val();
    var id = $('#teamID').val();

    var selectedUserItems = [];
    $('input[name="selectedItemsUser"]:checked').each(function () {
        selectedUserItems.push(parseInt($(this).val()));
    });

    if (name == "") {
        Swal.fire({
            title: 'Thông báo!',
            text: ' Vui lòng nhập tên đội nhóm!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }

    if (techstack == "") {
        Swal.fire({
            title: 'Thông báo!',
            text: ' Vui lòng chọn ít nhất một lĩnh vực!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }

    if (techstack.length > 3) {
        Swal.fire({
            title: 'Thông báo!',
            text: "Chỉ được chọn tối đa 3 lĩnh vực!",
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }

    $.ajax({
        url: "/Teams/UpdateTeam",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            id: id,
            name: name,
            techStack: techstack,
            Description: desc,
            UserIds: selectedUserItems,
        }),
        beforeSend: function () {
            $("#modalLoad").modal("show");
        },
        success: function (response) {
            $("#updateApp").modal("hide");
            $("#modalLoad").modal("hide");
            if (response == -1) {
                Swal.fire({
                    title: 'Không thể cập nhật đội nhóm',
                    text: 'Tên đội nhóm này đã được sử dụng!',
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
                    text: 'Cập nhật đội nhóm thành công!',
                    icon: 'success',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
                setTimeout(function () {
                    window.location = "/Teams/Index?";
                    searchTeam();
                }, 2000);
            }

            else {
                Swal.fire({
                    title: 'Có lỗi xảy ra!',
                    text: ' Không thể cập nhật đội nhóm!',
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

function deleteTeam(id) {
    if (!navigator.onLine) {
        Swal.fire({
            title: 'Có lỗi xảy ra!',
            text: ' Kiểm tra kết nối internet!',
            icon: 'error',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }
    Swal.fire({
        title: 'Bạn chắc chắn muốn xóa chứ?',
        text: "Dữ liệu đội nhóm và thành viên thuộc nhóm này sẽ bị xóa!",
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
                url: '/Teams/RemoveTeam',
                data: { id: id },
                type: "POST",
                success: function (result) {
                    if (result == 1) {
                        Swal.fire({
                            title: 'Thành công!',
                            text: 'Xóa đội nhóm thành công!',
                            icon: 'success',
                            customClass: {
                                confirmButton: 'btn btn-primary'
                            },
                            buttonsStyling: false
                        });
                        setTimeout(function () {
                            window.location = "/Teams/Index?";
                            searchTeam();
                        }, 2000);
                    } else if (result == -2) {
                        Swal.fire({
                            title: 'Xóa đội nhóm thất bại!',
                            text: 'Đội nhóm hiện không tồn tại hoặc đã bị xóa!',
                            icon: 'warning',
                            customClass: {
                                confirmButton: 'btn btn-primary'
                            },
                            buttonsStyling: false
                        });
                        setTimeout(
                            function () {
                                window.location = "/Teams/Index?";
                                searchTeam();
                            }, 1000);

                    }
                    else {
                        Swal.fire({
                            title: 'Có lỗi xảy ra!',
                            text: ' Không thể xóa đội nhóm!',
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

function checkAllUser() {
    var checkboxes = document.getElementsByName('selectedItemsUser');
    var checkAllCheckbox = document.getElementById('checkAllUser');

    for (var i = 0; i < checkboxes.length; i++) {
        checkboxes[i].checked = checkAllCheckbox.checked;
    }
}

function searchUserTeam() {
    if (!navigator.onLine) {
        swal({
            title: "Check internet connection!",
            text: "",
            icon: "warning"
        })
        return;
    }
    var key = $("#txt_search_user_team").val().replace(/\s\s+/g, ' ');

    $.ajax({
        url: '/Teams/SearchUser',
        data: {
            page: 1,
            search: key,
            targetId: 'list_user_option_create'
        },
        type: 'POST',
        success: function (response) {
            $("#list_user_option_create").html(response);
        },
        error: function (result) {
            console.log(result.responseText);
        }
    });
}

function searchUserTeamUpdate() {
    if (!navigator.onLine) {
        swal({
            title: "Check internet connection!",
            text: "",
            icon: "warning"
        })
        return;
    }
    var key = $("#txt_search_user_team_update").val().replace(/\s\s+/g, ' ');
    console.log(key)
    $.ajax({
        url: '/Teams/SearchUser',
        data: {
            page: 1,
            search: key,
            targetId: 'list_user_option_update'
        },
        type: 'POST',
        success: function (response) {
            $("#list_user_option_update").html(response);
        },
        error: function (result) {
            console.log(result.responseText);
        }
    });
}