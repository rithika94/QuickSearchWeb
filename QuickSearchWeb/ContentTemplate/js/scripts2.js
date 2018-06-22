(function ($) {

    $('#row-select1').DataTable({
        //stateSave: true,
        "order": [[0, "desc"]],
        "columnDefs": [

            {
                "targets": [0],
                "visible": false,
                "searchable": false
            }
        ]
    });



})(jQuery);