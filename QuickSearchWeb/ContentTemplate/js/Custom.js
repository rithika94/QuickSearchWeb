//document.querySelector('.sweet-confirm').onclick = function () {
//    swal({
//        title: "Are you sure to delete ?",
//        text: "You will not be able to recover this imaginary file !!",
//        type: "warning",
//        showCancelButton: true,
//        confirmButtonColor: "#DD6B55",
//        confirmButtonText: "Yes, delete it !!",
//        closeOnConfirm: false
//    },
//        function () {
//            //delete row
//            swal("Deleted !!", "Hey, your imaginary file has been deleted !!", "success");
//        });
//};



//$(document).ready(function () {
//    debugger;
   
//    PlatformChanged();   
   
//});


    //window.onload = function () {
    //        debugger;
    //        PlatformChanged();
    //    };
//////////////////Inventory screen dropdowns////////////////////////


   
    //function FillProductType() {
       
    //    var category = $("#Category").val();
    //    CategoryChanged();        
    //    $.ajax
    //        ({
    //        url: '/Inventory/GetProductTypeList',
    //            type: 'POST',
    //            datatype: 'application/json',
    //            contentType: 'application/json',
    //            data: JSON.stringify({
    //        categoryId: +category
    //            }),
    //            success: function (result) {
    //        $("#ProductType").html("");
    //    $.each($.parseJSON(result), function (i, producttype) {
    //        $("#ProductType").append
    //            ($('<option></option>').val(producttype.Id).html(producttype.Name))
    //    })
    //            },
    //            error: function () {
    //        alert("Whooaaa! Something went wrong..")
    //    },
    //        });
    //}

    //function hideSoftwareFields() {
    //    debugger;
    //    $('#Platform').hide();
    //    $('#Url').hide();
    //    $('#Username').hide();
    //    $('#Password').hide();
    //    $('#ActivationKey').hide();
    //}
    //function hideCloudsFields() {
    //    debugger;
    //    $('#Platform').show();
    //    $('#Url').hide();
    //    $('#Username').hide();
    //    $('#Password').hide();
    //    $('#ActivationKey').show();
    //}
    //function hideActivationFields() {
    //    debugger;
    //    $('#Platform').show();
    //    $('#Url').show();
    //    $('#Username').show();
    //    $('#Password').show();
    //    $('#ActivationKey').hide();
    //}
    //function hideActivationCloudFields() {
    //    debugger;
    //    $('#Url').hide();
    //    $('#Username').hide();
    //    $('#Password').hide();
    //    $('#ActivationKey').hide();
    //}

    //function CategoryChanged() {
    //    debugger;
    //    var value = jQuery("#Category option:selected").text();
    //    //var value = $(this).val();
    //    if (value == 'Software') {
    //        $('#Platform').show();
    //    } else {
    //        hideSoftwareFields();
    //    }
    //}

    //function PlatformChanged() {
    //    debugger;
    //    var value = jQuery("#Platform option:selected").text();
    //    //var value = $(this).val();
    //    if (value == 'Cloud') {
    //        hideActivationFields();
    //    } else if (value == 'OnSite') {
    //        hideCloudsFields();
    //    } else {
    //        hideActivationCloudFields();
    //    }
    //}

//////////////////Inventory screen dropdowns////////////////////////


    $(function () { // edit user change password checkbox activates textbox
        debugger;
        $("#ChangePassword").click(function () {
            debugger;
            if ($(this).is(":checked")) {
                $("#NewPassword").removeAttr("disabled");
                $("#NewPassword").focus();
            } else {
                $("#NewPassword").attr("disabled", "disabled");
            }
        });
      
       
});




