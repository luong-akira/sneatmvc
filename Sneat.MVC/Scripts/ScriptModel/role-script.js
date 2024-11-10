function searchRole() {
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
        url: '/Roles/SearchRole',
        data: {
            page: 1,
            search: key
        },
        type: 'POST',
        success: function (response) {
            $("#list_role").html(response);
        },
        error: function (result) {
            console.log(result.responseText);
        }
    });
}

function saveCreateRole() {
    var name = $('#nameCreate').val();
    var description = $('#txtDescription').val();

    var checkboxTree = $('#tree-role');
    var selectedNodes = checkboxTree.jstree("get_selected", true);
    var selectedNodeIds = selectedNodes.map(function (node) {
        return node.id;
    });

    if (name == "") {
        Swal.fire({
            title: 'Thông báo!',
            text: ' Vui lòng nhập tên vai trò!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }

    $.ajax({
        url: "/Roles/CreateRole",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            name: name,
            description: description,
            permissionIDs: selectedNodeIds
        }),
        beforeSend: function () {
            $("#modalLoad").modal("show");
        },
        success: function (response) {
            $("#modalLoad").modal("hide");
            if (response == -1) {
                Swal.fire({
                    title: 'Không thể tạo vai trò',
                    text: `Đã tồn tại vai trò có tên ${name}!`,
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
                    text: 'Tạo vai trò thành công!',
                    icon: 'success',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
                setTimeout(function () {
                    window.location = "/Roles/Index?";
                    //searchUser();
                }, 2000);
            }

            else {
                Swal.fire({
                    title: 'Có lỗi xảy ra!',
                    text: ' Không thể tạo vai trò!',
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

function saveUpdateRole() {
    var id = $("#roleID").val();
    var name = $('#nameEdit').val();
    var description = $('#txtDescriptionEdit').val();

    var checkboxTree = $('#update-tree-role');
    var selectedNodes = checkboxTree.jstree("get_selected", true);
    var selectedNodeIds = selectedNodes.map(function (node) {
        return node.id;
    });

    if (name == "") {
        Swal.fire({
            title: 'Thông báo!',
            text: ' Vui lòng nhập tên vai trò!',
            icon: 'warning',
            customClass: {
                confirmButton: 'btn btn-primary'
            },
            buttonsStyling: false
        });
        return;
    }

    $.ajax({
        url: "/Roles/UpdateRole",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            ID: id,
            name: name,
            description: description,
            permissionIDs: selectedNodeIds
        }),
        beforeSend: function () {
            $("#modalLoad").modal("show");
        },
        success: function (response) {
            $("#modalLoad").modal("hide");
            if (response == -1) {
                Swal.fire({
                    title: 'Không thể cập nhật vai trò',
                    text: `Đã tồn tại vai trò có tên ${name}!`,
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
                    text: 'Cập nhật vai trò thành công!',
                    icon: 'success',
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
                setTimeout(function () {
                    window.location = "/Roles/Index?";
                    //searchUser();
                }, 2000);
            }

            else {
                Swal.fire({
                    title: 'Có lỗi xảy ra!',
                    text: ' Không thể cập nhật vai trò!',
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

function deleteRole(id) {
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
        text: "Dữ liệu vai trò này sẽ bị xóa!",
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
                url: '/Roles/RemoveRole',
                data: { id: id },
                type: "POST",
                success: function (result) {
                    if (result == 1) {
                        Swal.fire({
                            title: 'Thành công!',
                            text: 'Xóa vai trò thành công!',
                            icon: 'success',
                            customClass: {
                                confirmButton: 'btn btn-primary'
                            },
                            buttonsStyling: false
                        });
                        setTimeout(function () {
                            window.location = "/Roles/Index?";
                            //searchUser();
                        }, 2000);
                    } else if (result == -2) {
                        Swal.fire({
                            title: 'Xóa vai trò thất bại!',
                            text: 'Vai trò hiện không tồn tại hoặc đã bị xóa!',
                            icon: 'warning',
                            customClass: {
                                confirmButton: 'btn btn-primary'
                            },
                            buttonsStyling: false
                        });
                        setTimeout(
                            function () {
                                window.location = "/Roles/Index?";
                                //searchUser();
                            }, 1000);

                    }
                    else {
                        Swal.fire({
                            title: 'Có lỗi xảy ra!',
                            text: ' Không thể xóa vai trò!',
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

function updateRoleModal(id) {
    $.ajax({
        url: '/Roles/DetailRole',
        type: 'GET',
        data: { id: id },
        success: function (res) {
            $("#listPermissonIds").val("");
            $("#nameEdit").val(res.Name);
            $("#txtDescriptionEdit").val(res.Description);
            $("#listPermissonIds").val(res.PermissionIDs);
            $("#roleID").val(id);

            var theme = $('html').hasClass('light-style') ? 'default' : 'default-dark',
                checkboxTreeUpdate = $('#update-tree-role');

            // List tree for updating
            if (checkboxTreeUpdate.length) {
                $.ajax({
                    url: '/Roles/GetAllPermissions',
                    method: 'GET',
                    success: function (data) {
                        var jsTreeData = transformToJsTreeFormatUpdate(data);
                        initializeJsTreeUpdate(jsTreeData);
                    },
                    error: function (error) {
                        console.error('Error fetching permissions:', error);
                    }
                });
            }

            function transformToJsTreeFormatUpdate(data) {

                var listIds = $("#listPermissonIds").val();// 1,2,3,4,5
                var preSelectedIds = listIds.split(',').map(Number);// [1, 2, 3, 4, 5]
                console.log(listIds)
                function transformNode(node) {
                    return {
                        id: node.Item.ID,
                        text: node.Item.Name,
                        children: node.Children ? node.Children.map(transformNode) : [],
                        state: {
                            opened: true,
                            selected: preSelectedIds.includes(node.Item.ID)
                        },
                        type: node.Item.TabIcon,
                    };
                }

                return {
                    id: data.Id,
                    text: data.Name,
                    children: data.Childrens ? data.Childrens.map(transformNode) : [],
                    state: {
                        opened: true
                    }
                };
            }

            function initializeJsTreeUpdate(data) {
                checkboxTreeUpdate.jstree("destroy").empty();
                checkboxTreeUpdate.jstree({
                    core: {
                        themes: {
                            name: theme
                        },
                        data: [data]
                    },
                    plugins: ['types', 'checkbox', 'wholerow'],
                    types: {
                        default: {
                            icon: 'bx bxl-stripe text-primary'
                        },
                        user: {
                            icon: 'bx bx-user text-secondary'
                        },
                        home: {
                            icon: 'bx bx-home-circle text-info'
                        },
                        role: {
                            icon: 'bx bx-key text-danger'
                        },
                        team: {
                            icon: 'bx bx-group text-warning'
                        },
                    }
                });

            }
        }
    })
}