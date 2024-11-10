
'use strict';

$(function () {
    var theme = $('html').hasClass('light-style') ? 'default' : 'default-dark',
        checkboxTree = $('#tree-role');

    // List Tree fo creating
    if (checkboxTree.length) {
        $.ajax({
            url: '/Roles/GetAllPermissions', 
            method: 'GET',
            success: function (data) {
                console.log(data)
                var jsTreeData = transformToJsTreeFormat(data);
                initializeJsTree(jsTreeData);
            },
            error: function (error) {
                console.error('Error fetching permissions:', error);
            }
        });
    }

    function transformToJsTreeFormat(data) {
        function transformNode(node) {
            return {
                id: node.Item.ID,       
                text: node.Item.Name,    
                children: node.Children ? node.Children.map(transformNode) : [], 
                state: {
                    opened: true,
                    //selected: preSelectedIds.includes(node.Item.ID)
                    selected: false
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

    function initializeJsTree(data) {
        checkboxTree.jstree({
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
                
               /* img: {
                    icon: 'bx bx-image text-success'
                },*/
                team: {
                    icon: 'bx bx-group text-warning'
                },
                user: {
                    icon: 'bx bx-user text-secondary'
                },
                home: {
                    icon: 'bx bx-home-circle text-info'
                },
                role: {
                    icon: 'bx bx-key text-danger'
                }
            }
        });
    }

    window.treeData = function () {
        // Get selected nodes
        var selectedNodes = checkboxTree.jstree("get_selected", true);
        var selectedItems = selectedNodes.map(function (node) {
            return {
                id: node.id,
                text: node.text
            };
        });

        console.log('Selected Items:', selectedItems);
    }
});
